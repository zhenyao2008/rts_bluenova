using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(UnitAttribute))]
public class Enemy : UnitBase
{

	public GameObject playerUIPrefab;
	public GameObject playerUI;
	public bool showUI = false;

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
	public Vector3 targetPos;

	public const float syncFactor = 0.1f;

	public UnitState preState = UnitState.Idle;

	public override void Awake ()
	{
		base.Awake ();
		anim = GetComponentInChildren<Animation> (true);
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		mTrans = transform;
		unitAttribute = GetComponent<UnitAttribute> ();
		mTrans.position = pos;
		mTrans.rotation = qua;
		if (!NetworkServer.active) {
			anim.gameObject.SetActive (false);
		}
		if (NetworkServer.active && NetworkClient.active) {
			commonAttackDelay = 0;
		}
	}

	public override void Start ()
	{
		base.Start ();
		if (NetworkServer.active) {
			RpcInitTrans (mTrans.position, mTrans.rotation, playerIndex);
		}
		for (int i = 0; i < attackActions.Length; i++) {
			attackActions [i].unitBase = this;
		}
		if (NetworkClient.active) {
			if (showUI) {
                GameObject go = BlueNoah.UI.UnitUIManager.Instance.CreateUnitUI();
				playerUI = go;
				go.GetComponent<PlayerUI> ().followPoint = headPoint;
				go.GetComponent<PlayerUI> ().unitAttribute = unitAttribute;
				int nameIndex = Random.Range (0, 6);
				switch (nameIndex) {
				case 0:
					go.GetComponent<PlayerUI> ().uiName.text = "加个功能";
					break;
				case 1:
					go.GetComponent<PlayerUI> ().uiName.text = "再改一版界面";
					break;
				case 2:
					go.GetComponent<PlayerUI> ().uiName.text = "帮我P个图";
					break;
				case 3:
					go.GetComponent<PlayerUI> ().uiName.text = "程序员鼓励师";
					break;
				case 4:
					go.GetComponent<PlayerUI> ().uiName.text = "今天做不出来就不要回家";
					break;
				case 5:
					go.GetComponent<PlayerUI> ().uiName.text = "晚上加下班";
					break;
				}
			}

		}
	}

	void Update ()
	{
		if (NetworkServer.active) {
			UpdateServer ();
		}
		if (NetworkClient.active) {
			UpdateClient ();
		}
	}

	public override Transform GetHitPoint ()
	{
		if (hitPoints == null || hitPoints.Length == 0) {
			return mTrans;
		}
		return hitPoints [Random.Range (0, hitPoints.Length)];
	}

	//被击中
	public override void Damage (UnitBase attacker, int damage)
	{
		base.Damage (attacker, damage);
		if (unitAttribute.currentHealth <= 0 && state != UnitState.Death) {
			state = UnitState.Death;
			if (attacker != null) {
				attacker.OnKillEnemy (this);
			}
			RpcShowMsgTips (2, "+" + unitAttribute.killPrice, Color.yellow, 3, new Vector3 (0, 20, 0));
			if (nav.enabled)
				nav.isStopped = true;
            if (onDeadAction != null){
                onDeadAction();
                onDeadAction = null;
            }
			StartCoroutine (NetDestroy (2));
		}
		RpcUpdateHealth (unitAttribute.currentHealth);
		RpcShowMsgTips (0, Mathf.Max (damage, 1).ToString (), Color.white, 1, Vector3.zero);
	}

	public override void OnKillEnemy (UnitBase target)
	{
		this.playerAttribute.corn += target.unitAttribute.killPrice;
	}

	[ClientRpc]
	public void RpcUpdateHealth (int health)
	{
		unitAttribute.currentHealth = health;
	}

	public override void ShowMsgTips (int index, string msg, Color color, float duration, Vector3 offset)
	{
		RpcShowMsgTips (index, msg, color, duration, offset);
	}

	[ClientRpc]
	public void RpcShowMsgTips (int index, string msg, Color color, float duration, Vector3 offset)
	{
		playerUI.GetComponent<PlayerUI> ().ShowMsgTips (index, msg, color, duration, offset);
	}

	IEnumerator NetDestroy (float delay)
	{
		this.nav.enabled = false;
		GetComponent<Collider> ().enabled = false;
		yield return new WaitForSeconds (delay);
		NetworkServer.Destroy (gameObject);
	}

