using System.Collections.Generic;
using UnityEngine;
///
/// @file  BezierPathSmoother.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{

    public class BezierPathSmoother : BasePathSmoother
    {
        public BezierPathSmoother(GStarGrid grid) : base(grid) { }

        public override List<Vector3> Smooth(List<Node> nodes) {

            List<Vector3> results = new List<Vector3>();

            for (int i = 0; i < nodes.Count - 2; i++)
            {
                results.Add(BezierCurveUtility.GetPosition(nodes[i].Pos , nodes[i + 1].Pos , nodes[i + 2].Pos , 0));

                results.Add(BezierCurveUtility.GetPosition(nodes[i].Pos , nodes[i + 1].Pos , nodes[i + 2].Pos , 0.25f));

                results.Add(BezierCurveUtility.GetPosition(nodes[i].Pos , nodes[i + 1].Pos , nodes[i + 2].Pos , 0.5f));

                results.Add(BezierCurveUtility.GetPosition(nodes[i].Pos, nodes[i + 1].Pos, nodes[i + 2].Pos, 0.75f));

                results.Add(BezierCurveUtility.GetPosition(nodes[i].Pos, nodes[i + 1].Pos, nodes[i + 2].Pos, 1f));
            }

            return results;
        }

    }
}