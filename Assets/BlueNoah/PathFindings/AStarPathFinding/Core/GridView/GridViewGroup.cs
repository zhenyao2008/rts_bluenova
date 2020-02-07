using System.Collections.Generic;
using UnityEngine;
namespace BlueNoah.PathFinding
{
    public class GridViewGroup
    {
        List<GridView> GridViews;

        Dictionary<int, GridView> GridViewDic;

        int mXCount;

        int mZCount;

        float mNodeSize;

        public void InitGridViewGroup(int xCount, int zCount, float nodeSize, Material material, int layer,  Transform parentTrans)
        {
            GridViews = new List<GridView>();
            GridViewDic = new Dictionary<int, GridView>();
            mXCount = xCount;
            mZCount = zCount;
            mNodeSize = nodeSize;
            RectInt rectInt1 = new RectInt(0, 0, mXCount / 2, mZCount / 2);
            RectInt rectInt2 = new RectInt(mXCount / 2, 0, mXCount - mXCount / 2, mZCount / 2);
            RectInt rectInt3 = new RectInt(0, mZCount / 2, mXCount / 2, mZCount - mZCount / 2);
            RectInt rectInt4 = new RectInt(mXCount / 2, mZCount / 2, mXCount - mXCount / 2, mZCount - mZCount / 2);

            GridView gridView = InitSubView(xCount,zCount,nodeSize,material,layer,rectInt1,parentTrans, "sub_view_1");
            GridViews.Add(gridView);
            GridViewDic.Add(0, gridView);
            gridView = InitSubView(xCount, zCount, nodeSize, material, layer, rectInt2, parentTrans, "sub_view_2");
            GridViews.Add(gridView);
            GridViewDic.Add(1, gridView);
            gridView = InitSubView(xCount, zCount, nodeSize, material, layer, rectInt3, parentTrans, "sub_view_2");
            GridViews.Add(gridView);
            GridViewDic.Add(2, gridView);
            gridView = InitSubView(xCount, zCount, nodeSize, material, layer, rectInt4, parentTrans, "sub_view_3");
            GridViews.Add(gridView);
            GridViewDic.Add(3, gridView);
        }

        GridView InitSubView(int xCount,int zCount,float nodeSize,Material material,int layer, RectInt rectInt,Transform parentTrans, string viewName)
        {
            GameObject view = new GameObject(viewName);
            GridView gridView = view.GetOrAddComponent<GridView>();
            gridView.InitGridView(xCount,zCount,nodeSize, 0, material, layer, rectInt);
            view.transform.SetParent(parentTrans);
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

        public void ApplyVertexs()
        {
            for (int i = 0; i < GridViews.Count; i++)
            {
                GridViews[i].ApplyVertex();
            }
        }

        public void SetNodeColor(int x,int z, Color color)
        {
            GetCurrentGridView(x,z).SetNodeColor(x,z, color);
        }

        GridView GetCurrentGridView(int x,int z)
        {
            int index = (x < mXCount / 2 ? 0 : 1) + (z < mZCount / 2 ? 0 : 2);
            return GridViewDic[index];
        }

        public void SetNodeHeight(int x, int z,Vector3 position,Vector3 normal)
        {
            GetCurrentGridView(x, z).SetNodeHeight(x, z, position, normal);
        }
    }
}
