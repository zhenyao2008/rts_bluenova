///
/// @file   PathAgent.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

using UnityEngine;
using System.Collections.Generic;

namespace BlueNoah.RPG.PathFinding
{
    //シングルのグリッドを探索用パスエージェント。
    public class PathAgent : PathAgentBase
    {
        public PathAgent(GStarGrid grid):base(grid)
        {
            PathStyleIndex = 0;
        }
        public List<Node> FindRangeNodes(List<Node> sourceNodes, ActorCore unitModel, int minRange,int maxRange)
        {
            if (sourceNodes == null || sourceNodes.Count == 0)
                return null;

            SearchIdentity.Increase();

            NodeSearchList nodeSearchList = new NodeSearchList();

            for (int i = 0; i < sourceNodes.Count; i++)
            {
                if (sourceNodes[i].HaltReserveAgent == null || sourceNodes[i].HaltReserveAgent == unitModel.MoveAgent)
                {
                    Grid.SeekerService.GetNodesByRangeSimple(nodeSearchList, sourceNodes[i], unitModel.MoveAgent.XSize, unitModel.MoveAgent.ZSize, minRange, maxRange, SearchIdentity.Value);
                }
            }
            return nodeSearchList.List;
        }

        //現地とmaxCost距離範囲内のノードを獲得。
        public List<Node> FindNodes(Node mainNode,int maxCost,GStarMoveAgentBase moveAgent)
        {
            maxCost = maxCost * GStarGrid.Multiple;

            SearchIdentity ++;

            NodeSearchList nodeSearchList = new NodeSearchList();

#if UNITY_EDITOR
            CloseList.Clear();
#endif
            mainNode.G = 0;

            nodeSearchList.Add(mainNode,SearchIdentity);

            Node neighbor = null;

            while (!nodeSearchList.IsEmpty)
            {
                //リスト中にF値一番小さいのノード
                Node node = nodeSearchList.Extract();

                AddToCloseList(node);

                for (int i = node.Neighbors.Count - 1; i >= 0; i--)
                {
                    neighbor = node.Neighbors[i];

                    if (IsInClose(neighbor))
                    {
                        continue;
                    }

                    if (!IsMaskAvailable(neighbor, moveAgent.GridLayerMask, moveAgent.SubGridLayerMask))
                    {
                        continue;
                    }
                    //ドーアの専用処理。
                    if (neighbor.IsDoor)
                    {
                        continue;
                    }
                    //柵などの処理。
                    if (neighbor.IsViolableObstacle && !moveAgent.IsFlyingUnit)
                    {
                        continue;
                    }
                    
                    //ビッグユニット場合。
                    if (!IsNeighborOffsetsAvailable(neighbor, node, node.NeighborCosts[i], moveAgent , moveAgent.GridLayerMask, moveAgent.SubGridLayerMask))
                    {
                        continue;
                    }

                    //重要、もしMultiplyCostが0になったら、経路が変になる、そして、必ずデフォルト値を設定して，
                    int neighborCost = node.NeighborCosts[i];

                    int G = node.G  + neighborCost;
                 
                    /*
                    if (PathStyleIndex % 3 == 1 && IsApplyRandomPath)
                    {
                        if (node.X != TargetNode.X && node.Z != TargetNode.Z)
                        {
                            //曲げる程度を調整
                            G += PathAgentUtility.CalculateDirect(PathType, node, neighbor);
                        }
                    }*/
                    if (G <= maxCost)
                    {
                        if (neighbor.G > G || neighbor.IsOpen != SearchIdentity)
                        {
                            neighbor.G = G;

                            nodeSearchList.Add(neighbor, SearchIdentity);
                        }
                    }
                }
            }
            List<Node> result = new List<Node>();
            result.AddRange(nodeSearchList.List);
            return result;
        }
        /// <summary>
        /// 特別処理：検索した移動範囲ノード、もし敵がいる場合削除、もし全ての隣のノードに敵がいると削除。
        /// </summary>
        /// <param name="moveableNodes">検索した移動範囲ノー</param>
        /// <param name="unit">ユニット自体</param>
        /// <returns>フィルターした移動範囲</returns>
        public List<Node> FilterNodeByGStar(List<Node> moveableNodes,ActorCore unit)
        {

            NodeSearchList nodeSearchListFiltered = new NodeSearchList();

            PlusAgentIndex();

            int preAgentIndex = unit.MoveAgent.MainNode.IsOpen;

            nodeSearchListFiltered.Add(unit.MoveAgent.MainNode, SearchIdentity);

            Node neighbor = null;

            bool isOffsetAvailable = true;

            Node offsetNode = null;

            Node node = null;

            while (!nodeSearchListFiltered.IsEmpty)
            {
                node = nodeSearchListFiltered.Extract();
                for (int i = node.Neighbors.Count - 1; i >= 0; i--)
                {
                    isOffsetAvailable = true;

                    neighbor = node.Neighbors[i];
                    for (int x = neighbor.X; x < neighbor.X + unit.MoveAgent.XSize; x++)
                    {
                        for (int z = neighbor.Z; z < neighbor.Z + unit.MoveAgent.ZSize; z++)
                        {
                            offsetNode = Grid.GetNode(x, z);
                            if (offsetNode == null) { continue; }

                            if (offsetNode.HaltReserveAgent != null &&  offsetNode.HaltReserveAgent.UnitModel.TeamId != unit.TeamId)
                            {
                                isOffsetAvailable = false;
                            }
                        }
                    }

                    if (!isOffsetAvailable)
                    {
                        neighbor.IsOpen = SearchIdentity;
                        continue;
                    }

                    if (neighbor.IsOpen == preAgentIndex)
                    {
                        if (neighbor.HaltReserveAgent == null || neighbor.HaltReserveAgent.UnitModel.TeamId == unit.TeamId)
                        {
                            nodeSearchListFiltered.Add(neighbor, SearchIdentity);
                        }
                    }
                }
            }
            return nodeSearchListFiltered.List;
        }

