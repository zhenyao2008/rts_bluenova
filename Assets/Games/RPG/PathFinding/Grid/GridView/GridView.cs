///
/// @file   GridView.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
using UnityEngine;

namespace BlueNoah.RPG.PathFinding
{
    //このグリッドが基本は一つのMeshである。ノードのカーラーはメッシュの「colors」と言う変数中に保っている
    //Unityメッシュの最大頂点数は65000くらいので、もし極端な場合、(TODO_AI)グリッドに対して最大頂点数を65000を超える場合、対応しなければいけない。
    public class GridView : MonoBehaviour
    {
        //全グリッド用マテリアル、一つだけ
        Material Material;
        //全てのメッシュ、全グリッド共通。
        Mesh Mesh;
        //グリッドビュー用データー.
        GStarGrid Grid;
        //マスの間共通の距離.
        float Padding = 0.04f;
        //The GameObject for the gridView.Not Monobehaviour.gameObject.
        //メッシュを付けたゲームオブジェクト
        public GameObject GridGameObject;
        //The vertic's colors of the Mesh.It maybe change by runtime and no GC.
        //各頂点のカーラー。
        Color[] Colors;
        //各頂点のuv;
        Vector2[] UVs;
        //各頂点
        Vector3[] Vertex;
        //通常のノードの色
        public Color NormalColor = new Color(1, 1, 1, 0.9f);
        //ブロックされたノードの色
        public Color BlockColor = new Color(1, 0, 0, 0.0f);

        public bool IsShowGrid;

        public RectInt ViewRect { get; private set; }

        public void InitGridView(GStarGrid grid,float padding, RectInt rectInt)
        {
            Grid = grid;
            Padding = padding;
            Material = Resources.Load<Material>("Materials/node");
            GameObject go = MeshUtility.DrawGridGameObject(Grid, Material, Color.white, Padding , NormalColor, BlockColor, rectInt);
            Mesh = go.GetComponent<MeshFilter>().mesh;
            Colors = Mesh.colors;
            UVs = Mesh.uv;
            Vertex = Mesh.vertices;
            ViewRect = rectInt;
            go.transform.SetParent(transform);
            go.transform.localPosition = Grid.CenterPosition;
            go.name = "Grid";
            GridGameObject = go;
            HideGrid();
        }

        public void ChangeTexture(Texture2D texture2D)
        {
            Material.mainTexture = texture2D;
        }

        public void ShowGrid()
        {
            IsShowGrid = true;
            Color color = GridGameObject.GetComponent<MeshRenderer>().material.color;
            GridGameObject.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 1);
        }

