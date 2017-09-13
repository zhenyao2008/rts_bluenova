using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

[ExecuteInEditMode]
public class ResourceManager : MonoBehaviour {

	public bool load;
	public bool clean;

	public List<string> paths;
	public List<GameObject> pathObjects;

	void Update()
	{
		if(load)
		{
			load = false;
			Load();
		}

		if(clean)
		{
			clean = false;
		}
	}
	public string json;
	void Load()
	{
//		JsonConvert.DeserializeObject
		Product product = new Product();
		product.name01 = "Apple";
//		product.Expiry = new DateTime(2008, 12, 28);
//		product.Sizes = new string[] { "Small" };
		json = JsonConvert.SerializeObject(product);
	}

	void Clean()
	{

	}
}
public class Product
{
	public string name01;
//	public DateTime Expiry;
//	public string[] Sizes;
}




