using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.SceneControl
{
    public class Framer : SimpleSingleMonoBehaviour<Framer>
    {
        protected override void Awake()
        {
            base.Awake();
            mFrame = 0;
        }

        static long mFrame;
        public long CurrentFrame
        {
            get
            {
                return mFrame;
            }
        }
        void FixedUpdate()
        {
            mFrame++;
        }
    }
}