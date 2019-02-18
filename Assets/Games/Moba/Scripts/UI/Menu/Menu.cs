using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//TODO 正式なコードじゃ無い
public class Menu : SingleMonoBehaviour<Menu>
{

    List<MenuSkillButton> mSkillList;

    GameObject mSkillRoot;

    void Start()
    {
        transform.Find("btnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            ServerController_III serverController_III = FindObjectOfType<ServerController_III>();
            if (serverController_III != null)
            {
                serverController_III.StopHost();
                serverController_III.StopClient();
                serverController_III.StopServer();
            }
            SceneManager.LoadScene("Main");
        });
        mSkillRoot = transform.Find("Skills").gameObject;
        mSkillList = new List<MenuSkillButton>();
        mSkillList.AddRange(transform.Find("Skills").GetComponentsInChildren<MenuSkillButton>());
        for (int i = 0; i < mSkillList.Count; i++)
        {
            mSkillList[i].onSkill = (string skillId) =>
            {
                MasterSkillService.DoSkill(skillId);
            };
        }
        HideSkills();
    }

    public void ShowSkills()
    {
        mSkillRoot.SetActive(true);
    }

    public void HideSkills()
    {
        mSkillRoot.SetActive(false);
    }
}
