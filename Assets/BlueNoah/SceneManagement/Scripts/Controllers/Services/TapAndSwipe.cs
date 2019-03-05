// using BlueNoah.Event;
// using TD;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections.Generic;

// public sealed class TapAndSwipe : SingletonMonoBehaviour<TapAndSwipe>
// {
//     private EffectSpawnService mEffectSpawnService;
//     private EasyInput mEasyInputInstance;
//     private GameObject mSwipeEffect;
//     Dictionary<int, GameObject> mSwipeEffectDic;

//     public TapAndSwipe()
//     {
//         mEffectSpawnService = new EffectSpawnService();
//     }

//     void Start()
//     {
//         RegisterEvent();
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         RegisterEvent();
//     }

//     void RegisterEvent()
//     {
//         if (mEasyInputInstance == EasyInput.Instance)
//             return;

//         mSwipeEffectDic = new Dictionary<int, GameObject>();

//         EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.Click, OnClick);

//         EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.Touch, OnTouch);

//         EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.TouchEnd, OnTouchUp);
//     }

//     void OnClick(EventData eventData)
//     {
//         Debug.Log("OnClick");
//         GameObject effect = mEffectSpawnService.GetTapEffect();
//         // effect.transform.SetParent(this.transform);
//         SetEffectPosition(effect, eventData);
//         Destroy(effect, 3f);
//     }

//     void OnTouch(EventData eventData)
//     {
//         //3mm距離以上
//         if (Vector3.Distance(eventData.currentTouch.startTouch.position, eventData.currentTouch.touch.position) / Screen.dpi * 25.4f > 3)
//         {
//             if (!mSwipeEffectDic.ContainsKey(eventData.currentTouch.touch.fingerId))
//             {
//                 mSwipeEffectDic.Add(eventData.currentTouch.touch.fingerId, null);
//             }
//             if (mSwipeEffectDic[eventData.currentTouch.touch.fingerId] == null)
//             {
//                 GameObject swipeEffect = mEffectSpawnService.GetSwipeEffect();
//                 // swipeEffect.transform.SetParent(this.transform);
//                 mSwipeEffectDic[eventData.currentTouch.touch.fingerId] = swipeEffect;
//             }
//             ParticleSystem particle = mSwipeEffectDic[eventData.currentTouch.touch.fingerId].GetComponent<ParticleSystem>();
//             particle.Play(true);
//             float speed = eventData.currentTouch.touch.deltaPosition.magnitude / Time.deltaTime;
//             ParticleSystem[] particles = mSwipeEffectDic[eventData.currentTouch.touch.fingerId].GetComponentsInChildren<ParticleSystem>(true);
//             for (int i = 0; i < particles.Length; i++)
//             {
//                 ParticleSystem.MinMaxCurve minMaxCurve = particles[i].emission.rateOverTime;
//                 ParticleSystem.EmissionModule emissionModule = particles[i].emission;
//                 minMaxCurve.constantMin = 40 * Mathf.Clamp(speed / 500f, 1, 3);
//                 minMaxCurve.constantMax = 80 * Mathf.Clamp(speed / 500f, 1, 3);
//                 emissionModule.rateOverTime = minMaxCurve;
//             }
//             SetEffectPosition(mSwipeEffectDic[eventData.currentTouch.touch.fingerId], eventData);
//         }
//     }

//     void OnTouchUp(EventData eventData)
//     {
//         if (mSwipeEffectDic.ContainsKey(eventData.currentTouch.touch.fingerId))
//         {
//             GameObject swipeEffect = mSwipeEffectDic[eventData.currentTouch.touch.fingerId];
//             if (swipeEffect != null)
//             {
//                 swipeEffect.GetComponent<ParticleSystem>().Stop();
//                 GameObject.Destroy(swipeEffect, 3f);
//             }
//             mSwipeEffectDic.Remove(eventData.currentTouch.touch.fingerId);
//         }
//     }

//     void SetEffectPosition(GameObject effect, EventData eventData)
//     {
//         Camera targetCamera;
//         if (GhostUI.GhostUIEngine.Instance != null)
//         {
//             targetCamera = GhostUI.GhostUIEngine.Instance.Camera();
//         }
//         else
//         {
//             targetCamera = Camera.main;
//         }
//         Vector3 pos = targetCamera.ScreenToWorldPoint(eventData.currentTouch.touch.position);
//         effect.transform.position = pos + targetCamera.transform.forward * 3;
//         effect.transform.rotation = targetCamera.transform.rotation;
//     }
// }
