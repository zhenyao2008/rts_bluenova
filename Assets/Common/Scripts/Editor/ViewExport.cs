//==========================================
// Created By [lijun] At 2015/4/27 16:23
//==========================================
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;

public class ViewExport 
{
    class CtrlInfo
    {
        public string path;
        public Transform trans;
    }
    ///////////////////////////////////以下为静态成员//////////////////////////////

    static string flag = "#";  //控件名称结尾标记
    static List<CtrlInfo> allCtrls = new List<CtrlInfo>();
    static string tempCode = string.Empty;
    static string varPrefix = "        ";
    static string codePrefix = "            ";
    static string luaCodePrefix = "    ";

    ///////////////////////////////////以下为非静态成员///////////////////////////
    /// <summary>
    /// 应用路径
    /// </summary>
    static string AppPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    static bool IsNodeChanged(string filepath, List<CtrlInfo> ctrls)
    {
        if (File.Exists(filepath))
        {
            string[] lines = File.ReadAllLines(filepath);
            if (lines.Length > 0)
            {
                bool isNewExist = false;
                List<string> oldNodes = new List<string>(lines);
                foreach (CtrlInfo info in ctrls)
                {
                    if (!oldNodes.Contains(info.path))
                    {
                        isNewExist = true;
                        break;
                    }
                }
                if (!isNewExist) //没有新节点
                {
                    return false;
                }
            }
            File.Delete(filepath);  //删除老的
        }
        Debug.LogWarning(filepath + " have changed!!!");
        List<string> allNodes = new List<string>();
        foreach (CtrlInfo info in ctrls)
        {
            allNodes.Add(info.path);
        }
        File.WriteAllLines(filepath, allNodes.ToArray());
        return true;
    }

    /// <summary>
    /// 导出lua view层代码
    /// </summary>
    [MenuItem("Game/Export Lua View Code")]
    public static void ExportLuaViewCode()
    {


        //string destPath = AppPath + "/Scripts/Lua/View/";
        //string srcPath = AppPath + "/res/prefabs/ui/lp/";
        //string tempPath = AppPath + "/Templates/LuaPanelView.txt";
        //string panelDataPath = AppPath + "/Editor/PanelData/Lua/";

        string configPath = AppPath + "/_Common/Scripts/UGUI/Config.txt";
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath);
        Dictionary<string,string> config = SysConfig.Load(ta);
        string destPath = AppPath + config["luaViewPath"];
        string srcPath = AppPath + config["luaPrefabPath"];
        string tempPath = AppPath + config["luaTemplatePath"];
        string panelDataPath = AppPath + config["luaPanelDataPath"];
        
        //EditorExtend.XmlConfigEditorDataHolder.ParserXmlConfigEditor();
        //string destPath = AppPath + EditorExtend.XmlConfigEditorDataHolder.GetLuaViewScriptPath();
        //string srcPath = AppPath + EditorExtend.XmlConfigEditorDataHolder.GetUIPrefabLuaPanelPath();
        //string tempPath = AppPath + EditorExtend.XmlConfigEditorDataHolder.GetLuaPanelViewTemplatePath();
        //string panelDataPath = AppPath + EditorExtend.XmlConfigEditorDataHolder.GetLuaPanelDataPath();


