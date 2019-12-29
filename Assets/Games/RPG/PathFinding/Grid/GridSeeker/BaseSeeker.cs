using System.Collections.Generic;
using UnityEngine;
///
/// @file  BaseSeeker.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public abstract class BaseSeeker : GStarGridBaseService
    {

        protected Identity NodeSearchIdentity;

        public BaseSeeker(GStarGrid grid) : base(grid)
        {
            NodeSearchIdentity = grid.SearchIdentity;
        }

        protected void AddToCloseList(Node node)
        {
            node.IsClose = NodeSearchIdentity.Value;
        }

        public abstract List<Node> GetNodesByRange(Node startNode, int xSize, int zSize, int minRange, int maxRange);

        public abstract List<Node> GetNodesByRange(Vector3Int startPos, int xSize, int zSize, int minRange, int maxRange);
        [System.Obsolete]
        public abstract List<Node> GetNodesByRange(Vector3Int startPos, int xSize, int zSize, int targetRange);
        [System.Obsolete]
        public abstract List<Node> GetNeighborhoods(Node mainNode, int xSize, int zSize);

        public abstract void GetNodesByRangeSimple(NodeSearchList impactNodes, Node startNode, int xSize, int zSize, int minRange, int maxRange, int searchIndex);

    }
}
