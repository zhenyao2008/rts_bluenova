using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(UnitAttribute))]
public class PlayerII : UnitBase {

	public bool isTower;
	public bool isHero;

	public Transform tower;
	[SyncVar]
	public Quaternion towerQua;

	public bool isManualMove  = false;
//	public bool isManualAttack = false;

	public Vector3 controlledPos;

	public bool isControllAble = true;

	public bool isReSpawnAble = true;
	public PlayerController controller;
	public Vector3 defaultSpawnPos;

	
	public GameObject playerUIPrefab;
	public PlayerUI playerUI;
	public bool showUI =false;

	
	public Transform headPoint;
	
	public Transform[] hitPoints;
	
	public Transform defaultTarget;
	[SyncVar]
	public Vector3 pos;
	[SyncVar]
	public Quaternion qua;
	[SyncVar]
	public Vector3 velocity;
	[SyncVar]
	public int layer;

	public const float syncFactor = 0.1f;

	public UnitState preState = UnitState.Idle;

	public AttackAction defaultAttackAction;
	
	public string moveAnimStateName = "Run01";
	public float moveAnimStateSpeed = 1;
	public string idleAnimStateName = "StandBy01";
	public string attackAnimStateName = "Attack01";
	public string deathAnimStateName = "Death01";
	public string skill01AnimStateName = "Skill01";

	public GameObject levelUpPrefab;

	Collider mCollier;
    
	public override void Awake(){
		anim = GetComponentInChildren<Animation> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		mTrans = transform;
		unitAttribute = GetComponent<UnitAttribute>();
		mCollier = GetComponent<Collider> ();
		mAudioSource = GetComponent<AudioSource> ();

		pos = mTrans.position;
		qua = mTrans.rotation;
	}
	
	public override void Start(){
		base.Start ();
		if(nav!=null && isClient && !isServer)
		{
			nav.enabled = false;
		}
		if(NetworkServer.active)
		{
			layer = gameObject.layer;
		}
		if (NetworkClient.active ) {
			//			nav.enabled = false;
			if(showUI)
			{
				GameObject go = Instantiate (playerUIPrefab) as GameObject;
				playerUI = go.GetComponent<PlayerUI>();
				go.GetComponent<PlayerUI>().followPoint = headPoint;
				go.GetComponent<PlayerUI>().unitAttribute = unitAttribute;
//				int nameIndex = Random.Range(0,6);
//				if(unitName!="")
//				{
//					go.GetComponent<PlayerUI>().uiName.text = unitName;
//				}else{
//					switch(nameIndex)
//					{
//					case 0:go.GetComponent<PlayerUI>().uiName.text = "加个功能";break;
//					case 1:go.GetComponent<PlayerUI>().uiName.text = "再改一版界面";break;
//					case 2:go.GetComponent<PlayerUI>().uiName.text = "帮我P个图";break;
//					case 3:go.GetComponent<PlayerUI>().uiName.text = "程序员鼓励师";break;
//					case 4:go.GetComponent<PlayerUI>().uiName.text = "今天做不出来就不要回家";break;
//					case 5:go.GetComponent<PlayerUI>().uiName.text = "晚上加下班";break;
//					}
//				}
			}
		}
	}
	
	void Update(){
		if (NetworkServer.active) {
			UpdateServer();
		}
		if (NetworkClient.active) {
			UpdateClient();
		}
	}

	public override Transform GetHitPoint(){
		if(hitPoints==null || hitPoints.Length == 0)
		{
			return mTrans;
		}
		return hitPoints[Random.Range (0,hitPoints.Length)];
	}
	
	public override void Damage(UnitBase attacker, int damage){
		base.Damage (attacker,damage);

		if(unitAttribute.currentHealth<=0)
		{
			if(NetworkServer.active){
				if(attacker!=null)
					attacker.OnKillEnemy(this);
				if(!isTower && nav!=null && nav.enabled)
					nav.Stop();
				state = UnitState.Death;
				StartCoroutine(NetDestroy(2));
			}
		}
	}

	public override void OnKillEnemy (UnitBase ua)
	{
		base.OnKillEnemy (ua);
		if (isHero) {
			unitAttribute.corn += ua.unitAttribute.killPrice;
			unitAttribute.exp += ua.unitAttribute.killExp;
			if(unitAttribute.exp>=unitAttribute.levelUpExp)
			{
				unitAttribute.level += unitAttribute.exp / unitAttribute.levelUpExp;
				unitAttribute.exp = unitAttribute.exp%unitAttribute.levelUpExp;
				unitAttribute.ReCalculateAttribute();
				RpcLevelUp();
			}
		}
	}

