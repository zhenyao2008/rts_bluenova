using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerAttribute{

	public PlayerController_III playerController;
	public int corn = 800;
	public int killNum = 0;
	public int cornPerRound = 10;//每回合加钱数

	public void OnKillEnemy(UnitBase ub)
	{
		this.corn += ub.unitAttribute.killPrice;
		this.killNum ++;
		if (playerController != null) {
			playerController.OnKillEnemy();
		}
	}

}
