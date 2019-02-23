using System.Collections;
using TMPro;
using UnityEngine;

namespace BlueNoah.AI.View.RTS
{
    public class ActorHeadUI : MonoBehaviour
    {

        TextMeshProUGUI mTextMeshProUGUI;

        ActorViewer mActorViewer;

        void Awake()
        {
            mTextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        public void SetUnitCore(ActorViewer actorViewer)
        {
            this.mActorViewer = actorViewer;
        }

        void LateUpdate()
        {
            if (mActorViewer != null)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(mActorViewer.transform.position + new Vector3(0, mActorViewer.GetComponent<CapsuleCollider>().height * 0.8f, 0));
                GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2);
            }
            gameObject.SetActive(mActorViewer.gameObject.activeInHierarchy);
        }

        Coroutine mShowTextCoroutine;

        public void ShowTextDialog(string text, float hideDelay)
        {
            this.mTextMeshProUGUI.text = text;
            if (mShowTextCoroutine != null)
            {
                StopCoroutine(mShowTextCoroutine);
                mShowTextCoroutine = null;
            }
            mShowTextCoroutine = StartCoroutine(_ShowTextDialog(text, hideDelay));
        }

        IEnumerator _ShowTextDialog(string text, float hideDelay)
        {
            this.mTextMeshProUGUI.text = text;
            this.mTextMeshProUGUI.enabled = true;
            yield return new WaitForSeconds(hideDelay);
            this.mTextMeshProUGUI.enabled = false;
        }
    }
}