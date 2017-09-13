using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalSoundManager : MonoBehaviour {

	public float commonSpatialBlend = 1;
	public int commonMaxDistance = 50;

	public List<AudioClipInterval> sounds; 
	//省略版单例
	public static LocalSoundManager SingleTon(){
		return instance;
	}
	
	static LocalSoundManager instance;

	void Awake()
	{
		instance = this;
		playingSounds = new Dictionary<AudioClip, float> ();
	}

	Dictionary<AudioClip,float> playingSounds;
	public float soundInterval = 0.1f;
	public bool IsPlayable(AudioClip clip)
	{
		if (playingSounds.ContainsKey (clip)) {
			if (playingSounds [clip] < Time.time) {
				playingSounds [clip] = Time.time + soundInterval;
				return true;
			} else {
				return false;
			}
		} else {
			playingSounds.Add(clip,Time.time + soundInterval);
			return true;
		}
	}


}

public class AudioClipInterval
{
	public AudioClip clip;
	public float minInterval;
}

