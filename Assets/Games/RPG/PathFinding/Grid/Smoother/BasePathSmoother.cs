using System.Collections.Generic;
using UnityEngine;
///
/// @file  BasePathSmoother.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{

    public abstract class BasePathSmoother : GStarGridBaseService
    {
        public BasePathSmoother(GStarGrid grid) : base(grid) { }

        public abstract List<Vector3> Smooth(List<Node> nodes);

    }
}
