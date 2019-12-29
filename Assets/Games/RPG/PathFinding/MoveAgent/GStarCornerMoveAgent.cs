using System.Collections.Generic;
using UnityEngine;
///
/// @file  GStarCornerMoveAgent.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

namespace BlueNoah.RPG.PathFinding
{
    public class GStarCornerMoveAgent : GStarMoveAgentBase
    {

        //bool UseSmooth = false;

        Vector3 CornerPos;

        protected Vector3 NextCorner {
            get {
                return PathPositions[_CurrentIndex + 1];
            }
        }

        protected Vector3 CurrentCorner
        {
            get
            {
                return PathPositions[_CurrentIndex]; 
            }
        } 


#if false
        public override void CancelMove()
        {
            if (UnitModel.ActionStatus.IsMoveDone)
            {
                UnitModel.HackPos(BeforeMovePosition);
                UnitModel.HackRot(BeforeMoveRotation);
                ReplaceHaltBlock(MainNode);
                UnitModel.ActionStatus.IsMoveDone = false;
                SkillTargetNodeDisplayUtility.ShowSkillCastableNodes(UnitModel,UnitModel.RegularAttack);
            }
        }
#endif

        //手動の場合
#if false
        public IEnumerator Move()
        {
            var routes = UnitModel.ActionStatus.RoutePoints;

            UnitModel actionTarget = UnitModel.ActionStatus.ActionTarget;

            if (actionTarget != null)
            {
                Vector3Int targetPos = actionTarget.BattleStatus.CenterPosition;

                yield return Move(targetPos, UnitModel.RegularAttack.TargetRange);
            }
            else if (routes.Count > 0)
            {
                Vector3Int targetPos = routes[routes.Count - 1];

                yield return Move(targetPos);
            }
            yield return null;
        }
#endif
        //普通移動

#if false
        public IEnumerator Move(Vector3Int targetPos)
        {
            yield return Move(targetPos, 0);
        }

        public IEnumerator Move(Vector3Int targetPos,float range)
        {
            Node node = PathFindingManager.Single.Grid.GetNode(targetPos.x, targetPos.z);
            yield return Move(node.GridPos, range);
        }

        public IEnumerator Move(Vector3Int targetPos, int range)
        {
            List<Node> path = FindPath(targetPos, range);

            SkillTargetNodeDisplayUtility.ShowPathNodes(path);
            
            if (UseSmooth)
            {
                PathPositions = Grid.BarrierService.SmoothPath(path, GridLayerMask);
            }
            else
            {
                path = Grid.BarrierService.FilterPath(path, GridLayerMask);
                PathPositions = new List<Vector3>();
                for (int i = 0; i < path.Count; i++)
                {
                    PathPositions.Add(path[i].Pos);
                }
            }
            
            //UnitModel.ActionStatus.Nodes = path;

            _CurrentIndex = 0;

            bool isMove = false;
            BeforeMovePosition = UnitModel.BattleStatus.Position;
            BeforeMoveRotation = UnitModel.BattleStatus.Rotation;
            if (path.Count > 0 &&  PathPositions.Count > 0)
            {
                if (OnNextNode())
                {
                    isMove = true;
                }
                while (isMove)
                {
                    isMove = TickMove(Time.deltaTime);
#if UNITY_EDITOR
                    //DebugMovePosition();
#endif
                    yield return null;
                }
            }
            ReplaceHaltBlock(MainNode);
            yield return new WaitForSeconds(1);
        }
#endif

