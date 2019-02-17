using BlueNoah.Math.FixedPoint;
using UnityEngine;
using System.Collections.Generic;

namespace BlueNoah.PathFinding.FixedPoint
{

    public delegate void PathMoveAction();

    [System.Serializable]
    public class FixedPointMoveAgent
    {
        //prepare trigge other character.
        public event PathMoveAction preTrigger;

        PathMoveAction mOnMoveDone;

        public PathMoveAction onMove;

        public PathMoveAction onStop;

        bool mIsMoving = false;

        FixedPoint64 MIN_STOP_DISTANCE = 0.1f;

        List<FixedPointNode> mTargetPosList;

        public FixedPointNode currentNode;

        FixedPointNode mTargetNode;

        int mCurrentIndex = 0;

        public int priority = 0;

        public FixedPointTransform transform;

        public UnityEngine.Transform viewTransform;
        //Per frame,not per second.
        public FixedPoint64 deltaDistancePerFrame;

        int mUnitCountWhenEnterNode;

        FixedPointVector3 mTargetPos;

        bool mIsBlocked;

        bool mIsWaitBridge;

        bool mIsWaitForOther;

        int mWaitTime = 40;

        int mExitTime;

        bool mIgnoreMode = false;

        FixedPointNode mWaitingNode;

        //need call it when gameobject destory or moveagent disabled;
        public void Disable()
        {

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
                    mExitTime++;
                    if (mExitTime >= mWaitTime)
                    {
                        mIgnoreMode = true;
                        mIsBlocked = false;
                        if (onMove != null)
                        {
                            onMove();
                        }
                    }
                    if (mWaitingNode.unitCount == 0)
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
            if (mIsMoving)
            {
                if (WaitForOther())
                {
                    DoMoveWithWhile();
                }
                //else
                //{
                //    unitCore.GetComponent<UnitAnimation>().UnRun();
                //}
            }
        }

        public void ReDestination()
        {
            SetDestination(this.mTargetPos, mOnMoveDone);
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
            //Debug.LogError("Search Time : " + (Time.realtimeSinceStartup -time));
            StartMove(allTargetMovePointList, onMoveDone);
        }

        public void SetDestination(FixedPointVector3 targetPos, PathMoveAction onMoveDone = null)
        {
            List<FixedPointNode> targetPosList = PathFindingMananger.Single.Find(transform.position, targetPos);

            mTargetPos = targetPos;

            StartMove(targetPosList, onMoveDone);
        }

        public void StartMove(List<FixedPointNode> targetPosList, PathMoveAction onMoveDone)
        {
            mOnMoveDone = onMoveDone;

            mTargetPosList = targetPosList;

            mCurrentIndex = 0;

            mIsMoving = true;

            MoveToNextPoint();
        }

        public void UpdateCurrentNode()
        {
            if (currentNode != null)
            {
                currentNode.unitCount--;
                currentNode.moveAgent = null;
            }
            currentNode = null;
            FixedPointNode node = PathFindingMananger.Single.GetNode(transform.position);
            if (node != null)
            {
                currentNode = node;
                currentNode.unitCount++;
                mUnitCountWhenEnterNode = currentNode.unitCount;
                currentNode.moveAgent = this;
            }
        }

