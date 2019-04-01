using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.Math
{


    //线性代数。 
    //線形代数。
    //Linearalgebra.
    public static class MathUtility
    {

        public static bool IsInPlane(Vector3 point, Vector3 p0, Vector3 p1, Vector3 p2)
        {



            // Vector3.Cross();

            // float minX = p0.x <= p1.x ? p0.x : p1.x;
            // float maxX = p0.x > p1.x ? p0.x : p1.x;
            // float minY = p0.y <= p1.y ? p0.y : p1.y;
            // float maxY = p0.y > p1.y ? p1.y : p1.y;
            // float minZ = p0.z <= p1.z ? p0.z : p1.z;
            // float maxZ = p0.z > p1.z ? p0.z : p1.z;

            // minX = minX <= p2.x ? minX : p2.x;
            // maxX = maxX > p2.x ? maxX : p2.x;
            // minY = minY <= p2.y ? minY : p2.y;
            // maxY = maxY > p2.y ? maxY : p2.y;
            // minZ = minZ <= p2.z ? minZ : p2.z;
            // maxZ = maxZ > p2.z ? maxZ : p2.z;

            // Vector3 center = new Vector3((minX + ));


            return true;
        }


        //先判断是否在三角形的AABB内。
        //判断点是否在三角形内。方法1:点在三条边的同一侧；方法2:点所划分的三个三角形的面积之和等于判断三角形的面积。
        public static bool IsInTriangle(Vector3 point, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            //TODO is in AABB.

            Vector3 dir0 = (point - p0).normalized;
            Vector3 dir1 = (p1 - p0).normalized;

            Vector3 dir2 = (point - p1).normalized;
            Vector3 dir3 = (p2 - p1).normalized;

            Vector3 dir4 = (point - p2).normalized;
            Vector3 dir5 = (p0 - p2).normalized;





            return true;
        }
    }
}