	Vector3 mPrePos;
	//	string mCurrentAnimState = "";
	public void UpdateClient ()
	{
		if (pos != Vector3.zero) {
//			mTrans.position = Vector3.Lerp (mTrans.position,pos,Mathf.Clamp(3.0f / Vector3.Distance (mTrans.position, pos),syncFactor ,0.5f));
			if (Vector3.Distance (mTrans.position, targetPos) < 5) {
//				Debug.Log(Vector3.Distance (mTrans.position, targetPos));
				mTrans.position = Vector3.Lerp (mTrans.position, pos, syncFactor * 2 * 5 / Vector3.Distance (mTrans.position, targetPos));
			} else {
				mTrans.position = Vector3.Lerp (mTrans.position, pos, syncFactor);
			}
		}
		mTrans.rotation = Quaternion.Lerp (mTrans.rotation, qua, syncFactor);


		if (state == UnitState.Attack) {
			mTrans.rotation = Quaternion.Lerp (mTrans.rotation, qua, syncFactor);
		}
		if (state == UnitState.Idle) {
			anim.wrapMode = WrapMode.Loop;
			anim.Play (this.idleAnimStateName);
		}
		if (state == UnitState.Move) {
			anim.wrapMode = WrapMode.Loop;
			if (Vector3.Distance (mTrans.position, mPrePos) > 0) {
				anim.Play (this.moveAnimStateName);
			} else {
				anim.Play (this.idleAnimStateName);
			}
		}

		mPrePos = mTrans.position;
	}

	void UpdateServer ()
	{
		pos = mTrans.position;
		qua = mTrans.rotation;
		velocity = nav.velocity;

		if (unitAttribute.currentHealth <= 0) {
			state = UnitState.Death;
			RpcDeathState ();
		}

		if (buffDics.Count > 0 && buffDics.Values.Count > 0) {
			foreach (BuffBase buffBase in buffDics.Values) {
				buffBase.OnUpdate ();
			}
			foreach (System.Type st in oldBuffs) {
				buffDics.Remove (st);
			}
			oldBuffs.Clear ();
		}

		switch (state) {
		case UnitState.NavOffLink:
			;
			break;
		case UnitState.Idle:
			OnStateIdle ();
			break;
		case UnitState.Move:
			StateMove ();
			break;
		case UnitState.Attack:
			StateAttack ();
			break;
		case UnitState.Skill:
			StateSkill ();
			break;
		case UnitState.Death:
			;
			break;
		}
	}


	public void OnStateIdle ()
	{
		if (preState != UnitState.Idle) {
			preState = UnitState.Idle;
			mNextScanTime = mScanInterval + Time.time;
			if (nav.enabled)
				nav.isStopped=true;
		}
		if (mNextScanTime < Time.time) {
			mNextScanTime = mScanInterval + Time.time;
			if (CheckSkillAble ()) {
				state = UnitState.Skill;
				return;
			}
			Scan ();
		}
	}

	bool CheckSkillAble ()
	{
		for (int i = 0; i < attackActions.Length; i++) {
			if (attackActions [i].IsSkillAble ()) {
				mCurrentSkill = attackActions [i];
				return true;
			}
		}
		return false;
	}

	UnitBase mMainTarget;
	public float mScanRadius = 20;
	public float attackRadius = 2;
	float mNextAttackTime;

	//目前是根据动画表现来定这个时间。最好不能太小，否则会状态快速反复切换会有偏差
	//暂定最小值是0.1f
	public float realDamageTime = 0.1f;
//攻击的前置时间
	float commonAttackDelay = 0.65f;
	float mNextRpcAttackTime = 0;