	[ClientRpc]
	public void RpcLevelUp(){
		if (levelUpPrefab != null) {
			GameObject go = Instantiate(levelUpPrefab,mTrans.position,Quaternion.identity) as GameObject;
			Destroy(go,3);
		}
	}

	public GameObject towerExplosionPrefab;
	IEnumerator NetDestroy(float delay){

		mCollier.enabled = false;
		if(nav!=null)nav.enabled = false;
		if(isTower)
			RpcTowerDestroy ();
		else
			RpcDeath();
	
		if(onDead!=null)onDead (this);
		yield return new WaitForSeconds (delay);
		if (isReSpawnAble) {
			RpcInActive();
			yield return new WaitForSeconds(1.5f);
			mTrans.position = defaultSpawnPos;
			pos = defaultSpawnPos;
			RpcReSetPos(defaultSpawnPos);
			yield return new WaitForSeconds(1.5f);
			ReSet();
			mCollier.enabled = true;
			state = UnitState.Idle;
			RpcActive();
			if(nav!=null)nav.enabled = true;
		} else {

			NetworkServer.Destroy(gameObject);
		}
	}

	[ClientRpc]
	public void RpcTowerDestroy(){
		if (towerExplosionPrefab == null) {
			towerExplosionPrefab = Resources.Load<GameObject>("Effects/Explosion_Dead_New");
		}
		if (towerExplosionPrefab) {
			GameObject go = Instantiate (towerExplosionPrefab, headPoint.position, Quaternion.identity) as GameObject;
			Destroy (go,2);
		}

		if (deathClip != null) {
			mAudioSource.clip = deathClip;
			mAudioSource.Play ();
		}
	}

	[ClientRpc]
	public void RpcDeath()
	{
		if (deathClip != null && isLocalPlayer) {
			mAudioSource.clip = deathClip;
			mAudioSource.Play ();
		}
			
	}

	void ReSet(){
		unitAttribute.currentHealth = unitAttribute.maxHealth;
	}

	[ClientRpc]
	public void RpcReSetPos(Vector3 pos){
//		Debug.Log ("RpcReSetPos" + pos);
		mTrans.position = pos;
	}

	[ClientRpc]
	public void RpcInActive(){
		anim.gameObject.SetActive (false);
	}

	[ClientRpc]
	public void RpcActive(){
		anim.gameObject.SetActive (true);
	}
	
	string mCurrentAnimState = "";


	void UpdateClient(){
		if (gameObject.layer != layer) {
			gameObject.layer = layer;
		}

		if (Vector3.Distance (mTrans.position, pos) < 0.5f) {
			mTrans.position = Vector3.Lerp (mTrans.position,pos,syncFactor * 2);
		} else {
			mTrans.position = Vector3.Lerp (mTrans.position,pos,syncFactor);
		}
		mTrans.rotation = Quaternion.Lerp (mTrans.rotation,qua,syncFactor * 3);
		if(isTower && tower!=null)
		{
			tower.rotation = Quaternion.Lerp (tower.rotation,towerQua,syncFactor);
		}
		if(state==UnitState.Attack)
		{
			if(anim)
			{
				if(Vector3.Distance (mTrans.position, pos) <= 0.1f)
				{
					anim.wrapMode = WrapMode.Loop;
					anim.Play(attackAnimStateName);
					mCurrentAnimState = attackAnimStateName;
				}
			}
		}
		if(state==UnitState.Idle)
		{
			if(anim)
			{
				if(Vector3.Distance (mTrans.position, pos) <= 0.1f)
				{
					anim.wrapMode = WrapMode.Loop;
					anim.Play(idleAnimStateName);
					mCurrentAnimState = idleAnimStateName;
				}
			}
		}
		if(state==UnitState.Move)
		{
			if(anim)
			{
				anim.wrapMode = WrapMode.Loop;
				anim.Play(moveAnimStateName);
				anim[moveAnimStateName].speed = moveAnimStateSpeed;
				mCurrentAnimState = moveAnimStateName;
			}

		}
		if(state==UnitState.Death && mCurrentAnimState != deathAnimStateName)
		{
			if(anim)
			{
				anim.wrapMode = WrapMode.Once;
				anim.Play(deathAnimStateName);
				mCurrentAnimState = deathAnimStateName;
			}
		}
		if(state==UnitState.Skill01 && mCurrentAnimState != skill01AnimStateName)
		{
			if(anim)
			{
				anim.wrapMode = WrapMode.Once;
				anim.Play(skill01AnimStateName);
				mCurrentAnimState = skill01AnimStateName;
			}
		}
	
	}
	