        public void HideGrid()
        {
            IsShowGrid = false;
            Color color = GridGameObject.GetComponent<MeshRenderer>().material.color;
            GridGameObject.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0);
        }

        bool GetNodeStartIndex(Color[] colors,Vector3 pos,out int number)
        {
            number = -1;
            pos = pos + new Vector3(Grid.XCount / 2f * Grid.EdgeLength, 0, Grid.ZCount / 2f * Grid.EdgeLength);
            int x = Mathf.FloorToInt((pos.x - GridGameObject.transform.localPosition.x) / Grid.EdgeLength);
            int z = Mathf.FloorToInt((pos.z - GridGameObject.transform.localPosition.z) / Grid.EdgeLength);
            return GetNodeStartIndex(x,z,out number);
        }

        bool GetNodeStartIndex(int x, int z, out int number)
        {
            number = (x - ViewRect.x) * ViewRect.height + (z - ViewRect.y);
            if (number * 4 < 0 || number * 4 >= Colors.Length)
            {
                return false;
            }
            return true;
        }

        public void SetNodeColor(Node node, Color color)
        {
            SetNodeColorByNode(ref Colors, node, color);
        }

        #region 1.Change colors
        /*
        public void SetNodeColorByWorldPosition(Vector3 pos, Color color)
        {
            SetNodeColorByWorldPosition(ref Colors, pos, color);
        }

        public void SetNodeColorByLocalPosition(Vector3 pos, Color color)
        {
            SetNodeColorByLocalPosition(ref Colors, pos, color);
        }
        */
        /*
        void SetNodeColorByWorldPosition(ref Color[] colors, Vector3 pos, Color color)
        {
            pos = GridGameObject.transform.InverseTransformPoint(pos);
            SetNodeColorByLocalPosition(ref colors, pos, color);
        }

        void SetNodeColorByLocalPosition(ref Color[] colors, Vector3 pos, Color color)
        {
            int number;
            if (GetNodeStartIndex(colors, pos, out number))
            {
                colors[number * 4] = color;
                colors[number * 4 + 1] = color;
                colors[number * 4 + 2] = color;
                colors[number * 4 + 3] = color;
            }
        }
        */

        void SetNodeColorByNode(ref Color[] colors, Node node,Color color)
        {
            int number;
            if (GetNodeStartIndex( node.X,node.Z, out number))
            {
                colors[number * 4] = color;
                colors[number * 4 + 1] = color;
                colors[number * 4 + 2] = color;
                colors[number * 4 + 3] = color;
            }
        }

        public void ResetAllNodeColors()
        {
            for (int i = 0; i < Colors.Length; i++)
            {
                Colors[i] = NormalColor;
            }
        }

        //Memeory allocate.
        public void ApplyColors()
        {
            Mesh.colors = Colors;
        }
        #endregion

        #region 2.Change UVs
        public void SetNodeUVByWorldPosition(Vector3 pos, Vector2 uv)
        {
            SetNodeUVByWorldPosition(ref UVs, pos, uv);
        }

        public void SetNodeUVByWorldPosition(ref Vector2[] uvs, Vector3 pos, Vector2 uv)
        {
            pos = GridGameObject.transform.InverseTransformPoint(pos);
            SetNodeUVByLocalPosition(ref uvs, pos, uv);
        }

        public void SetNodeUVByLocalPosition(ref Vector2[] uvs, Vector3 pos, Vector2 uv)
        {
            pos = pos + new Vector3(Grid.XCount / 2f * Grid.EdgeLength, 0, Grid.ZCount / 2f * Grid.EdgeLength);
            int x = Mathf.FloorToInt((pos.x - GridGameObject.transform.localPosition.x) / Grid.EdgeLength);
            int z = Mathf.FloorToInt((pos.z - GridGameObject.transform.localPosition.z) / Grid.EdgeLength);
            int number;
            if (GetNodeStartIndex(x,z,out number))
            {
                uvs[number * 4] = uv;
                uvs[number * 4 + 1] = new Vector2(uv.x, uv.y + 0.25f);
                uvs[number * 4 + 2] = new Vector2(uv.x + 0.25f, uv.y + 0.25f);
                uvs[number * 4 + 3] = new Vector2(uv.x + 0.25f, uv.y);
            }
        }

        public void ApplyUVs()
        {
            Mesh.uv = UVs;
        }
        #endregion

        #region 3.Change Vertex
        public void SetNodeVertexByLocalPosition(Vector3 pos, float scale)
        {
            SetNodeVertexByLocalPosition(ref Vertex,pos,scale);
        }

        public void SetNodeVertexByLocalPosition(ref Vector3[] Vertex, Vector3 pos, float scale)
        {
            Vector3 originPos = pos;
            pos = pos + new Vector3(Grid.XCount / 2f * Grid.EdgeLength, 0, Grid.ZCount / 2f * Grid.EdgeLength);
            int x = Mathf.FloorToInt((pos.x - GridGameObject.transform.localPosition.x) / Grid.EdgeLength);
            int z = Mathf.FloorToInt((pos.z - GridGameObject.transform.localPosition.z) / Grid.EdgeLength);
            int number;
            if (GetNodeStartIndex(x,z,out number))
            {
                Vertex[number * 4] = originPos + new Vector3(-Grid.EdgeLength / 2f * scale, 0, -Grid.EdgeLength / 2f * scale);
                Vertex[number * 4 + 1] = originPos + new Vector3(-Grid.EdgeLength / 2f * scale, 0, Grid.EdgeLength / 2f * scale);
                Vertex[number * 4 + 2] = originPos + new Vector3(Grid.EdgeLength / 2f * scale, 0, Grid.EdgeLength / 2f * scale);
                Vertex[number * 4 + 3] = originPos + new Vector3(Grid.EdgeLength / 2f * scale, 0, -Grid.EdgeLength / 2f * scale);
            }
        }

        public void ApplyVertex()
        {
            Mesh.vertices = Vertex;
        }
        #endregion
    }
}