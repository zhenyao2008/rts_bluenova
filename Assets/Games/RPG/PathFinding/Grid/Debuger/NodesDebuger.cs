using UnityEngine;
///
/// @file  NodesDebuger.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class NodesDebuger : GStarGridBaseService
    {
        public NodesDebuger(GStarGrid grid) : base(grid) { }

        public void DrawNodeInfos()
        {
#if UNITY_EDITOR
            float viewDistance = 100f;
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = 10;
            style.normal.textColor = Color.green;
            for (int i = 0; i < Grid.XCount; i++)
            {
                for (int j = 0; j < Grid.ZCount; j++)
                {
                    Node node = Grid.Nodes[i, j];
                    float distance = Vector3.Distance(node.Pos, UnityEditor.SceneView.currentDrawingSceneView.camera.transform.position);
                    if (distance < viewDistance)
                    {
                        UnityEditor.Handles.Label(node.Pos, node.ToString(), style);
                    }
                }
            }
#endif
        }

    }
}
