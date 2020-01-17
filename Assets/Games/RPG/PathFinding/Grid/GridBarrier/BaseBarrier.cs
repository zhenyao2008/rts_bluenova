using System.Collections.Generic;
using UnityEngine;
///
/// @file  BaseBarrier.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public abstract class BaseBarrier: GStarGridBaseService
    {
        public BaseBarrier(GStarGrid grid) : base(grid) { }

        public abstract bool HasBarrier(Node startNode, Node endNode, GridLayerMask gridLayerMask);
        [System.Obsolete]
        public abstract List<Node> FilterPath(List<Node> nodes, GridLayerMask mask);
        [System.Obsolete]
        public abstract List<Node> GetNodes(Node startNode, Node endNode);

        public abstract List<Vector3> SmoothPath(List<Node> nodes, GridLayerMask mask);

    }
}
