using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BlueNoah.UI;
//using GameEngine.Res;
//using BlueNoah.Assets;
//using TMPro;
using UnityEngine.UI;

public enum CharacterRank
{
    R1 = 1,
    SR = 2,
    SSR = 3,
    UR = 4
}

//TODO
public static class AssetsManager
{

//    //TODO
//    public static GameObject LoadPanelPrefab(string path)
//    {
//        path = path.Substring(path.LastIndexOf("/", System.StringComparison.CurrentCulture) + 1);
//        path = path.Substring(0, path.IndexOf(".", System.StringComparison.CurrentCulture));
//        Debug.Log(path);
//        GameObject gameObject = LoadFromAssetBundleOrEditor<GameObject>(AssetBundleNameConstant.BASE_UI_PANEL_PATH, path);
//        ResetShader(gameObject);
//        return gameObject;

//        path = GameEngine.IO.FileManager.AssetDataPathToResourcesPath(path);
//        //TODO to move the Prefab to ~Resource 見る.
//        return Load<GameObject>(path);
//    }

//    //TODO
//    public static GameObject LoadDialogPrefab(string path)
//    {

//        path = path.Substring(path.LastIndexOf("/", System.StringComparison.CurrentCulture) + 1);
//        path = path.Substring(0, path.IndexOf(".", System.StringComparison.CurrentCulture));
//        Debug.Log(path);
//        //return LoadPanelPrefabFromAssetBundle("uiprefabs/dialogs.ab", path);
//        GameObject gameObject = LoadFromAssetBundleOrEditor<GameObject>(AssetBundleNameConstant.BASE_UI_DIALOG_PATH, path);
//        ResetShader(gameObject);
//        return gameObject;
//        path = GameEngine.IO.FileManager.AssetDataPathToResourcesPath(path);
//        return Load<GameObject>(path);
//    }

//    public static GameObject ResetShader(GameObject prefab)
//    {
//#if UNITY_EDITOR

//        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);
//        for (int i = 0; i < renderers.Length; i++)
//        {
//            renderers[i].sharedMaterial.shader = Shader.Find(renderers[i].sharedMaterial.shader.name);
//        }

//        ParticleSystem[] particles = prefab.GetComponentsInChildren<ParticleSystem>(true);
//        for (int i = 0; i < particles.Length; i++)
//        {
//            renderers[i].GetComponent<Renderer>().sharedMaterial.shader = Shader.Find(renderers[i].GetComponent<Renderer>().sharedMaterial.shader.name);
//        }

//        TextMeshProUGUI[] texts = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
//        for (int i = 0; i < texts.Length; i++)
//        {
//            texts[i].material.shader = Shader.Find(texts[i].material.shader.name);
//            texts[i].fontSharedMaterial.shader = Shader.Find(texts[i].fontSharedMaterial.shader.name);
//        }

//        Image[] images = prefab.GetComponentsInChildren<Image>(true);
//        for (int i = 0; i < images.Length; i++)
//        {
//            images[i].material.shader = Shader.Find(images[i].material.shader.name);
//        }

//#endif
//        return prefab;
//    }

//    public static Sprite GetSinBattleIcon(int sinId)
//    {
//        MSinIcon sinIcon = MasterDataManage.GetMasterData<MSinIcon>(sinId);
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_SIN_ICON_PATH, sinIcon.battle_map_icon);
//    }

//    public static Sprite GetSinCharaIcon(int sinId)
//    {
//        MSinIcon sinIcon = MasterDataManage.GetMasterData<MSinIcon>(sinId);
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_SIN_ICON_PATH, sinIcon.chara_icon);
//    }

//    public static Sprite GetAttributIcon(int attributeId)
//    {
//        MAttributeIcon mAttribute = MasterDataManage.GetMasterData<MAttributeIcon>(attributeId);
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_AWARD_PATH, mAttribute.attribute_icon_name);
//    }

//    public static Sprite GetAttributArrowIcon(int attributeId)
//    {
//        MAttributeIcon mAttribute = MasterDataManage.GetMasterData<MAttributeIcon>(attributeId);
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_AWARD_PATH, mAttribute.attribute_arrow_icon_name);
//    }


