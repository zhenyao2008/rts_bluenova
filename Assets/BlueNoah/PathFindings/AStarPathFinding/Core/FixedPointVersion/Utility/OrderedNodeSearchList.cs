/*
 應　彧剛（yingyugang@gmail.com）
 */
namespace BlueNoah.PathFinding.FixedPoint
{
    public class OrderedNodeSearchList : NodeSearchList
    {
        public int PathStyleIndex;

        public OrderedNodeSearchList(int pathStyleIndex) : base()
        {
            PathStyleIndex = pathStyleIndex;
        }

        public override void Add(FixedPointNode node, int agentIndex)
        {
            if(node.isOpen == agentIndex)
            {
                //Alert node.
                SearchList.Remove(node);
            }
            else
            {
                node.isOpen = agentIndex;
            }
            int i = CurrentIndex;
            for (; i < SearchList.Count; i++)
            {
                //八方面の場合、Ｆ：斜め優先。Ｇ：真っ直ぐ優先。
                if (PathStyleIndex % 3 == 0)
                {
                    if (SearchList[i].G > node.G)
                    {
                        break;
                    }
                }
                else
                {
                    if (SearchList[i].F > node.F)
                    {
                        break;
                    }
                }
            }
            SearchList.Insert(i, node);
        }
    }
}