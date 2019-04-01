using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace BlueNoah.PathFinding
{
    [System.Serializable]
    public class ExportData
    {
        public int[] triangles;
        public List<Vector3Int> vertices;
    }
    public static class NavMeshExporter
    {

        public static void Export()
        {
            NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
            int[] triangles = navMeshTriangulation.indices;
            Vector3[] vertices = navMeshTriangulation.vertices;
            HashSet<Vector3Int> verticeSet = new HashSet<Vector3Int>();
            List<Vector3Int> verticeList = new List<Vector3Int>();

            ExportData exportData = new ExportData();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3Int vertor3Int = Vector3Int.RoundToInt(vertices[i] * 100);
                verticeList.Add(vertor3Int);
                // if (!verticeSet.Contains(vertor3Int))
                // {
                //     verticeSet.Add(vertor3Int);
                // }
                // else
                // {
                // }
            }

            exportData.triangles = triangles;
            exportData.vertices = verticeList;
            string stringData = JsonUtility.ToJson(exportData);
            Debug.Log(stringData);
        }

    }
}
