using System.Collections.Generic;
using UnityEngine;
///
/// @file  EightNeighborSeeker.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class EightNeighborSeeker: BaseSeeker
    {

        public EightNeighborSeeker(GStarGrid grid) : base(grid){}

        //このポジションと一番近いAvailableノードを探す。
        public Node[] GetNearestAvailableNodes(Vector3Int position,int xSize,int zSize,GridLayerMask mask)
        {
            Node startNode = Grid.GetNode(position);
            if (startNode == null)
            {
                return null;
            }

            NodeSearchIdentity.Increase();
            List<Node> openList = new List<Node>(1000);
            openList.Add(startNode);
            startNode.NodeSearchIndex = NodeSearchIdentity.Value;
            int j = 0;
            Node currentNode = null;
            Node neighborNode = null;
            Node[] nodes = new Node[xSize*zSize];
            int n = 0;
            bool available = false;
            while (openList.Count > j)
            {
                currentNode = openList[j];

                Grid.GetNodes(currentNode,xSize,zSize,ref nodes);

                n = 0;
                available = true;
                for (; n < nodes.Length ; n++)
                {
                    //ここで判断条件が変更できる
                    if (nodes[n] == null || nodes[n].IsBlock || nodes[n].IsViolableObstacle || nodes[n].IsDoor || nodes[n].HaltReserveAgent != null || !mask.ContainLayer(1 << nodes[n].LayerMask))
                    {
                        available = false;
                        break;
                    }
                }

                if (available)
                {
                    return nodes;
                }
                
                for (int i = 0; i < currentNode.Neighbors.Count; i++)
                {
                    neighborNode = currentNode.Neighbors[i];
                    if (neighborNode.NodeSearchIndex != NodeSearchIdentity.Value)
                    {
                        openList.Add(neighborNode);
                        neighborNode.NodeSearchIndex = NodeSearchIdentity.Value;
                    }
                }
                j++;
            }
            return null;
        }
        
        public override List<Node> GetNodesByRange(Vector3Int startPos,int xSize,int zSize, int range)
        {
            throw new System.NotImplementedException();
        }
        //ユニットのサイズに基づいて周囲一マスのノードをゲット。
        public override List<Node> GetNeighborhoods(Node sourceNode, int xSize, int zSize)
        {
            List<Node> nodes = new List<Node>();
            Node node;
            int minX = sourceNode.X - 1;
            int minZ = sourceNode.Z - 1;
            int maxX = sourceNode.X + xSize + 1;
            int maxZ = sourceNode.Z + zSize + 1;
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minZ; j < maxZ; j++)
                {
                    if (i == minX || j == minZ || i == maxX - 1 || j == maxZ - 1)
                    {
                        node = Grid.GetNode(i, j);
                        if (node != null)
                        {
                            nodes.Add(node);
                        }
                    }
                }
            }
            return nodes;
        }
        //Bounds範囲内ノードを獲得。
        /*
        public List<Node> GetNodes(Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Node nodeMin = Grid.GetNode(min);
            Node nodeMax = Grid.GetNode(max);
            List<Node> nodes = new List<Node>();
            for (int i = nodeMin.X; i <= nodeMax.X; i++)
            {
                for (int j = nodeMin.Z; j <= nodeMax.Z; j++)
                {
                    nodes.Add(Grid.GetNode(i, j));
                }
            }
            return nodes;
        }
        */

        #region For test

        bool GetClickPosition(out RaycastHit raycastHit)
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition.z = 10;

            Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 forward = (position - Camera.main.transform.position).normalized;

            return Physics.Raycast(Camera.main.transform.position, forward, out raycastHit, Mathf.Infinity, 1 << 8);
        }

        //例：TestGetNearestAvailable(2,3,NodeLayer.Land);
        /*
        public void TestGetNearestAvailable(int xSize , int zSize, int layer)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    GridLayerMask gridLayerMask = new GridLayerMask();
                    //複数レーヤーができる
                    gridLayerMask.AddLayer((uint)layer);

                    Node[] nodes = GetNearestAvailableNodes(raycastHit.point, xSize, zSize, gridLayerMask);

                    Grid.ColorChangerService.ChangeNodeGroupColors(0,nodes,Color.green);

                    Grid.ColorChangerService.ApplyColors();
                }
            }
        }
        */

        /*
        public void TestGetNodesByRange()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    Range += Grid.EdgeLength / 2;

                    Node[] nodes = GetNodesByRange(raycastHit.point,1,1, Range).ToArray();

                    Grid.ColorChangerService.ChangeNodeGroupColors(0, nodes, Color.green);

                    Grid.ColorChangerService.ApplyColors();
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    Range -= Grid.EdgeLength / 2;

                    Node[] nodes = GetNodesByRange(raycastHit.point,1,1, Range).ToArray();

                    Grid.ColorChangerService.ChangeNodeGroupColors(0, nodes, Color.green);

                    Grid.ColorChangerService.ApplyColors();
                }
            }
        }
        */
        /*
        [System.Obsolete]
        public void TestGetNodesByRange1()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    Range += Grid.EdgeLength;

                    Node[] nodes = GetNodesByRange1(raycastHit.point, 1, 1, Range).ToArray();

                    Grid.GStarGridColorChanger.ChangeNodeGroupColors(0, nodes, Color.green);

                    Grid.GStarGridColorChanger.ApplyColors();
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    Range -= Grid.EdgeLength;

                    Node[] nodes = GetNodesByRange1(raycastHit.point, 1, 1, Range).ToArray();

                    Grid.GStarGridColorChanger.ChangeNodeGroupColors(0, nodes, Color.green);

                    Grid.GStarGridColorChanger.ApplyColors();
                }
            }
        }
        */
        /*
        public void TestGetMeleeNodes()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit;

                if (GetClickPosition(out raycastHit))
                {
                    Node node = Grid.GetNode(raycastHit.point);

                    int xSize = UnityEngine.Random.Range(1,6);

                    int zSize = UnityEngine.Random.Range(1,6);

                    Debug.Log(string.Format("{0};{1}",xSize,zSize));

                    Node[] nodes = GetNeighborhoods(node, xSize, zSize).ToArray();

                    Grid.ColorChangerService.ChangeNodeGroupColors(0, nodes, Color.green);

                    Grid.ColorChangerService.ApplyColors();
                }
            }
        }
        */
        public override List<Node> GetNodesByRange(Node startNode, int xSize, int zSize, int minRange, int maxRange)
        {
            throw new System.NotImplementedException();
        }

        public override List<Node> GetNodesByRange(Vector3Int startPos, int xSize, int zSize, int minRange, int maxRange)
        {
            throw new System.NotImplementedException();
        }

        public override void GetNodesByRangeSimple(NodeSearchList impactNodes, Node startNode, int xSize, int zSize, int minRange, int maxRange, int searchIndex)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        /*
        //グリッド数で範囲内マスを探す、一旦残る,TODO_AI（ビッグユニットだたっら、修正が必要）。
        [System.Obsolete]
        public List<Node> GetNodesByRangeInt(Node startNode, int xSize, int zSize, int range,bool straight = true)
        {
            NodeSearchIdentity.Increase();
            float currentSquareRange = 0;
            Node currentNode = null;
            Node neighborNode = null;
            List<Node> openList = new List<Node>(1000);
            openList.Add(startNode);
            startNode.NodeSearchIndex = NodeSearchIdentity.Value;

            int xMin = startNode.X;
            int xMax = startNode.X + xSize - 1;
            int zMin = startNode.Z;
            int zMax = startNode.Z + zSize - 1;
            
            int j = 0;
            while (openList.Count > j)
            {
                currentNode = openList[j];
                for (int i = 0; i < currentNode.Neighbors.Count; i++)
                {
                    neighborNode = currentNode.Neighbors[i];
                    if (neighborNode.NodeSearchIndex != NodeSearchIdentity.Value)
                    {
                        if (straight)
                        {
                            if (!(neighborNode.X >= xMin && neighborNode.X <= xMax) && !(neighborNode.Z >= zMin && neighborNode.Z <= zMax))
                            {
                                continue;
                            }
                        }
                        int x = 0,z = 0;
                        if (neighborNode.X <= xMin)
                        {
                            x = xMin - neighborNode.X;
                        }
                        else if (neighborNode.X >= xMax)
                        {
                            x = neighborNode.X - xMax;
                        }

                        if (neighborNode.Z <= zMin)
                        {
                            z = zMin - neighborNode.Z;
                        }
                        else if (neighborNode.Z >= zMax)
                        {
                            z = neighborNode.Z - zMax;
                        }
                        currentSquareRange = x + z;
                        if (currentSquareRange <= range - xSize / 2)
                        {
                            openList.Add(neighborNode);
                        }
                        neighborNode.NodeSearchIndex = NodeSearchIdentity.Value;
                    }
                }
                j++;
            }
            return openList;
        }

        //方法２、一旦残る,TODO_AI 
        [System.Obsolete]
        public List<Node> GetNodesByRange1(Vector3Int center, int xSize, int zSize, float range)
        {
            List<Node> nodes = new List<Node>();

            Node startNode = Grid.GetNode(center);

            center = startNode.Pos + Grid.GetOffset(xSize, zSize);

            //整数の範囲をカント。
            int totalRange = Mathf.RoundToInt(range / Grid.EdgeLength);

            int minX = Mathf.Max(0, startNode.X - totalRange - 1);

            int maxX = Mathf.Min(startNode.X + totalRange + xSize + 1, Grid.XCount );

            int minZ = Mathf.Max(0, startNode.Z - totalRange - 1);

            int maxZ = Mathf.Min(startNode.Z + totalRange + zSize + 1, Grid.ZCount);

            float squareTotalRange = range * range;

            for (int i0 = minX; i0 < maxX; i0++)
            {
                for (int j0 = minZ; j0 < maxZ; j0++)
                {
                    Node nodeInRange = Grid.GetNode(i0, j0);

                    float squareRange = BattleUtils.SquareMagnitudeXZ(nodeInRange.Pos - startNode.Pos);

                    if (squareRange <= squareTotalRange)
                    {
                        nodes.Add(nodeInRange);
                    }
                }
            }
            if (nodes.Count == 0)
            {
                nodes.Add(startNode);
            }
            return nodes;
        }
        */
    }
}
