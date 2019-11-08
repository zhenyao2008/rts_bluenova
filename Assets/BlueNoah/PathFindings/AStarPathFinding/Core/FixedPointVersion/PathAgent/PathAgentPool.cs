using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.PathFinding.FixedPoint
{
    public class PathAgentPool
    {

        public List<PathAgentQueueItem> pathAgentList;
        public int maxSearchNodePerFrame = 1000;

        public PathAgentPool()
        {
            pathAgentList = new List<PathAgentQueueItem>();
        }

        public void Remove(PathAgentQueueItem item)
        {
            pathAgentList.Remove(item);
        }

        public PathAgentQueueItem StartFind(FixedPointPathAgent pathAgent, FixedPointNode startNode, FixedPointNode endNode, UnityAction<List<FixedPointNode>> onComplete)
        {
            PathAgentQueueItem item = new PathAgentQueueItem();
            item.pathAgent = pathAgent;
            item.startNode = startNode;
            item.endNode = endNode;
            item.onComplete = onComplete;
            pathAgentList.Add(item);
            return item;
        }

        public void OnUpdate()
        {
            while (pathAgentList.Count > 0 )
            {
                PathAgentQueueItem item = pathAgentList[0];
                pathAgentList.RemoveAt(0);
                List<FixedPointNode> path = item.pathAgent.StartFind(item.startNode, item.endNode,null);
                if (item.onComplete != null)
                    item.onComplete(path);
            }
        }

    }

    public class PathAgentQueueItem
    {
        public FixedPointPathAgent pathAgent;
        public FixedPointNode startNode;
        public FixedPointNode endNode;
        public UnityAction<List<FixedPointNode>> onComplete;
    }

}
