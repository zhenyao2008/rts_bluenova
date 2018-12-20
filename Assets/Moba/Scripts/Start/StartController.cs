using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartController : MonoBehaviour {

    public CanvasGroup containerBack;

    public CanvasGroup containerLogo;

	// Use this for initialization
    IEnumerator Start () {
        yield return new WaitForSeconds(1f);
        containerBack.DOFade(1, 2f);
        yield return new WaitForSeconds(1f);
        containerLogo.DOFade(1, 1.3f);
        yield return new WaitForSeconds(3f);
        containerLogo.DOFade(0, 1.3f);
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("Main");
	}
	
	
}
