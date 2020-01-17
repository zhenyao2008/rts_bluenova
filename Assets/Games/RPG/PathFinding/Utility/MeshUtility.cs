///
/// @file   MeshUtility.cs
/// @author Ying YuGang
/// @date   
/// @brief  
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
using UnityEngine;
using System.Collections.Generic;

namespace BlueNoah.RPG.PathFinding
{
    /*
     *　見えるグリッドを生成用ツール
     * データによってノードを生成して、頂点色によって、ノードの色を変わる。
     * いくらノード（マス）関係なく、一つのである。
     */ 
    public static class MeshUtility
    {
        //シェーダーは頂点色を計算しないと描画が違う
        //シェーダーが頂点の計算が必要
        static GameObject CreateMeshGameObject(Material meshMaterial, Color color)
        {
            GameObject go = new GameObject();
            go.transform.position = Vector3.zero;
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            if (meshMaterial == null)
            {
                Shader meshShader = Shader.Find("Diffuse");
                meshMaterial = new Material(meshShader);
            }
            meshMaterial.color = color;
            meshRenderer.material = meshMaterial;
            meshRenderer.sharedMaterial = meshMaterial;
            go.AddComponent<MeshFilter>();
            return go;
        }

        public static Matrix4x4 GetYAxisMatrix(float angle)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.m00 = Mathf.Cos(angle / 180 * Mathf.PI);
            matrix.m20 = -Mathf.Sin(angle / 180 * Mathf.PI);
            matrix.m11 = 1;
            matrix.m02 = Mathf.Sin(angle / 180 * Mathf.PI); ;
            matrix.m22 = Mathf.Cos(angle / 180 * Mathf.PI);
            return matrix;
        }

        public static Matrix4x4 GetZAxisMatrix(float angle)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.m00 = Mathf.Cos(angle / 180 * Mathf.PI);
            matrix.m10 = Mathf.Sin(angle / 180 * Mathf.PI);
            matrix.m01 = -Mathf.Sin(angle / 180 * Mathf.PI);
            matrix.m11 = Mathf.Cos(angle / 180 * Mathf.PI); ;
            matrix.m22 = 1;
            return matrix;
        }

        #region Draw grid for pathfinding.

        public static GameObject DrawGridGameObject(GStarGrid grid, Material meshMaterial, Color materialColor, float padding, Color meshColor,Color blockColor, RectInt rectInt)
        {
            padding = padding * grid.EdgeLength;
            padding = Mathf.Min(padding, grid.EdgeLength / 2f);
            GameObject go = CreateMeshGameObject(meshMaterial, materialColor);
            //Mesh mesh = DrawGridMesh(gridSize, x, y, padding, meshColor);
            Mesh mesh = DrawPathFindingGridMesh(grid, padding, meshColor,blockColor, rectInt);
            go.GetComponent<MeshFilter>().mesh = mesh;
            return go;
        }

        public static Mesh DrawPathFindingGridMesh(GStarGrid grid, float padding, Color normalColor,Color blockColor,RectInt rectInt)
        {
            Mesh mesh = new Mesh();
            List<Mesh> meshList = new List<Mesh>();
            List<Vector3> vertics = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Color> colors = new List<Color>();
            int count = 0;
           
            for (int i = rectInt.x; i < rectInt.x + rectInt.width; i++)
            {
                for (int j = rectInt.y; j < rectInt.y + rectInt.height; j++)
                {
                    Mesh subMesh;
                    if (grid.Nodes[i, j].IsBlock)
                    {
                        subMesh = DrawNodeMesh(grid.Nodes[i, j].Pos, grid.EdgeLength, padding, blockColor, grid.Nodes[i, j].otherInfo.Normal);
                    }
                    else
                    {
                         subMesh = DrawNodeMesh(grid.Nodes[i, j].Pos, grid.EdgeLength, padding, normalColor, grid.Nodes[i, j].otherInfo.Normal);
                    }
                    vertics.AddRange(subMesh.vertices);
                    for (int i0 = 0; i0 < subMesh.triangles.Length; i0++)
                    {
                        triangles.Add(subMesh.triangles[i0] + count * 4);
                    }
                    uvs.AddRange(subMesh.uv);
                    colors.AddRange(subMesh.colors);
                    count++;
                }
            }
            mesh.vertices = vertics.ToArray();
            mesh.colors = colors.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh DrawNodeMesh(Vector3 position, float nodeSize, float padding, Color color,Vector3 normal)
        {
            Mesh mesh = new Mesh();
            Vector3[] vertics = new Vector3[4];
            vertics[0] = position + new Vector3(- nodeSize / 2f + padding, 0,- nodeSize / 2f + padding);
            //vertics[0] = RotateByNormal(normal, position, vertics[0]);
            vertics[1] = position + new Vector3(- nodeSize / 2f + padding, 0, nodeSize / 2f - padding);
            //vertics[1] = RotateByNormal(normal, position, vertics[1]);
            vertics[2] = position + new Vector3(nodeSize / 2f - padding,0 ,nodeSize / 2f - padding);
            //vertics[2] = RotateByNormal(normal, position, vertics[2]);
            vertics[3] = position + new Vector3(nodeSize / 2f - padding, 0,- nodeSize / 2 + padding);
            //vertics[3] = RotateByNormal(normal, position, vertics[3]);
            Color[] colors = new Color[4];
            colors[0] = color;
            colors[1] = color;
            colors[2] = color;
            colors[3] = color;
            int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 2;
            triangles[4] = 3;
            triangles[5] = 0;
            mesh.vertices = vertics;
            mesh.triangles = triangles;
            mesh.colors = colors;
            Vector2 vertices0 = new Vector2(0, 0);
            Vector2 vertices1 = new Vector2(0, 1);
            Vector2 vertices2 = new Vector2(1, 1);
            Vector2 vertices3 = new Vector2(1, 0);
            mesh.uv = new Vector2[] { vertices0, vertices1, vertices2, vertices3 };
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector3 RotateByNormal(Vector3 normal,Vector3 center ,Vector3 pos)
        {
            float dot = Vector3.Dot((pos - center).normalized, normal);
            float angle = Mathf.Acos(dot) ;
            Vector3 cross = Vector3.Cross(normal, (pos - center).normalized);
            return Rotate(pos,center,cross, (Mathf.PI / 2f - angle) / Mathf.PI * 180 );
        }

        public static Vector3 Rotate(Vector3 pos,Vector3 center,Vector3 axis,float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, axis);
            Vector3 dir = pos - center;
            dir = rot * dir;
            return center + dir;
        }

        #endregion
    }
}