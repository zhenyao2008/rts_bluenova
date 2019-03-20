using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    public class AreaService
    {
        public int xCount = 10;
        public int zCount = 10;
        public int AreaSize = 10;
        Area[,] mAreas;
        List<ActorCore> mActorCores;

        public void Init(List<ActorCore> actorCores)
        {
            mAreas = new Area[xCount, zCount];
            mActorCores = actorCores;
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
                    mAreas[x, z].actors.Remove(actorCore);
                    actorCore.x = x;
                    actorCore.z = z;
                    mAreas[x, z].actors.Add(actorCore);
                }
            }
        }

        public List<ActorCore> ScaneActors(FixedPointVector3 position, FixedPoint64 radius)
        {
            List<ActorCore> actorCores = new List<ActorCore>();

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
