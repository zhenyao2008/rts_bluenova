using UnityEngine;
using System.Collections;

namespace UIFrame
{
	//Ctrl 和 View 分开的好处是相同的View可以有不同的Ctrl。
	public class BaseCtrl : MonoBehaviour
	{

		public UIManager UIMgr;

		//parameters模拟浏览器的get/post请求，此为后面的请求参数，比如name=aaaa.
		public virtual void ShowPanel (Hashtable parameters = null)
		{
		
		}

		public virtual void Close ()
		{

		}

		public virtual void Back ()
		{

		}
	}
}