        //スムース、毎フラームの移動距離が同じくない。
        void DoMoveWithoutWhile()
        {
            FixedPoint64 distance = FixedPointVector3.Distance(mTargetNode.pos, transform.position);

            if (distance > MIN_STOP_DISTANCE)
            {
                FixedPoint64 detalDistance = FixedPointMath.Min(distance, deltaDistancePerFrame);

                transform.position += detalDistance * transform.forward;

                viewTransform.position = transform.position.ToVector3();

                viewTransform.forward = transform.forward.ToVector3();
            }
            else
            {
                //remain distance.
                MoveToNextPoint();
            }
        }
        //もっとスムース、毎フラーム移動距離を同じい。
        void DoMoveWithWhile()
        {
            FixedPoint64 deltaDistance = deltaDistancePerFrame / FixedPointMath.Max(1, mUnitCountWhenEnterNode);

            int count = 0;

            while (deltaDistance > 0 && count < 3 && mIsMoving && !mIsBlocked && !mIsWaitBridge && !mIsWaitForOther)
            {
                if (!mIgnoreMode && !mTargetPosList[mCurrentIndex].isBridge)
                {
                    int countForward = Mathf.Min(2, mTargetPosList.Count - 1 - mCurrentIndex);

                    for (int i = 0; i < countForward; i++)
                    {
                        FixedPointNode node = mTargetPosList[mCurrentIndex + 1 + i];
                        if (node.unitCount > 0)
                        {
                            if (node.moveAgent != null && node.moveAgent.priority < priority)
                            {
                                //mIsBlocked = true;
                                //mCurrentNode.IsBlock = true;
                                List<FixedPointNode> checkedList = new List<FixedPointNode>();

                                for (int j = 0; j < countForward; j++)
                                {
                                    checkedList.Add(mTargetPosList[mCurrentIndex + 1 + j]);
                                }
                                for (int j = 0; j < checkedList.Count; j++)
                                {
                                    checkedList[j].consumeRoadSizePlus += 10;
                                }
                                ReDestination();
                                for (int j = 0; j < checkedList.Count; j++)
                                {
                                    checkedList[j].consumeRoadSizePlus -= 10;
                                }
                                //mCurrentNode.IsBlock = false;
                            }
                            else
                            {
                                mExitTime = 0;
                                mIsBlocked = true;
                                mWaitingNode = node;
                                if (onStop!=null)
                                {
                                    onStop();
                                }
                                //unitCore.GetComponent<UnitAnimation>().UnRun();
                            }
                            return;
                        }
                    }
                }

                FixedPoint64 currentTargetNodeDistance = FixedPointVector3.Distance(mTargetNode.pos, transform.position);

                if (deltaDistance < currentTargetNodeDistance)
                {
                    transform.position += deltaDistance * transform.forward;

                    viewTransform.position = transform.position.ToVector3();

                    viewTransform.forward = transform.forward.ToVector3();

                    deltaDistance = 0;
                }
                else
                {
                    transform.position += currentTargetNodeDistance * transform.forward;

                    viewTransform.position = transform.position.ToVector3();

                    viewTransform.forward = transform.forward.ToVector3();

                    deltaDistance = deltaDistance - currentTargetNodeDistance;

                    MoveToNextPoint();
                }

                UpdateCurrentNode();

                count++;
            }
        }

        void WaitingBridge()
        {
            FixedPointNode nextNode = mTargetPosList[mCurrentIndex + 1];

            FixedPointNode currentNode = mTargetPosList[mCurrentIndex];

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
            if (mTargetPosList.Count > mCurrentIndex + 1 && !mTargetPosList[mCurrentIndex].isBridge)
            {
                FixedPointNode nextNode = mTargetPosList[mCurrentIndex + 1];
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
            if (mTargetPosList.Count > mCurrentIndex + 1)
            {
                FixedPointNode nextNode = mTargetPosList[mCurrentIndex + 1];
                FixedPointNode currentNode = mTargetPosList[mCurrentIndex];
                //Check if bridge enterable. 
                if (nextNode.isBridge && !mTargetPosList[mCurrentIndex].isBridge)
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
                                mExitTime = 0;
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

        void MoveToNextPoint()
        {
            if (CheckUpOnBridge())
            {
                mIsMoving = true;
                mCurrentIndex++;
                if (mCurrentIndex < mTargetPosList.Count)
                {
                    if (mCurrentIndex > 3)
                    {
                        mIgnoreMode = false;
                    }
                    mTargetNode = mTargetPosList[mCurrentIndex];
                    transform.forward = (mTargetNode.pos - transform.position).normalized;
                }
                else
                {
                    mIsMoving = false;
                    mIgnoreMode = false;
                    if (mOnMoveDone != null)
                    {
                        mOnMoveDone();
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
            mTargetPosList = null;
            mIsMoving = false;
        }
    }
}