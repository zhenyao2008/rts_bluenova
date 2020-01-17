using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
//using System;

public class AnimationClipImporter  {

	public static void Import()
	{
		Object[] objs = Selection.objects;
		foreach (Object obj in objs) {
			string path = AssetDatabase.GetAssetPath (obj);
			if (path.ToLower ().IndexOf (".fbx") != -1) {
				ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath (path);
				string fileAnim = Application.dataPath + Path.ChangeExtension(path, ".txt").Substring(6);
				StreamReader file = new StreamReader(fileAnim);
				
				string sAnimList = file.ReadToEnd();
				file.Close();
				
				//				if (EditorUtility.DisplayDialog("FBX Animation Import from file: ",
				//				                                fileAnim, "Import", "Cancel"))
				//				{
				System.Collections.ArrayList list = new ArrayList();
				ParseAnimFile(sAnimList, ref list);
//				ModelImporter modelImporter = assetImporter as ModelImporter;
				ModelImporterClipAnimation[] modelImporterClipAnimationms = (ModelImporterClipAnimation[])list.ToArray(typeof(ModelImporterClipAnimation));
				//				ModelImporterClipAnimation modelImporterClipAnimationm = modelImporterClipAnimationms[0];
				//				AnimationEvent[] events = new AnimationEvent[1];
				//				events[0].functionName = "GetTest";
				//				events[0].time = 0.1f;
				//				
				//				events[0].floatParameter = 1.1234f;
				//				modelImporterClipAnimationm.events = events;
				//				modelImporterClipAnimationms[0] = modelImporterClipAnimationm;
				//				Debug.Log(modelImporterClipAnimationms[0].name);
				modelImporter.clipAnimations = modelImporterClipAnimationms;
				modelImporter.SaveAndReimport();
				Debug.Log(path + "  分帧 Success");
			}
		}
	}


	static void ParseAnimFile(string sAnimList, ref System.Collections.ArrayList List)
	{
		Regex regexString = new Regex(" *(?<firstFrame>[0-9]+) *- *(?<lastFrame>[0-9]+) *(?<loop>(loop|noloop| )) *(?<name>[^\r^\n]*[^\r^\n^ ])",
		                              RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		
		Match match = regexString.Match(sAnimList, 0);
		while (match.Success)
		{
			ModelImporterClipAnimation clip = new ModelImporterClipAnimation();
			
			if (match.Groups["firstFrame"].Success)
			{
				clip.firstFrame = System.Convert.ToInt32(match.Groups["firstFrame"].Value, 10);
			}
			if (match.Groups["lastFrame"].Success)
			{
				clip.lastFrame = System.Convert.ToInt32(match.Groups["lastFrame"].Value, 10);
			}
			if (match.Groups["loop"].Success)
			{
				clip.loop = match.Groups["loop"].Value == "loop";
			}
			if (match.Groups["name"].Success)
			{
				clip.name = match.Groups["name"].Value;
			}
			
			List.Add(clip);
			
			match = regexString.Match(sAnimList, match.Index + match.Length);
		}
	}
}
