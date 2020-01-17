using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
/// @file  EightNeighborCalculater.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class EightNeighborCalculater : BaseNeighborCalculater
    {
        public EightNeighborCalculater(GStarGrid grid) : base(grid) { }

        protected override void CalculateNeighbors(Node node)
        {
            node.Neighbors.Clear();
            node.NeighborCosts.Clear();
            int i = node.X;
            int j = node.Z;
            if (i > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i - 1, j]);
                node.NeighborCosts.Add(1);
            }
            if (j > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i, j - 1]);
                node.NeighborCosts.Add(1);
            }
            if (i < Grid.XCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i + 1, j]);
                node.NeighborCosts.Add(1);
            }
            if (j < Grid.ZCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i, j + 1]);
                node.NeighborCosts.Add(1);
            }
            if (i > 0 && j > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i - 1, j - 1]);
                node.NeighborCosts.Add(GStarGrid.DiagonalPlus);
            }
            if (i < Grid.XCount - 1 && j < Grid.ZCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i + 1, j + 1]);
                node.NeighborCosts.Add(GStarGrid.DiagonalPlus);
            }
            if (i > 0 && j < Grid.ZCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i - 1, j + 1]);
                node.NeighborCosts.Add(GStarGrid.DiagonalPlus);
            }
            if (i < Grid.XCount - 1 && j > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i + 1, j - 1]);
                node.NeighborCosts.Add(GStarGrid.DiagonalPlus);
            }
        }
    }
}