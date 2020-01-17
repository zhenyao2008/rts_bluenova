using System.Collections.Generic;
using UnityEngine;
///
/// @file  FourNeighborBarrier.cs
/// @author Ying YuGang
/// @date   
/// @brief　
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class FourNeighborBarrier : BaseBarrier
    {
        public FourNeighborBarrier(GStarGrid grid) : base(grid) { }

        public override List<Vector3> SmoothPath(List<Node> nodes, GridLayerMask mask)
        {
            List<Vector3> pathPosList = new List<Vector3>();
            if (nodes==null || nodes.Count < 2)
            {
                return pathPosList;
            }
            if (nodes.Count <= 2)
            {
                pathPosList.Add(nodes[0].Pos);
                pathPosList.Add(nodes[1].Pos);
                return pathPosList;
            }
            List<Node> filter = new List<Node>();
            if (!HasBarrier(nodes[0], nodes[nodes.Count - 1], mask))
            {
                pathPosList.Add(nodes[0].Pos);
                pathPosList.Add(nodes[nodes.Count - 1].Pos);
                return pathPosList;
            }
            //コーナーを獲得。
            List<int> corners = new List<int>();
            int direct = nodes[1].X != nodes[0].X ? 1 : 0;
            for (int i = 1; i < nodes.Count - 1; i++)
            {
                int currentDirect = 0;
                if (nodes[i].X != nodes[i + 1].X)
                {
                    currentDirect = 1;
                }
                if (currentDirect != direct)
                {
                    direct = currentDirect;
                    corners.Add(i);
                }
            }

            for (int i=0;i<nodes.Count;i++)
            {
                pathPosList.Add(nodes[i].Pos);
            }

            for (int i = 0; i < corners.Count; i++)
            {

                int index = corners[i] + i * 2;

                Vector3 startPos = pathPosList[index - 1];

                Vector3 middlePos = pathPosList[index];

                Vector3 endPos = pathPosList[index + 1];

                pathPosList.RemoveAt(index);

                pathPosList.Insert(index, BezierCurveUtility.GetPosition(startPos, middlePos, endPos,0.25f));

                pathPosList.Insert(index + 1, BezierCurveUtility.GetPosition(startPos, middlePos, endPos, 0.5f));

                pathPosList.Insert(index + 2, BezierCurveUtility.GetPosition(startPos, middlePos, endPos, 0.75f));
            }
            return pathPosList;
        }
        [System.Obsolete]
        public List<Node> FilterPath1(List<Node> nodes, GridLayerMask mask)
        {
            List<Node> filters = new List<Node>();

            if (nodes == null || nodes.Count <2)
            {
                return nodes;
            }

            HashSet<Node> filterSet = new HashSet<Node>();

            Node preNode = nodes[0];
            filterSet.Add(preNode);
            //forward direction.
            for (int i = 2 ; i < nodes.Count - 1 ; i++)
            {
                Node currentNode = nodes[i];
                if (HasBarrier(preNode,currentNode,mask))
                {
                    filterSet.Add(currentNode);
                    preNode = currentNode;
                }
            }
            filterSet.Add(nodes[nodes.Count - 1]);

            preNode = nodes[nodes.Count - 1];
            filterSet.Add(preNode);
            //backward direction.
            for (int i = nodes.Count - 1; i > 0 ; i--)
            {
                Node currentNode = nodes[i];
                if (HasBarrier(preNode, currentNode, mask))
                {
                    filterSet.Add(currentNode);
                    preNode = currentNode;
                }
            }

            for (int i = 0 ; i < nodes.Count ; i++)
            {
                if (filterSet.Contains(nodes[i]))
                {
                    filters.Add(nodes[i]);
                }
            }
            return filters;
        }

        public override List<Node> FilterPath(List<Node> nodes, GridLayerMask mask)
        {
            if (nodes.Count <= 2)
            {
                return nodes;
            }
            List<Node> filter = new List<Node>();
            if (!HasBarrier(nodes[0], nodes[nodes.Count - 1], mask))
            {
                filter.Add(nodes[0]);
                filter.Add(nodes[nodes.Count - 1]);
                return filter;
            }
            List<int> corners = new List<int>();
            int direct = nodes[1].X != nodes[0].X ? 1 : 0;
            for (int i = 1 ; i < nodes.Count -1 ; i++)
            {
                int currentDirect = 0;
                if (nodes[i].X != nodes[i+1].X)
                {
                    currentDirect = 1;
                }
                if (currentDirect != direct )
                {
                    direct = currentDirect;

                    corners.Add(i);
                    /*
                    int cornerIndex = 1;

                    while (true)
                    {
                        cornerIndex ++;

                        if (i - cornerIndex >=0 && i + cornerIndex < nodes.Count && !HasBarrier(nodes[i - cornerIndex],nodes[i + cornerIndex],mask))
                        {
                            corners.Add(i - cornerIndex + 1);
                            corners.Add(i + cornerIndex - 1);
                        }
                        else
                        {
                            break;
                        }
                    }*/
                }
            }

            /*
            List<Node> results = new List<Node>();
            for (int i = 0 ; i<nodes.Count ; i++)
            {
                if (!corners.Contains(i))
                {
                    results.Add(nodes[i]);
                }
            }
            return results;
            */

            for (int i = 0;i < corners.Count ;i ++)
            {
                nodes.RemoveAt(corners[i] - i);
            }
            return nodes;
        }
       
        public override bool HasBarrier(Node startNode, Node endNode, GridLayerMask gridLayerMask)
        {
            int distX = Mathf.Abs(endNode.X - startNode.X);
            int distZ = Mathf.Abs(endNode.Z - startNode.Z);
            bool loopDirection = distX > distZ;
            Node checkStartNode = null;
            Node checkEndNode = null;
            if (loopDirection)
            {
                checkStartNode = startNode.X < endNode.X ? startNode : endNode;

                checkEndNode = startNode.X < endNode.X ? endNode : startNode;

                startNode = checkStartNode;

                endNode = checkEndNode;

                for (int x = startNode.X; x <= endNode.X; x += 1)
                {
                    float z = Lerp(x, startNode.X, endNode.X, startNode.Z, endNode.Z);
                    
                    Node node = Grid.GetNode(new Vector3Int(x, 0, Mathf.RoundToInt(z)));

                    if (gridLayerMask != null && !GStarGrid.IsNodeValid(node, gridLayerMask))
                    {
                        return true;
                    }
                }
            }
            else
            {
                checkStartNode = startNode.Z < endNode.Z ? startNode : endNode;

                checkEndNode = startNode.Z < endNode.Z ? endNode : startNode;

                startNode = checkStartNode;

                endNode = checkEndNode;

                for (int z = startNode.Z; z <= endNode.Z; z += 1)
                {
                    float x = Lerp(z, startNode.Z, endNode.Z, startNode.X, endNode.X);
                    
                    Node node = Grid.GetNode(new Vector3Int(Mathf.RoundToInt(x), 0, z));

                    if (gridLayerMask != null && !GStarGrid.IsNodeValid(node, gridLayerMask))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override List<Node> GetNodes(Node startNode, Node endNode)
        {
            return new List<Node>();
        }

        /*
        //間のノードの全て検証。Test用
        public override List<Node> GetNodes(Node startNode, Node endNode)
        {
            List<Node> nodes = new List<Node>();

            float distX = Mathf.Abs(endNode.Pos.x - startNode.Pos.x);

            float distZ = Mathf.Abs(endNode.Pos.z - startNode.Pos.z);

            bool loopDirection = distX > distZ;

            float defaultSlope = (endNode.Pos.z - startNode.Pos.z) / (endNode.Pos.x - startNode.Pos.x);

            Node checkStartNode = null;

            Node checkEndNode = null;

            if (loopDirection)
            {
                
                //横の方向
                checkStartNode = startNode.Pos.x < endNode.Pos.x ? startNode : endNode;

                checkEndNode = startNode.Pos.x < endNode.Pos.x ? endNode : startNode;

                startNode = checkStartNode;

                endNode = checkEndNode;

                defaultSlope = (endNode.Pos.z - startNode.Pos.z) / (endNode.Pos.x - startNode.Pos.x);

                for (float xPos = startNode.Pos.x; xPos <= endNode.Pos.x; xPos += Grid.EdgeLength)
                {
                    float z = Lerp(xPos, startNode.Pos.x, endNode.Pos.x, startNode.Pos.z, endNode.Pos.z);

                    float y = Lerp(xPos, startNode.Pos.x, endNode.Pos.x, startNode.Pos.y, endNode.Pos.y);

                    Node node = Grid.GetNode(new Vector3(xPos, 0, z));

                    nodes.Add(node);

                    //途中通過したノードを獲得。
                    if (preNode != null)
                    {
                        float slope = (node.Pos.z - preNode.Pos.z) / (node.Pos.x - preNode.Pos.x);
                        if (slope > defaultSlope)
                        {
                            nodes.Add(Grid.GetNode(node.X, node.Z - 1));
                        }
                    }

                    preNode = node;
                }
            }
            else
            {
                //従の方向
                checkStartNode = startNode.Pos.z < endNode.Pos.z ? startNode : endNode;

                checkEndNode = startNode.Pos.z < endNode.Pos.z ? endNode : startNode;

                startNode = checkStartNode;

                endNode = checkEndNode;

                defaultSlope = Mathf.Abs((endNode.Pos.z - startNode.Pos.z) / (endNode.Pos.x - startNode.Pos.x));

                for (float zPos = startNode.Pos.z; zPos <= endNode.Pos.z; zPos += Grid.EdgeLength)
                {

                    float x = Lerp(zPos, startNode.Pos.z, endNode.Pos.z, startNode.Pos.x, endNode.Pos.x);

                    float y = Lerp(zPos, startNode.Pos.z, endNode.Pos.z, startNode.Pos.y, endNode.Pos.y);

                    Node node = Grid.GetNode(new Vector3(x, 0, zPos));

                    nodes.Add(node);

                    //途中通過したノードを獲得。
                    if (preNode != null)
                    {
                        float slope = Mathf.Abs((node.Pos.z - preNode.Pos.z) / (node.Pos.x - preNode.Pos.x));
                        if (slope < defaultSlope)
                        {
                            nodes.Add(Grid.GetNode(node.X - 1, node.Z));
                        }
                    }

                    preNode = node;
                }
            }
            return nodes;
        }
        */

        float GetRate(float v, float start, float end)
        {
            return Mathf.Clamp((v - start) / (end - start), 0f, 1f);
        }

        float Lerp(float valueSource, float startSource, float endSource, float startDest, float endDest)
        {
            float rate = GetRate(valueSource, startSource, endSource);
            return Mathf.Lerp(startDest, endDest, rate);
        }

        float GetZByXRate(Vector3 startPos, Vector3 endPos, float rate)
        {
            return startPos.z + (endPos.z - startPos.z) * rate;
        }

        float GetYByXRate(Vector3 startPos, Vector3 endPos, float rate)
        {
            return startPos.y + (endPos.y - startPos.y) * rate;
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

        float GetYByRateX(float nodeSize, Vector3 startPos, Vector3 endPos, float xPos)
        {
            float rate = (endPos.x - xPos) / (xPos - startPos.x);
            return (endPos.y + startPos.y * rate) / (1 + rate);
        }

        float GetYByRateZ(float nodeSize, Vector3 startPos, Vector3 endPos, float zPos)
        {
            float rate = (endPos.z - zPos) / (zPos - startPos.z);
            return (endPos.y + startPos.y * rate) / (1 + rate);
        }

        //Node PreNode;

        /*
        public void TestHasBarrier()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    if (PreNode == null)
                    {
                        PreNode = Grid.GetNode(Vector3Int.zero);
                    }
                    Node endNode = Grid.GetNode(raycastHit.point);
                    List<Node> nodes = GetNodes(PreNode, endNode);
                    foreach (Node node in nodes)
                    {
                        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        go.transform.position = node.Pos;
                        GameObject.Destroy(go, 5);
                    }
                    PreNode = endNode;
                }
            }
        }
        */
        
        bool GetClickPosition(out RaycastHit raycastHit)
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition.z = 10;

            Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 forward = (position - Camera.main.transform.position).normalized;

            return Physics.Raycast(Camera.main.transform.position, forward, out raycastHit, Mathf.Infinity, 1 << 8);
        }

    }
}
