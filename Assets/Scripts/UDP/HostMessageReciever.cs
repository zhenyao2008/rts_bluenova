using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;
using System;
using UnityEngine.Events;

public class HostMessageReciever : SingleMonoBehaviour<HostMessageReciever> {
	public static Dictionary<string,float> ips;
	//メセージを監視用UDPクライアント。
	public static UdpClient udp;
	//メセージを監視用スレッド
	public Thread thread;
	static float time;
	static int[] ints;
	const int LOCAL_PORT = 50001;
	public static bool isRunning = true;

	protected override void Awake ()
	{
		base.Awake ();
		//監視しているポート
		ips = new Dictionary<string, float> ();
		udp = new UdpClient(LOCAL_PORT);
		thread = new Thread(new ThreadStart(ThreadMethod));
		thread.IsBackground = true;
		thread.Start();
	}

	void Update(){
		time = Time.time;	
	}


	public void StopReceive(){
		Debug.Log ("StopReceive");
		thread.Abort();
		udp.Close ();
	}

	private static void ThreadMethod()
	{
		while(isRunning)
		{
			//メセージを受け取っていない時、読み取ない。
			if (udp.Available == 0) {
				Thread.Sleep (100);
				continue;
			}
			IPEndPoint remoteEP = null;
			byte[] data = udp.Receive(ref remoteEP);
			if (!ips.ContainsKey (remoteEP.Address.ToString ()))
				ips.Add (remoteEP.Address.ToString (),time);
			else
				ips[remoteEP.Address.ToString ()] = time;
		}
		Debug.Log ("Thread Done!");
	} 

	void OnApplicationQuit(){
		thread.Abort();
	}

	//	static int GetInt()
	//	{
	//		if (ints.Length <= mRecieverIndex)
	//		{
	//			Debug.LogError(string.Format("mRecieverIndex:{0} not exsiting!", mRecieverIndex));
	//			return 0;
	//		}
	//		int v = ints[mRecieverIndex];
	//		mRecieverIndex++;
	//		return v;
	//	}

}