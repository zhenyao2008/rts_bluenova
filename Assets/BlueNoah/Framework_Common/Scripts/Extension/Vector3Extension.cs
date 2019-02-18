using BlueNoah.Math.FixedPoint;
using UnityEngine;

public static class Vector3Extension  {

    public static FixedPointVector3 ToFixedPointVector3(this Vector3 vector3)
    {
        return new FixedPointVector3(vector3.x, vector3.y, vector3.z);
    }
}
