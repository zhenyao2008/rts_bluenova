using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum UnitState{Idle,Move,Attack,NavOffLink,Death,Special,Skill,Skill01,Skill02};
public class UnitBase : NetworkBehaviour {

	public delegate void OnUnitDelegate(UnitBase ub);
	public OnUnitDelegate onDead;//被杀死

//	public IPlayerController player;
	public PlayerAttribute playerAttribute;
	public int playerIndex;

	public UnitAttribute unitAttribute;
	public List<int> targetLayers;
	public int targetLayer;

	public bool useUnitRes;
	public UnitRes unitRes;
	public string unitResPath;

	public float bodyRadius;//身体半径

//	public BaseState defaultAttackState;
	public SkillBase[] attackActions;
	public bool isMelee = true;
	public GameObject bulletPrefab;
	public Transform shootPoint;
	
	public UnityEngine.AI.NavMeshAgent nav;
	public Animation anim;

	public List<UnitRenderer> playerRenderers;

	[SyncVar]
	public UnitState state = UnitState.Idle;

	public GameObject hitPrefab;

	public bool isFlying = false;

	//自身被动
	public List<DamageReduction> damageReductions= new List<DamageReduction> ();//绝对伤害减免
	public List<DamageIncrease> damageIncreases = new List<DamageIncrease> ();//额外绝对伤害
	public List<ArmorIncrease> armorIncreases= new List<ArmorIncrease> ();
	public List<AttackIncreasePercent> attackIncreasePercents = new List<AttackIncreasePercent> ();
//	public List<HealthRecover> healthRecover;

	//光环类被动
	public ArmorIncrease armorIncreasesByLightRing;

	public Dictionary<System.Type,BuffBase> buffDics = new Dictionary<System.Type, BuffBase> ();
	public List<System.Type> oldBuffs = new List<System.Type>();

	protected Transform mTrans;

	public virtual void Awake()
	{
		mTrans = transform;
	}

	public virtual void Start(){
		targetLayer = 0;
		foreach(int ly in targetLayers)
		{
			targetLayer = targetLayer | 1<<ly;
		}
		if (GetComponent<AudioSource> ()) {
			GetComponent<AudioSource> ().maxDistance = LocalSoundManager.SingleTon().commonMaxDistance;
			GetComponent<AudioSource> ().spatialBlend = LocalSoundManager.SingleTon().commonSpatialBlend;
			GetComponent<AudioSource> ().rolloffMode = AudioRolloffMode.Linear;
		}
			
		if (NetworkServer.active) {
			attackActions = GetComponents<SkillBase>();
			for(int i=0;i<attackActions.Length;i++)
			{
				attackActions[i].unitBase = this;
				attackActions[i].OnAwake();
			}
		}
	}

	[ClientRpc]
	public void RpcInitTrans(Vector3 p,Quaternion q,int pi)
	{
		if (useUnitRes) {
			GameObject prefab = Resources.Load<GameObject>(unitResPath);
			unitRes = Instantiate<GameObject>(prefab).GetComponent<UnitRes>();
			unitRes.transform.parent = mTrans;
			unitRes.transform.localPosition = Vector3.zero;
		}
		
		mTrans.position = p;
		mTrans.rotation = q;
		playerIndex = pi;
		anim.gameObject.SetActive(true);
		for(int i=0;i<playerRenderers.Count;i++)
		{
			if(playerIndex==0)
			{
				playerRenderers[i].renderer.materials = playerRenderers[i].mats0;
			}
			else if(playerIndex==1)
			{
				playerRenderers[i].renderer.materials = playerRenderers[i].mats1;
			}
		}
	}

	public virtual Transform GetHitPoint()
	{
		return transform;
	}

	public virtual void Damage(UnitBase attacker,int damage)
	{
		if(unitAttribute!=null)
		{
			unitAttribute.OnDamage(attacker.unitAttribute,damage);
		}
	}

	public virtual void ShowMsgTips(int index,string msg,Color color,float duration,Vector3 offset){
	
	}

	//当杀死敌人时候调用
	public virtual void OnKillEnemy(UnitBase target)
	{

	}

	public void ChangeClientState(string animName,UnitState unitState)
	{
		RpcChangeClientState (animName, unitState);
	}

	[ClientRpc]
	public void RpcChangeClientState(string animName,UnitState unitState)
	{
		state = unitState;
		anim.Play (animName);
	}

	public virtual void RemoteAttack(UnitBase ub)
	{
		
	}

	Dictionary <int,GameObject> ringGos = new Dictionary<int, GameObject>();

	public void AddRing(int prefabIndex){
		RpcAddRing (prefabIndex);
	}

	public void RemoveRing(int prefabIndex){
		RpcRemoveRing (prefabIndex);
	}

	[ClientRpc]
	public void RpcAddRing(int prefabIndex)
	{
		Debug.Log ("RpcAddRing");
		if(ringGos.ContainsKey(prefabIndex))
		{
			return;
		}
		GameObject prefab = LocalEffectManager.SingleTon().ringPrefabs[prefabIndex];
		GameObject go = Instantiate (prefab, transform.position, transform.rotation) as GameObject;
		go.transform.parent = transform;
		ringGos.Add (prefabIndex,go);
	}

	[ClientRpc]
	public void RpcRemoveRing(int prefabIndex)
	{
		if(!ringGos.ContainsKey(prefabIndex))
		{
			return;
		}
		GameObject go = ringGos [prefabIndex];
		Destroy (go);
		ringGos.Remove (prefabIndex);
	}


}

[System.Serializable]
public class UnitRenderer
{
	public Renderer renderer;
	public Material[] mats0;
	public Material[] mats1;
	public Material[] mats2;
	public Material[] mats3;
}

