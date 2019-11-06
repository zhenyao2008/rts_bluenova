using System.Collections.Generic;
/*
 應　彧剛（yingyugang@gmail.com）
 */
namespace BlueNoah.PathFinding.FixedPoint
{
    public class NodeSearchList
    {

        protected List<FixedPointNode> SearchList;

        protected int CurrentIndex;

        public List<FixedPointNode> List
        {
            get
            {
                return SearchList;
            }
        }

        public FixedPointNode[] ToArray()
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
            SearchList = new List<FixedPointNode>();
            CurrentIndex = 0;
        }

        public virtual void Add(FixedPointNode node, int agentIndex)
        {
            node.isOpen = agentIndex;
            SearchList.Add(node);
        }

        public FixedPointNode Extract()
        {
            if (SearchList.Count > CurrentIndex)
            {
                FixedPointNode node = SearchList[CurrentIndex];
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