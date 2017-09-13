using System;
using LuaInterface;

public class UserInfoWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateUserInfo),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("userName", get_userName, set_userName),
			new LuaField("password", get_password, set_password),
			new LuaField("userLevel", get_userLevel, set_userLevel),
			new LuaField("userExp", get_userExp, set_userExp),
			new LuaField("workNum", get_workNum, set_workNum),
			new LuaField("protectTime", get_protectTime, set_protectTime),
			new LuaField("maxCorn", get_maxCorn, set_maxCorn),
			new LuaField("currentCorn", get_currentCorn, set_currentCorn),
			new LuaField("maxWood", get_maxWood, set_maxWood),
			new LuaField("currentWood", get_currentWood, set_currentWood),
			new LuaField("maxStone", get_maxStone, set_maxStone),
			new LuaField("currentStone", get_currentStone, set_currentStone),
			new LuaField("currentBaoShi", get_currentBaoShi, set_currentBaoShi),
		};

		LuaScriptMgr.RegisterLib(L, "UserInfo", typeof(UserInfo), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUserInfo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UserInfo obj = new UserInfo();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UserInfo.New");
		}

		return 0;
	}

	static Type classType = typeof(UserInfo);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_userName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.userName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_password(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name password");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index password on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.password);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_userLevel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userLevel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userLevel on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.userLevel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_userExp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userExp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userExp on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.userExp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_workNum(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name workNum");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index workNum on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.workNum);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_protectTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name protectTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index protectTime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.protectTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxCorn(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxCorn");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxCorn on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxCorn);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentCorn(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentCorn");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentCorn on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.currentCorn);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxWood(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxWood");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxWood on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxWood);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentWood(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentWood");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentWood on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.currentWood);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxStone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxStone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxStone on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.maxStone);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentStone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentStone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentStone on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.currentStone);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentBaoShi(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentBaoShi");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentBaoShi on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.currentBaoShi);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_userName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userName on a nil value");
			}
		}

		obj.userName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_password(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name password");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index password on a nil value");
			}
		}

		obj.password = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_userLevel(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userLevel");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userLevel on a nil value");
			}
		}

		obj.userLevel = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_userExp(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name userExp");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index userExp on a nil value");
			}
		}

		obj.userExp = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_workNum(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name workNum");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index workNum on a nil value");
			}
		}

		obj.workNum = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_protectTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name protectTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index protectTime on a nil value");
			}
		}

		obj.protectTime = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxCorn(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxCorn");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxCorn on a nil value");
			}
		}

		obj.maxCorn = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_currentCorn(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentCorn");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentCorn on a nil value");
			}
		}

		obj.currentCorn = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxWood(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxWood");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxWood on a nil value");
			}
		}

		obj.maxWood = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_currentWood(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentWood");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentWood on a nil value");
			}
		}

		obj.currentWood = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxStone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name maxStone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index maxStone on a nil value");
			}
		}

		obj.maxStone = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_currentStone(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentStone");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentStone on a nil value");
			}
		}

		obj.currentStone = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_currentBaoShi(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UserInfo obj = (UserInfo)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name currentBaoShi");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index currentBaoShi on a nil value");
			}
		}

		obj.currentBaoShi = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}
}

