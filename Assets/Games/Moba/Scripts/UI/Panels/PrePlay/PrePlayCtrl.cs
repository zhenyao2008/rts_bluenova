using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
    public class PrePlayCtrl : BaseCtrl
    {
        PrePlayPanelView mPrePlayPanelView;

        public override void ShowPanel(Hashtable parameters)
        {
            base.ShowPanel(parameters);
            bool isCreate;
            mPrePlayPanelView = UIMgr.ShowPanel<PrePlayPanelView>(UIManager.UILayerType.Common, out isCreate);
            int playerIndex = (int)parameters["playerIndex"];
            int isPlayer0Ready = (int)parameters["isPlayer0Ready"];
            int isPlayer1Ready = (int)parameters["isPlayer1Ready"];
            bool isAIMode = (bool)parameters["isAIMode"];

            Button mCurrentReadyButton;
            if (playerIndex == 0)
            {
                mCurrentReadyButton = mPrePlayPanelView.readyButton;
                if (isPlayer0Ready != 1)
                {
                    mPrePlayPanelView.readyButton.enabled = true;
                    mPrePlayPanelView.readyButton.onClick.RemoveAllListeners();
                    mPrePlayPanelView.readyButton.onClick.AddListener(PlayerController_III.instance.ClientReady);
                    mPrePlayPanelView.race0.enabled = true;
                }
                else if (isPlayer0Ready == 1)
                {
                    mPrePlayPanelView.race0.enabled = false;
                }
                if (isAIMode || isPlayer1Ready != -1)
                {
                    mPrePlayPanelView.readyButton1.enabled = false;
                    mPrePlayPanelView.race1.enabled = false;
                    mPrePlayPanelView.race1.value = 0;
                }
                else if (isPlayer1Ready == -1)
                {
                    mPrePlayPanelView.readyButton1.enabled = true;
                    mPrePlayPanelView.readyButton1.GetComponentInChildren<Text>().text = "Add AI";
                    mPrePlayPanelView.race1.enabled = true;
                    mPrePlayPanelView.readyButton1.onClick.RemoveAllListeners();
                    mPrePlayPanelView.readyButton1.onClick.AddListener(PlayerController_III.instance.AddAI);
                }
            }
            else if (playerIndex == 1)
            {
                mCurrentReadyButton = mPrePlayPanelView.readyButton1;
                mPrePlayPanelView.readyButton.enabled = false;
                mPrePlayPanelView.race0.enabled = false;
                mPrePlayPanelView.race0.value = 0;

                if (isPlayer1Ready != 1)
                {
                    mPrePlayPanelView.readyButton1.enabled = true;
                    mPrePlayPanelView.race1.enabled = true;
                }
                mPrePlayPanelView.readyButton1.onClick.RemoveAllListeners();
                mPrePlayPanelView.readyButton1.onClick.AddListener(PlayerController_III.instance.ClientReady);
            }

            if (isPlayer0Ready == -1)
            {
                mPrePlayPanelView.playerMsg0.text = "Empty";
            }
            else if (isPlayer0Ready == 0)
            {
                mPrePlayPanelView.playerMsg0.text = "Not Ready";
            }
            else if (isPlayer0Ready == 1)
            {
                mPrePlayPanelView.playerMsg0.text = "Ready";
            }

            if (isPlayer1Ready == -1)
            {
                if (isAIMode)
                    mPrePlayPanelView.playerMsg1.text = "AI";
                else
                    mPrePlayPanelView.playerMsg1.text = "Empty";
            }
            else if (isPlayer1Ready == 0)
            {
                mPrePlayPanelView.playerMsg1.text = "Not Ready";
            }
            else if (isPlayer1Ready == 1)
            {
                mPrePlayPanelView.playerMsg1.text = "Ready";
            }
            mPrePlayPanelView.root.SetActive(true);
        }

        void SetItem(Transform item, int id, SpawnPoint sp)
        {

        }

        public override void Close()
        {
            base.Close();
            mPrePlayPanelView.Close();
        }
    }
}
