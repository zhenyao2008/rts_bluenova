using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointGridDebuger : FixedPointGridBaseService
    {
        public FixedPointGridDebuger(FixedPointGrid grid) : base(grid) { }

        public void DrawNodeInfos()
        {
#if UNITY_EDITOR
            float viewDistance = 10;
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = 9;
            style.normal.textColor = Color.green;
            for (int i = 0; i < mFixedPointGrid.xCount; i++)
            {
                for (int j = 0; j < mFixedPointGrid.zCount; j++)
                {
                    FixedPointNode node = mFixedPointGrid.GetNode(i, j);
                    FixedPoint64 distance = FixedPointVector3.Distance(node.pos, UnityEditor.SceneView.currentDrawingSceneView.camera.transform.position.ToFixedPointVector3());
                    if (distance < viewDistance)
                    {
                        //UnityEditor.Handles.Label(node.pos.ToVector3(), node.ToString(), style);
                    }
                }
            }
#endif
        }

    }
}
