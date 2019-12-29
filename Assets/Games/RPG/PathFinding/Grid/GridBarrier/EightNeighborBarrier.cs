using System.Collections.Generic;
using UnityEngine;
///
/// @file  EightNeighborBarrier.cs
/// @author Ying YuGang
/// @date   
/// @brief　経路がスムーズになるために。
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class EightNeighborBarrier : BaseBarrier
    {
        public EightNeighborBarrier(GStarGrid grid) : base(grid) { }

        public override List<Vector3> SmoothPath(List<Node> nodes, GridLayerMask mask)
        {
            return null;
        }

        public override List<Node> FilterPath(List<Node> nodes, GridLayerMask mask)
        {
            if (nodes.Count <= 2)
            {
                return nodes;
            }
            List<Node> filter = new List<Node>();
            Node startNode = nodes[0];
            filter.Add(startNode);
            for (int i = 0;i<nodes.Count - 1;i++)
            {
                Node node = nodes[i + 1];
                if (i + 1 == nodes.Count - 1)
                {
                    filter.Add(node);
                }
                else
                {
                    if (HasBarrier(startNode, node, mask))
                    {
                        Debug.Log("HasBarrier");
                        startNode = node;
                        filter.Add(startNode);
                    }
                }
            }
            return filter;
        }

        public override bool HasBarrier(Node startNode, Node endNode,GridLayerMask gridLayerMask)
        {

            if (startNode == endNode)
            {
                return false;
            }
            float distX = Mathf.Abs(endNode.Pos.x - startNode.Pos.x);
            float distZ = Mathf.Abs(endNode.Pos.z - startNode.Pos.z);
            bool loopDirection = distX > distZ;

            int i;
            int loopStart;
            int loopEnd;

            List<Node> checkList = new List<Node>();

            if (loopDirection)
            {
                loopStart = (int)(Mathf.Min(startNode.Pos.x, endNode.Pos.x) / Grid.EdgeLength);
                loopEnd = (int)(Mathf.Max(startNode.Pos.x, endNode.Pos.x) / Grid.EdgeLength);
                float height = startNode.Pos.y;
                for (i = loopStart; i < loopEnd; i++)
                {
                    /*
                    float posX = (i + 1) * Grid.EdgeLength;
                    float posZ = GetZByRateX(Grid.EdgeLength, startNode.Pos, endNode.Pos, posX);
                    Node node = Grid.GetNode(new Vector3(posX, 0, posZ));
                    //横の方向
                    if (!GStarGrid.IsNodeValid(node, gridLayerMask))
                    {
                        return true;
                    }
                    */
                }
            }
            else
            {
                loopStart = (int)(Mathf.Min(startNode.Pos.z, endNode.Pos.z) / Grid.EdgeLength);//     Math.min(startY, endY);
                loopEnd = (int)(Mathf.Max(startNode.Pos.z, endNode.Pos.z) / Grid.EdgeLength);
                float height = startNode.Pos.y;
                for (i = loopStart; i < loopEnd; i++)
                {
                    /*
                    float posZ = (i + 1) * Grid.EdgeLength;
                    float posX = GetXByRateZ(Grid.EdgeLength, startNode.Pos, endNode.Pos, posZ);
                    Node node = Grid.GetNode(new Vector3(posX, 0, posZ));
                    //従の方向
                    if (!GStarGrid.IsNodeValid(node, gridLayerMask))
                    {
                        return true;
                    }
                    */
                }
            }
            return false;
        }

        public override List<Node> GetNodes(Node startNode, Node endNode)
        {
            return null;
        }
        //To fast check if there has any barriar between startPos and endPos that base on node properties.
        //二つポジションの間障害物があるかしらを判断する。
        public bool HasBarrier(Node startNode, Node endNode)
        {
            return HasBarrier(startNode, endNode, null);
        }

        float GetZByRateX(float nodeSize, Vector3 startPos, Vector3 endPos, float xPos)
        {
            float rate = (endPos.x - xPos) / (xPos - startPos.x);
            return (endPos.z + startPos.z * rate) / (1 + rate);
        }

        float GetXByRateZ(float nodeSize, Vector3 startPos, Vector3 endPos, float zPos)
        {
            float rate = (endPos.z - zPos) / (zPos - startPos.z);
            return (endPos.x + startPos.x * rate) / (1 + rate);
        }
    }
}
