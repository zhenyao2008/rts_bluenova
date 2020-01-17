using System.Collections.Generic;
using UnityEngine;
///
/// @file  GridViewGroup.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class GridViewGroup : GStarGridBaseService
    {
        List<GridView> GridViews;

        Dictionary<int, GridView> GridViewDic;

        public GridViewGroup(GStarGrid grid) : base(grid)
        {
            GridViews = new List<GridView>();
            GridViewDic = new Dictionary<int, GridView>();
            RectInt rectInt1 = new RectInt(0, 0, grid.XCount / 2, grid.ZCount / 2);
            RectInt rectInt2 = new RectInt(grid.XCount / 2, 0, grid.XCount - grid.XCount / 2, grid.ZCount / 2);
            RectInt rectInt3 = new RectInt(0, grid.ZCount / 2, grid.XCount / 2, grid.ZCount - grid.ZCount / 2);
            RectInt rectInt4 = new RectInt(grid.XCount / 2, grid.ZCount / 2, grid.XCount - grid.XCount / 2, grid.ZCount - grid.ZCount / 2);
            GridView gridView = InitSubView(rectInt1, "sub_view_1");
            GridViews.Add(gridView);
            GridViewDic.Add(0, gridView);
            gridView = InitSubView(rectInt2, "sub_view_2");
            GridViews.Add(gridView);
            GridViewDic.Add(1, gridView);
            gridView = InitSubView(rectInt3, "sub_view_3");
            GridViews.Add(gridView);
            GridViewDic.Add(2, gridView);
            gridView = InitSubView(rectInt4, "sub_view_4");
            GridViews.Add(gridView);
            GridViewDic.Add(3, gridView);
        }

        GridView InitSubView(RectInt rectInt, string viewName)
        {
            GameObject view = new GameObject(viewName);
            GridView gridView = view.GetOrAddComponent<GridView>();
            gridView.InitGridView(Grid, 0, rectInt);
            view.transform.SetParent(Grid.transform);
            return gridView;
        }

        public void ShowGrid()
        {
            for (int i = 0; i < GridViews.Count; i++)
            {
                GridViews[i].ShowGrid();
            }
        }

        public void HideGrid()
        {
            for (int i = 0; i < GridViews.Count; i++)
            {
                GridViews[i].HideGrid();
            }
        }

        public void ApplyColors()
        {
            for (int i = 0; i < GridViews.Count; i++)
            {
                GridViews[i].ApplyColors();
            }
        }

        public void SetNodeColor(Node node, Color color)
        {
            GetCurrentGridView(node).SetNodeColor(node,color);
        }

        GridView GetCurrentGridView(Node node)
        {
            int index = (node.X < Grid.XCount / 2 ? 0 : 1) + (node.Z < Grid.ZCount / 2 ? 0 : 2);
            return GridViewDic[index];
        }
    }
}