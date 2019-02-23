using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlueNoah.PathFinding.FixedPoint
{
    public class FixedPointGridDebuger
    {
        FixedPointGrid mFixedPointGrid;
        public FixedPointGridDebuger(FixedPointGrid fixedPointGrid)
        {
            this.mFixedPointGrid = fixedPointGrid;
        }
        public void OnDrawGizmos()
        {
            if (mFixedPointGrid.NodeList != null)
            {
                for (int i = 0; i < mFixedPointGrid.NodeList.Count; i++)
                {
                    if (mFixedPointGrid.NodeList[i] != null)
                    {
                        if (mFixedPointGrid.NodeList[i].IsBlock || !mFixedPointGrid.NodeList[i].Enable)
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            if ((mFixedPointGrid.NeighborCount == 4 && mFixedPointGrid.NodeList[i].neighbors.Count < 4) || (mFixedPointGrid.NeighborCount == 8 && mFixedPointGrid.NodeList[i].neighbors.Count < 8))
                            {
                                Gizmos.color = Color.blue;
                            }
                            else
                            {
                                Gizmos.color = Color.green;
                            }
                        }
                        if (mFixedPointGrid.NodeList[i].consumeRoadSizePlus > 0)
                        {
                            Gizmos.color = Color.yellow;
                        }

                        if (mFixedPointGrid.NodeList[i].isBridge)
                        {
                            Gizmos.color = Color.white;
                        }
                        Gizmos.DrawCube(mFixedPointGrid.NodeList[i].pos.ToVector3(), Vector3.one * mFixedPointGrid.NodeSize.AsFloat() * 0.3f);
                    }
                }
            }
        }
    }
}