	void UpdateServer(){
		pos = mTrans.position;
		qua = mTrans.rotation;
		if(isTower&&tower!=null)towerQua = tower.rotation;
		if(!isTower && nav!=null)velocity = nav.velocity;
		
		if(unitAttribute.currentHealth <= 0)
		{
			state = UnitState.Death;
		}
		switch(state)
		{
		case UnitState.NavOffLink:;break;
		case UnitState.Idle:StateIdle();break;
		case UnitState.Move:StateMove();break;
		case UnitState.Attack:StateAttack ();break;
		case UnitState.Death:;break;
		case UnitState.Special:StateSpecial();break;
		case UnitState.Skill01:StateSkill01();break;
		}
	}
	
	public void StateIdle(){
		if(preState!=UnitState.Idle)
		{
			preState = UnitState.Idle;
			mNextScanTime = mScanInterval + Time.time;
			if(!isTower && nav!=null)nav.Stop();
			if(!isOneShootGunEffect)
			{
				RpcHideGunEffect();
			}
		}
		if(mNextScanTime < Time.time)
		{
			mNextScanTime = mScanInterval + Time.time;
			Scan ();
		}
	}
	
	UnitBase mTarget;
	public float mScanRadius = 20;
	public float attackRadius = 2;
	public float mAttackInterval = 0.10f;
	float mNextAttackTime;
	public void StateAttack(){
		if(preState!=UnitState.Attack)
		{
			preState = UnitState.Attack;
			mNextAttackTime = Time.time + mAttackInterval;
			if(!isTower && nav!=null)nav.Stop();
			if(!isOneShootGunEffect)
			{
				RpcShowGunEffect();
			}

		}
		if(mTarget==null)
		{
			state = UnitState.Idle;
			return;
		}
		if (isMelee) {
			if(Vector3.Distance(mTarget.transform.position,mTrans.position) > (attackRadius + mTarget.GetComponent<CapsuleCollider>().radius)*1.2f)
			{
				state = UnitState.Idle;
				return;
			}
		} else {
			if(Vector3.Distance(mTarget.transform.position,mTrans.position) > (mScanRadius + 1))
			{
				state = UnitState.Idle;
				return;
			}
		}
		
		if(mTarget.unitAttribute.currentHealth <= 0)
		{
			state = UnitState.Idle;
			return;
		}
		if(!isTower && nav!=null)nav.Stop();
		Vector3 forward = mTarget.transform.position - mTrans.position;
		forward.y = 0;
		mTrans.rotation = Quaternion.LookRotation (forward);

		if (isTower && tower != null) {
			tower.LookAt(mTarget.transform);
		}

		if(mNextAttackTime< Time.time)
		{
			mNextAttackTime = Time.time + mAttackInterval;
			if(isMelee)
			{
				MeleeAttack();
			}
			else
			{
				RemoteAttack(mTarget);
			}
		}
	}

	public bool isOneShootGunEffect = false;
	public GameObject startBackCityPrefab;
	public GameObject endBackCityPrefab;
	
	void MeleeAttack(){
		mTarget.Damage(this,unitAttribute.GetHitDamage(mTarget));
	}

	public override void RemoteAttack(UnitBase ub){
		GameObject go = Instantiate<GameObject> (bulletPrefab);
		go.transform.position = shootPoint.position ;
		go.transform.forward = transform.forward;
		Bullet bullet = go.GetComponent<Bullet> ();
		
		Vector3 direct = transform.forward;
		int angle = Random.Range (-5,5);
		float anglePI = angle * Mathf.PI / 180 / 2;
		direct =new Quaternion (0,Mathf.Sin(anglePI),0,Mathf.Cos(anglePI)) * direct;
		
		Vector3 targetPos = mTarget.GetHitPoint().position;
		if(isOneShootGunEffect)
		{
			RpcGunEffect();
		}
		bullet.Shoot (this,mTarget,80,targetPos,this.targetLayer,true);
		RpcRangeAttack (80,targetPos,0,this.targetLayer);
	}

	public AudioClip shootClip;
	public AudioSource mAudioSource;

	public AudioClip deathClip;

