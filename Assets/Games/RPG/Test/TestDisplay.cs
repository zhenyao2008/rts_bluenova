using BlueNoah.RPG;
using BlueNoah.RPG.PathFinding;
using System.Collections.Generic;
using UnityEngine;
///
/// @file  TestDisplay.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

public class TestDisplay : MonoBehaviour
{
    public enum RangeDisplayType {
        ShowGridView,
        HideGridView,
        SkillRange,
        BigGimmickRange,
        MoveRange,
        MoveRangeForDisplay,
        MoveSkillRange,
        BigGimmickMoveSkillRange,
        MoveToSkillableNodes,
        InfluenceMapForPlayer,
        InfluenceMapForComputer,
        ScanRange
    };

    public RangeDisplayType DisplayType;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {

            switch (DisplayType)
            {
                case RangeDisplayType.ShowGridView:
                        PathFindingManager.Single.Grid.GridView.ShowGrid();
                    Debug.Log(DisplayType);
                    break;
                case RangeDisplayType.HideGridView:
                    PathFindingManager.Single.Grid.GridView.HideGrid();
                    break;
                default:
                    break;
            }
        }
    }
    /*
    public UnitModel GetCurrentUnit()
    {
        return BattlePresenter.Find().__UnitTurnContext.Unit;
    }

    public UnitModel GetHome()
    {
        return BattlePresenter.Find().BattleModel.FindHome(TeamId.PlayerOne);
    }

    public void TestMove(GStar.Vector3Int targetPos)
    { 
        UnitModel unitModel = BattlePresenter.Find().__UnitTurnContext.Unit;
    }

    public UnitModel GetUnit(string unitName)
    {
        List<UnitModel> units = BattlePresenter.Find().BattleModel.TurnOrderedUnitList;
        foreach (UnitModel unit in units)
        {
            if (unit.Name == unitName)
            {
                return unit;
            }
        }
        return null;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            BattlePresenter.Find().OnTestMove = TestMove;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {



            switch (DisplayType)
            {
                case RangeDisplayType.ScanRange:
                    {
                        UnitModel unitModel = GetUnit("テオナ");
                        SkillTargetNodeDisplayUtility.ShowSkillRange(unitModel,0, unitModel.AI.ScanDistance + unitModel.RegularAttack.TargetRangeFar);
                    }
                    break;
                case RangeDisplayType.SkillRange:
                    {
                        UnitModel unitModel = BattlePresenter.Find().__UnitTurnContext.Unit;
                        SkillTargetNodeDisplayUtility.ShowSkillRange(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                    }
                    break;
                case RangeDisplayType.BigGimmickRange:
                    {
                        UnitModel unitModel = BattlePresenter.Find().BattleModel.FindHome(TeamId.PlayerOne);
                        SkillTargetNodeDisplayUtility.ShowSkillRange(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                    }
                    break;
                case RangeDisplayType.MoveRange:
                    {
                        UnitModel unitModel = GetCurrentUnit();
                        SkillTargetNodeDisplayUtility.ShowMoveableNodes(unitModel);
                    }
                    break;
                case RangeDisplayType.MoveRangeForDisplay:
                    {
                        UnitModel unitModel = GetCurrentUnit();
                        SkillTargetNodeDisplayUtility.ShowMoveableNodesForDisplay(unitModel);
                    }
                    break;
                case RangeDisplayType.MoveSkillRange:
                    {
                        UnitModel unitModel = GetCurrentUnit();
                        SkillTargetNodeDisplayUtility.ShowSkillCastableNodes(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                    }
                    break;
                case RangeDisplayType.BigGimmickMoveSkillRange:
                    {
                        UnitModel unitModel = GetHome();
                        SkillTargetNodeDisplayUtility.ShowSkillCastableNodes(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);
                    }
                    break;
                case RangeDisplayType.MoveToSkillableNodes:
                    {
                        UnitModel unitModel = BattlePresenter.Find().__UnitTurnContext.Unit;
                        UnitModel target = GetUnit("テオナ");
                        List<Node> nodes = unitModel.MoveAgent.FindPath(target.BattleStatus.MinGridPosition,target,0);
                        SkillTargetNodeDisplayUtility.ShowPathNodes(nodes);
                    }
                    break;
                case RangeDisplayType.InfluenceMapForPlayer:
                    {
                        SkillTargetNodeDisplayUtility.ShowPlayerInfluenceEdgeNodes();
                    }
                    break;
                case RangeDisplayType.InfluenceMapForComputer:
                    {
                        SkillTargetNodeDisplayUtility.ShowComputerInfluenceEdgeNodes();
                    }
                    break;
                default:
                    break;
            }
        }
    }*/
}
