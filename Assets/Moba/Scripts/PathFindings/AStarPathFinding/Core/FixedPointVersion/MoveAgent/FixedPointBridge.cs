using System.Collections.Generic;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointBridge : MonoBehaviour
    {

        BoxCollider mBridgeCollider;

        List<FixedPointNode> mNodes;

        public List<FixedPointMoveAgent> moveAgents;

        public bool isBridgeUsed;

        public FixedPointVector3 forward;

        private void Awake()
        {
            mBridgeCollider = GetComponent<BoxCollider>();
            moveAgents = new List<FixedPointMoveAgent>();
        }

        public void AddNode(FixedPointNode node)
        {
            if (mNodes == null)
                mNodes = new List<FixedPointNode>();
            mNodes.Add(node);
            node.bridge = this;
            node.isBridge = true;
        }

        public List<FixedPointNode> GetNodes()
        {
            return mNodes;
        }

        public void EnterBridge(FixedPointMoveAgent moveAgent)
        {
            moveAgents.Add(moveAgent);
            isBridgeUsed = true;
        }

        public void OutBridge(FixedPointMoveAgent moveAgent)
        {
            moveAgents.Remove(moveAgent);
            if (moveAgents.Count == 0)
            {
                isBridgeUsed = false;
            }
        }

        public bool IsBlocked()
        {
            for (int i = 0; i < mNodes.Count; i++)
            {
                if (mNodes[i].consumeUsedPlus > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