	public void StateAttack ()
	{
		if (preState != UnitState.Attack) {
			mNextRpcAttackTime = Time.time + commonAttackDelay;
			preState = UnitState.Attack;
			mNextAttackTime = commonAttackDelay + Time.time + realDamageTime * unitAttribute.attackInterval / anim [this.attackAnimStateName].length;
			if (nav.enabled)
				nav.isStopped=true;
		}

		if (CheckSkillAble ()) {
			state = UnitState.Skill;
			return;
		}

		if (mMainTarget == null) {
			state = UnitState.Idle;
			return;
		}
		if (isMelee) {
            if (Vector3.Distance (mMainTarget.transform.position, mTrans.position) > (unitAttribute.RealAttackRange * 1.5f + mMainTarget.bodyRadius)) {
				state = UnitState.Idle;
				return;
			}
		} else {
			if (Vector3.Distance (mMainTarget.transform.position, mTrans.position) > (mScanRadius + 1 + mMainTarget.bodyRadius)) {
				state = UnitState.Idle;
				return;
			}
		}
		if (mMainTarget.unitAttribute.currentHealth <= 0) {
			state = UnitState.Idle;
			return;
		}

		if (nav.enabled)
			nav.isStopped=true;
		Vector3 forward = mMainTarget.transform.position - mTrans.position;
		forward.y = 0;
		if (forward != Vector3.zero)
			mTrans.rotation = Quaternion.LookRotation (forward);
		
		if (mNextRpcAttackTime < Time.time) {
			mNextRpcAttackTime = Time.time + unitAttribute.attackInterval;
			RpcAttackState ();
		}
		if (mNextAttackTime < Time.time) {
			mNextAttackTime = Time.time + unitAttribute.attackInterval;
			if (isMelee) {
                Collider[] colls = Physics.OverlapSphere(mTrans.position, unitAttribute.RealAttackRange, targetLayer);
                int otherCount = 0;
                for (int i = 0; i < colls.Length; i++)
                {
                    if (otherCount >= unitAttribute.maxTarget - 1)
                    {
                        break;
                    }
                    Transform target = colls[i].transform;
                    if (target != mMainTarget.transform)
                    {
                        otherCount++;
                        MeleeAttack(target.GetComponent<UnitBase>(),false);
                    }
                }
                MeleeAttack (mMainTarget,true);
			} else {
                Collider[] colls = Physics.OverlapSphere(mTrans.position, unitAttribute.RealAttackRange, targetLayer);
                int otherCount = 0;
                for (int i = 0; i < colls.Length;i ++){
                    if (otherCount >= unitAttribute.maxTarget - 1)
                    {
                        break;
                    }
                    Transform target = colls[i].transform;
                    if (target != mMainTarget.transform)
                    {
                        otherCount++;
                        RemoteAttack(target.GetComponent<UnitBase>());
                    }
                }
				RemoteAttack (mMainTarget);
			}
			RpcAttack ();
		}
	}

	SkillBase mCurrentSkill;

	public void StateSkill ()
	{
		if (preState != UnitState.Skill) {
			preState = UnitState.Skill;
			if (nav.enabled)
				nav.isStopped=true;
			mCurrentSkill.OnEnter ();
		}
		mCurrentSkill.OnUpdate ();
	}

	public bool isOneShootGunEffect = false;

    void MeleeAttack (UnitBase target,bool showAttackEffect = false)
	{
        if (hitPrefab != null) {

            Transform trans = target.GetHitPoint ();
			RpcShowHitEffect (trans.position);
		}
        int damage = unitAttribute.GetHitDamage (target);
        if (isOneShootGunEffect && gunEffect != null && showAttackEffect) {
			RpcGunEffect ();
		}
        target.Damage (this, damage);
	}

	IEnumerator _NetworkServerDestoryDelay (GameObject go, float delay)
	{
		yield return new WaitForSeconds (delay);
		NetworkServer.Destroy (go);
	}

	public override void RemoteAttack (UnitBase target)
	{
		if (target == null) {
			return;
		}
		GameObject go = Instantiate<GameObject> (bulletPrefab);
		go.transform.position = shootPoint.position;
		go.transform.forward = transform.forward;
		Bullet bullet = go.GetComponent<Bullet> ();
		Vector3 targetPos = target.GetHitPoint ().position;
		if (isOneShootGunEffect && gunEffect) {
			RpcGunEffect ();
		}
		bullet.Shoot (this, target, 80, targetPos, this.targetLayer, true);
		RpcRangeAttack (80, targetPos, 0, this.targetLayer);
	}

	[ClientRpc]
	public void RpcRangeAttack (int speed, Vector3 targetPos, int damage, int layer)
	{
		GameObject go = Instantiate (bulletPrefab, shootPoint.position, shootPoint.rotation) as GameObject;
		Bullet bullet = go.GetComponent<Bullet> ();
		if (hitAudioClips != null && hitAudioClips.Length > 0) {
			bullet.hitAudioClip = hitAudioClips [Random.Range (0, hitAudioClips.Length)];
		}
		bullet.Shoot (null, null, speed, targetPos, layer, false);
	}

	public GameObject gunEffect;
	public float gunEffectUnspawnDelay = 2;

	[ClientRpc]
	public void RpcGunEffect ()
	{
		GameObject go = BattleFramework.PoolManager.SingleTon ().Spawn (gunEffect, shootPoint.position, shootPoint.rotation);
		BattleFramework.PoolManager.SingleTon ().UnSpawn (gunEffectUnspawnDelay, go);
	}

