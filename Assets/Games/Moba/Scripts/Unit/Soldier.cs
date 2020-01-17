using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Soldier:NetworkBehaviour {

	public Animation anim;

	public UnitState unitState = UnitState.Idle;
	public UnitState preUnitState = UnitState.Idle;
	public UnityEngine.AI.NavMeshAgent navAgent;


	public float speed = 20;


	[SyncVar]
	public Vector3 pos;
	[SyncVar]
	public Quaternion qua;

	Transform mTrans;
	float lerpFactor = 0.2f;

	// Use this for initialization
	void Start () {
		anim.Play ("StandBy01");
		anim.wrapMode = WrapMode.Loop;
		mTrans = transform;
		pos = mTrans.position;
		qua = mTrans.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(NetworkServer.active)
		{

		}
		if(NetworkClient.active)
		{
			UpdateClient();
		}
	}

	void UpdateClient()
	{
		mTrans.position = Vector3.Lerp (mTrans.position,pos,lerpFactor);
		mTrans.rotation = Quaternion.Lerp (mTrans.rotation,qua,lerpFactor);
	}

	void UpdateServer()
	{
		switch(unitState)
		{
			case UnitState.Idle:;break;
			case UnitState.Move:Move();break;

		}
	}

	void Move()
	{

	}

}
