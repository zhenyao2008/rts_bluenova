/*
 MoveAgent for town.
 Has avoid.Cost is high.
 應　彧剛（yingyugang@gmail.com）
 */
using BlueNoah.Common;
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
        public FixedPointNode currentNode;
        public GridLayerMask layerMask;
        public GridLayerMask subLayerMask;
        List<FixedPointNode> mPath;
        FixedPointNode mTargetNode;
        bool mIsMoving = false;
        int mCurrentIndex = 0;
        public int priority = 0;
        public FixedPointTransform transform;
        //Per frame distance,not per second.
        FixedPoint64 mSpeed;
        int mUnitCountWhenEnterNode;
        FixedPointVector3 mTargetPos;
        long mNextMoveTime;
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

        public void OnUpdate()
        {
            if (mIsMoving)
            {
                DoMove();
            }
        }
        /*
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
                    targetMovePointList = PathFindingMananger.Single.Find(transform.position, targetPosList[0],this);
                    if (targetMovePointList.Count > 1)
                    {
                        allTargetMovePointList.AddRange(targetMovePointList);
                    }
                }
                else
                {
                    targetMovePointList = PathFindingMananger.Single.Find(targetPosList[i - 1], targetPosList[i],this);
                    if (targetMovePointList.Count > 1)
                    {
                        allTargetMovePointList.AddRange(targetMovePointList.GetRange(1, targetMovePointList.Count - 1));
                    }
                }
            }
            SetPath(allTargetMovePointList, onMoveDone);
        }*/

        public void Stop()
        {
            mIsMoving = false;
        }

        public void SetDestination(FixedPointVector3 targetPos, PathMoveAction onMoveDone = null)
        {
            List<FixedPointNode> targetPosList = PathFindingMananger.Single.Find(transform.position, targetPos,this);

            if (targetPosList == null || targetPosList.Count == 0)
            {
                return;
            }

            mTargetPos = targetPos;

            SetPath(targetPosList, onMoveDone);
        }

        void SetPath(List<FixedPointNode> targetPosList, PathMoveAction onMoveDone)
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

        void UpdateCurrentNode()
        {
            if (currentNode != null)
            {
                currentNode.moveAgent = null;
            }
            currentNode = null;
            FixedPointNode node = PathFindingMananger.Single.GetNode(transform.position);
            if (node != null)
            {
                currentNode = node;
                currentNode.moveAgent = this;
            }
        }

        int mCount = 0;
        const int MAX_MOVE_COUNT = 3;
        //もっとスムース、毎フラーム移動距離を同じい。
        void DoMove()
        {
            FixedPoint64 deltaDistance = Speed / FixedPointMath.Max(1, mUnitCountWhenEnterNode);
            mCount = 0;
            while (deltaDistance > 0 && mCount < MAX_MOVE_COUNT && mIsMoving  )
            {

                FixedPoint64 currentTargetNodeDistance = FixedPointVector3.Distance(mTargetNode.pos, transform.position);

                if (deltaDistance < currentTargetNodeDistance)
                {
                    transform.position += deltaDistance * transform.forward;

                    deltaDistance = 0;
                }
                else
                {
                    transform.position += currentTargetNodeDistance * transform.forward;

                    deltaDistance = deltaDistance - currentTargetNodeDistance;

                    MoveToNextPoint();
                }

                UpdateCurrentNode();

                mCount++;
            }
            if (onPositionChange != null)
            {
                onPositionChange(transform);
            }
        }

        FixedPointNode mCurrentBookNode;

        void MoveToNextPoint()
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
                if (onMoveDone != null)
                {
                    onMoveDone();
                }
            }
        }
    }
}