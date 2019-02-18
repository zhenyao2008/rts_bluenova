using UnityEngine;
using System.Collections;

public class Curve : MonoBehaviour {

	public static Vector2 Bezier2(Vector2 start,Vector2  control,Vector2  end,float  t)
	{
		return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}
	
	public static Vector3 Bezier2(Vector3 start,Vector3 control,Vector3  end,float t)
	{
		return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}

	public static Vector2 Bezier3(Vector2 s,Vector2 st,Vector2 et,Vector2  e,float t)
	{
		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
	}
	
	public static Vector3 Bezier3(Vector3 s,Vector3 st,Vector3 et,Vector3 e,float t)
	{
		return (((-s + 3*(st-et) + e)* t + (3*(s+et) - 6*st))* t + 3*(st-s))* t + s;
	}


	public float Bezier2Len(Vector3 start,Vector3 control,Vector3 end,float t)
	{
		float length = 0;
		float ax = start.x - 2 * control.x + end.x;
		float ay = start.y - 2 * control.y + end.y;
		float az = start.z - 2 * control.z + end.z;

		float bx = 2 * (control.x - start.x);
		float by = 2 * (control.y - start.y);
		float bz = 2 * (control.z - start.z);

		float A = 4 * (ax * ax + ay * ay + az * az);
		float B = 4 * (ax * bx + ay * by + az * bz);
		float C = (bx * bx + by * by + bz * bz);
		
		/*
		 * 
		L(t_) = ((2*Sqrt[A]*(2*A*t*Sqrt[C + t*(B + A*t)] + B*(-Sqrt[C] + Sqrt[C + t*(B + A*t)])) + (B^2 - 4*A*C) (Log[B + 2*Sqrt[A]*Sqrt[C]] - Log[B + 2*A*t + 2 Sqrt[A]*Sqrt[C + t*(B + A*t)]]))
		/(8* A^(3/2)));
		*
		*/
		float temp1 = Mathf.Sqrt(C+t*(B+A*t));
		float temp2 = (2*A*t*temp1+B*(temp1-Mathf.Sqrt(C)));
		float temp3 = Mathf.Log(B+2*Mathf.Sqrt(A)*Mathf.Sqrt(C));
		float temp4 = Mathf.Log(B+2*A*t+2*Mathf.Sqrt(A)*temp1);
		float temp5 = 2*Mathf.Sqrt(A)*temp2;
		float temp6 = (B*B-4*A*C)*(temp3-temp4);
		length = (temp5+temp6)/(8*Mathf.Pow(A,1.5f));
		return length;
	}

	public float Bezier3Len(Vector3 s,Vector3 st,Vector3 et,Vector3 e,float t)
	{
		float[] x = {s.x,st.x,et.x,e.x};
		float[] y = {s.y,st.y,et.y,e.y};
		float[] z = {s.z,st.z,et.z,e.z};
		return L (x, y, z, t);
	}

	private float getBezierSpeed(float[] P, float t)
	{
		float it = 1 - t;
		float a = -3 * P[0] * it * it;
		float b = 3 * P[1] * it * it;
		float c = -6 * P[1] * it * t;
		float d = 6 * P[2] * it * t;
		float e = -3 * P[2] * t * t;
		float f = 3 * P[3] * t * t;
		return a + b + c + d + e + f;
		
	}
	//将X,Y,Z方向上的速度合成，即为曲线在t点时的速度。
	private float S(float[] x, float[] y, float[] z, float t)
	{
		float sx = getBezierSpeed(x, t);
		float sy = getBezierSpeed(y, t);
//		float sz = 
			getBezierSpeed(z, t);
		return Mathf.Sqrt(sx * sx + sy * sy);
	}
	//速度S是t的函数，由速度S对t进行积分，即可得到曲线长度。由于该函数不可积，因此采用辛普森公式求近似解。
	private float L(float[] x, float[] y, float[] z,float t)
	{
		float a = S(x, y, z,0);
		float b = 4 * S(x, y,z, t / 2);
		float c = S(x, y,z ,t);
		return (t / 6) * (a + b + c);
	}

}
