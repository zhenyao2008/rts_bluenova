using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneUtility  {

	public static void LoadBattle(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_BATTLE);
	}

	public static void LoadServerManage(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_SERVER_MANAGE);
	}
}
