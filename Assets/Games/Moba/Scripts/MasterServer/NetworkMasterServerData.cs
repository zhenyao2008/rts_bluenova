using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class NetworkMasterServerData
{
	static NetworkMasterServerData instance;
	public static NetworkMasterServerData SingleTon()
	{
		if(instance == null)
		{
			instance = new NetworkMasterServerData();
		}
		return instance;
	}

	private NetworkMasterServerData(){

	}

	//---------------Logic--------------
	Dictionary<string,User> mUsers = new Dictionary<string, User>();//用字典方便查找
	Dictionary<int,User> mConnUsers = new Dictionary<int, User>();//用字典方便查找
	public List<User> onlinePlaySearchingUsers = new List<User> ();


	public bool ContainUser(User user)
	{
		return mUsers.ContainsKey(user.userName) || mConnUsers.ContainsKey(user.connectionId); 
	}
	public bool AddUser(User user)
	{
		if(ContainUser(user))
		{
			return false;
		}
		mUsers.Add (user.userName,user);
		mConnUsers.Add (user.connectionId,user);
		return true;
	}
	public bool GetUser(string userName,out User user)
	{
		if(mUsers.TryGetValue(userName,out user))
		{
			return true;
		}
		return false;
	}
	public bool GetUser(int conn,out User user)
	{
		if(mConnUsers.TryGetValue(conn,out user))
		{
			return true;
		}
		return false;
	}
	public void RemoveUser(int connectionId)
	{
		if(mConnUsers.ContainsKey(connectionId))
		{
			User user = mConnUsers[connectionId];
			mConnUsers.Remove(connectionId);
			mUsers.Remove(user.userName);
		}
	}
	public void RemoveUser(string userName)
	{
		
	}
}

public class User
{
	public string userName;
	public int connectionId;
	public NetworkConnection conn;
	public int level;
}








