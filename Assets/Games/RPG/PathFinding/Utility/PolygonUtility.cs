using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
///
/// @file  PolygonUtility.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
public class PolygonGrid
{
    public List<PolygonNode> Nodes;

    public PolygonGrid()
    {
        Nodes = new List<PolygonNode>();
    }
}

public class PolygonNode
{
    public Vector3 Vertex;
    public List<PolygonNode> Neighbors;

    public PolygonNode(Vector3 vertex)
    {
        Vertex = vertex;
        Neighbors = new List<PolygonNode>();
    }
}
public static class PolygonGridUtility
{
    public static PolygonGrid GetPolygonGridByMesh(Mesh mesh)
    {
        PolygonGrid polygonGrid = new PolygonGrid();

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            polygonGrid.Nodes.Add(new PolygonNode(mesh.vertices[i]));
        }

        for (int i = 0; i < mesh.triangles.Length / 3; i++)
        {
            int index0 = mesh.triangles[i * 3];
            int index1 = mesh.triangles[i * 3 + 1];
            int index2 = mesh.triangles[i * 3 + 2];

            polygonGrid.Nodes[index0].Neighbors.Add(polygonGrid.Nodes[index1]);
            polygonGrid.Nodes[index0].Neighbors.Add(polygonGrid.Nodes[index2]);

            polygonGrid.Nodes[index1].Neighbors.Add(polygonGrid.Nodes[index0]);
            polygonGrid.Nodes[index1].Neighbors.Add(polygonGrid.Nodes[index2]);

            polygonGrid.Nodes[index2].Neighbors.Add(polygonGrid.Nodes[index0]);
            polygonGrid.Nodes[index2].Neighbors.Add(polygonGrid.Nodes[index1]);
        }

        return polygonGrid;
    }
    public static PolygonGrid GetPolygonGridByNavMesh(NavMeshTriangulation mesh)
    {
        PolygonGrid polygonGrid = new PolygonGrid();

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            polygonGrid.Nodes.Add(new PolygonNode(mesh.vertices[i]));
        }

        for (int i = 0; i < mesh.indices.Length / 3; i++)
        {
            int index0 = mesh.indices[i * 3];
            int index1 = mesh.indices[i * 3 + 1];
            int index2 = mesh.indices[i * 3 + 2];

            polygonGrid.Nodes[index0].Neighbors.Add(polygonGrid.Nodes[index1]);
            polygonGrid.Nodes[index0].Neighbors.Add(polygonGrid.Nodes[index2]);

            polygonGrid.Nodes[index1].Neighbors.Add(polygonGrid.Nodes[index0]);
            polygonGrid.Nodes[index1].Neighbors.Add(polygonGrid.Nodes[index2]);

            polygonGrid.Nodes[index2].Neighbors.Add(polygonGrid.Nodes[index0]);
            polygonGrid.Nodes[index2].Neighbors.Add(polygonGrid.Nodes[index1]);
        }
        //TODO to get the area
        return polygonGrid;
    }
}