//    public static Sprite GetJobIcon(int jobId)
//    {
//        MUnitJob job = MasterDataManage.GetMasterData<MUnitJob>(jobId);
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_JOB_ICON_PATH, job.job_icon);
//    }

//    public static Sprite GetSkillIcon(int skillId)
//    {
//        MSkillIcon skillIcon = MasterDataManage.GetMasterData<MSkillIcon>(skillId);
//        if (skillIcon != null)
//        {
//            if (string.IsNullOrEmpty(skillIcon.icon_name))
//            {
//                Debug.LogError("skill icon_name is empty : id is " + skillId);
//                //TODO
//                if (skillIcon.icon_type == 0)
//                {
//                    return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_SKILL_ICON_PATH, MasterDataManage.GetMasterData<MSkillIcon>(16).icon_name);
//                }
//                else
//                {
//                    return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_SKILL_ICON_PATH, MasterDataManage.GetMasterData<MSkillIcon>(1).icon_name);
//                }
//            }
//            return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_SKILL_ICON_PATH, skillIcon.icon_name);
//        }
//        else
//            Debug.LogError("please check m_skill_icon csv, there no id: " + skillId);
//        return null;
//    }

//    const string CHARAICON_FRONT_NAME = "charaicon_frame_";

//    public static Sprite GetCharaRankFrame(CharacterRank rank)
//    {
//        string fileName = CHARAICON_FRONT_NAME + rank.ToString();
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_CHARAICON_FRAME_PATH, fileName);
//        //return Load<Sprite>(BASE_CHARAICON_FRAME_PATH + fileName);
//    }

//    public static Sprite GetUnitIcon(int charaId)
//    {
//        MUnitIcon unitIcon = MasterDataManage.GetMasterData<MUnitIcon>(charaId);
//        if (unitIcon == null)
//        {
//            Debug.LogError("please check m_unit_icon csv, there no id: " + charaId);
//            unitIcon = MasterDataManage.GetMasterData<MUnitIcon>(1);
//        }
//        if (unitIcon != null)
//        {
//            if (string.IsNullOrEmpty(unitIcon.icon_name))
//            {
//                Debug.LogError("unit icon_name is empty : id is " + charaId);
//            }
//            return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_CHARAICON_PATH, unitIcon.icon_name);
//        }
//        return null;
//    }

//    public static Sprite GetUnitCutInIcon(int charaId, int cutInIndex)
//    {
//        MUnitIcon unitIcon = MasterDataManage.GetMasterData<MUnitIcon>(charaId);
//        if (unitIcon == null)
//        {
//            Debug.LogError("please check m_unit_icon csv, there no id: " + charaId);
//            unitIcon = MasterDataManage.GetMasterData<MUnitIcon>(1);
//        }
//        if (unitIcon != null)
//        {
//            if (string.IsNullOrEmpty(unitIcon.icon_name))
//            {
//                Debug.LogError("unit icon_name is empty : id is " + charaId);
//            }
//            string iconName = "";
//            switch (cutInIndex)
//            {
//                case 1:
//                    iconName = unitIcon.icon_cutIn_one;
//                    break;
//                case 2:
//                    iconName = unitIcon.icon_cutIn_two;
//                    break;
//                default:
//                    iconName = unitIcon.icon_cutIn_one;
//                    break;
//            }
//            return LoadFromAssetBundleOrEditor<Sprite>("images/" + unitIcon.icon_folder, iconName);
//        }
//        return null;
//    }

//    //TODO to manage the icon by types.
//    const string BASE_COMMON_PATH = "icon_common_";
//    public static Sprite GetAwardIcon(AwardType type)
//    {
//        string tempPath = "";
//        if (type.Equals(AwardType.Box))
//        {
//            tempPath = "icon_battlemap_09";
//        }
//        else
//        {
//            if ((int)type < 10)
//            {
//                tempPath = BASE_COMMON_PATH + "0" + (int)type;
//            }
//            else
//                tempPath = BASE_COMMON_PATH + (int)type;
//        }
//        return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_AWARD_PATH, tempPath);
//    }


