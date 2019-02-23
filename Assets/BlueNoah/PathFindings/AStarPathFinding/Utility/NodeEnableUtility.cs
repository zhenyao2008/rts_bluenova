using BlueNoah.Utility;
using UnityEngine;

namespace BlueNoah.PathFinding.FixedPoint
{

    public static class NodeEnableUtility
    {

        public static void EnableNodes(FixedPointGrid fixedPointGrid)
        {
            for (int i = 0; i < fixedPointGrid.NodeList.Count; i++)
            {
                if (Physics.CheckBox(fixedPointGrid.NodeList[i].pos.ToVector3(), Vector3.one * (fixedPointGrid.GridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_ROAD))
                {
                    fixedPointGrid.NodeList[i].Enable = true;
                }
                else
                {
                    fixedPointGrid.NodeList[i].Enable = false;
                }
            }
            for (int i = 0; i < fixedPointGrid.NodeList.Count; i++)
            {
                if (Physics.CheckBox(fixedPointGrid.NodeList[i].pos.ToVector3(), Vector3.one * (fixedPointGrid.GridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_ROAD_SIDE))
                {
                    fixedPointGrid.NodeList[i].Enable = true;
                    fixedPointGrid.NodeList[i].consumeRoadSizePlus = 4;
                }
            }
        }
        //自動的に節点をブロックする
        public static void CheckNodeBlocking(FixedPointGrid fixedPointGrid)
        {
            for (int i = 0; i < fixedPointGrid.NodeList.Count; i++)
            {
                fixedPointGrid.NodeList[i].Enable = true;
                if (Physics.CheckBox(fixedPointGrid.NodeList[i].pos.ToVector3(), Vector3.one * (fixedPointGrid.GridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, ~(1 << LayerConstant.LAYER_GROUND)))
                {
                    fixedPointGrid.NodeList[i].IsBlock = true;
                }
                else
                {
                    fixedPointGrid.NodeList[i].IsBlock = false;
                }
            }
        }

        public static void CheckBridge(FixedPointGrid fixedPointGrid)
        {
            for (int i = 0; i < fixedPointGrid.NodeList.Count; i++)
            {
                Collider[] colliders = Physics.OverlapBox(fixedPointGrid.NodeList[i].pos.ToVector3(), Vector3.one * (fixedPointGrid.GridSetting.nodeWidth.AsFloat() * 0.4f), Quaternion.identity, 1 << LayerConstant.LAYER_BRIDGE);
                if (colliders.Length > 0)
                {
                    Collider collider = colliders[0];
                    FixedPointBridge bridge = collider.gameObject.GetOrAddComponent<FixedPointBridge>();
                    bridge.AddNode(fixedPointGrid.NodeList[i]);
                }
            }
        }

        public static void BlockForTown(FixedPointGrid fixedPointGrid)
        {
            EnableNodes(fixedPointGrid);

            CheckBridge(fixedPointGrid);
        }

        public static void BlockForSample(FixedPointGrid fixedPointGrid)
        {
            for (int i = 0; i < fixedPointGrid.NodeList.Count; i++)
            {
                fixedPointGrid.NodeList[i].Enable = true;
            }

            CheckNodeBlocking(fixedPointGrid);
        }
    }
}