        public int UnitSpeed { get { return UnitModel.BattleStatus.MoveSpeed; } }
#if false
        [System.Obsolete]
        float ViewSpeed;
        [System.Obsolete]
        public bool StartMove(List<Vector3> path,float viewSpeed)
        {
            this.PathPositions = path;
            _CurrentIndex = 0;
            ViewSpeed = viewSpeed;
            return OnNextNode();
        }
        [System.Obsolete]
        public override bool TickMove(float deltaTime)
        {

            float deltaDistance = deltaTime * ViewSpeed;// this.GeneralSpeedMultiple;

            float remainDistance = 0;

            int count = 0;

            Vector3 direction = Vector3.zero;

            Vector3 pos = UnitModel.BattleStatus.Position;

            while (count < 10)
            {
                /*
                if (IsFlyingUnit)
                {
                    remainDistance = (CornerPos + new Vector3(0, UnitModel.BattleStatus.FlyingAltitude, 0) - pos).magnitude;
                    direction = (CornerPos + new Vector3(0, UnitModel.BattleStatus.FlyingAltitude, 0) - pos).normalized;
                }
                else
                {*/
                    remainDistance = (CornerPos - pos).magnitude;
                    direction = (CornerPos - pos).normalized;
                //}

                if (remainDistance < deltaDistance)
                {
                    deltaDistance -= remainDistance;

                    //UnitModel.HackPos(UnitModel.BattleStatus.Position + remainDistance * direction);
                    pos += remainDistance * direction;

                    UnitView.SetViewPosition(pos, Quaternion.LookRotation(direction));

                    if (!OnNextNode())
                    {
                        return false;
                    }
                }
                else
                {
                    //UnitModel.HackPos(UnitModel.BattleStatus.Position + deltaDistance * direction);
                    pos += deltaDistance * direction;
                    UnitView.SetViewPosition(pos, Quaternion.LookRotation(direction));
                    return true;
                }
                count++;
            }
            return true;
        }
        [System.Obsolete]
        public override bool OnNextNode(bool nextAble = true)
        {
            if (_CurrentIndex >= PathPositions.Count - 1)
            {
                return false;
            }

            Vector3 Orientation = (NextCorner - CurrentCorner).normalized;

            CornerPos = NextCorner;

            //UnitModel.HackRot(Quaternion.LookRotation(new Vector3(Orientation.x, 0, Orientation.z)));

            _CurrentIndex++;

            return true;
        }
#endif
        public override List<Node> FindPath(Vector3Int destination,ActorCore target, int stopDistance)
        {
            List<Node> nodes = PathFindingManager.Single.FindPath(UnitModel.BattleStatus.MainGridPosition, destination, target, float.MaxValue, stopDistance, this);
            List<Node> moveableNodes = GetMoveableNodes();
            List<Node> results = new List<Node>();
            if (nodes == null || nodes.Count == 0)
            {
                return results;
            }
            int maxCost = this.UnitSpeed * GStarGrid.Multiple;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].G <= maxCost)
                {
                    results.Add(nodes[i]);
                }
            }
            while (results.Count > 0)
            {
                Node node = results[results.Count - 1];
                if (IsUsedByOthers(node) || !moveableNodes.Contains(node))
                {
                    results.RemoveAt(results.Count - 1);
                }
                else
                {
                    break;
                }
            }
            return results;
        }

        public List<Node> GetMoveableNodes()
        {
            PathAgent pathAgent = PathFindingManager.Single.PathAgent;
            Node node = MainNode;
            if (node == null)
                return new List<Node>();
            List<Node> nodes = pathAgent.FindNodes(MainNode, UnitSpeed, this);
            nodes = pathAgent.FilterNodeByGStar(nodes,UnitModel);
            return nodes;
        }

        public List<Node> GetRealMoveableNodes()
        {
            List<Node> nodes = GetMoveableNodes();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].HaltReserveAgent != null && nodes[i].HaltReserveAgent.UnitModel != UnitModel)
                {
                    nodes.RemoveAt(i);
                    i--;
                }
            }
            return nodes;
        }

        public List<Node> GetSkillableNodesPlus(List<Node> moveRanges,ActorCore unitModel,int minRange, int maxRange)
        {
            PathAgent pathAgent = PathFindingManager.Single.PathAgent;
            List<Node> rangeNodes = pathAgent.FindRangeNodes(moveRanges, unitModel, minRange, maxRange);
            return rangeNodes;
        }
        [System.Obsolete]
        public int GetSkillRangeNodeCount(int skillRange)
        {
            return Mathf.Max(skillRange, 0);
        }

        public override bool IsReachable(Vector3Int targetPos)
        {
            Node node = Grid.GetNode(targetPos.x, targetPos.z);
            if (IsUsedByOthers(node))
                return false;
            List<Node> rangeNodes = GetMoveableNodes();
            return rangeNodes.Contains(node);
        }

        bool IsUsedByOthers(Node node)
        {
            for (int x = node.X; x < node.X + UnitModel.MoveAgent.XSize; x++)
            {
                for (int z = node.Z; z < node.Z + UnitModel.MoveAgent.ZSize; z++)
                {
                    if (Grid.GetNode(x, z) == null)
                    {
                        return true;
                    }
                    if (Grid.GetNode(x, z).HaltReserveAgent != null && Grid.GetNode(x, z).HaltReserveAgent != this)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
