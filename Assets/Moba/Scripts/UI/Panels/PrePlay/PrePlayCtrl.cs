using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrame
{
	public class PrePlayCtrl : BaseCtrl
	{
		PrePlayPanelView mPrePlayPanelView;

		public override void ShowPanel (Hashtable parameters)
		{
			base.ShowPanel (parameters);
			bool isCreate;
			mPrePlayPanelView = UIMgr.ShowPanel<PrePlayPanelView> (UIManager.UILayerType.Common, out isCreate);
			int playerIndex = (int)parameters["playerIndex"];
			int isPlayer0Ready = (int)parameters["isPlayer0Ready"];
			int isPlayer1Ready = (int)parameters["isPlayer1Ready"];
			bool isAIMode  = (bool)parameters["isAIMode"];

//			Button mCurrentReadyButton;
//			if (playerIndex == 0) {
//				mCurrentReadyButton = mPrePlayPanelView.readyButton;
//				if (isPlayer0Ready != 1) {
//					mPrePlayPanelView.readyButton.enabled = true;
//					mPrePlayPanelView.readyButton.onClick.RemoveAllListeners ();
//					mPrePlayPanelView.readyButton.onClick.AddListener (PlayerController_III.ClientReady);
//					mPrePlayPanelView.race0.enabled = true;
//				} else if (isPlayer0Ready == 1) {
//					mPrePlayPanelView.race0.enabled = false;
//				}
//				if (isAIMode || isPlayer1Ready != -1) {
//					mPrePlayPanelView.readyButton1.enabled = false;
//					mPrePlayPanelView.race1.enabled = false;
//					mPrePlayPanelView.race1.value = mPrePlayPanelView.race1.items [playerRace1];
//				} else if (isPlayer1Ready == -1) {
//					mServerMsgPanel.readyButton1.isEnabled = true;
//					mServerMsgPanel.readyButton1.GetComponentInChildren<UILabel> ().text = "Add AI";
//					mServerMsgPanel.race1.GetComponent<UIButton> ().isEnabled = true;
//					mServerMsgPanel.readyButton1.onClick.Clear ();
//					mServerMsgPanel.readyButton1.onClick.Add (new global::EventDelegate (AddAI));
//				}
//			} else if (playerIndex == 1) {
//				mCurrentReadyButton = mServerMsgPanel.readyButton1;
//				mServerMsgPanel.readyButton.isEnabled = false;
//				mServerMsgPanel.race0.GetComponent<UIButton> ().isEnabled = false;
//				mServerMsgPanel.race0.value = mServerMsgPanel.race0.items [playerRace0];
//
//				if (isPlayer1Ready != 1) {
//					mServerMsgPanel.readyButton1.isEnabled = true;
//					mServerMsgPanel.race1.GetComponent<UIButton> ().isEnabled = true;
//				}
//
//				mServerMsgPanel.readyButton1.onClick.Clear ();
//				mServerMsgPanel.readyButton1.onClick.Add (new global::EventDelegate (ClientReady));
//			}
//
//			if (isPlayer0Ready == -1) {
//				mServerMsgPanel.playerMsg0.text = "Empty";
//			} else if (isPlayer0Ready == 0) {
//				mServerMsgPanel.playerMsg0.text = "Not Ready";
//			} else if (isPlayer0Ready == 1) {
//				mServerMsgPanel.playerMsg0.text = "Ready";
//			}
//
//			if (isPlayer1Ready == -1) {
//				if (isAIMode)
//					mServerMsgPanel.playerMsg1.text = "AI";
//				else
//					mServerMsgPanel.playerMsg1.text = "Empty";
//			} else if (isPlayer1Ready == 0) {
//				mServerMsgPanel.playerMsg1.text = "Not Ready";
//			} else if (isPlayer1Ready == 1) {
//				mServerMsgPanel.playerMsg1.text = "Ready";
//			}





			mPrePlayPanelView.root.SetActive (true);
		}

		void SetItem(Transform item,int id,SpawnPoint sp){
			
		}

		public override void Close ()
		{
			base.Close ();
			mPrePlayPanelView.Close ();
		}
	}
}
