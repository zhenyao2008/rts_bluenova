///
/// @file  BaseNeighborCalculater.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public abstract class BaseNeighborCalculater : GStarGridBaseService
    {
        public BaseNeighborCalculater(GStarGrid grid):base(grid){}

        public void CalculateNeighbors()
        {
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    CalculateNeighbors(Grid.Nodes[i, j]);
                }
            }
        }
        protected abstract void CalculateNeighbors(Node node);

    }
}