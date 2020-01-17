///
/// @file   GStarMoveAgentBase.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    public abstract class GStarMoveAgentBase
    {
        [System.Obsolete]
        public List<Node> PathNodes;

        public List<Vector3> PathPositions;

        public bool IsFlyingUnit;

        public GridLayerMask GridLayerMask;

        public GridLayerMask SubGridLayerMask;

        public ActorCore UnitModel;
        [System.Obsolete]
        public bool IsMoving;

        protected GStarGrid Grid;
        [System.Obsolete]
        public int PathStype = 0;
        [System.Obsolete]
        public Vector3Int Offset;
        [System.Obsolete]
        public Vector3 BeforeMovePosition;
        [System.Obsolete]
        public Quaternion BeforeMoveRotation;
        [System.Obsolete]
        public float RemainTimeByTurn;
        //現在のノード索引。
        protected int _CurrentIndex;
        [System.Obsolete]
        protected const int DefaultPathStyle = 0;

        protected GameObject DebugGO;
        //速度倍数
        public int GeneralSpeedMultiple = 30;
        
        int _XSize = 1;

        int _ZSize = 1;

        public int XSize
        {
            set
            {
                _XSize = Mathf.Max(1, value);
                Offset = Grid.GetOffset(_XSize, _ZSize);
            }
            get
            {
                if(UnitModel.BattleStatus.Dir == BattleDir.East || UnitModel.BattleStatus.Dir == BattleDir.West)
                {
                    return _ZSize;
                }
                else
                {
                    return _XSize;
                }
            }
        }

        public int ZSize
        {
            set
            {
                _ZSize = Mathf.Max(1, value);
                Offset = Grid.GetOffset(_XSize, _ZSize);
            }
            get
            {
                if (UnitModel.BattleStatus.Dir == BattleDir.East || UnitModel.BattleStatus.Dir == BattleDir.West)
                {
                    return _XSize;
                }
                else
                {
                    return _ZSize;
                }
            }
        }

        public Node MainNode{get {return Grid.GetNode(UnitModel.BattleStatus.MinGridPosition );}}
        
        public Node[] HaltBlockNodes;

        public GStarMoveAgentBase()
        {
            GridLayerMask = 0;
            SubGridLayerMask = 0;
            Grid = PathFindingManager.Single.Grid;
        }

        public void ReplaceHaltBlock(Node node)
        {
            if (HaltBlockNodes!=null)
            {
                for (int i = 0; i < HaltBlockNodes.Length; i++)
                {
                    if (HaltBlockNodes[i]!=null)
                    {
                        HaltBlockNodes[i].HaltReserveAgent = null;
                        HaltBlockNodes[i] = null;
                    }
                }
                if (PathFindingManager.Single.IsEditorDebug)
                    Debug.Log(string.Format("Clear HaltBlock。{0}", UnitModel.Name));
            }
            if (node != null)
            {
                HaltBlockNodes = Grid.GetNodes(node, XSize, ZSize);

                if (HaltBlockNodes != null)
                {
                    for (int i = 0; i < HaltBlockNodes.Length; i++)
                    {
                        if (HaltBlockNodes[i] != null)
                        {
                            HaltBlockNodes[i].HaltReserveAgent = this;
                        }
                    }
                    if (PathFindingManager.Single.IsEditorDebug)
                        Debug.Log(string.Format("HaltBlock。{0}", UnitModel.Name));
                }
            }
        }
        [System.Obsolete]
        public void PlusHaltBlockNodesAddtionCost()
        {
            if (HaltBlockNodes!=null)
            {
                for (int i=0;i < HaltBlockNodes.Length;i++)
                {
                    if (HaltBlockNodes[i]!=null)
                    {
                        HaltBlockNodes[i].PlusCommonAddtionCost();
                    }
                }
            }
        }
        [System.Obsolete]
        public void MinusHaltBlockNodesAddtionCost()
        {
            if (HaltBlockNodes != null)
            {
                for (int i = 0; i < HaltBlockNodes.Length; i++)
                {
                    if (HaltBlockNodes[i] != null)
                    {
                        HaltBlockNodes[i].MinusCommonAddtionCost();
                    }
                }
            }
        }
        [System.Obsolete]
        public virtual bool OnNextNode(bool nextAble = true){ return false; }
        [System.Obsolete]
        public virtual bool TickMove(float deltaTime){ return true; }
        [System.Obsolete]
        protected virtual void OnTermination(){}
        [System.Obsolete]
        public virtual void Reset(){}
        [System.Obsolete]
        public virtual void CancelMove(){}
        [System.Obsolete]
        public Node GetNextNode(){ return null; }
        [System.Obsolete]
        public virtual List<Node> FindPath() { return null; }

        public virtual List<Node> FindPath (Vector3Int destination, ActorCore target, int skillRange) { return null; }
        [System.Obsolete]
        public virtual bool IsReachable(Vector3Int targetPos)
        {
            return true;
        }

        public List<Vector3> SmoothPath(List<Node> path) {
            var positions = Grid.BarrierService.SmoothPath(path, GridLayerMask);
            return positions;
        }
        //[System.Obsolete]
        //public int CurrentIndex => _CurrentIndex;
        //[System.Obsolete]
        //protected virtual Node CurrentNode => PathNodes[_CurrentIndex];
        //[System.Obsolete]
        //protected virtual Node NextNode => PathNodes[_CurrentIndex + 1];
        //[System.Obsolete]
        //protected virtual bool HasPath => PathNodes != null && PathNodes.Count > 0;
        //[System.Obsolete]
        //protected virtual bool HasNext => HasPath && PathNodes.Count > _CurrentIndex + 1;
        ////ユニットの正常スピード。
        //public virtual int UnitSpeed => UnitModel.BattleStatus.Current.MoveSpeed * GeneralSpeedMultiple;
        //[System.Obsolete]
        //protected virtual int ReckonCurrentCostPlus
        //{
        //    get
        //    {
        //        return ReckonCostPlus(CurrentNode, NextNode);
        //    }
        //}
        [System.Obsolete]
        protected int ReckonCostPlus(Node fromNode, Node toNode)
        {
            if (fromNode.X != toNode.X && fromNode.Z != toNode.Z)
            {
                return GStarGrid.DiagonalPlus;
            }
            return GStarGrid.Multiple;
        }
        [System.Obsolete]
        protected void DebugMovePosition()
        {
            if (DebugGO == null)
                DebugGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            DebugGO.transform.position = UnitModel.BattleStatus.Position;
        }
    }
}
