///
/// @file  FourNeighborCalculater.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class FourNeighborCalculater : BaseNeighborCalculater
    {
        public FourNeighborCalculater(GStarGrid grid) : base(grid) { }

        protected override void CalculateNeighbors(Node node)
        {
            node.Neighbors.Clear();
            node.NeighborCosts.Clear();
            int i = node.X;
            int j = node.Z;
            if (i > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i - 1, j]);
                node.NeighborCosts.Add(GStarGrid.Multiple);
            }
            if (j > 0)
            {
                node.Neighbors.Add(Grid.Nodes[i, j - 1]);
                node.NeighborCosts.Add(GStarGrid.Multiple);
            }
            if (i < Grid.XCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i + 1, j]);
                node.NeighborCosts.Add(GStarGrid.Multiple);
            }
            if (j < Grid.ZCount - 1)
            {
                node.Neighbors.Add(Grid.Nodes[i, j + 1]);
                node.NeighborCosts.Add(GStarGrid.Multiple);
            }
        }
    }
}
