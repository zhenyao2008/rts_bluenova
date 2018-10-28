using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class UnitRes : MonoBehaviour {

	public int group ;
	public Color outLineColor = new Color(0,151/255.0f,1);
	public float outLineWidth = 0.05f;
	public List<UnitRenderer> normalRenderers;
	public List<UnitRenderer> coloredRenderers;

	public string moveAnimStateName = "Run01";
	public string idleAnimStateName = "StandBy01";
	public string attackAnimStateName = "StandBy01";
	public string deathAnimStateName = "Death01";

	public Animation anim;

	public bool load;
	public bool noShadow;
	void Awake()
	{
		if (!anim)
			anim = GetComponentInChildren<Animation> ();
	}

	void Update(){
		if(load)
		{
			Enemy enemy = GetComponent<Enemy>();
			this.moveAnimStateName = enemy.moveAnimStateName;
			this.idleAnimStateName = enemy.idleAnimStateName;
			this.attackAnimStateName = enemy.attackAnimStateName;
			this.deathAnimStateName = enemy.deathAnimStateName;
			load = false;
		}
		if(noShadow)
		{
			Renderer[] rrs = GetComponentsInChildren<Renderer>();
			foreach(Renderer rr in rrs)
			{
				rr.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.Off;
				rr.receiveShadows = false;
			}
			noShadow = false;
		}
	}


	public void PlayAnim(string animStr){
		if(anim && anim[animStr])
		{
			anim.wrapMode = WrapMode.Once;
			anim.Play(animStr);
		}
	}

	public void ShowOutlineMaterial()
	{
		for(int i = 0 ; i < normalRenderers.Count;i ++)
		{
			normalRenderers[i].renderer.materials = normalRenderers[i].mats1;
		}
		for(int i = 0 ; i < coloredRenderers.Count;i++)
		{
			if(group == 0)
			{
				coloredRenderers[i].renderer.materials = coloredRenderers[i].mats2;
			}
			else if(group == 1)
			{
				coloredRenderers[i].renderer.materials = coloredRenderers[i].mats3;
			}
		}
	}

	public void ShowNormalMaterial()
	{
		for(int i = 0 ; i < normalRenderers.Count;i ++)
		{
			normalRenderers[i].renderer.materials = normalRenderers[i].mats0;
		}
		for(int i = 0 ; i < coloredRenderers.Count;i++)
		{
			if(group == 0)
			{
				coloredRenderers[i].renderer.materials = coloredRenderers[i].mats0;
			}
			else if(group == 1)
			{
				coloredRenderers[i].renderer.materials = coloredRenderers[i].mats1;
			}
		}
	}


}