	public string moveAnimStateName = "Run01";
	public string idleAnimStateName = "StandBy01";
	public string attackAnimStateName = "StandBy01";
	public string deathAnimStateName = "Death01";

	public AudioClip[] moveAudioClips;
	public AudioClip[] attackAudioClips;
	public AudioClip[] deathAudioClips;
	public AudioClip[] hitAudioClips;

	public void StateMove ()
	{
		if (preState != UnitState.Move) {
			preState = UnitState.Move;
			mNextScanTime = mScanInterval + Time.time;
			RpcMoveState ();
		}

		if (nav.enabled)
			nav.isStopped = false;

		if (CheckSkillAble ()) {
			state = UnitState.Skill;
			return;
		}

		if (mMainTarget != null) {
			if (nav.enabled) {
				nav.SetDestination (mMainTarget.transform.position);
				targetPos = mMainTarget.transform.position;
			}
            if (Vector3.Distance (mMainTarget.transform.position, mTrans.position) <= unitAttribute.RealAttackRange + mMainTarget.bodyRadius) {
				if (nav.enabled)
					nav.isStopped=true;
				state = UnitState.Attack;
			}
		}

		if (mNextScanTime < Time.time) {
			mNextScanTime = mScanInterval + Time.time;
			Scan ();
		}
	}

	public float mScanInterval = 0.1f;
	float mNextScanTime;

	void Scan ()
	{
        mMainTarget = null;
		if (targetLayers.Count > 0) {
			Collider[] colls = Physics.OverlapSphere (mTrans.position, mScanRadius, targetLayer);
			if (colls != null && colls.Length > 0) {
				Transform target = null;
				target = colls [Random.Range (0, colls.Length)].transform;
				if (target != null) {
					mMainTarget = target.GetComponent<UnitBase> ();
					state = UnitState.Move;
				}
			}
		}
		if (mMainTarget == null && defaultTarget != null) {
			if (nav.enabled) {
				nav.SetDestination (defaultTarget.position);
				targetPos = defaultTarget.position;
			}
			state = UnitState.Move;
		}
	}

	public void Move (Vector3 targetPos)
	{
		nav.destination = targetPos;
		state = UnitState.Move;
	}

	[ClientRpc]
	public void RpcMoveState ()
	{
		this.state = UnitState.Move;
		anim.Stop ();
		anim.Play (moveAnimStateName);

		if (moveAudioClips != null && moveAudioClips.Length > 0) {
			GetComponent<AudioSource> ().clip = moveAudioClips [Random.Range (0, moveAudioClips.Length)];
			if (LocalSoundManager.SingleTon ().IsPlayable (GetComponent<AudioSource> ().clip)) {
				GetComponent<AudioSource> ().Play ();
			}
		}
	}

	[ClientRpc]
	public void RpcShowHitEffect (Vector3 pos)
	{
		GameObject go = BattleFramework.PoolManager.SingleTon ().Spawn (hitPrefab, pos, Quaternion.identity);
		//go.GetComponent<ParticleEmitter> ().emit = true;
		BattleFramework.PoolManager.SingleTon ().UnSpawn (0.2f, go);
	}

	[ClientRpc]
	public void RpcAttackState ()
	{
		this.state = UnitState.Attack;
		anim.Stop ();
		anim.Play (attackAnimStateName);
        unitAttribute.attackInterval = Mathf.Max(unitAttribute.attackInterval,0.01f);
		anim [this.attackAnimStateName].speed = anim [this.attackAnimStateName].length / unitAttribute.attackInterval;
	}

	[ClientRpc]
	public void RpcAttack ()
	{
		if (attackAudioClips != null && attackAudioClips.Length > 0) {
			GetComponent<AudioSource> ().clip = attackAudioClips [Random.Range (0, attackAudioClips.Length)];
			if (LocalSoundManager.SingleTon ().IsPlayable (GetComponent<AudioSource> ().clip)) {
				GetComponent<AudioSource> ().Play ();
			}
		}
	}

	[ClientRpc]
	public void RpcDeathState ()
	{
		anim.wrapMode = WrapMode.Once;
		anim.Play (this.deathAnimStateName);

		if (deathAudioClips != null && deathAudioClips.Length > 0) {
			GetComponent<AudioSource> ().clip = deathAudioClips [Random.Range (0, deathAudioClips.Length)];
			GetComponent<AudioSource> ().Play ();
		}
		enabled = false;
	}

}