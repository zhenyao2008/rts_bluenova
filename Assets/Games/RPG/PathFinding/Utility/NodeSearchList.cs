using System.Collections.Generic;
///
/// @file  NodeSearchList.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class NodeSearchList
    {

        protected List<Node> SearchList;

        protected int CurrentIndex;

        public List<Node> List
        {
            get
            {
                return SearchList;
            }
        }

        public Node[] ToArray()
        {
            return SearchList.ToArray();
        }

        public int Count
        {
            get
            {
                return SearchList.Count;
            }
        }

        public NodeSearchList()
        {
            SearchList = new List<Node>();
            CurrentIndex = 0;
        }

        public virtual void Add(Node node , int agentIndex)
        {
            node.IsOpen = agentIndex;
            SearchList.Add(node);
        }

        public Node Extract()
        {
            if (SearchList.Count > CurrentIndex)
            {
                Node node = SearchList[CurrentIndex];
                CurrentIndex++;
                return node;
            }
            return null;
        }

        public bool IsEmpty
        {
            get
            {
                return (SearchList.Count <= 0) || (CurrentIndex >= SearchList.Count);
            }
        }
    }
}

