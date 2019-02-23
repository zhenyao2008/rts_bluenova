using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{
    //grid start from (-x/2,0,-z/2).
    public class FixedPointGrid
    {

        FixedPointVector3 mStartPos = new FixedPointVector3(0, 0, 0);

        FixedPointGridSetting mGridSetting;

        FixedPointNode[,] mNodeArray;

        List<FixedPointNode> mNodeList;

        public int NeighborCount
        {
            get
            {
                return mGridSetting.neighborCount;
            }
        }

        public FixedPoint64 xCount
        {
            get
            {
                return mGridSetting.xCount;
            }
        }

        public FixedPoint64 zCount
        {
            get
            {
                return mGridSetting.zCount;
            }
        }

        public FixedPoint64 NodeSize
        {
            get
            {
                return mGridSetting.nodeWidth;
            }
        }

        public FixedPointGridSetting GridSetting
        {
            get
            {
                return this.mGridSetting;
            }
        }

        public void Init(FixedPointGridSetting gridSetting)
        {
            this.mGridSetting = gridSetting;
            Init();
        }

        public void Reset()
        {
            for (int i = 0; i < mNodeList.Count; i++)
            {
                mNodeList[i].previous = null;
            }
        }

        void Init()
        {
            mNodeArray = new FixedPointNode[mGridSetting.xCount, mGridSetting.zCount];

            mNodeList = new List<FixedPointNode>();

            mStartPos = new FixedPointVector3(mGridSetting.offsetPos.x - mGridSetting.nodeWidth * (mGridSetting.xCount - 1) / new FixedPoint64(2), mGridSetting.offsetPos.y, mGridSetting.offsetPos.z - mGridSetting.nodeWidth * (mGridSetting.zCount - 1) / new FixedPoint64(2));

            for (int i = 0; i < mGridSetting.xCount; i++)
            {
                for (int j = 0; j < mGridSetting.zCount; j++)
                {
                    FixedPointNode node = new FixedPointNode();
                    node.x = i;
                    node.z = j;
                    FixedPoint64 width = mGridSetting.xCount * mGridSetting.nodeWidth;
                    FixedPoint64 height = mGridSetting.zCount * mGridSetting.nodeWidth;
                    FixedPoint64 x = node.x * mGridSetting.nodeWidth + mStartPos.x;
                    FixedPoint64 z = node.z * mGridSetting.nodeWidth + mStartPos.z;
                    node.pos = new FixedPointVector3(x, 0, z);
                    mNodeList.Add(node);
                    mNodeArray[i, j] = node;
                }
            }
            CalculateNeighbors();
        }

        void CalculateNeighbors()
        {
            for (int i = 0; i < mNodeList.Count; i++)
            {
                CalculateNeighbor(mNodeList[i]);
            }
        }

        void CalculateNeighbor(FixedPointNode node)
        {
            bool north = false;
            bool south = false;
            bool east = false;
            bool west = false;
            node.neighbors.Clear();
            node.consumes.Clear();
            int i = node.x;
            int j = node.z;
            if (i > 0)
            {
                west = AddNeighbor(node, mNodeArray[i - 1, j], mGridSetting.nodeWidth);
            }

            if (j > 0)
            {
                south = AddNeighbor(node, mNodeArray[i, j - 1], mGridSetting.nodeWidth);
            }

            if (i < mGridSetting.xCount - 1)
            {
                east = AddNeighbor(node, mNodeArray[i + 1, j], mGridSetting.nodeWidth);
            }

            if (j < mGridSetting.zCount - 1)
            {
                north = AddNeighbor(node, mNodeArray[i, j + 1], mGridSetting.nodeWidth);
            }

            if (i > 0 && j > 0)
            {
                if (west && south)
                {
                    AddNeighbor(node, mNodeArray[i - 1, j - 1], mGridSetting.diagonalPlus * mGridSetting.nodeWidth);
                }
            }

            if (i > 0 && j < mGridSetting.zCount - 1)
            {
                if (west && north)
                {
                    AddNeighbor(node, mNodeArray[i - 1, j + 1], mGridSetting.diagonalPlus * mGridSetting.nodeWidth);
                }
            }

            if (i < mGridSetting.xCount - 1 && j > 0)
            {
                if (east && south)
                {
                    AddNeighbor(node, mNodeArray[i + 1, j - 1], mGridSetting.diagonalPlus * mGridSetting.nodeWidth);
                }
            }

            if (i < mGridSetting.xCount - 1 && j < mGridSetting.zCount - 1)
            {
                if (east && south)
                {
                    AddNeighbor(node, mNodeArray[i + 1, j + 1], mGridSetting.diagonalPlus * mGridSetting.nodeWidth);
                }
            }
        }

        bool AddNeighbor(FixedPointNode node, FixedPointNode neighbor, FixedPoint64 width)
        {
            if (!neighbor.IsBlock)
            {
                node.neighbors.Add(neighbor);
                node.consumes.Add(width);
                return true;
            }
            return false;
        }

        public FixedPointNode GetNearestNode(FixedPointVector3 pos)
        {
            FixedPoint64 xIndex = FixedPointMath.Round((pos.x + mStartPos.x) / mGridSetting.nodeWidth);
            FixedPoint64 yIndex = FixedPointMath.Round((pos.z + mStartPos.z) / mGridSetting.nodeWidth);
            xIndex = FixedPointMath.Clamp(xIndex, 0, mGridSetting.xCount - 1); // Mathf.Clamp(xIndex, 0, mGridSetting.xCount - 1);
            yIndex = FixedPointMath.Clamp(yIndex, 0, mGridSetting.zCount - 1);
            return GetNode(xIndex.AsInt(), yIndex.AsInt());
        }

        public FixedPointNode GetNode(FixedPointVector3 pos)
        {
            FixedPoint64 xIndex = (pos.x - mStartPos.x) / mGridSetting.nodeWidth;
            FixedPoint64 zIndex = (pos.z - mStartPos.z) / mGridSetting.nodeWidth;
            return GetNode(FixedPointMath.Round(xIndex).AsInt(), FixedPointMath.Round(zIndex).AsInt());
        }

        public FixedPointNode GetNode(int xIndex, int yIndex)
        {
            if (xIndex < 0 || xIndex >= mGridSetting.xCount || yIndex < 0 || yIndex >= mGridSetting.zCount)
            {
                return null;
            }
            else
            {
                return mNodeArray[xIndex, yIndex];
            }
        }
        //自動的に節点をブロックする
        //public void BlockNodes()
        //{
        //    for (int i = 0; i < mNodeList.Count; i++)
        //    {
        //        if (Physics.CheckBox(mNodeList[i].pos.ToVector3(), Vector3.one * (mGridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, ~(1 << LayerConstant.LAYER_GROUND)))
        //        {
        //            mNodeList[i].IsBlock = true;
        //        }
        //        else
        //        {
        //            mNodeList[i].IsBlock = false;
        //        }
        //    }
        //}

        //public void EnableNodes()
        //{
        //    for (int i = 0; i < mNodeList.Count; i++)
        //    {
        //        if (Physics.CheckBox(mNodeList[i].pos.ToVector3(), Vector3.one * (mGridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_ROAD))
        //        {
        //            mNodeList[i].Enable = true;
        //        }
        //        else
        //        {
        //            mNodeList[i].Enable = false;
        //        }
        //    }
        //    for (int i = 0; i < mNodeList.Count; i++)
        //    {
        //        if (Physics.CheckBox(mNodeList[i].pos.ToVector3(), Vector3.one * (mGridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_ROAD_SIDE))
        //        {
        //            mNodeList[i].Enable = true;
        //            mNodeList[i].consumeRoadSizePlus = 4;
        //        }
        //    }
        //}

        //public void CheckBridge()
        //{
        //    for (int i = 0; i < mNodeList.Count; i++)
        //    {
        //        Collider[] colliders = Physics.OverlapBox(mNodeList[i].pos.ToVector3(), Vector3.one * (mGridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_BRIDGE);
        //        if (colliders.Length > 0)
        //        {
        //            Collider collider = colliders[0];
        //            FixedPointBridge bridge = collider.gameObject.GetOrAddComponent<FixedPointBridge>();
        //            bridge.AddNode(mNodeList[i]);
        //        }
        //    }
        //}

        public List<FixedPointNode> NodeList
        {
            get
            {
                return mNodeList;
            }
        }

        GUIStyle mGUIStyle;

        public void OnDrawGizmos()
        {
            if (mNodeList != null)
            {
                for (int i = 0; i < mNodeList.Count; i++)
                {
                    if (mNodeList[i] != null)
                    {

#if UNITY_EDITOR
                        if (mGUIStyle == null)
                        {
                            mGUIStyle = new GUIStyle();
                            mGUIStyle.active.textColor = Color.green;
                        }

                        //UnityEditor.Handles.color = Color.green;
                        //if (mNodeList[i].F > 0)
                        //UnityEditor.Handles.Label(mNodeList[i].pos.ToVector3(), mNodeList[i].F.ToString());
#endif
                        //continue;
                        if (mNodeList[i].IsBlock || !mNodeList[i].Enable)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            if ((mGridSetting.neighborCount == 4 && mNodeList[i].neighbors.Count < 4) || (mGridSetting.neighborCount == 8 && mNodeList[i].neighbors.Count < 8) || mNodeList[i].blockNeighborCount > 0)
                            {
                                Gizmos.color = Color.blue;
                            }
                            else
                            {
                                Gizmos.color = Color.green;
                            }
                        }
                        if (mNodeList[i].consumeRoadSizePlus > 0)
                        {
                            Gizmos.color = Color.yellow;
                        }

                        //if (mNodeList[i].unitCount > 0)
                        //{
                        //    Gizmos.color = Color.yellow;
                        //}

                        if (mNodeList[i].isBridge)
                        {
                            Gizmos.color = Color.white;
                        }
                        Gizmos.DrawCube(mNodeList[i].pos.ToVector3(), Vector3.one * mGridSetting.nodeWidth.AsFloat() * 0.3f);
                    }
                }
            }
        }
    }
}

