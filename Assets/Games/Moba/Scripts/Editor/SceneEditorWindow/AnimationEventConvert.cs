using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AnimationEventConvert{

	public static void ImportAnimationEvents(){
		Object[] objs = Selection.objects;
		foreach(Object obj in objs)
		{
			string path = AssetDatabase.GetAssetPath(obj);
			if(path.ToLower().IndexOf(".fbx")!=-1)
			{
				ModelImporter modelImporter = (ModelImporter)  AssetImporter.GetAtPath(path);
				SerializedObject so = new SerializedObject(modelImporter);
				SerializedProperty clips = so.FindProperty("m_ClipAnimations");

				string uPath = path.Substring(0,path.LastIndexOf(".")) + ".asset";
				ModelAnimationEventSerializeble maesz = AssetDatabase.LoadAssetAtPath<ModelAnimationEventSerializeble>(uPath);
				if(maesz==null)
				{
					continue;
				}


				for (int i = 0; i < modelImporter.clipAnimations.Length; i++)
				{

					AnimationEventGroupSerializeble aeg = GetAnimClipEvents(maesz.clips,modelImporter.clipAnimations[i].name);
					AnimationEventSerializeble[] animationEventSerializebles = aeg.events;
					AnimationEvent[] aes = new AnimationEvent[animationEventSerializebles.Length];
					AnimationEvent ae = new AnimationEvent();
//					ae.functionName = "Test";
//					ae.time = 0.2f;
//					aes[animationEventSerializebles.Length] = ae;

					for(int j=0;j< animationEventSerializebles.Length;j++ )
					{
						ae = new AnimationEvent();
						ae.functionName = animationEventSerializebles[j].functionName;

					

						ae.time = animationEventSerializebles[j].timeValue;
//						ae.floatParameter = animationEventSerializebles[j].floatValue;
//						ae.intParameter = animationEventSerializebles[j].intValue;
//						ae.stringParameter = animationEventSerializebles[j].stringValue;
						aes[j] = ae;
					}
//					AnimationEvent ae = new AnimationEvent();
//					ae.functionName = "Test";
//					ae.time = 0.2f;
//					aes[0] = ae;
//
//					ae = new AnimationEvent();
//					ae.functionName = "Test1" + i;
//					ae.time = 0.4123131f + i * 0.1f;
//					aes[1] = ae;
					AnimationEventConvert.SetEvents(clips.GetArrayElementAtIndex(i), aes);
				}
				so.ApplyModifiedProperties();





				// Make your changes and write them, destroying the events.
//				for (int i = 0; i < modelImporter.clipAnimations.Length; i++)
//				{
//					AnimationEventGroupSerializeble aeg = GetAnimClipEvents(maesz.clips,modelImporter.clipAnimations[i].name);
//					Debug.Log(aeg.clipName);
////					if(aeg!=null)
////					{
//						AnimationEventSerializeble[] animationEventSerializebles = aeg.events;
//						AnimationEvent[] aes = new AnimationEvent[animationEventSerializebles.Length];
//						for(int j=0;j< animationEventSerializebles.Length;j++ )
//						{
//							
//							AnimationEvent ae = new AnimationEvent();
//							ae.functionName = animationEventSerializebles[j].functionName;
//							ae.time = animationEventSerializebles[j].timeValue;
//							ae.floatParameter = animationEventSerializebles[j].floatValue;
//							ae.intParameter = animationEventSerializebles[j].intValue;
//							ae.stringParameter = animationEventSerializebles[j].stringValue;
//							aes[j] = ae;
//							
//						}
//						SetEvents( clips.GetArrayElementAtIndex(i), aes);
////						SerializedProperty so1 =);
//						
////						so.ApplyModifiedProperties();
////						modelImporter.SaveAndReimport();
////					}
////					else
////					{
////						SetEvents(clips.GetArrayElementAtIndex(i), null);
////					}
//				}
				so.ApplyModifiedProperties();
				modelImporter.SaveAndReimport();
			}
		}
		EditorUtility.DisplayDialog("Imported","导入成功！", "OK");
	}
	
	static AnimationEventGroupSerializeble GetAnimClipEvents(AnimationEventGroupSerializeble[] aes,string clipName){
		foreach(AnimationEventGroupSerializeble ae in aes)
		{
			if(ae.clipName == clipName)
			{
				return ae;
			}
		}
		return null;
	}


	public static void ExportAnimationEvents(){
		Object[] objs = Selection.objects;
		foreach (Object obj in objs) {
			
			string path = AssetDatabase.GetAssetPath (obj);

			if(path.ToLower().IndexOf(".fbx")!=-1)
			{
//				Object[] o1 = AssetDatabase.LoadAllAssetsAtPath (path);
				ModelImporter modelImporter = (ModelImporter)  AssetImporter.GetAtPath(path);
				SerializedObject so = new SerializedObject(modelImporter);
				SerializedProperty clips = so.FindProperty("m_ClipAnimations");
				ModelAnimationEventSerializeble mes = ScriptableObject.CreateInstance<ModelAnimationEventSerializeble>();
				mes.clips = new AnimationEventGroupSerializeble[modelImporter.clipAnimations.Length];
				for (int i = 0; i < modelImporter.clipAnimations.Length; i++)
				{
					string clipName =  modelImporter.clipAnimations[i].name;
					AnimationEventGroupSerializeble group = new AnimationEventGroupSerializeble();
					mes.clips[i] = group;
					group.clipName = clipName;
					AnimationEvent[] aevs = GetEvents(clips.GetArrayElementAtIndex (i));
					AnimationEventSerializeble[] events = new AnimationEventSerializeble[aevs.Length];
					for(int j=0;j<aevs.Length;j++)
					{
						AnimationEventSerializeble aej = new AnimationEventSerializeble();
						aej.functionName = aevs[j].functionName;
						aej.floatValue = aevs[j].floatParameter;
						aej.intValue = aevs[j].intParameter;
						aej.timeValue = aevs[j].time;
						aej.stringValue = aevs[j].stringParameter;
						events[j] = aej;
					}
					group.events = events;
				}

//				List<AnimationClip> clips = new List<AnimationClip> ();
//				foreach (Object o2 in o1) {
//					if (o2.GetType () == typeof(AnimationClip)) {
//						AnimationClip clip = (AnimationClip)o2;
//						if(clip.name.IndexOf("Take 001")!=-1)
//							continue;
//						clips.Add(clip);
//					}
//				}
				
//				ModelAnimationEventSerializeble mes = ScriptableObject.CreateInstance<ModelAnimationEventSerializeble>();
//				mes.clips = new AnimationEventGroupSerializeble[clips.Count];
//				for(int j=0;j < clips.Count; j++)
//				{
//					AnimationEventGroupSerializeble group = new AnimationEventGroupSerializeble();
//					group.clipName = clips[j].name;
//					mes.clips[j] = group;
//					AnimationEvent[] aevs = clips[j].events;
//					AnimationEventSerializeble[] events = new AnimationEventSerializeble[aevs.Length];
//					for(int i=0;i<aevs.Length;i++)
//					{
//						AnimationEventSerializeble aej = new AnimationEventSerializeble();
//						aej.functionName = aevs[i].functionName;
//						aej.floatValue = aevs[i].floatParameter;
//						aej.intValue = aevs[i].intParameter;
//						aej.timeValue = aevs[i].time;
//						aej.stringValue = aevs[i].stringParameter;
//						events[i] = aej;
//					}
//					group.events = events;
//				}
				
				
				string uPath = path.Substring(0,path.LastIndexOf(".")) + ".asset";
				AssetDatabase.CreateAsset(mes,uPath);
				AssetDatabase.Refresh();
			}
		}
	}
	
	
	//从meta文件读取动画事件
	public static AnimationEvent[] GetEvents (SerializedProperty sp)
	{
		SerializedProperty serializedProperty = sp.FindPropertyRelative("events");
		AnimationEvent[] array = null;
		
		if (serializedProperty != null && serializedProperty.isArray)
		{
			int count = serializedProperty.arraySize;
			array = new AnimationEvent[count];
			
			for (int i = 0; i < count; i++)
			{
				AnimationEvent animationEvent = new AnimationEvent();
				
				SerializedProperty eventProperty = serializedProperty.GetArrayElementAtIndex (i);
				animationEvent.floatParameter = eventProperty.FindPropertyRelative ("floatParameter").floatValue;
				animationEvent.functionName = eventProperty.FindPropertyRelative ("functionName").stringValue;
				animationEvent.intParameter = eventProperty.FindPropertyRelative ("intParameter").intValue;
				animationEvent.objectReferenceParameter = eventProperty.FindPropertyRelative ("objectReferenceParameter").objectReferenceValue;
				animationEvent.stringParameter = eventProperty.FindPropertyRelative ("data").stringValue;
				animationEvent.time     = eventProperty.FindPropertyRelative ("time").floatValue;
				Debug.Log(eventProperty.FindPropertyRelative ("time").floatValue);
				array [i] = animationEvent;
			}
		}
		return array;
	}

	//保存动画事件到meta文件
	public static void SetEvents (SerializedProperty sp, AnimationEvent[] newEvents)
	{
		SerializedProperty serializedProperty = sp.FindPropertyRelative("events");
//		if (serializedProperty != null && serializedProperty.isArray)
//			serializedProperty.ClearArray ();
		if (serializedProperty != null && serializedProperty.isArray && newEvents != null && newEvents.Length > 0)
		{
			serializedProperty.ClearArray ();
			for (int i = 0; i < newEvents.Length; i++)
			{
				AnimationEvent animationEvent = newEvents [i];
				serializedProperty.InsertArrayElementAtIndex (serializedProperty.arraySize);
				
				SerializedProperty eventProperty = serializedProperty.GetArrayElementAtIndex (i);
				eventProperty.FindPropertyRelative ("floatParameter").floatValue = animationEvent.floatParameter;
				eventProperty.FindPropertyRelative ("functionName").stringValue = animationEvent.functionName;
				eventProperty.FindPropertyRelative ("intParameter").intValue = animationEvent.intParameter;
				eventProperty.FindPropertyRelative ("objectReferenceParameter").objectReferenceValue = animationEvent.objectReferenceParameter;
				eventProperty.FindPropertyRelative ("data").stringValue = animationEvent.stringParameter;
				eventProperty.FindPropertyRelative ("time").floatValue = animationEvent.time;
			}
		}
	}

}
