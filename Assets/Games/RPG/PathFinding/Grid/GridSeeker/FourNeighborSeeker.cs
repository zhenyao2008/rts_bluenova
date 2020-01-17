using System.Collections.Generic;
using UnityEngine;
///
/// @file  FourNeighborSeeker.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class FourNeighborSeeker : BaseSeeker
    {
        public FourNeighborSeeker(GStarGrid grid) : base(grid) { }

        public override List<Node> GetNodesByRange(Vector3Int startPos, int xSize, int zSize, int targetRange)
        {
            throw new System.NotImplementedException();
        }

        public override List<Node> GetNodesByRange(Node startNode,int xSize,int zSize,int minRangeInt, int maxRangeInt)
        {
            NodeSearchIdentity.Increase();

            minRangeInt = minRangeInt * GStarGrid.Multiple;

            maxRangeInt = maxRangeInt * GStarGrid.Multiple;

            NodeSearchList nodeSearchList = new NodeSearchList();

            Node originalNode = null;
            for (int x = startNode.X; x < startNode.X + xSize;x++)
            {
                for (int z = startNode.Z; z < startNode.Z + zSize; z++)
                {
                    originalNode = Grid.GetNode(x,z);
                    if (originalNode == null) { continue; }
                    originalNode.G = 0;
                    nodeSearchList.Add(originalNode, NodeSearchIdentity.Value);
                }
            }
            Node neighbor = null;

            while (!nodeSearchList.IsEmpty)
            {

                Node node = nodeSearchList.Extract();

                AddToCloseList(node);

                for (int i = node.Neighbors.Count - 1; i >= 0; i--)
                {
                    neighbor = node.Neighbors[i];

                    if (neighbor.IsClose == NodeSearchIdentity.Value)
                    {
                        continue;
                    }

                    //重要、もしMultiplyCostが0になったら、経路が変になる、そして、必ずデフォルト値を設定して，
                    int neighborCost = node.NeighborCosts[i];

                    int G = node.G + neighborCost;

                    if ( G <= maxRangeInt)
                    {
                        if (neighbor.G > G)
                        {
                            neighbor.G = G;
                        }

                        if (neighbor.IsOpen != NodeSearchIdentity.Value)
                        {
                            neighbor.G = G;

                            nodeSearchList.Add( neighbor, NodeSearchIdentity.Value);
                        }
                    }
                }
            }
            List<Node> nodes = new List<Node>();

            RectInt rect1 = new RectInt(startNode.X, startNode.Z, xSize, zSize);

            int distance;
            for (int i = 0; i < nodeSearchList.List.Count; i++)
            {
                distance = NodeObtainUtils.GetDistance(rect1, new RectInt(nodeSearchList.List[i].X, nodeSearchList.List[i].Z, 1, 1));
                //TODO_AI distance = 0;
                if (distance == 0 || distance >= minRangeInt / GStarGrid.Multiple)
                {
                    nodes.Add(nodeSearchList.List[i]);
                }
            }
            return nodes;
        }
        [System.Obsolete]
        public override List<Node> GetNeighborhoods(Node mainNode, int xSize, int zSize)
        {
            throw new System.Exception("この関数が廃止した");
        }

        public override List<Node> GetNodesByRange(Vector3Int startPos, int xSize, int zSize, int minRange, int maxRange)
        {
            Node startNode = Grid.GetNode(startPos);
            return GetNodesByRange(startNode, xSize, zSize, minRange, maxRange);
        }
        //簡単な速い検索
        public override void GetNodesByRangeSimple(NodeSearchList nodeSearchList, Node startNode, int xSize, int zSize, int minRange, int maxRange, int searchIndex)
        {
            int minXIndex = Mathf.Max(0, startNode.X - maxRange);
            int maxXIndex = Mathf.Min(startNode.X + xSize + maxRange, Grid.XCount - 1);
            int minZIndex = Mathf.Max(0, startNode.Z - maxRange);
            int maxZIndex = Mathf.Min(startNode.Z + zSize + maxRange, Grid.ZCount - 1);
            Node node = null;
            RectInt rect1 = new RectInt(startNode.X, startNode.Z, xSize, zSize);
            for (int i = minXIndex; i <= maxXIndex; i++)
            {
                for (int j = minZIndex; j <= maxZIndex; j++)
                {
                    node = Grid.GetNode(i,j);
                    if (node.IsOpen != searchIndex)
                    {
                        if (NodeObtainUtils.IsInRange(rect1, new RectInt(node.X, node.Z, 1, 1), minRange, maxRange))
                        {
                            node.IsOpen = searchIndex;
                            nodeSearchList.Add(node, searchIndex);
                        }
                    }
                }
            }
        }
    }
}