//    public static Sprite GetBuffDebuffIcon(int buffId)
//    {
//        MBuffDebuffIcon buffDebuffIcon = MasterDataManage.GetMasterData<MBuffDebuffIcon>(buffId);
//        if (buffDebuffIcon == null)
//        {
//            buffDebuffIcon = MasterDataManage.GetMasterData<MBuffDebuffIcon>(102);
//            Debug.LogError("buffDebuffIcon is empty : id is " + buffId);
//        }

//        if (buffDebuffIcon != null)
//        {
//            return LoadFromAssetBundleOrEditor<Sprite>(AssetBundleNameConstant.BASE_BUFF_DEBUFF_ICON_PATH, buffDebuffIcon.icon_name);
//        }
//        return null;
//    }

//    public static GameObject LoadBattleUICombatText()
//    {
//        return Resources.Load<GameObject>("Prefabs/Battle/UICombatText");
//    }


//    public static GameObject LoadBattleUnitHead(bool isFriend)
//    {
//        if (isFriend)
//            return Resources.Load<GameObject>("Prefabs/Battle/BattleUnitFriendHead");
//        else
//            return Resources.Load<GameObject>("Prefabs/Battle/BattleUnitEnemyHead");
//    }

//    public static GameObject LoadBattleUIBattleUnit(FactionType _playerType = FactionType.FRIEND)
//    {
//        switch (_playerType)
//        {
//            case FactionType.ENEMY: return Resources.Load<GameObject>("prefabs/Battle/EnemyHpAndTpInfo");
//            case FactionType.FRIEND: return Resources.Load<GameObject>("prefabs/Battle/HpAndTpInfo");
//            default: return Resources.Load<GameObject>("prefabs/Battle/HpAndTpInfo");
//        }

//    }

//    public static AudioClip LoadAudioBGM(string bgm)
//    {
//        return LoadFromAssetBundleOrEditor<AudioClip>(AssetBundleNameConstant.BASE_AUDIO_BGM_PATH, bgm);
//    }


//    //TODO
//    static Dictionary<string, Object> mLoadedObjectDic = new Dictionary<string, Object>();

//    //TODO may same path with defferent object;
//    //need read from serval ways.
//    static T Load<T>(string path) where T : Object
//    {
//        if (mLoadedObjectDic.ContainsKey(path) && mLoadedObjectDic[path] != null)
//        {
//            return mLoadedObjectDic[path] as T;
//        }
//        else
//        {
//            T t = Resources.Load<T>(path);
//            if (t != null)
//            {
//                mLoadedObjectDic.Add(path, t);
//            }
//            if (t == null)
//                Debug.LogError("this path is null: " + path);
//            return t;
//        }
//    }

//    public static T LoadFromAssetBundleOrEditor<T>(string path, string fileName) where T : Object
//    {
//        T t = null;
//#if UNITY_EDITOR
//        if (fileName.IndexOf(".", System.StringComparison.CurrentCulture) == -1)
//        {
//            if (typeof(T) == typeof(Sprite))
//            {
//                fileName += ".png";
//            }
//            else if (typeof(T) == typeof(GameObject))
//            {
//                fileName += ".prefab";
//            }
//            else if (typeof(T) == typeof(AudioClip))
//            {
//                //TODO
//                fileName += ".wav";
//            }
//            else if (typeof(T) == typeof(Material))
//            {
//                fileName += ".mat";
//            }
//            else
//            {
//                fileName += ".asset";
//            }
//        }
//        path = "Assets" + AssetBundleConstant.ASSETBUNDLE_RESOURCES_PATH + path + "/" + fileName;
//        t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
//        Debug.Log(path + "||" + t);
//#else
//        if(fileName.IndexOf(".",System.StringComparison.CurrentCulture) != -1){
//            fileName = fileName.Substring(0,fileName.IndexOf(".", System.StringComparison.CurrentCulture));
//        }
//        t = AssetBundleLoadManager.Instance.LoadAssetFromLocalAssetBundle<T>(path + ".ab", fileName);
//#endif
    //    return t;
    //}
}
