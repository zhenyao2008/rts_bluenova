using UnityEngine;
using System.Collections;


public class LightParam : MonoBehaviour {


	public int lightmapIndex;
	public Vector4 lightmapScaleOffset;

}

[System.Serializable]
public class LightParamBean
{
	public int instanceId;
	public int lightmapIndex;
	public float[] lightmapScaleOffset;
}
