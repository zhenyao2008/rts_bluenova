using UnityEngine;
using UnityEditor;
using System.Collections;

public class ModelImporterProcesser : AssetPostprocessor {

	void OnPreprocessModel()
	{
		if(assetPath.Substring(0,12) == "Assets/Level")
		{
			//在/fbx_1/下面的将被忽略
			if(assetPath.ToLower().IndexOf("/fbx_1/")==-1)
			{
				ModelImporter importer = assetImporter as ModelImporter;
				importer.animationType = ModelImporterAnimationType.None;
				importer.generateSecondaryUV = true;
				importer.importMaterials = false;
				importer.importAnimation =false;
			}
		}
		if (SceneEditorWindow.selectPath == assetPath)
		{
			ModelImporter importer = assetImporter as ModelImporter;
			importer.normalImportMode = ModelImporterTangentSpaceMode.None;
			SceneEditorWindow.selectPath = "";
		}

	}


}
