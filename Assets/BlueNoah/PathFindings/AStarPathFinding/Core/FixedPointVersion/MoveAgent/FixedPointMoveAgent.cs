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
                mSpeed = value;
            }
        }

        public void OnUpdate()
        {

            if (mIsMoving)
            {
                DoMove();
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
            while (deltaDistance > 0 && mCount < MAX_MOVE_COUNT && mIsMoving)
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

            if (mMoved)
            {
                if (onPositionChange != null)
                {
                    onPositionChange(transform);
                }
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

        //計算when 非直線の場合。
        List<FixedPointVector3> CatmullRomSmooth(List<FixedPointNode> path)
        {
            List<FixedPointVector3> points = new List<FixedPointVector3>();

            for (int i = 0; i < path.Count; i++)
            {
                
            }

            return points;
        }
        //when u = 0,u= 0.25,u=0.5,u=0.75
        FixedPointVector3[] FastCatmullRomSmooth(FixedPointVector3 point0, FixedPointVector3 point1, FixedPointVector3 point2, FixedPointVector3 point3)
        {
            return null;
        }

    }
}