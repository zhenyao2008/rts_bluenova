using UnityEngine;

namespace BlueNoah.RPG.View
{

    public class ActorAsset : MonoBehaviour
    {

        //List<Renderer> mUnitRenderers;

        //Transform mArtTrans;

        //HighlighterOccluder mHighlighterOccluder;

        //SkinnedMeshRenderer mHeadSkinnedMeshRenderer;

        //public Transform meshRoot;

        //public Transform shadow;

        //public float defaultOffsetHeight = 0;

        void Awake()
        {

            //mUnitRenderers = new List<Renderer>();

            //mUnitRenderers.AddRange(GetComponentsInChildren<Renderer>());
        }

        public void SetModel(GameObject artGameObject)
        {
            //if (mArtTrans != null)
            //{
            //    DestroyImmediate(mArtTrans.gameObject);
            //}
            //Transform artTransform = artGameObject.transform;
            //artTransform.SetParent(transform);
            ////モデルのサイズはクレーデブ側決めるので、これで変更必要がない。
            ////artTransform.localScale = Vector3.one;
            //artTransform.localEulerAngles = Vector3.zero;
            //artTransform.localPosition = Vector3.zero;
            //artGameObject.SetActive(true);
            //meshRoot = transform.Find("body");
            //mHighlighterOccluder = meshRoot.gameObject.GetOrAddComponent<HighlighterOccluder>();
            //mHighlighterOccluder.enabled = false;
            //meshRoot.gameObject.GetOrAddComponent<Highlighter>().enabled = false;
            //mArtTrans = artTransform;
            //SkinnedMeshRenderer[] skinnedMeshRenderers = meshRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            //List<SkinnedMeshRenderer> combineSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();
            //for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            //{
            //    if (skinnedMeshRenderers[i].name == "head")
            //    {
            //        mHeadSkinnedMeshRenderer = skinnedMeshRenderers[i];
            //    }
            //    else
            //    {
            //        combineSkinnedMeshRenderers.Add(skinnedMeshRenderers[i]);
            //    }
            //}
            ////TODO 合併したプレハブを保存すべき、もし複数ゲームオブジェクトを同じいプレハーブを繋げる場合、今はない。 
            //SkinnedMeshCombineUtility.Combine(meshRoot.gameObject);

            //SkinnedMeshRenderer skinnedMeshRenderer = SkinnedMeshCombineUtility.Combine(meshRoot.gameObject, combineSkinnedMeshRenderers.ToArray());

            //skinnedMeshRenderer.receiveShadows = false;

            //skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            //SetShadow(meshRoot.gameObject);
        }

        void Update()
        {
            //if (shadow != null)
            //{
            //    shadow.position = new Vector3(shadow.position.x, 0.01f, shadow.position.z);
            //}
        }

        void SetShadow(GameObject go)
        {
            //Transform shadowTrans = go.transform.Find("ch_root/base/shadow");
            //if (shadowTrans != null)
            //    shadowTrans.gameObject.SetActive(false);
            //shadowTrans = transform.Find("shadow");
            //if (shadowTrans != null)
            //{
            //    SpriteRenderer spriteRenderer = shadowTrans.GetComponent<SpriteRenderer>();
            //    spriteRenderer.sortingOrder = 2;
            //    shadow = shadowTrans;
            //    shadow.gameObject.SetActive(true);
            //    shadow.SetParent(transform);
            //}
        }

        public void DOHideShadow()
        {
            //if (shadow != null)
            //{
            //    SpriteRenderer spriteRenderer = shadow.GetComponent<SpriteRenderer>();
            //    spriteRenderer.DOKill();
            //    spriteRenderer.DOFade(0f, 1.5f);
            //}
        }

        public void DOShowShadow()
        {
            //if (shadow != null)
            //{
            //    SpriteRenderer spriteRenderer = shadow.GetComponent<SpriteRenderer>();
            //    spriteRenderer.DOKill();
            //    spriteRenderer.DOFade(0.25f, 1.5f);
            //}
        }

        public bool IsVisible
        {
            get
            {
                bool isVisible = false;
                //for (int i = 0; i < mUnitRenderers.Count; i++)
                //{
                //    if (mUnitRenderers[i] != null && mUnitRenderers[i].isVisible)
                //    {
                //        isVisible = mUnitRenderers[i].isVisible;
                //        break;
                //    }
                //}
                return isVisible;
            }
        }
    }
}

