using BlueNoah.RPG.PathFinding;
using System.Collections.Generic;
using UnityEngine;
///
/// @file  NodeObtainUtils.cs
/// @author Ying YuGang
/// @date   
/// @brief スキル範囲と移動範囲のノードを獲得。
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

namespace BlueNoah.RPG
{
    public static class NodeObtainUtils
    {
        #region 表示用
        //ヒットマップ表示用
        public static List<Node> ObtainBaseInfluenceNodes(ActorCore unitModel)
        {
            return ObtainMoveableNodesForDisplay(unitModel);
        }
        //ヒットマップ表示用
        public static List<Node> ObtainAddtionalInfluenceNodes(ActorCore unitModel,int minRange,int maxRange)
        {
            return ObtainSkillNodesForDisplay(unitModel, minRange,maxRange);
        }
        //移動範囲ノード表示用
        public static List<Node> ObtainMoveableNodesForDisplay(ActorCore unitModel) {
            List<Node> nodes = unitModel.MoveAgent.GetMoveableNodes();
            var grid = PathFindingManager.Single.Grid;
            grid.SearchIdentity.Increase();
            NodeSearchList nodeSearchList = new NodeSearchList();
            Node node = null;
            for (int i = 0; i < nodes.Count; i++)
            {
                node = nodes[i];
                ObtainUnitNodes(unitModel, node, nodeSearchList);
            }
            return nodeSearchList.List;
        }
        //移動範囲ノード表示用
        public static void ObtainMoveableNodesForDisplay(List<Vector3Int> nodeIndexes, ActorCore unitModel)
        {
            List<Node> displayNodes = ObtainMoveableNodesForDisplay(unitModel);
            for (int i = 0;i<displayNodes.Count;i++)
            {
                nodeIndexes.Add(new Vector3Int { x = displayNodes[i].X, y = 0, z = displayNodes[i].Z });
            }
        }
        #endregion
        //一ターン移動範囲（味方ユニット居るポジションを含まない）
        public static List<Node> ObtainRealMoveableNodes(ActorCore unitModel)
        {
            return unitModel.MoveAgent.GetRealMoveableNodes();
        }
        //直接にグリッド距離計算。(本陣にも対応)
        public static int GetDistance(ActorCore start, ActorCore end)
        {
            RectInt rect1 = start.BattleStatus.GridRect;
            RectInt rect2 = end.BattleStatus.GridRect;
            return GetDistance(rect1, rect2);
        }
        //直接にグリッド距離計算。(本陣にも対応)
        public static int GetDistance(RectInt rect1, RectInt rect2)
        {
            int x = 0;
            if (rect1.xMax - 1 < rect2.xMin)
            {
                x = rect2.xMin - rect1.xMax + 1;
            }
            else if (rect2.xMax - 1 < rect1.xMin)
            {
                x = rect1.xMin - rect2.xMax + 1;
            }

            int y = 0;
            if (rect1.yMax - 1 < rect2.yMin)
            {
                y = rect2.yMin - rect1.yMax + 1;
            }
            else if (rect2.yMax - 1 < rect1.yMin)
            {
                y = rect1.yMin - rect2.yMax + 1;
            }
            return x + y;
        }

        //直接にグリッド距離で範囲判断。(本陣にも対応)
        public static bool IsInRange(ActorCore caster, ActorCore target, int minRange, int maxRange)
        {
            RectInt rect1 = caster.BattleStatus.GridRect;
            RectInt rect2 = target.BattleStatus.GridRect;

            
            return IsInRange(rect1, rect2, minRange, maxRange);
        }
        //直接にグリッド距離で範囲判断。(本陣にも対応)
        public static bool IsInRange(RectInt rect1, RectInt rect2, int minRange, int maxRange)
        {
            int distance = GetDistance(rect1, rect2);
            return minRange <= distance && maxRange >= distance;
        }
        #region スキル範囲用（移動しない）
        //スキル範囲ノード表示用
        public static void ObtainSkillOnlyNodesForDisplay(List<Vector3Int> nodeIndexes, ActorCore unitModel,int minRange, int maxRange)
        {
            List<Node> skillableNodes = ObtainSkillOnlyNodes(unitModel, minRange, maxRange);
            if (skillableNodes != null)
            {
                for (int i = 0; i < skillableNodes.Count; i++)
                {
                    nodeIndexes.Add(new Vector3Int { x = skillableNodes[i].X, y = 0, z = skillableNodes[i].Z });
                }
            }
        }
        //中心まで半径内ノードをゲット。
        public static List<Node> ObtainSkillOnlyNodes(ActorCore unitModel, int minRange, int maxRange)
        {
            return ObtainSkillOnlyNodes(unitModel.GetMainNode(), unitModel.MoveAgent.XSize, unitModel.MoveAgent.ZSize, minRange, maxRange);
        }
        //中心まで半径内ノードをゲット。
        public static List<Node> ObtainSkillOnlyNodes(Node node, int xSize, int zSize, int minRange, int maxRange)
        {
            var gridSeeker = PathFindingManager.Single.Grid.SeekerService;
            return gridSeeker.GetNodesByRange(node, xSize, zSize, minRange, maxRange);
        }
        #endregion

