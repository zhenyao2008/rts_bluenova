using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
using BlueNoah.PathFinding.FixedPoint;
using UnityEngine;
using FixedPointGrid = BlueNoah.PathFinding.FixedPoint.FixedPointGrid;

namespace BlueNoah.PathFinding
{

    public static class Vector3Extention
    {
        public static FixedPointVector3 ToFixedPointVector3(this Vector3 vector3)
        {
            return new FixedPointVector3(vector3.x, vector3.y, vector3.z);
        }
    }

    public class PathFindingMananger
    {

        static PathFindingMananger mInstance;

        public static PathFindingMananger Single
        {
            get
            {
                if (mInstance == null)
                {
                    NewInstance();
                }
                return mInstance;
            }
        }

        public static PathFindingMananger NewInstance()
        {
            mInstance = new PathFindingMananger();
            mInstance.InitAStarPathFinding();
            return mInstance;
        }

        private PathFindingMananger()
        {
            mMoveAgentList = new List<FixedPointMoveAgent>();
        }

        public FixedPointMoveAgent CreateMoveAgent(FixedPointTransform transform, FixedPoint64 speed)
        {
            FixedPointMoveAgent moveAgent = new FixedPointMoveAgent();
            moveAgent.Speed = speed;
            moveAgent.transform = transform;
            moveAgent.priority = mMoveAgentList.Count;
            mMoveAgentList.Add(moveAgent);
            return moveAgent;
        }

        List<FixedPointMoveAgent> mMoveAgentList;

        FixedPointGrid mGrid;

        public FixedPointGrid Grid
        {
            get
            {
                return mGrid;
            }
        }

        Material mMaterial;

        FixedPointPathAgent mPathAgent;

        public FixedPointPathAgent PathAgent
        {
            get
            {
                return mPathAgent;
            }
        }

        void InitAStarPathFinding()
        {
            Debug.Log("InitAStarPathFinding");
            mMaterial = Resources.Load<Material>("Materials/node");
            mGrid = new FixedPoint.FixedPointGrid();
            FixedPointGridSetting gridSetting = new FixedPointGridSetting();
            //gridSetting.nodeWidth = 0.5f;
            //gridSetting.diagonalPlus = 1f;
            //gridSetting.startPos = new FixedPointVector3(0, 0, 0);
            //gridSetting.xCount = 120;
            //gridSetting.zCount = 120;
            gridSetting.nodeWidth = 1f;
            gridSetting.diagonalPlus = 1.4f;
            gridSetting.startPos = new FixedPointVector3(0, 0, 0);
            gridSetting.xCount = 100;
            gridSetting.zCount = 100;
            mGrid.Init(gridSetting);
            mPathAgent = new FixedPointPathAgent(mGrid);

            NodeEnableUtility.CheckNodeBlocking(mGrid);

            //NodeEnableUtility.EnableNodes(mGrid);

            //NodeEnableUtility.CheckBridge(mGrid);
        }

        public FixedPointNode GetNode(FixedPointVector3 position)
        {
            return mGrid.GetNode(position);
        }

        public List<FixedPointNode> Find(FixedPointVector3 startPos, FixedPointVector3 endPos,FixedPointMoveAgent fixedPointMoveAgent)
        {
            return mPathAgent.StartFind(startPos, endPos, fixedPointMoveAgent);
        }

        public List<FixedPointNode> Find(Vector3 startPos, Vector3 endPos, FixedPointMoveAgent fixedPointMoveAgent)
        {
            return mPathAgent.StartFind(startPos.ToFixedPointVector3(), endPos.ToFixedPointVector3(), fixedPointMoveAgent);
        }
        //外部統一制御している。
        public void OnUpdate()
        {
            for (int i = 0; i < mMoveAgentList.Count; i++)
            {
                mMoveAgentList[i].OnUpdate();
            }
        }

        public List<FixedPointVector3> InitNavPathFinding()
        {
            Debug.LogError("使ってない");
            return null;
        }

        public void OnDrawGizmos()
        {
            if (mGrid != null)
                mGrid.OnDrawGizmos();
            //if (mPathAgent != null)
            //    mPathAgent.OnDrawGizmos();
        }
    }
}