	[ClientRpc]
	public void RpcRangeAttack(int speed, Vector3 targetPos,int damage,int layer){

		GameObject go = Instantiate(bulletPrefab,shootPoint.position,Quaternion.identity) as GameObject;
		Bullet bullet = go.GetComponent<Bullet> ();
		bullet.Shoot (null,null,speed,targetPos,layer,false);
	}
	
	public GameObject gunEffect;

	[ClientRpc]
	public void RpcGunEffect(){
		Instantiate (gunEffect,shootPoint.position,shootPoint.rotation);
		if (mAudioSource == null)
			mAudioSource = GetComponent<AudioSource> ();
		if (mAudioSource != null && shootClip != null) {
			mAudioSource.clip = shootClip;
			if(!mAudioSource.isPlaying || mAudioSource.time > 0.1f)
				mAudioSource.Play();
		}
	}

	[ClientRpc]
	public void RpcShowGunEffect(){
		if (mAudioSource == null)
			mAudioSource = GetComponent<AudioSource> ();
		if (mAudioSource != null && shootClip != null) {
			mAudioSource.clip = shootClip;
//			mAudioSource.spatialBlend = 1;

			if(!mAudioSource.isPlaying)mAudioSource.Play();
		}
		if (gunEffect != null)
			gunEffect.SetActive (true);
	}

	[ClientRpc]
	public void RpcHideGunEffect(){
		if (mAudioSource == null)
			mAudioSource = GetComponent<AudioSource> ();
		if (mAudioSource != null) {
//			mAudioSource.spatialBlend = 1;
			if(mAudioSource.isPlaying)mAudioSource.Stop();
		}
		if (gunEffect != null)
			gunEffect.SetActive (false);
	}


	public float specialTime =3;//回城时间
	float mEndSpecialTime;
	void StateSpecial(){
		if(preState != UnitState.Special)
		{
			Debug.Log("StateSpecial");
			preState = UnitState.Special;
			mEndSpecialTime = Time.time + specialTime;
			nav.enabled = false;
		}
		if(mEndSpecialTime<Time.time)
		{
			mTrans.position = defaultSpawnPos;
			pos = defaultSpawnPos;
			RpcReSetPos(defaultSpawnPos);
			state = UnitState.Idle;
			nav.enabled = true;
		}
	}

	public void StateMove(){
		if(preState!=UnitState.Move)
		{
			preState = UnitState.Move;
			mNextScanTime = mScanInterval + Time.time;
			if(!isTower && nav!=null)nav.Resume();
		}
		if (isManualMove ) {
			if(!isTower && nav!=null)
				nav.SetDestination(controlledPos);
			if(Vector3.Distance(controlledPos,mTrans.position) <= 0.5f )
			{
//				if(!isTower && nav!=null)nav.Stop ();
				isManualMove  = false;
				state = UnitState.Idle;
			}
		}
//		else if(isManualAttack )
//		{
//			if(!isTower && nav!=null)
//				nav.SetDestination(mTarget.transform.position);
//			if(mTarget!=null)
//			{
//				if(!isTower && nav!=null)nav.SetDestination(mTarget.transform.position);
//				if(Vector3.Distance(mTarget.transform.position,mTrans.position) <= (attackRadius+mTarget.GetComponent<CapsuleCollider>().radius) )
//				{
//					if(!isTower && nav!=null)nav.Stop ();
//					state = UnitState.Attack;
//				}
//			}
//		}
		else
		{
			if(mTarget!=null)
			{
				if(!isTower && nav!=null)nav.SetDestination(mTarget.transform.position);
				if(Vector3.Distance(mTarget.transform.position,mTrans.position) <= (attackRadius+mTarget.GetComponent<CapsuleCollider>().radius) )
				{
					if(!isTower && nav!=null)nav.Stop ();
					state = UnitState.Attack;
				}
			}
			if(mNextScanTime < Time.time)
			{
				mNextScanTime = mScanInterval + Time.time;
				Scan ();
			}
		}
	}

	public float skill01During = 3;

	float mQuitSkill01Time;
	public void StateSkill01(){
		if(preState!=UnitState.Skill01)
		{
			preState = UnitState.Skill01;
			if(!isTower && nav!=null)nav.Stop();
			mQuitSkill01Time = Time.time + skill01During;
		}
		if(mQuitSkill01Time < Time.time)
		{
			state = UnitState.Idle;
		}
	}

