using UnityEngine;
using System.Collections;

[System.Serializable]
public class ModelAnimationEventSerializeble :ScriptableObject
{
	public AnimationEventGroupSerializeble[] clips;
}

[System.Serializable]
public class AnimationEventSerializeble
{
	public string functionName;
	public float floatValue;
	public int intValue;
	public string stringValue;
	public float timeValue;
}

[System.Serializable]
public class AnimationEventGroupSerializeble
{
	public string clipName;
	public AnimationEventSerializeble[] events;
}

