using UnityEngine;
namespace RPG
{
    public static class RVOUtility  {

    	public static Vector3[] CalculateTangent(Vector3 center,float radius,Vector3 point)
        {
           
            if ((point - center).sqrMagnitude <= radius)
                return null;

            Vector3[] tangents = new Vector3[2];

            Vector3 interection = (point - center).normalized * radius;

            float angle = Mathf.Acos(radius / Vector3.Distance (point , center));

            Vector3 pos0 = new Quaternion(0, Mathf.Sin(angle /2f), 0, Mathf.Cos(angle/2f)) * interection + center;

            Vector3 pos1 = new Quaternion(0, Mathf.Sin(-angle / 2f), 0, Mathf.Cos(-angle/2f)) * interection + center;

            tangents[0] = pos0;

            tangents[1] = pos1;

            return tangents;
        }
    }
}