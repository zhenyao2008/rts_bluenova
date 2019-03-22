using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.Control.Service;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    //TODO 地理网格系统。
    public class AreaService : SimpleSingleTon<AreaService>, ServiceInterface
    {
        public int xCount = 5;
        public int zCount = 5;
        public int AreaSize = 10;
        Area[,] mAreas;
        List<ActorCore> mActorCores;

        public void Init(List<ActorCore> actorCores)
        {
            mAreas = new Area[xCount * 2, zCount * 2];
            mActorCores = actorCores;
        }

        public void OnAwake()
        {

        }

        public void OnDestory()
        {

        }

        public void OnStart()
        {

        }

        public void OnUpdate()
        {
            for (int i = 0; i < mActorCores.Count; i++)
            {
                ActorCore actorCore = mActorCores[i];
                FixedPointVector3 position = actorCore.transform.position;
                int x = FixedPointMath.Floor(position.x / AreaSize).AsInt();
                int z = FixedPointMath.Floor(position.z / AreaSize).AsInt();
                if (actorCore.x != x || actorCore.z != z)
                {
                    //TODO there is performance problem about Contains
                    if (mAreas[x, z].actors.Contains(actorCore))
                    {
                        mAreas[x, z].actors.Remove(actorCore);
                    }
                    actorCore.x = x;
                    actorCore.z = z;
                    mAreas[x, z].actors.Add(actorCore);
                }
            }
        }

        public List<ActorCore> ScanActors(FixedPointVector3 position, FixedPoint64 radius)
        {
            List<ActorCore> actorCores = new List<ActorCore>();
            int x = FixedPointMath.Floor(position.x / AreaSize).AsInt();
            int z = FixedPointMath.Floor(position.z / AreaSize).AsInt();
            int count = FixedPointMath.Ceiling(radius / AreaSize).AsInt();
            int minX = Mathf.Max(x - count, -xCount);
            int maxX = Mathf.Min(x + count, xCount);
            int minZ = Mathf.Max(z - count, -zCount);
            int maxZ = Mathf.Min(z + count, zCount);
            FixedPoint64 disX;
            FixedPoint64 disY;
            FixedPoint64 disZ;
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minZ; j < maxZ; j++)
                {
                    for (int n = 0; n < mAreas[i, j].actors.Count; n++)
                    {
                        disX = mAreas[i, j].actors[n].transform.position.x - position.x;
                        disY = mAreas[i, j].actors[n].transform.position.y - position.y;
                        disZ = mAreas[i, j].actors[n].transform.position.z - position.z;
                        if (disX * disX + disY * disY + disZ * disZ < radius * radius)
                        {
                            actorCores.Add(mAreas[i, j].actors[n]);
                        }
                    }
                }
            }
            return actorCores;
        }
    }

    public class Area
    {
        int mXIndex;

        int mZIndex;

        public List<ActorCore> actors;

        public Area(int xIndex, int zIndex)
        {
            mXIndex = xIndex;
            mZIndex = zIndex;
            actors = new List<ActorCore>();
        }
    }
}
