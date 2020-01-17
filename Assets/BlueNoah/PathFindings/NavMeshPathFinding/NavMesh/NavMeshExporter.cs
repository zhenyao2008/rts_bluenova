using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BlueNoah.AI.Pathfinding
{

    public class NavNode
    {
        public Vector3Int pos;

        public HashSet<Vector3Int> neightbors;

        public NavNode()
        {
            neightbors = new HashSet<Vector3Int>();
        }
    }

    public static class NavMeshExporter
    {

        static Dictionary<Vector3Int, NavNode> verticeDic;

        public static void Export()
        {
            verticeDic = new Dictionary<Vector3Int, NavNode>();

            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            int[] triangles = navMeshData.indices;

            Vector3[] vertices = navMeshData.vertices;

            Vector3Int[] verticesInt = new Vector3Int[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                verticesInt[i] = Vector3Int.RoundToInt(vertices[i] * 100);
            }

            for (int i = 0; i < triangles.Length; i = i + 3)
            {
                int a = triangles[i];

                int b = triangles[i + 1];

                int c = triangles[i + 2];

                if (!verticeDic.ContainsKey(verticesInt[a]))
                {
                    verticeDic.Add(verticesInt[a], new NavNode());
                }

                if (!verticeDic.ContainsKey(verticesInt[b]))
                {
                    verticeDic.Add(verticesInt[b], new NavNode());
                }

                if (!verticeDic.ContainsKey(verticesInt[c]))
                {
                    verticeDic.Add(verticesInt[c], new NavNode());
                }

                verticeDic[verticesInt[a]].neightbors.Add(verticesInt[b]);

                verticeDic[verticesInt[a]].neightbors.Add(verticesInt[c]);

                verticeDic[verticesInt[b]].neightbors.Add(verticesInt[a]);

                verticeDic[verticesInt[b]].neightbors.Add(verticesInt[c]);

                verticeDic[verticesInt[c]].neightbors.Add(verticesInt[a]);

                verticeDic[verticesInt[c]].neightbors.Add(verticesInt[b]);

            }
        }
    }
}
