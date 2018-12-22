using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah.UI
{
    public class UnitUI : MonoBehaviour
    {

        GameObject mTargetUnit;

        UnitBase mUnitBase;

        Renderer mMainRenderer;

        Image mImgArrow;

        int xDir = 0;

        int yDir = 0;

        private void Awake()
        {
            mImgArrow = GetComponentInChildren<Image>();
        }

        public void SetUnit(GameObject targetUnit)
        {
            mTargetUnit = targetUnit;
            mUnitBase = targetUnit.GetComponent<UnitBase>();
        }

        public bool IsInView(Vector3 worldPos)
        {
            Transform camTransform = Camera.main.transform;
            Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);//判断物体是否在相机前面

            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                return true;
            else
                return false;
        }

        void LateUpdate()
        {
            if (mTargetUnit != null)
            {
                if (mUnitBase.playerIndex == 0)
                {
                    mImgArrow.color = new Color(0.1f, 1, 1, 1);
                }
                else
                {
                    mImgArrow.color = new Color(0.7f, 0.1f, 0, 1);
                }


                Vector3 pos = Camera.main.WorldToScreenPoint(mTargetUnit.transform.position);// + new Vector3(0, 0.2f, 0) + new Vector3(0, mTargetUnit.transform.localScale.y * mTargetUnit.GetComponent<CapsuleCollider>().height, 0));
                Vector2 anchordPosition = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2);
                if (anchordPosition.x < -Screen.width / 2f)
                {
                    xDir = -1;
                }
                else if (anchordPosition.x > Screen.width / 2f)
                {
                    xDir = 1;
                }
                else
                {
                    xDir = 0;
                }

                if (anchordPosition.y < -Screen.height / 2f)
                {
                    yDir = -1;
                }
                else if (anchordPosition.y > Screen.height / 2f)
                {
                    yDir = 1;
                }
                else
                {
                    yDir = 0;
                }
                float angle = 0;
                if (IsInView(transform.position))
                {
                    mImgArrow.enabled = false;
                }
                else
                {
                    mImgArrow.enabled = true;
                    if (xDir == 1 && yDir == 0)
                    {
                        angle = 0;
                    }
                    else if (xDir == 1 && yDir == -1)
                    {
                        angle = -45;
                    }
                    else if (xDir == 0 && yDir == -1)
                    {
                        angle = -90;
                    }
                    else if (xDir == -1 && yDir == -1)
                    {
                        angle = -135;
                    }
                    else if (xDir == -1 && yDir == 0)
                    {
                        angle = -180;
                    }
                    else if (xDir == -1 && yDir == 1)
                    {
                        angle = -225;
                    }
                    else if (xDir == 0 && yDir == 1)
                    {
                        angle = -270;
                    }
                    else
                    {
                        mImgArrow.enabled = false;
                    }
                    transform.eulerAngles = new Vector3(0, 0, angle);
                    anchordPosition = new Vector2(Mathf.Clamp(anchordPosition.x, -Screen.width / 2f + 20, Screen.width / 2f - 20), Mathf.Clamp(anchordPosition.y, -Screen.height / 2f + 25, Screen.height / 2f - 25));
                }
                GetComponent<RectTransform>().anchoredPosition = anchordPosition;
                //TODO the ui when out of screen.
                gameObject.SetActive(mTargetUnit.gameObject.activeInHierarchy);
            }
        }
    }
}