        if (!Directory.Exists(panelDataPath)){
            Directory.CreateDirectory(panelDataPath);
        }
        allCtrls.Clear();   //清空控件
        Recursive.Start(srcPath, ".prefab");
        List<string> files = Recursive.GetFiles();
        for (int i = 0; i < files.Count; i++)
        {
            if (!files[i].EndsWith("Panel.prefab"))
            {
                continue;   //过滤非lua面板prefab
            }
            int index = files[i].IndexOf("/Assets/");
            string assetPath = files[i].Remove(0, index + 1);

            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
            if (o == null)
            {
                continue;
            }
            tempCode = File.ReadAllText(tempPath);  //读取模板代码

            string panelName = Path.GetFileName(files[i]);
            panelName = panelName.Replace(".prefab", "");

            string initVarCode = string.Empty;  //初始化代码
            string destroyCode = string.Empty;  //销毁代码

            allCtrls.Clear();
            GetAllObject((o as GameObject).transform);

            string filepath = panelDataPath + panelName + ".txt";
            if (!IsNodeChanged(filepath, allCtrls)) //是否有节点改变过
            {
                continue;
            }
            foreach (var obj in allCtrls)
            {
                string ctrlName = obj.trans.name;
                if (!ctrlName.StartsWith(flag) || ctrlName.IndexOf("_") == -1)
                {
                    continue;   //没有生成代码标记，跳过。
                }
                ctrlName = ctrlName.Remove(0, 1);
                string bakPath = obj.path;  //备份路径
                obj.path = obj.path.Replace("/" + panelName + "/", "");

                string varName = FormateWord(ctrlName, bakPath);
                if (ctrlName.StartsWith("btn_"))    //按钮
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Button');\n";
                }
                if (ctrlName.StartsWith("cdbtn_"))    //按钮
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('CountDownButton');\n";
                }
                if (ctrlName.StartsWith("img_"))    //图片
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Image');\n";
                }
                if (ctrlName.StartsWith("txt_"))    //文本
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Text');\n";
                }
                if (ctrlName.StartsWith("input_"))  //输入框
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('InputField');\n";
                }
                if (ctrlName.StartsWith("container_"))  //容器
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("slider_")) //滑块
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Slider');\n";
                }
                if (ctrlName.StartsWith("progress_"))   //进度条
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('ProgressBarView');\n";
                }
                if (ctrlName.StartsWith("mprogress_"))   //进度条
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('MultiProgressBarView');\n";
                }
                if (ctrlName.StartsWith("loading_"))   //进度条
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('LoadingBarView');\n";
                }
                if (ctrlName.StartsWith("toggle_")) //复选框
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Toggle');\n";
                }
                if (ctrlName.StartsWith("scroll_"))  //滚动视图
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("drop_"))   //拖拽面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("anim_"))   //UI动画
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("tabtn_"))    //标签面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('TabButton');\n";
                }
                if (ctrlName.StartsWith("obj_"))    //标签面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("star_"))    //星星组件
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('StarBar');\n";
                }
                if (ctrlName.StartsWith("hlayoutbox_"))    //水平框组件
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('HorLayoutBox');\n";
                }
                if (ctrlName.StartsWith("tab_"))    //标签面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("grid_"))    //网格面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\");\n";
                }
                if (ctrlName.StartsWith("rtrans_"))    //标签面板
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('RectTransform');\n";
                }
                if (ctrlName.StartsWith("eftxt_"))    //动态字体+位图字体效果字
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('Text');\n";
                }
                if (ctrlName.StartsWith("karma_"))    //幸运值组件
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('KarmaBarView');\n";
                }
                if (ctrlName.StartsWith("itembox_"))   //道具组件
                {
                    initVarCode += luaCodePrefix + panelName + "." + varName + " = m_Trans:FindChild(\"" + obj.path + "\"):GetComponent('ItemBox');\n";
                }
                destroyCode += luaCodePrefix + panelName + "." + varName + " = nil;\n";
            }
            string newVarstr = string.Empty;
            string[] varNames = initVarCode.Split('\n');
            foreach (string var in varNames)
            {
                if (string.IsNullOrEmpty(var))
                {
                    continue;
                }
                string newline = var.Substring(0, var.IndexOf("m_Trans"));
                newline = newline.Replace("    ", " ");
                newline = newline.Replace("this.", "");   
                newVarstr += "local" + newline + "nil;\n";
            }
            //tempCode = tempCode.Replace("[VarNames]", newVarstr);     //替换变量代码
            tempCode = tempCode.Replace("[PanelName]", panelName);      //替换UI面板名称
            tempCode = tempCode.Replace("[InitVarCode]", initVarCode);  //替换变量代码
            tempCode = tempCode.Replace("[DestroyVar]", destroyCode);   //替换销毁代码

            string newDestPath = destPath + panelName + ".lua";
            File.WriteAllText(newDestPath, tempCode);
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log(newDestPath + "\n" + tempCode);  
        }
    }

    /// <summary>
    /// 导出c# view层代码
    /// </summary>
    [MenuItem("Game/Export C# View Code")]
    public static void ExportCSharpViewCode()
    {
        
        string configPath = "Assets/_Common/Scripts/UGUI/Config.txt";
        Debug.Log(configPath);
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(configPath);
        Debug.Log(ta);
        Dictionary<string, string> config = SysConfig.Load(ta);
        string destPath = AppPath + config["CSViewPath"];
        string srcPath = AppPath + config["CSPrefabPath"];
        string tempPath = AppPath + config["CSTemplatePath"];
        string panelDataPath = AppPath + config["CSPanelDataPath"];

        if (!Directory.Exists(panelDataPath))
        {
            Directory.CreateDirectory(panelDataPath);
        }
        allCtrls.Clear();
        Recursive.Start(srcPath, ".prefab");
        List<string> files = Recursive.GetFiles();

        for (int i = 0; i < files.Count; i++)
        {
            if (!files[i].EndsWith("Panel.prefab")) 
            {
                continue;   //过滤非面板prefab+lua面板
            }
            int index = files[i].IndexOf("/Assets/");
            string assetPath = files[i].Remove(0, index + 1);

            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
            if (o == null)
            {
                continue;
            }
            
            tempCode = File.ReadAllText(tempPath);  //读取模板代码

            string panelName = Path.GetFileName(files[i]);
            panelName = panelName.Replace(".prefab", "");

            string destroyCode = string.Empty;  //销毁代码
            string ctrlVarName = varPrefix + "public Transform m_Trans;\n";  //控件名
            string initVarCode = codePrefix + "m_Trans = transform;\n";  //初始化代码

            allCtrls.Clear();
            GetAllObject((o as GameObject).transform);

            string filepath = panelDataPath + panelName + ".txt";
            if (!IsNodeChanged(filepath, allCtrls)) //是否有节点改变过
            {
                continue;
            }
            foreach (var obj in allCtrls)
            {
                string ctrlName = obj.trans.name;
                if (!ctrlName.StartsWith(flag) || ctrlName.IndexOf("_") == -1)
                {
                    continue;   //没有生成代码标记，跳过。
                }
                ctrlName = ctrlName.Remove(0, 1);
                string bakPath = obj.path; //备份一下
                obj.path = obj.path.Replace("/" + panelName + "/", "");

                string varName = FormateWord(ctrlName, bakPath);
                if (ctrlName.StartsWith("btn_"))    //按钮
                {
                    ctrlVarName += varPrefix + "public Button m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Button>();\n";
                }
                if (ctrlName.StartsWith("cdbtn_"))    //倒计时按钮
                {
                    ctrlVarName += varPrefix + "public CountDownButton m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<CountDownButton>();\n";
                }
                if (ctrlName.StartsWith("img_"))    //图片
                {
                    ctrlVarName += varPrefix + "public Image m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Image>();\n";
                }
                if (ctrlName.StartsWith("txt_"))    //文本
                {
                    ctrlVarName += varPrefix + "public Text m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Text>();\n";
                }
                if (ctrlName.StartsWith("input_"))  //输入框
                {
                    ctrlVarName += varPrefix + "public InputField m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<InputField>();\n";
                }
                if (ctrlName.StartsWith("container_"))  //容器
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("slider_")) //滑块
                {
                    ctrlVarName += varPrefix + "public Slider m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Slider>();\n";
                }
                if (ctrlName.StartsWith("progress_"))   //进度条
                {
                    ctrlVarName += varPrefix + "public ProgressBarView m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<ProgressBarView>();\n";
                }
                if (ctrlName.StartsWith("mprogress_"))   //进度条
                {
                    ctrlVarName += varPrefix + "public MultiProgressBarView m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<MultiProgressBarView>();\n";
                }
                if (ctrlName.StartsWith("loading_"))   //进度条
                {
                    ctrlVarName += varPrefix + "public LoadingBarView m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<LoadingBarView>();\n";
                }
                if (ctrlName.StartsWith("toggle_")) //复选框
                {
                    ctrlVarName += varPrefix + "public Toggle m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Toggle>();\n";
                }
                if (ctrlName.StartsWith("scroll_"))     //滚动视图
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("drop_"))   //拖拽面板
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("anim_"))   //UI动画
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("tab_"))    //标签面板
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("tabtn_"))    //标签面板
                {
                    ctrlVarName += varPrefix + "public TabButton m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<TabButton>();\n";
                }
                if (ctrlName.StartsWith("obj_"))    //标签面板
                {
                    ctrlVarName += varPrefix + "public GameObject m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").gameObject;\n";
                }
                if (ctrlName.StartsWith("star_"))    //星星条组件
                {
                    ctrlVarName += varPrefix + "public StarBar m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<StarBar>();\n";
                }
                if (ctrlName.StartsWith("hlayoutbox_"))    //水平框组件
                {
                    ctrlVarName += varPrefix + "public HorLayoutBox m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<HorLayoutBox>();\n";
                }
                if (ctrlName.StartsWith("grid_"))    //标签面板
                {
                    ctrlVarName += varPrefix + "public Transform m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\");\n";
                }
                if (ctrlName.StartsWith("rtrans_"))    //标签面板
                {
                    ctrlVarName += varPrefix + "public RectTransform m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<RectTransform>();\n";
                }
                if (ctrlName.StartsWith("eftxt_"))    //动态字体+位图字体效果字
                {
                    ctrlVarName += varPrefix + "public Text m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<Text>();\n";
                }
                if (ctrlName.StartsWith("karma_"))    //动态字体+位图字体效果字
                {
                    ctrlVarName += varPrefix + "public KarmaBarView m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<KarmaBarView>();\n";
                }
                if (ctrlName.StartsWith("itembox_")) //道具组件
                {
                    ctrlVarName += varPrefix + "public ItemBox m_" + varName + ";\n";
                    initVarCode += codePrefix + "m_" + varName + " = m_Trans.FindChild(\"" + obj.path + "\").GetComponent<ItemBox>();\n";
                }
                destroyCode += codePrefix + "m_" + varName + " = null;\n";
            }
            tempCode = tempCode.Replace("[UserName]", "yingyugang");
            tempCode = tempCode.Replace("[CreateTime]", DateTime.Now.ToString());
            tempCode = tempCode.Replace("[PanelName]", panelName);      //替换UI面板名称
            tempCode = tempCode.Replace("[CtrlVarName]", ctrlVarName);  //替换控件名称
            tempCode = tempCode.Replace("[InitVarCode]", initVarCode);  //替换变量代码
            tempCode = tempCode.Replace("[DestroyVar]", destroyCode);   //销毁代码

            string newDestPath = destPath + panelName + "/";
            if (!Directory.Exists(newDestPath))
            {
                Directory.CreateDirectory(newDestPath);
            }
            newDestPath += panelName + "View.cs";
            File.WriteAllText(newDestPath, tempCode);
            AssetDatabase.Refresh();

            string info = newDestPath + "\n" + tempCode;
            Debug.Log(info);
        }
	}

    /// <summary>
    /// 格式化变量
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private static string FormateWord(string word, string bakPath)
    {
        int index = word.IndexOf("_");
        if (index == -1)
        {
            Debug.LogError("Formate Word Error!!!");
            return null;
        }
        string result = string.Empty;
        if (bakPath.Contains("$"))    //没有等级标记
        {
            string newPath = bakPath.Remove(0, bakPath.IndexOf("$") + 1);
            newPath = newPath.Replace("/#" + word, string.Empty);
            newPath = newPath.Replace('/', '_');

            string leftstr = word.Substring(0, index);
            string middleVarChar = word.Substring(index + 1, 1).ToUpper();
            string rightstr = word.Substring(index + 2, word.Length - index - 2);

            string middlePathChar = newPath.Substring(0, 1).ToUpper();
            newPath = newPath.Remove(0, 1);
            string newName = leftstr + middlePathChar + newPath + "_" + middleVarChar + rightstr;
            result = newName.Replace("#", "");
        }
        else
        {
            string leftstr = word.Substring(0, index);
            string middleChar = word.Substring(index + 1, 1).ToUpper();
            string rightstr = word.Substring(index + 2, word.Length - index - 2);
            result = leftstr + middleChar + rightstr;
            result = result.Replace("#", "");
        }
        return result;
    }

    private static string GetObjectPath(Transform obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent;
            path = "/" + obj.name + path;
        }
        return path;  
    }

    /// <summary>
    /// 获取所有对象
    /// </summary>
    private static void GetAllObject(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform st = t.GetChild(i);
            if (st.childCount > 0)
            {
                GetAllObject(st);
            }
            CtrlInfo info = new CtrlInfo();
            info.trans = st;
            info.path = GetObjectPath(st);
            allCtrls.Add(info);
        }
    }
}
