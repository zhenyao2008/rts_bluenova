/*
 *　2019.2.23 午後７時６分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/

using System.Collections.Generic;
using BlueNoah.AI.RTS;
using UnityEngine;

namespace BlueNoah.AI.Spawn.View
{
    public abstract class ActorViewBaseSpawnService
    {
        protected Dictionary<int, GameObject> mCachedActor;

        public void Init()
        {
            mCachedActor = new Dictionary<int, GameObject>();
        }
    }
}

