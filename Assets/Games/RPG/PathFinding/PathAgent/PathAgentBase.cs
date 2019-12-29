///
/// @file   PathAgentBase.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///

using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    public delegate void PathAgentAction<T0>(T0 t);

    public enum StraightLinePathFinding { Normal = 0, Straight = 1, Curve = 2 };

    public class StepInfo
    {
        public float MaxStepDown;
        public float MaxStepUp;
    }

    public abstract class PathAgentBase
    {

        public PathAgentBase(GStarGrid grid)
        {
            Grid = grid;
            SearchIdentity = grid.SearchIdentity;
        }

        protected Identity SearchIdentity;
        //外側から操作数。
        static int _StartPathIndex;

        public int PathStyleIndex
        {
            get
            {
                return _StartPathIndex;
            }
            set
            {
                _StartPathIndex = value;
            }
        }

        protected GStarGrid Grid;

        protected List<Node> OpenList = new List<Node>(1000);

        protected MinBinaryHeap MinBinaryHeap = new MinBinaryHeap(2000);

#if UNITY_EDITOR
        protected List<Node> CloseList = new List<Node>(1000);
#endif
        protected int CurrentIndex = 0;
        //経路はもっとまっすぐになるか、もっと曲がりになるか、この変数でコントロール。
        //直線にさせたり、弧にさせたり
        [System.Obsolete]
        public StraightLinePathFinding PathType = StraightLinePathFinding.Curve;
        //ゴールに一番近いノード（マス）、ゴールに通わない時。
        public Node NearestNode;
        
        protected bool IsApplyBinaryHeap = false;
        [System.Obsolete]
        public bool IsApplyNearestNode = true;

        protected void AddToOpen(Node node)
        {
            if (IsApplyBinaryHeap)
            {
                AddToOpenBinaryHeap(node);
            }
            else
            {
                AddToOpenList(node);
            }
        }

        protected bool IsOpenEmpty()
        {
            return (IsApplyBinaryHeap && MinBinaryHeap.Count <= 0) || (!IsApplyBinaryHeap && CurrentIndex >= OpenList.Count );
        }

        protected Node ExtractOpenNode()
        {
            if (IsApplyBinaryHeap)
            {
                return ExtractFromBinaryHeap();
            }
            else
            {
                return RemoveFirstFromOpenList();
            }
        }

        protected void AlternInOpen(Node node)
        {
            if (IsApplyBinaryHeap)
            {
                AlterInOpenBinaryHeap(node);
            }
            else
            {
                AlterInOpenList(node);
            }
        }

        protected void ClearOpen()
        {
            if (IsApplyBinaryHeap)
            {
                ClearOpenBinaryHeap();
            }
            else
            {
                ClearOpenList();
            }
        }

        #region BinaryHeapでOpenList操作
        void AddToOpenBinaryHeap(Node node)
        {
            if (NearestNode == null)
            {
                NearestNode = node;
            }
            else
            {
                if (NearestNode.H > node.H)
                {
                    NearestNode = node;
                }
            }
            node.IsOpen = SearchIdentity;
            this.MinBinaryHeap.InsertKey(node);
        }

        void AlterInOpenBinaryHeap(Node node)
        {
            MinBinaryHeap.DeleteKey(node);
            AddToOpenBinaryHeap(node);
        }
        
        Node ExtractFromBinaryHeap()
        {
            return MinBinaryHeap.Extract();
        }

        void ClearOpenBinaryHeap()
        {
            MinBinaryHeap.Clear();
        }
        #endregion

        #region ListでOpenList操作
        //このノード（マス）がオッペンリストに入れる。
        protected void AddToOpenList(Node node)
        {
            //理論的ゴールに一番近いノード（マス）を検索
            //Search the node that nearest the goal.
            if (NearestNode == null)
            {
                NearestNode = node;
            }
            else
            {
                if (NearestNode.H > node.H)
                {
                    NearestNode = node;
                }
            }
            node.IsOpen = SearchIdentity;
            bool added = false;
            for (int i = CurrentIndex; i < OpenList.Count; i++)
            {
                //Ｆでソートする場合、斜め優先、
                //Ｇでソートする場合、真っ直ぐ優先。
                if (PathStyleIndex % 3 == 0)
                {
                    if (OpenList[i].G > node.G)
                    {
                        OpenList.Insert(i, node);
                        added = true;
                        break;
                    }
                }
                else
                {
                    if (OpenList[i].F > node.F)
                    {
                        OpenList.Insert(i, node);
                        added = true;
                        break;
                    }
                }
            }
            if (!added)
            {
                OpenList.Add(node);
            }
        }
    
        protected void AlterInOpenList(Node node)
        {
            OpenList.Remove(node);
            AddToOpenList(node);
        }

        //ノードをオッペンリストから削除
        protected Node RemoveFirstFromOpenList()
        {
            Node node = OpenList[CurrentIndex];
            CurrentIndex++;
            return node;
        }

        protected void ClearOpenList()
        {
            /*
            for (int i = 0; i < OpenList.Count; i++)
            {
                OpenList[i].G = 0;
                OpenList[i].H = 0;
                OpenList[i].F = 0;
                OpenList[i].Previous = null;
            }*/
            OpenList.Clear();
        }
        #endregion

        protected void AddToCloseList(Node node)
        {
#if UNITY_EDITOR
            CloseList.Add(node);
#endif
            node.IsClose = SearchIdentity;
        }

        protected int PlusAgentIndex()
        {
            return SearchIdentity.Increase();
        }
        
        //検索したノードを整理とリータン。
        protected List<Node> GetMovePath(Node node)
        {
            List<Node> resultPath = new List<Node>();
            Node currentNode = node;
            resultPath.Insert(0, currentNode);
            while (currentNode.Previous != null && currentNode.IsOpen == SearchIdentity)
            {
                resultPath.Insert(0, currentNode.Previous);
                currentNode = currentNode.Previous;
            }
            return resultPath;
        }
        //残るコストを予測
        protected virtual int GetH(Node node, Node targetNode) {
            return 0;
        }
        //ステップ高度を検証
        protected virtual bool IsStepable(Node nextNode, Node node, StepInfo stepInfo)
        {
        float currentStep = nextNode.Pos.y - node.Pos.y;
        if (stepInfo != null)
        {
            //Upstairs
            if (currentStep > 0 && stepInfo.MaxStepUp < currentStep)
            {
                return false;
            }
            //Downstairs
            if (currentStep < 0 && stepInfo.MaxStepDown < Mathf.Abs(currentStep))
            {
                return false;
            }
        }
        return true;
        }
    }
}
