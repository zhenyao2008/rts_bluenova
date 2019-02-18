using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessageDialog : DialogBase {

	public Text txt_msg;

	public void ShowMessage(string msg){
		txt_msg.text = msg;
	}

}
