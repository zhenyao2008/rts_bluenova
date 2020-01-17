using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {


//	[SyncVar]
//	Vector3 mDirect;
	[SyncVar]
	public float mSpeed =80;
	[SyncVar]
	public Vector3 mTargetPos;



	public int targetLayer;

	public int damage;

	public UnitBase attacker;

	public UnitBase target;

	float exploRadius = 1;
	Vector3 startPos;
	public bool isProjectile = false;
	[SyncVar]
	public bool isShooting = true;
	bool isServerShooting;
	public GameObject hitEffect;
	public AudioClip hitAudioClip;
	Transform mTrans;
	void Awake()
	{
		mTrans = transform;
		if(NetworkClient.active)
		{
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.spatialBlend = LocalSoundManager.SingleTon().commonSpatialBlend;
			audioSource.maxDistance = LocalSoundManager.SingleTon().commonMaxDistance;
			audioSource.enabled = true;
		}
	}


	public bool isOverlay = false;
	IEnumerator _ServerShoot()
	{
		Vector3 startPos = mTrans.position;
		float totalTime = Vector3.Distance (startPos, mTargetPos) / mSpeed;
		float t = 0;
		Vector3 controllPos = GetControllPos (startPos,mTargetPos);
		while (t < 1) {
			t += Time.deltaTime / totalTime;
			BulletMove(t,totalTime,startPos,mTargetPos,controllPos);
			yield return null;
		}

		if (isOverlay) {
			Collider[] hits = Physics.OverlapSphere (mTrans.position, exploRadius, targetLayer);
			for (int i=0; i<hits.Length; i++) {
				if (hits [i].GetComponent<UnitBase> () != null){
					damage = attacker.unitAttribute.GetHitDamage (hits [i].GetComponent<UnitBase> ());
					hits [i].GetComponent<UnitBase> ().Damage (attacker, damage);
				}
			}
		} else {
			if(target!=null){
				damage = attacker.unitAttribute.GetHitDamage (target);
				target.Damage(attacker, damage);
			}
		}
		Destroy(gameObject);
	}
	
	public void Shoot(UnitBase attacker,UnitBase target,float speed,Vector3 targetPos,int layer,bool serverShoot){
		this.attacker = attacker;
		this.target = target;
		this.mTargetPos = targetPos;
		this.isShooting = false;
//		this.damage = damage;
		this.targetLayer = layer;
		if (serverShoot) {
			StartCoroutine (_ServerShoot ());
		} else {
			StartCoroutine (_ClientShoot ());
		}
	}

	IEnumerator _ClientShoot()
	{
		mTrans.LookAt (mTargetPos);
		Vector3 startPos = mTrans.position;
		float totalTime = Vector3.Distance (startPos, mTargetPos) / mSpeed;
		float t = 0;
		Vector3 controllPos = GetControllPos (startPos,mTargetPos);
		Vector3 prePos = mTrans.position;
		while (t < 1) {
			t += Time.deltaTime / totalTime;
			BulletMove(t,totalTime,startPos,mTargetPos,controllPos);

			mTrans.forward = mTrans.position - prePos;
			prePos = mTrans.position;
//			t += Time.deltaTime/totalTime;
//			mTrans.position = Vector3.Lerp(startPos,mTargetPos,t);
			yield return null;
		}
//		gameObject.SetActive (false);
		ProjectileScript ps = GetComponent<ProjectileScript> ();
		if(ps!=null)ps.Hit ();
		if (hitAudioClip != null) {
			if(LocalSoundManager.SingleTon().IsPlayable(hitAudioClip))
			{
				GetComponent<AudioSource>().enabled = true;
				GetComponent<AudioSource>().clip = hitAudioClip;
				GetComponent<AudioSource>().Play();
			}
		}
		if (hitEffect != null)
		{
//			GameObject go = BattleFramework.PoolManager.SingleTon().Spawn(hitEffect,mTrans.position,Quaternion.identity);
//			BattleFramework.PoolManager.SingleTon().UnSpawn(0.2f,go);
  		}
		Destroy(gameObject);
	}

	void BulletMove(float t,float totalTime,Vector3 startPos,Vector3 targetPos,Vector3 controllPos)
	{
		if (!isProjectile) {
			mTrans.position = Vector3.Lerp (startPos, targetPos, t);
		} else {
			mTrans.position = Curve.Bezier2(startPos,controllPos,targetPos,t);
		}
	}


	public float minProjectorDistance = 15;//最短的投射射击距离，低于这个值就不需要抛物线。
	public float projectorFactor = 0.5f;//投射因子,投射高度和投射长度的关系
	public float controllFactor = 0.65f;//最高点在开始结束点之间的位置，比率
	Vector3 GetControllPos(Vector3 startPos,Vector3 endPos)
	{
		float distance = Vector3.Distance (startPos,endPos);
		Vector3 controllPos = Vector3.Lerp (startPos,endPos,controllFactor);
		controllPos += new Vector3 (0,(distance-minProjectorDistance)/2 * projectorFactor,0);
		return controllPos;
	}

}
