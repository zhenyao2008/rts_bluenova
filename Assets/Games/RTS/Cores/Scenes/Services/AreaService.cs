using System.Collections;
using System.Collections.Generic;
using BlueNoah.AI.RTS;
using BlueNoah.Math.FixedPoint;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    public class AreaService
    {
        public int xCount;
        public int zCount;
        public int AreaSize = 10;
        Area[,] areas;
        List<ActorCore> mActorCore;

        public void Init(List<ActorCore> actorCores)
        {
            areas = new Area[xCount, zCount];
            mActorCore = actorCores;
        }

        public void OnUpdate()
        {
            for (int i = 0; i < mActorCore.Count; i++)
            {

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
