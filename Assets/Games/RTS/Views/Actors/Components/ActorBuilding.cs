/*
 *　2019.2.29 午後７時４分、横浜青葉台
 *　應　彧剛(yingyugang@gmail.com)
*/
using System.Collections.Generic;
using BlueNoah.PathFinding.FixedPoint;
using DG.Tweening;
using UnityEngine;

namespace BlueNoah.RPG.View
{
    public class ActorBuilding : MonoBehaviour
    {

        public int xSize;

        public int ySize;

        public BoxCollider buildBlockArea;

        public BoxCollider unwalkableArea;

        public GameObject meshRoot;

        public List<FixedPointNode> currentNodes;

        Vector3 mTargetScale;

        Vector3 mTargetScale1;

        Vector3 mDefaultScale;

        void Awake()
        {
            buildBlockArea = transform.Find("colliders/collider").GetComponent<BoxCollider>();

            unwalkableArea = transform.Find("unwalkables/unwalkable").GetComponent<BoxCollider>();

            mDefaultScale = transform.localScale;

            mTargetScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 0.9f, transform.localScale.z * 1.1f);

            mTargetScale1 = new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 1.1f, transform.localScale.z * 0.9f);
        }

        public void OnSpring()
        {
            transform.DOScale(mTargetScale, 0.11f).OnComplete(() =>
            {
                transform.DOScale(mTargetScale1, 0.11f).OnComplete(() =>
                {
                    transform.DOScale(mDefaultScale, 0.11f);
                });
            });
        }
    }
}