        public List<Node> StartFind(Vector3Int startPos,Vector3Int endPos, ActorCore targetUnit, float maxCost,int stopDistance,GStarMoveAgentBase moveAgent)
        {
            Node startNode = Grid.GetNode(startPos);
            Node endNode = Grid.GetNode(endPos);
            return StartFind(startNode,endNode, targetUnit, maxCost, stopDistance, moveAgent);
        }

        //経路を計算
        public List<Node> StartFind(Node startNode, Node targetNode,ActorCore targetUnit, float maxG, int stopDistance, GStarMoveAgentBase moveAgent)
        {

            maxG = maxG * GStarGrid.Multiple;

            stopDistance = stopDistance * GStarGrid.Multiple;

            if (startNode == null || targetNode == null)
            {
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log(string.Format("<color=green>startNode:{0};targetNode:{1}</color>", startNode, targetNode));

                return null;
            }

            if (startNode == targetNode)
            {
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log(string.Format("<color=green>startNode:{0} equals targetNode:{1}</color>", startNode, targetNode));

                return null;
            }
            if (startNode.IsBlock || !moveAgent.GridLayerMask.ContainLayer(1 << startNode.LayerMask))
            {
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log(string.Format("<color=green>startNode:{0};targetNode:{1}</color>", startNode, targetNode));

                return null;
            }

            if (startNode.AreaId != targetNode.AreaId)
            {
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log(string.Format("<color=green>startNode:{0};targetNode:{1}</color>", startNode, targetNode));

            }
            //数学でノード毎にを基づいて、ゴールへ通えるかどうか判断する。
            /*
            if (!CheckBarrier(startNode, targetNode,mask))
            {
                List<Node> result = new List<Node>();
                result.Add(startNode);
                result.Add(targetNode);
                return result;
            }*/

            PlusAgentIndex();

            CurrentIndex = 0;

            bool searched = false;

            ClearOpen();
#if UNITY_EDITOR
            CloseList.Clear();
#endif
            //Clear the G value for first node.
            startNode.G = 0;

            startNode.H = GetH(startNode, targetNode);

            startNode.F = startNode.G + startNode.H;

            startNode.Previous = null;

            AddToOpen(startNode);

            Node neighbor = null;

            while (!IsOpenEmpty() && !searched)
            {
                //リスト中にF値一番小さいのノード
                Node node = ExtractOpenNode();
                if (stopDistance > 0 && node.HaltReserveAgent == null)
                {
                    if (targetUnit!=null)
                    {
                        int distance = NodeObtainUtils.GetDistance(new RectInt(node.X,node.Z,1,1),targetUnit.BattleStatus.GridRect) * GStarGrid.Multiple;
                        if (distance <= stopDistance)
                        {
                            searched = true;
                            targetNode = node;
                        }
                    }
                    else
                    {
                        if (GetH(targetNode, node) <= stopDistance)
                        {
                            searched = true;
                            targetNode = node;
                        }
                    }
                }

                if (node != targetNode)// || targetNode.HaltReserveAgent != null)
                {
                    AddToCloseList(node);

                    for (int i = node.Neighbors.Count - 1; i >= 0; i--)
                    {
                        neighbor = node.Neighbors[i];

                        if (IsInClose(neighbor))
                        {
                            continue;
                        }

                        if (!IsMaskAvailable(neighbor,moveAgent.GridLayerMask, moveAgent.SubGridLayerMask))
                        {
                            continue;
                        }
                        //ドーアの専用処理。
                        if (neighbor.IsDoor)
                        {
                            continue;
                        }
                        //柵などの処理。
                        if (neighbor.IsViolableObstacle && !moveAgent.IsFlyingUnit)
                        {
                            continue;
                        }

                        /*
                        //ビッグユニット場合。
                        if (!IsNeighborOffsetsAvailable(neighbor, node, node.NeighborCosts[i], stepInfo, moveAgent.GridLayerMask, moveAgent.SubGridLayerMask))
                        {
                            continue;
                        }
                        */

                        //重要、もしMultiplyCostが0になったら、経路が変になる、そして、必ずデフォルト値を設定して，
                        int neighborCost = node.NeighborCosts[i];// * neighbor.MultiplyCost + neighbor.AdditionCost;

                        /*
                        #region ドーアと柵の処理。
                        //ドーアの専用処理。
                        if (neighbor.IsDoor)
                        {
                            neighborCost += neighbor.Gimmick.ObstacleCost;
                        }
                        //柵などの処理。
                        if (neighbor.IsViolableObstacle && !IsFly)
                        {
                            neighborCost += neighbor.Gimmick.ObstacleCost;
                        }
                        #endregion
                        */

                        int G = node.G + neighborCost;
                        //if(!moveAgent.IsFlyingUnit)
                        //    G -= (int)(GStarGrid.Multiple * battleModel.GetFieldSpeedBonus(neighbor.Pos, (neighbor.Pos - node.Pos).normalized));

                        /*
                        // if (StartPathIndex % 2 == 1)
                        if (PathStyleIndex % 3 == 1 && IsApplyRandomPath)
                        {
                            if (node.X != targetNode.X && node.Z != targetNode.Z)
                            {
                                //曲げる程度を調整
                                G += PathAgentUtility.CalculateDirect(PathType, node, neighbor);
                            }
                        }
                        */

                        int H = GetH(neighbor, targetNode);

                        int F = G + H;

                        if (G <= maxG && (neighbor.F > F || neighbor.IsOpen != SearchIdentity))
                        {

                            neighbor.G = G;
                            neighbor.H = H;
                            neighbor.F = F;
                            neighbor.Previous = node;

                            if (neighbor.IsOpen == SearchIdentity)
                            {
                                AlternInOpen(neighbor);
                            }
                            else
                            {
                                AddToOpen(neighbor);
                            }
                        }
                    }
                }
                else
                {
                    searched = true;
                }
            }
            List<Node> resultPath = new List<Node>();
            if (searched)
            {
                resultPath = GetMovePath(targetNode);
            }
            else
            {
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log("target can't be reached!");
            }
            return resultPath;
        }

