using UnityEngine;

///
/// @file  Snapshoter.cs
/// @author Ying YuGang
/// @date   
/// @brief 
/// Copyright 2019 Grounding Inc. All Rights Reserved.
///
namespace BlueNoah.RPG.PathFinding
{
    public class Snapshoter:GStarGridBaseService
    {

        public bool IsShowSnapshot
        {
            get { return SnapshotNodes != null; }
        }

        public Snapshoter(GStarGrid grid) : base(grid){ }

        Node[,] SnapshotNodes = null;
        public void Snapshot()
        {
            SnapshotNodes = AI.DeepCopyUtility.DeepClone<Node[,]>(Grid.Nodes);
        }

        public void ClearSnapshot()
        {
            SnapshotNodes = null;
        }

        public void DrawSnapshotNodeInfos()
        {
#if UNITY_EDITOR
            float viewDistance = 100f;
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = 10;

            GameObject selectGO = UnityEditor.Selection.activeGameObject;
            if (selectGO == null )
            {
                ClearSnapshot();
            }

            if (SnapshotNodes != null)
            {
                style.normal.textColor = Color.yellow;
                for (int i = 0; i < Grid.XCount; i++)
                {
                    for (int j = 0; j < Grid.ZCount; j++)
                    {
                        Node node = Grid.Nodes[i, j];
                        float distance = Vector3.Distance(node.Pos,UnityEditor.SceneView.currentDrawingSceneView.camera.transform.position);
                        if (distance < viewDistance)
                        {
                            UnityEditor.Handles.Label(node.Pos, SnapshotNodes[i, j].ToString(), style);
                        }
                    }
                }
            }
#endif
        }


    }
}