	public float mScanInterval = 0.1f;
	float mNextScanTime;
	void Scan(){
		//		Debug.Log ("Scan");
		if (targetLayers.Count > 0) {
			Collider[] colls = Physics.OverlapSphere (mTrans.position,mScanRadius,targetLayer);
			if(colls !=null && colls.Length > 0)
			{
				float minDistance = Mathf.Infinity;
				Transform target = null;
				for(int i=0;i<colls.Length;i++)
				{
					if(Vector3.Distance(mTrans.position,colls[i].transform.position) < minDistance && colls[i].GetComponent<UnitBase>().unitAttribute.currentHealth>0)
					{
						target = colls[i].transform;
						minDistance = Vector3.Distance(mTrans.position,colls[i].transform.position);
					}
				}
				if(target!=null)
				{
					mTarget = target.GetComponent<UnitBase>();
					if(controller!=null){
						controller.SetTarget(mTarget.transform);
					}
					state = UnitState.Move;
				}
			}
		}
		if (mTarget == null && defaultTarget != null) {
			if(!isTower && nav!=null)nav.SetDestination(defaultTarget.position);
			state = UnitState.Move;
		}
	}

	public void ManualAttack(UnitBase ub){
//		isManualAttack = true;
		isManualMove = false;
		mTarget = ub;
		state = UnitState.Move;
	}

	public void ManualMove(Vector3 cameraPos,Vector3 targetPos){
		RaycastHit hit;
		if (Physics.Raycast (cameraPos, targetPos - cameraPos, out hit, Mathf.Infinity, targetLayer)) {
			Debug.Log("ManualMove");
			mTarget = hit.transform.GetComponent<UnitBase> ();
			state = UnitState.Move;
			isManualMove =false;
		} else {
			isManualMove  = true;
			//		isManualAttack = false;
			controlledPos = targetPos;
			state = UnitState.Move;
			//		StartCoroutine (_Move(targetPos));
		}
	}

	public GameObject skillPrefab01;
	public GameObject skillHitPrefab01;
	public int skillDmamage01 = 1000;

	float mNextSkillTime01;
	public float skillInterval01 = 10;
	public float skill01RealDelay = 2;//为何配合动画
	public bool Skill01(Vector3 direct){
		if (mNextSkillTime01 < Time.time) {
			mNextSkillTime01 = Time.time + skillInterval01;
			Debug.Log ("Skill01");
			StartCoroutine (_Skill01(direct));
			RpcSkill01 ( direct,skillInterval01);
			state = UnitState.Skill01;
			return true;
		}
		return false;
	}

	IEnumerator _Skill01(Vector3 direct){
		yield return new WaitForSeconds (skill01RealDelay);//为何配合动画
		if(skillPrefab01==null)skillPrefab01 = Resources.Load<GameObject> ("Effects/Skill01");
		if(skillHitPrefab01==null)skillHitPrefab01 = Resources.Load<GameObject>("HIT_LightEffect/Effect_Hit_Mag_Common");
		GameObject go = Instantiate (skillPrefab01,mTrans.position,mTrans.rotation) as GameObject;
		List<Transform> targetList = new List<Transform> ();
		Transform skillTrans = go.transform;
		Vector3 startPos = mTrans.position;
		Vector3 endPos = mTrans.position + direct * 20;
		float dur = Vector3.Distance (endPos,startPos) / 20;
		float t = 0;
		while(t < 1)
		{
			t += Time.deltaTime / dur;
			skillTrans.position = Vector3.Lerp(startPos,endPos,t);
			if(isServer){

				Collider[] colls = Physics.OverlapSphere(skillTrans.position,3,targetLayer);
				for(int i=0;i<colls.Length;i++)
				{
					if(!targetList.Contains(colls[i].transform))
					{
						targetList.Add(colls[i].transform);
						colls[i].transform.GetComponent<UnitBase>().Damage(this,skillDmamage01 * 10);
						Transform hitTrans = colls[i].transform.GetComponent<UnitBase>().GetHitPoint();
						RpcSkill01Hit(hitTrans.position);
					}
				}
			}
			yield return null;
		}
		Destroy (go);
	}

	[ClientRpc]
	public void RpcSkill01Hit(Vector3 pos){
		GameObject go = Instantiate(skillHitPrefab01,pos,Quaternion.identity) as GameObject;
		Destroy (go,2);
	}

	[ClientRpc]
	public void RpcSkill01(Vector3 direct,float cdTime)
	{
		StartCoroutine (_Skill01( direct));
//		controller.
	}

//#if UNITY_STANDALONE
//
//	void OnGUI(){
//		if(isLocalPlayer)
//		{
//
//		}
//	}
//#endif

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position,mScanRadius);
	}
}
