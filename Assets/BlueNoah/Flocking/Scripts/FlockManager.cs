using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.AI.Flocking
{
    public class FlockManager : SimpleSingleMonoBehaviour<FlockManager>
    {
        //Captain is the flock group center.
        // public FlockComponent captain;
        public Transform center;
        //Menbers of the flock group.
        public FlockComponent[] members;

        public Vector3 speed = Vector3.forward;

        public float flockPercent;

        public float separationWeight = 0.33f;

        public float centerWeight = 0.33f;

        public float velocityWeight = 0.33f;
    }
}
