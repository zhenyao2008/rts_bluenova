using System.Collections.Generic;
using BlueNoah.RPG.PathFinding;
using BlueNoah.RPG.SceneControl;
using UnityEngine;
namespace BlueNoah.RPG.Influence
{
    public static class InfluenceUtility 
    {
        public static void UpdateInfluence(SceneCore battleModel)
        {
            GStarGrid grid = PathFindingManager.Single.Grid;
            PathAgent pathAgent = PathFindingManager.Single.PathAgent;
            ActorCore unitModel;

            for (int i = 0; i < grid.XCount; i++)
            {
                for (int j = 0; j < grid.ZCount; j++)
                {
                    Node node = grid.Nodes[i, j];
                    node.PlayerScore = 0;
                    node.ComputerScore = 0;
                    node.PlayerDistanceScore = 0;
                    node.ComputerDistanceScore = 0;
                }
            }

            foreach (List<ActorCore> group0 in battleModel.ActorCoreSpawnService.PlayerActors.Values)
            {
                for (int i = 0; i < group0.Count; i ++)
                {
                    unitModel = group0[i];

                    List<Node> impactNodes = NodeObtainUtils.ObtainBaseInfluenceNodes(unitModel);

                    List<Node> impactPlusNodes = NodeObtainUtils.ObtainSkillNodesForDisplay(unitModel, unitModel.RegularAttack.TargetRangeNear, unitModel.RegularAttack.TargetRangeFar);

                    grid.SearchIdentity++;

                    for (int j = 0; j < impactNodes.Count; j++)
                    {
                        if (impactNodes[j].IsOpen == grid.SearchIdentity)
                            continue;
                        if (impactNodes[j].otherInfo.MainAttr == FieldMainAttr.OutOfField)
                            continue;

                        int distance = NodeObtainUtils.GetDistance(unitModel.BattleStatus.GridRect, new RectInt(impactNodes[j].X, impactNodes[j].Z,1,1) );

                        if (unitModel.TeamId == TeamId.PlayerOne)
                        {
                            impactNodes[j].PlayerScore++;
                            impactNodes[j].PlayerDistanceScore += (float)distance / unitModel.MoveAgent.UnitSpeed;
                        }
                        else if (unitModel.TeamId == TeamId.PlayerTwo)
                        {
                            impactNodes[j].ComputerScore++;
                            impactNodes[j].ComputerDistanceScore += (float)distance / unitModel.MoveAgent.UnitSpeed;
                        }

                        impactNodes[j].IsOpen = grid.SearchIdentity;
                    }
                    for (int j = 0; j < impactPlusNodes.Count; j++)
                    {

                        if (impactPlusNodes[j].IsOpen == grid.SearchIdentity)
                            continue;
                        if (impactPlusNodes[j].otherInfo.MainAttr == FieldMainAttr.OutOfField)
                            continue;

                        int distance = NodeObtainUtils.GetDistance(unitModel.BattleStatus.GridRect, new RectInt(impactPlusNodes[j].X, impactPlusNodes[j].Z, 1, 1));

                        if (unitModel.TeamId == TeamId.PlayerOne)
                        {
                            impactPlusNodes[j].PlayerScore++;
                            impactPlusNodes[j].PlayerDistanceScore += (float)distance / unitModel.MoveAgent.UnitSpeed;
                        }
                        else if (unitModel.TeamId == TeamId.PlayerTwo)
                        {
                            impactPlusNodes[j].ComputerScore++;
                            impactPlusNodes[j].ComputerDistanceScore += (float)distance / unitModel.MoveAgent.UnitSpeed;
                        }
                        impactPlusNodes[j].IsOpen = grid.SearchIdentity;
                    }
                }
            }
        }
    }
}