        bool IsInClose(Node node)
        {
            return node.IsClose == SearchIdentity;
        }

        bool IsMaskAvailable(Node node,GridLayerMask mask,GridLayerMask subMask)
        {
            return GStarGrid.IsNodeValid(node, mask, subMask);
        }
        //4 Edges
        protected override int GetH(Node node, Node targetNode)
        {
            int x = Mathf.Abs(targetNode.X - node.X);
            int z = Mathf.Abs(targetNode.Z - node.Z);
            return (x + z) * GStarGrid.Multiple;
        }

        bool IsNeighborOffsetsAvailable(Node neighbor, Node origion, float neighborCost, GStarMoveAgentBase moveAgent, GridLayerMask mask, GridLayerMask subMask)
        {
            for (int xOffset = 0; xOffset < moveAgent.XSize; xOffset++)
            {
                for (int zOffset = 0; zOffset < moveAgent.ZSize; zOffset++)
                {
                    if (!moveAgent.IsFlyingUnit)
                    {
                        if (!IsNeighborOffsetAvailable(Grid.GetNode(neighbor.X + xOffset, neighbor.Z + zOffset), origion, mask, subMask))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!GStarGrid.IsNodeValid(Grid.GetNode(neighbor.X + xOffset, neighbor.Z + zOffset)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        bool IsNeighborOffsetAvailable(Node offsetNode, Node origin,  GridLayerMask mask, GridLayerMask subMask)
        {
            if (offsetNode == null)
            {
                return false;
            }
            if (!GStarGrid.IsNodeValid(offsetNode, mask, subMask))
            {
                return false;
            }
            return true;
        }

#if UNITY_EDITOR
        public bool showGizmos = true;
        public bool showHandle = false;
        float mDebugSize = 0.5f;
#endif
        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.color = Color.yellow;
            for (int i = 0; i < CloseList.Count; i++)
            {
                if (CloseList[i].F > 0 && showHandle)
                {
                    UnityEditor.Handles.Label(CloseList[i].Pos, CloseList[i].F.ToString());
                    if (CloseList[i].Previous != null)
                    {
                        Quaternion rotation = Quaternion.LookRotation((CloseList[i].Previous.Pos - CloseList[i].Pos).normalized, Vector3.up);
                        UnityEditor.Handles.ArrowHandleCap(0, CloseList[i].Pos, rotation, 0.5f, EventType.Repaint);
                    }
                }
                Gizmos.DrawCube(CloseList[i].Pos, Vector3.one * Grid.EdgeLength * mDebugSize);
            }
            Gizmos.color = Color.red;
            for (int i = CurrentIndex; i < OpenList.Count; i++)
            {
                if (OpenList[i].F > 0 && showHandle)
                {
                    UnityEditor.Handles.Label(OpenList[i].Pos, OpenList[i].F.ToString());

                    if (OpenList[i].Previous != null)
                    {
                        Quaternion rotation = Quaternion.LookRotation((OpenList[i].Previous.Pos - OpenList[i].Pos).normalized, Vector3.up);
                        UnityEditor.Handles.ArrowHandleCap(0, OpenList[i].Pos, rotation, 0.5f, EventType.Repaint);
                    }
                }
                Gizmos.DrawCube(OpenList[i].Pos, Vector3.one * Grid.EdgeLength * mDebugSize);
            }
            /*
             * resultPath は各パス自らデバッグ方がいい。一旦コメントしてしまう。
            for (int i = 0; i < resultPath.Count; i++)
            {
                float sin = Mathf.Sin(Time.time * mColorSpeed + i * mColorPlus);
                float cos = Mathf.Cos(Time.time * mColorSpeed + i * mColorPlus);
                Gizmos.color = new Color(cos, sin, cos, 1);
                if (resultPath[i].F > 0 && showHandle)
                    UnityEditor.Handles.Label(resultPath[i].Pos, resultPath[i].F.ToString());
                Gizmos.DrawCube(resultPath[i].Pos, Vector3.one * Grid.EdgeLength * mDebugSize);
            }
            */
#endif
        }


    }

}