        #region スキル範囲用（移動がある）
        //通常攻撃範囲ノード表示用(移動範囲に基づいて)
        public static void ObtainRegularAttackNodesForDisplay(List<Vector3Int> nodeIndexes, ActorCore unitModel)
        {
            List<Node> skillableNodes = ObtainRegularAttackNodesForDisplay(unitModel);
            if (skillableNodes != null)
            {
                for (int i = 0; i < skillableNodes.Count; i++)
                {
                    nodeIndexes.Add(new Vector3Int { x = skillableNodes[i].X, y = 0, z = skillableNodes[i].Z });
                }
            }
        }
        //通常攻撃範囲ノード表示用(移動範囲に基づいて)
        public static List<Node> ObtainRegularAttackNodesForDisplay(ActorCore unitModel)
        {
            return ObtainSkillNodesForDisplay(unitModel,unitModel.RegularAttack.TargetRangeNear,unitModel.RegularAttack.TargetRangeFar);
        }
        //スキル範囲ノード表示用(移動範囲に基づいて)
        public static List<Node> ObtainSkillNodesForDisplay(ActorCore unitModel,int minRange,int maxRange)
        {
            var grid = PathFindingManager.Single.Grid;
            List<Node> moveableNodes = unitModel.MoveAgent.GetMoveableNodes();
            List<Node> skillableNodes = unitModel.MoveAgent.GetSkillableNodesPlus(moveableNodes, unitModel, minRange, maxRange);
            return skillableNodes;
        }
        #endregion

        #region 移動攻撃専用
        //移動攻撃専用。
        public static Node ObtainMoveToSkillableNode(ActorCore caster, ActorCore target, int minRange, int maxRange)
        {
            List<Node> moveableNodes = caster.MoveAgent.GetRealMoveableNodes();
            RectInt targetRect = new RectInt(target.BattleStatus.MinGridPosition.x, target.BattleStatus.MinGridPosition.z, target.MoveAgent.XSize, target.MoveAgent.ZSize);
            RectInt casterRect;
            for (int i = 0; i < moveableNodes.Count; i++)
            {
                casterRect = new RectInt(moveableNodes[i].X, moveableNodes[i].Z, caster.MoveAgent.XSize, caster.MoveAgent.ZSize);
                if (IsInRange(casterRect, targetRect, minRange, maxRange))
                {
                    return moveableNodes[i];
                }
            }
            return null;
        }
        //移動攻撃専用。
        public static List<Node> ObtainMoveToSkillableNodes(ActorCore caster, ActorCore target, int minRange, int maxRange)
        {
            List<Node> moveableNodes = caster.MoveAgent.GetRealMoveableNodes();
            List<Node> skillableNodes = new List<Node>();
            RectInt targetRect = new RectInt(target.BattleStatus.MinGridPosition.x, target.BattleStatus.MinGridPosition.z, target.MoveAgent.XSize, target.MoveAgent.ZSize);
            RectInt casterRect;
            for (int i = 0; i < moveableNodes.Count; i++)
            {
                casterRect = new RectInt(moveableNodes[i].X, moveableNodes[i].Z, caster.MoveAgent.XSize, caster.MoveAgent.ZSize);
                if (IsInRange(casterRect, targetRect, minRange, maxRange))
                {
                    skillableNodes.Add(moveableNodes[i]);
                }
            }
            return skillableNodes;
        }
        //ユニットのサイズでノードをゲット。
        static void ObtainUnitNodes(ActorCore unitModel, Node targetNode, NodeSearchList nodeSearchList)
        {
            var grid = PathFindingManager.Single.Grid;
            for (int x = targetNode.X; x < targetNode.X + unitModel.MoveAgent.XSize; x++)
            {
                for (int z = targetNode.Z; z < targetNode.Z + unitModel.MoveAgent.ZSize; z++)
                {
                    if (grid.GetNode(x, z) != null)
                    {
                        if (grid.GetNode(x, z).IsOpen != grid.SearchIdentity.Value)
                        {
                            nodeSearchList.Add(grid.GetNode(x, z), grid.SearchIdentity.Value);
                        }
                    }
                }
            }
        }
        #endregion
        /*
        //実際の移動すべき距離を計算（最大攻撃距離と地形の影響も含む）
        public static int GetDistanceWithPathFind(ActorCore start, ActorCore end, int stopDistance)
        {
            if (!start.IsMovable)
            {
                return int.MaxValue;
            }
            else
            {
                List<Node> nodes = PathFindingManager.Single.FindPath(start.BattleStatus.MainGridPosition, end.BattleStatus.MinGridPosition, end, float.MaxValue, stopDistance, start.MoveAgent, start.Battle);
                if (nodes == null || nodes.Count == 0)
                {
                    //理論距離
                    return int.MaxValue;
                }
                else
                {
                    return nodes[nodes.Count - 1].G / GStarGrid.Multiple;
                }
            }
        }*/
    }
}

