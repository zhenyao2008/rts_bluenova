/*
 MoveAgent for town.
 Has avoid.Cost is high.
 應　彧剛（yingyugang@gmail.com）
 */
using BlueNoah.Math.FixedPoint;
// using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{
    public delegate void PathMoveAction();

    public delegate void PathMoveAction<T0>(T0 t);

    [System.Serializable]
    public class FixedPointMoveAgent
    {

        public PathMoveAction onMove;
        public PathMoveAction onStop;
        public PathMoveAction<FixedPointTransform> onPositionChange;
        public PathMoveAction<FixedPointTransform> onNodeChange;
        public PathMoveAction onMoveDone;
        List<FixedPointNode> mPath;
        public FixedPointNode currentNode;
        FixedPointNode mTargetNode;
        bool mIsMoving = false;
        int mCurrentIndex = 0;
        public int priority = 0;
        public FixedPointTransform transform;
        //Per frame distance,not per second.
        FixedPoint64 mSpeed;
        int mUnitCountWhenEnterNode;
        FixedPointVector3 mTargetPos;
        bool mIsBlocked;
        FixedPointMoveAgent mWaitingMoveAgent;
        bool mIsWaitBridge;
        bool mIsWaitForOther;
        int mWaitTime = 40;
        long mNextMoveTime;
        bool mIgnoreMode = false;
        long mNextAvoid;
        const int AVOID_INTERVAL = 20;
        const int WAIT_MOVE_NODE_COUNT = 6;

        //need call it when gameobject destory or moveagent disabled;
        public void Disable()
        {

        }
        public FixedPoint64 Speed
        {
            get
            {
                return mSpeed;
            }
            set
            {
                mSpeed = value / 60;
            }
        }
        public void Block(FixedPointMoveAgent waitMoveAgent)
        {

            mWaitingMoveAgent = waitMoveAgent;

            mNextMoveTime = SceneControl.Framer.Instance.CurrentFrame + mWaitTime;

            mIsBlocked = true;

            if (onStop != null)
            {
                onStop();
            }
        }

        public void OnUpdate()
        {
            if (mIsWaitBridge)
            {
                WaitingBridge();
            }
            else
            {
                if (mIsBlocked)
                {
                    if (mWaitingMoveAgent != null)
                    {
                        FixedPoint64 distance = FixedPointVector3.Distance(mWaitingMoveAgent.transform.position, transform.position);

                        if (mNextMoveTime < SceneControl.Framer.Instance.CurrentFrame && distance > 2)
                        {

                            mIsBlocked = false;

                            currentNode.IsBlock = false;

                            if (onMove != null)
                            {
                                onMove();
                            }
                        }
                    }
                }
            }
            if (mIsMoving)
            {
                if (WaitForOther())
                {
                    DoMove();
                }
            }
        }
        public void ReDestination()
        {
            SetDestination(this.mTargetPos, onMoveDone);
        }
        public void SetDestination(List<FixedPointVector3> targetPosList, PathMoveAction onMoveDone = null)
        {
            List<FixedPointNode> allTargetMovePointList = new List<FixedPointNode>();
            List<FixedPointNode> targetMovePointList;
            float time = Time.realtimeSinceStartup;
            for (int i = 0; i < targetPosList.Count; i++)
            {
                if (i == 0)
                {
                    targetMovePointList = PathFindingMananger.Single.Find(transform.position, targetPosList[0]);
                    if (targetMovePointList.Count > 1)
                    {
                        allTargetMovePointList.AddRange(targetMovePointList);
                    }
                }
                else
                {
                    targetMovePointList = PathFindingMananger.Single.Find(targetPosList[i - 1], targetPosList[i]);
                    if (targetMovePointList.Count > 1)
                    {
                        allTargetMovePointList.AddRange(targetMovePointList.GetRange(1, targetMovePointList.Count - 1));
                    }
                }
            }
            SetPath(allTargetMovePointList, onMoveDone);
        }
        public void SetDestination(FixedPointVector3 targetPos, PathMoveAction onMoveDone = null)
        {
            List<FixedPointNode> targetPosList = PathFindingMananger.Single.Find(transform.position, targetPos);

            if (targetPosList == null || targetPosList.Count == 0)
            {
                return;
            }

            mTargetPos = targetPos;

            SetPath(targetPosList, onMoveDone);
        }

        public void SetPath(List<FixedPointNode> targetPosList, PathMoveAction onMoveDone)
        {
            this.onMoveDone = onMoveDone;
            mPath = targetPosList;
            mCurrentIndex = 0;
            mIsMoving = true;
            if (targetPosList != null && targetPosList.Count > 0)
            {
                if (onMove != null)
                {
                    onMove();
                }
            }
            MoveToNextPoint();
        }

        public void UpdateCurrentNode()
        {
            if (currentNode != null)
            {
                // currentNode.unitCount--;
                currentNode.moveAgent = null;
            }
            currentNode = null;
            FixedPointNode node = PathFindingMananger.Single.GetNode(transform.position);
            if (node != null)
            {
                currentNode = node;
                // currentNode.unitCount++;
                currentNode.moveAgent = this;
            }
        }

        int mCount = 0;
        bool mMoved = false;
        const int MAX_MOVE_COUNT = 3;
        //もっとスムース、毎フラーム移動距離を同じい。
        void DoMove()
        {
            FixedPoint64 deltaDistance = Speed / FixedPointMath.Max(1, mUnitCountWhenEnterNode);
            mCount = 0;
            mMoved = false;
            while (deltaDistance > 0 && mCount < MAX_MOVE_COUNT && mIsMoving && !mIsBlocked && !mIsWaitBridge && !mIsWaitForOther)
            {
                if (AvoidChecking())
                {
                    mMoved = true;

                    FixedPoint64 currentTargetNodeDistance = FixedPointVector3.Distance(mTargetNode.pos, transform.position);

                    if (deltaDistance < currentTargetNodeDistance)
                    {
                        transform.position += deltaDistance * transform.forward;

                        // viewTransform.position = transform.position.ToVector3();

                        // viewTransform.forward = transform.forward.ToVector3();

                        deltaDistance = 0;
                    }
                    else
                    {
                        transform.position += currentTargetNodeDistance * transform.forward;

                        // viewTransform.position = transform.position.ToVector3();

                        // viewTransform.forward = transform.forward.ToVector3();

                        deltaDistance = deltaDistance - currentTargetNodeDistance;

                        MoveToNextPoint();
                    }

                    UpdateCurrentNode();

                    mCount++;
                }
            }

            if (mMoved)
            {
                if (onPositionChange != null)
                {
                    onPositionChange(transform);
                }
            }
        }

        bool AvoidChecking()
        {
            return true;
            if (mNextAvoid > SceneControl.Framer.Instance.CurrentFrame)
            {
                return true;
            }
            if (!mIgnoreMode && !mPath[mCurrentIndex].isBridge)
            {
                int countForward = UnityEngine.Mathf.Min(1, mPath.Count - 1 - mCurrentIndex);

                for (int i = 0; i < countForward; i++)
                {
                    FixedPointNode node = mPath[mCurrentIndex + 1 + i];
                    if (node.unitCount > 0)
                    {
                        if (node.moveAgent != null)
                        {
                            if ((node.moveAgent.Speed < Speed || (node.moveAgent.Speed == Speed && node.moveAgent.priority < priority)))
                            {
                                List<FixedPointNode> checkedList = new List<FixedPointNode>();

                                for (int j = 0; j < countForward; j++)
                                {
                                    checkedList.Add(mPath[mCurrentIndex + 1 + j]);
                                }

                                for (int j = 0; j < checkedList.Count; j++)
                                {
                                    checkedList[j].consumeRoadSizePlus += 10;
                                }

                                node.moveAgent.currentNode.consumeRoadSizePlus += 10;

                                ReDestination();

                                node.moveAgent.currentNode.consumeRoadSizePlus -= 10;

                                for (int j = 0; j < checkedList.Count; j++)
                                {
                                    checkedList[j].consumeRoadSizePlus -= 10;
                                }
                                // Debug.Log("Block Others.");
                                node.moveAgent.Block(this);
                            }
                            else
                            {
                                Block(node.moveAgent);
                            }
                            mNextAvoid = SceneControl.Framer.Instance.CurrentFrame + AVOID_INTERVAL;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        void WaitingBridge()
        {
            FixedPointNode nextNode = mPath[mCurrentIndex + 1];

            FixedPointNode currentNode = mPath[mCurrentIndex];

            if (!nextNode.bridge.isBridgeUsed || (!nextNode.bridge.moveAgents.Contains(this) && nextNode.bridge.forward == (nextNode.pos - currentNode.pos).normalized))
            {
                nextNode.bridge.forward = (nextNode.pos - currentNode.pos).normalized;

                nextNode.bridge.EnterBridge(this);

                currentNode.isTempBlock = false;

                currentNode.tempBlockMoveAgent = null;

                mIsWaitBridge = false;

                if (onMove != null)
                {
                    onMove();
                }
            }
        }

        bool WaitForOther()
        {
            return true;
            if (mPath.Count > mCurrentIndex + 1 && !mPath[mCurrentIndex].isBridge)
            {
                FixedPointNode nextNode = mPath[mCurrentIndex + 1];
                if (nextNode.isTempBlock && nextNode.tempBlockMoveAgent != this)
                {
                    return false;
                }
            }
            return true;
        }

        bool CheckUpOnBridge()
        {
            //has next node.
            if (mPath.Count > mCurrentIndex + 1)
            {
                FixedPointNode nextNode = mPath[mCurrentIndex + 1];
                FixedPointNode currentNode = mPath[mCurrentIndex];
                //Check if bridge enterable. 
                if (nextNode.isBridge && !mPath[mCurrentIndex].isBridge)
                {
                    if (!nextNode.bridge.isBridgeUsed)
                    {
                        nextNode.bridge.forward = (nextNode.pos - currentNode.pos).normalized;
                        nextNode.bridge.isBridgeUsed = true;
                        nextNode.bridge.EnterBridge(this);
                    }
                    else
                    {
                        if (!nextNode.bridge.moveAgents.Contains(this))
                        {
                            if (nextNode.bridge.forward == (nextNode.pos - currentNode.pos).normalized)
                            {
                                nextNode.bridge.EnterBridge(this);
                            }
                            else
                            {
                                Debug.Log("Stop");
                                mIsWaitBridge = true;
                                currentNode.isTempBlock = true;
                                currentNode.tempBlockMoveAgent = this;
                                if (onStop != null)
                                {
                                    onStop();
                                }
                                return false;
                            }
                        }
                    }
                }
                if (currentNode.isBridge && !nextNode.isBridge)
                {
                    currentNode.bridge.OutBridge(this);
                }
            }
            return true;
        }

        FixedPointNode mCurrentBookNode;

        void MoveToNextPoint()
        {
            if (CheckUpOnBridge())
            {
                mIsMoving = true;
                mCurrentIndex++;

                if (mCurrentBookNode != null)
                {
                    mCurrentBookNode.unitCount--;
                    mCurrentBookNode = null;
                }

                if (mCurrentIndex < mPath.Count)
                {
                    if (mCurrentIndex > 3)
                    {
                        mIgnoreMode = false;
                    }
                    mTargetNode = mPath[mCurrentIndex];
                    transform.forward = (mTargetNode.pos - transform.position).normalized;
                    mCurrentBookNode = mTargetNode;
                    mCurrentBookNode.unitCount++;
                    mCurrentBookNode.moveAgent = this;
                    //ムーブ速度を調整
                    mUnitCountWhenEnterNode = mCurrentBookNode.unitCount + 1;

                    if (onNodeChange != null)
                    {
                        onNodeChange(transform);
                    }
                }
                else
                {
                    mIsMoving = false;
                    mIgnoreMode = false;
                    if (onMoveDone != null)
                    {
                        onMoveDone();
                    }
                }
            }
        }

        public bool IsMoveDone
        {
            get
            {
                return !mIsMoving;
            }
        }

        public void Stop()
        {
            mPath = null;
            mIsMoving = false;
        }
    }
}