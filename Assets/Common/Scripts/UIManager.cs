using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace Framework
{
    public class UIManager : MonoBehaviour
    {

        static UIManager instance;
        public static UIManager Instance()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("_UIManager");
                instance = go.AddComponent<UIManager>();
                instance.InitLayers();
                instance.InitCtrollers();
            }
            return instance;
        }

        private Dictionary<UILayerType, GameObject> m_uiLayers = new Dictionary<UILayerType, GameObject>();
        private Dictionary<string, GameObject> m_panels = new Dictionary<string, GameObject>();

        void Awake()
        {
            //instance = this;
            //InitLayers();
            //InitCtrollers();
        }

        public MainInterfaceCtrl mainInterfaceCtrl;
        public MaskCtrl maskCtrl;
        public MsgCtrl msgCtrl;
        public PlayerListCtrl playerListCtrl;
        public SettingCtrl settingCtrl;
        public ShopCtrl shopCtrl;
        public TeamListCtrl teamListCtrl;
        public AchivementCtrl achivementCtrl;

        void InitCtrollers()
        {
            mainInterfaceCtrl = InitCtroller<MainInterfaceCtrl>();
            maskCtrl = InitCtroller<MaskCtrl>();
            msgCtrl = InitCtroller<MsgCtrl>();
            playerListCtrl = InitCtroller<PlayerListCtrl>();
            settingCtrl = InitCtroller<SettingCtrl>();
            shopCtrl = InitCtroller<ShopCtrl>();
            teamListCtrl = InitCtroller<TeamListCtrl>();
            achivementCtrl = InitCtroller<AchivementCtrl>();
        }

        T InitCtroller<T>() where T : BaseCtrl
        {
            T t = gameObject.AddComponent<T>();
            t.UIMgr = this;
            return t;
        }

        public void InitLayers()
        {
            GameObject tempLayer;
            Debug.Log("InitLayers");
            //底层
            tempLayer = AddALayer("UILayer_Bottom");
            m_uiLayers.Add(UILayerType.Bottom, tempLayer);

            //场景辅助层
            tempLayer = AddALayer("UILayer_MapSceneAbout", false);
            m_uiLayers.Add(UILayerType.MapSceneAbout, tempLayer);

            //固定面板层
            tempLayer = AddALayer("UILayer_Fixed");
            m_uiLayers.Add(UILayerType.Fixed, tempLayer);

            //普通面板层
            tempLayer = AddALayer("UILayer_Mask");
            m_uiLayers.Add(UILayerType.Mask, tempLayer);

            //普通面板层
            tempLayer = AddALayer("UILayer_Common");
            m_uiLayers.Add(UILayerType.Common, tempLayer);

            //普通面板层
            tempLayer = AddALayer("UILayer_Movie");
            m_uiLayers.Add(UILayerType.Movie, tempLayer);

            //普通上层面板层
            tempLayer = AddALayer("UILayer_FontCommon");
            m_uiLayers.Add(UILayerType.FontCommon, tempLayer);
            ////进度条层
            //tempLayer = AddALayer("UILayer_Loading");
            //m_uiLayers.Add(UILayerType.Loading, tempLayer);

            //UI特效层层
            tempLayer = AddALayer("UIEffectLayer", false);
            m_uiLayers.Add(UILayerType.UIEffect, tempLayer);

            //顶层
            tempLayer = AddALayer("UILayer_Top");
            m_uiLayers.Add(UILayerType.Top, tempLayer);
        }

        private GameObject AddALayer(string name, bool mouseEventable = true)
        {
            GameObject canvas = GameObject.FindWithTag("UICanvas");
            GameObject retLayer;
            retLayer = new GameObject(name);
            retLayer.layer = LayerMask.NameToLayer("UI");
            RectTransform rect = retLayer.AddComponent<RectTransform>();
            retLayer.transform.SetParent(canvas.transform);
            retLayer.transform.localPosition = Vector3.zero;
            //retLayer.transform.SetSiblingIndex(1000);
            retLayer.transform.localScale = Vector3.one;
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);

            retLayer.AddComponent<CanvasGroup>().blocksRaycasts = mouseEventable;

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            return retLayer;
        }

        /// <summary>
        /// UI层类型
        /// </summary>
        public enum UILayerType
        {
            /// <summary>
            /// //最顶层
            /// </summary>
            Top,
            /// <summary>
            /// //UI特效层
            /// </summary>
            UIEffect,
            /// <summary>
            /// //普通上一层
            /// </summary>
            FontCommon,
            /// <summary>
            /// //任务、剧情
            /// </summary>
            Movie,
            /// <summary>
            /// //普通面板层
            /// </summary>
            Common,
            /// <summary>
            /// //固定面板层
            /// </summary>
            Fixed,
            /// <summary>
            /// //遮罩层
            /// </summary>
            Mask,
            /// <summary>
            /// 场景辅助层
            /// </summary>
            MapSceneAbout,
            /// <summary>
            /// //最底层
            /// </summary>
            Bottom
        }

        //单独显示panel
        public GameObject ShowPanel(UILayerType type, string panelStr, out bool isCreate)
        {
            GameObject panelGo = null;
            if (m_panels.ContainsKey(panelStr))
            {
                panelGo = m_panels[panelStr];
                isCreate = false;
            }
            else
            {
                GameObject prefab = ResourceManager.Instance().LoadLocalPanelPrefab(panelStr);
                panelGo = Instantiate<GameObject>(prefab);
                m_panels.Add(panelStr, panelGo);
                panelGo.transform.parent = m_uiLayers[type].transform;
                panelGo.transform.localPosition = Vector3.zero;
                panelGo.transform.localRotation = Quaternion.identity;
                panelGo.transform.localScale = Vector3.one;
                RectTransform rectTrans = panelGo.GetComponent<RectTransform>();
                if (rectTrans != null)
                {
                    rectTrans.sizeDelta = Vector2.zero;
                    rectTrans.anchoredPosition = Vector2.zero;
                }
                isCreate = true;
            }
            panelGo.SetActive(true);
            return panelGo;
        }
        //显示panel并绑定Mono
        public T ShowPanel<T>(UILayerType type, string panelStr, out bool isCreate) where T :MonoBehaviour
        {
            GameObject go = ShowPanel(type, panelStr, out isCreate);
            T t = go.GetComponent<T>();
            if (t ==null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }
        //显示panel并绑定Mono（根据Mono的名字来查找panel）
        public T ShowPanel<T>(UILayerType type, out bool isCreate) where T : PanelBase
        {
            string panelStr = GetPanelByCSharp<T>();
            GameObject go = ShowPanel(type, panelStr, out isCreate);
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
                t.Awake();
            }
            return t;
        }

        //显示panel并绑定Mono（根据Mono的名字来查找panel）
        public T ShowPanel<T>(UILayerType type) where T : PanelBase
        {
            bool isCreate;
            string panelStr = GetPanelByCSharp<T>();
            GameObject go = ShowPanel(type, panelStr, out isCreate);
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
                t.Awake();
            }
            return t;
        }



        public void ClosePanel(string panelStr)
        {
            if (m_panels.ContainsKey(panelStr))
            {
                m_panels[panelStr].SetActive(false);
            }
        }


        public bool IsActived(string panelStr)
        {
            if (m_panels.ContainsKey(panelStr))
            {
                if (m_panels[panelStr].activeInHierarchy) {
                    return true;
                }
            }
            return false;
        }

        //显示/隐藏Panel
        public void TogglePanel(UILayerType type, string panelStr)
        {
            bool isCreate;
            if (m_panels.ContainsKey(panelStr))
            {
                if (m_panels[panelStr].activeInHierarchy)
                {
                    ClosePanel(panelStr);
                }
                else
                {
                    ShowPanel(type, panelStr,out isCreate);
                }
            }
            else
            {
                ShowPanel(type, panelStr,out isCreate);
            }
        }
        //显示/隐藏Panel
        public void TogglePanel<T>(UILayerType type)
        {
            string panelStr = GetPanelByCSharp<T>();
            TogglePanel(type, panelStr);
        }
        //根据Mono的名字来查找panel
        string GetPanelByCSharp<T>()
        {
            string panelStr = typeof(T).ToString();
            if (panelStr.IndexOf(".") != -1)
                panelStr = panelStr.Remove(0, panelStr.LastIndexOf(".") + 1);
            panelStr = panelStr.Replace("View", "");
            return panelStr;
        }


    }
}