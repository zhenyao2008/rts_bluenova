using System.Collections.Generic;
using UnityEngine;
///
/// @file  SkillTargetNodeDisplayUtility.cs
/// @author Ying YuGang
/// @date   
/// @brief スキルターゲット表示周り（ディバグ用）。
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public static class SkillTargetNodeDisplayUtility
    {

        //表示されてるグリッド全てキャンセル。
        public static void ClearSkillTargets()
        {
            GStarGrid grid = PathFindingManager.Single.Grid;
            grid.ColorChangerService.ClearAllGroupColors();
            grid.ColorChangerService.ApplyColors();
        }
        //移動範囲
        public static void ShowMoveableNodes(ActorCore unitModel)
        {
            List<Node> moveableNodes = NodeObtainUtils.ObtainRealMoveableNodes(unitModel);
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.MOVE_RANGE, moveableNodes.ToArray(), ColorChanger.TargetRangeColor);
            gridColorChanger.ApplyColors();
        }
        //表示用移動範囲
        public static void ShowMoveableNodesForDisplay(ActorCore unitModel)
        {
            List<Node> moveableNodes = NodeObtainUtils.ObtainMoveableNodesForDisplay(unitModel);
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.MOVE_RANGE, moveableNodes.ToArray(), ColorChanger.TargetRangeColor);
            gridColorChanger.ApplyColors();
        }
        //移動距離を加算してる。
        public static void ShowSkillCastableNodes(ActorCore unitModel, int minRange, int maxRange)
        {
            List<Node> skillableNodes = NodeObtainUtils.ObtainSkillNodesForDisplay(unitModel, minRange, maxRange);
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ClearAllGroupColors();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SKILL_TARGET_RANGE, skillableNodes.ToArray(), ColorChanger.TargetRangeColor);
            gridColorChanger.ApplyColors();
        }
        //ターゲットに攻撃出来るノード。
        public static void ShowMoveToSkillableNodes(ActorCore unitModel, ActorCore target, int minRange, int maxRange)
        {
            List<Node> skillableNodes = NodeObtainUtils.ObtainMoveToSkillableNodes(unitModel, target, minRange, maxRange);
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ClearAllGroupColors();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SKILL_TARGET_RANGE, skillableNodes.ToArray(), ColorChanger.TargetRangeColor);
            gridColorChanger.ApplyColors();
        }
        public static void ShowPathNodes(List<Node> path)
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.MOVE_PATH, path.ToArray(), ColorChanger.MovePathColor);
            gridColorChanger.ApplyColors();
        }
        #region ヒットマップ表示参考用
        public static void ShowPlayerInfluenceEdgeNodes()
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = true;
            gridColorChanger.IsShowTarget = false;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.UpdateAllNodesWithInfluenceEdge(true);
            gridColorChanger.ApplyColors();
        }

        public static void ShowPlayerInfluenceNodes()
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = true;
            gridColorChanger.IsShowTarget = false;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.UpdateAllNodesWithInfluence(true);
            gridColorChanger.ApplyColors();
        }

        public static void ShowComputerInfluenceEdgeNodes()
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = true;
            gridColorChanger.IsShowTarget = false;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.UpdateAllNodesWithInfluenceEdge(false);
            gridColorChanger.ApplyColors();
        }
        
        public static void ShowComputerFluenceNodes()
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = true;
            gridColorChanger.IsShowTarget = false;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.UpdateAllNodesWithInfluence(false);
            gridColorChanger.ApplyColors();
        }
        #endregion
        /*
        public static void ShowPerfectNode(ActorCore unit,UnitSkillModel skill)
        {
            SkillPredictResult skillPredictResult = SkillTargetFilterAIUtility2.ObtainSkillPerfectTargetNode(unit, skill);

            if (skillPredictResult!=null)
            {
                Node targetNode = null;
                GStarGrid grid = PathFindingManager.Single.Grid;
                if ( skillPredictResult.TargetNode != null)
                {
                    targetNode = skillPredictResult.TargetNode;
                }
                else if (skillPredictResult.TargetUnit != null)
                {
                    targetNode = grid.GetNode(skillPredictResult.TargetUnit.BattleStatus.MinGridPosition);
                }
                if (targetNode != null)
                {
                    int effectRange = skill.MainEffect.Range;
                    List<Node> finalNodes = PathFindingManager.Single.Grid.SeekerService.GetNodesByRange(targetNode, 1, 1, effectRange);
                    ColorChanger colorChanger = grid.ColorChangerService;
                    colorChanger.IsShowInfluence = false;
                    colorChanger.IsShowTarget = true;
                    colorChanger.ChangeNodeGroupColors(ColorChanger.SCAN_RANGE, finalNodes.ToArray(), ColorChanger.MoveImpactRangeColor);
                    colorChanger.ApplyColors();
                }
            }
        }
        
        public static void ShowSafeNode(ActorCore unitModel)
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            Node node = SkillTargetFilterAIUtility2.GetSafeNode(unitModel);
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.MOVE_PATH,new Node[1] { node},Color.red);
            gridColorChanger.ApplyColors();
        }

        public static void ShowSkillNodes(ActorCore unitModel, UnitSkillModel unitSkillModel)
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.ClearAllGroupColors();
            var grid = PathFindingManager.Single.Grid;
//#if UNITY_EDITOR
            grid.GridView.ShowGrid();
//#endif
            List<Vector3Int> nodeIndexes = new List<Vector3Int>();
            NodeObtainUtils.ObtainSkillOnlyNodesForDisplay(nodeIndexes, unitModel, unitSkillModel.TargetRange);
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < nodeIndexes.Count; i++)
            {
                if (!grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z).IsBlock)
                    nodes.Add(grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z));
            }
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SKILL_RANGE, nodes.ToArray(), ColorChanger.FireImpaceRangeColor);
            gridColorChanger.ApplyColors();
        }*/

        /*
        public static void ShowScanableNodes(ActorCore unitModel, UnitSkillModel unitSkillModel)
        {
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.ClearAllGroupColors();
            var grid = PathFindingManager.Single.Grid;
            //#if UNITY_EDITOR
            grid.GridView.ShowGrid();
            //#endif
            List<Vector3Int> nodeIndexes = new List<Vector3Int>();
            NodeObtainUtils.ObtainSkillOnlyNodesForDisplay(nodeIndexes, unitModel,0, unitSkillModel.TargetRangeFare);
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < nodeIndexes.Count; i++)
            {
                if (!grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z).IsBlock)
                    nodes.Add(grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z));
            }
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SKILL_TARGET_RANGE, nodes.ToArray(), ColorChanger.TargetRangeColor);
            nodeIndexes = new List<Vector3Int>();
            NodeObtainUtils.ObtainSkillOnlyNodesForDisplay(nodeIndexes, unitModel,0, unitModel.AI.ScanDistance);
            nodes = new List<Node>();
            for (int i = 0; i < nodeIndexes.Count; i++)
            {
                if (!grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z).IsBlock)
                    nodes.Add(grid.GetNode(nodeIndexes[i].x, nodeIndexes[i].z));
            }
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SCAN_RANGE, nodes.ToArray(), ColorChanger.ScanRangeColor);
            gridColorChanger.ApplyColors();
        }*/

        public static void ShowSkillRange(ActorCore unitModel, int minRange, int maxRange)
        {
            List<Node> nodes = NodeObtainUtils.ObtainSkillOnlyNodes(unitModel, minRange, maxRange);
            var gridColorChanger = PathFindingManager.Single.Grid.ColorChangerService;
            gridColorChanger.IsShowInfluence = false;
            gridColorChanger.IsShowTarget = true;
            PathFindingManager.Single.Grid.GridView.ShowGrid();
            gridColorChanger.ClearAllGroupColors();
            gridColorChanger.ChangeNodeGroupColors(ColorChanger.SKILL_TARGET_RANGE, nodes.ToArray(), ColorChanger.TargetRangeColor);
            gridColorChanger.ApplyColors();
        }

    }
}
