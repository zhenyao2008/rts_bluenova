using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuSkillButton : MonoBehaviour
{

    public Button btnSkill;
    public Text txtSkill;
    public Image imgSkill;
    public Image imgSkillBack;
    public Image imgSkillCooldown;
    public Image imgSkillMask;

    public string skillId;
    public int skillCount;
    public float cooldown;
    float mNextTime;
    public UnityAction<string> onSkill;

    void Start()
    {
        cooldown = Mathf.Max(1,cooldown);
        txtSkill.text = skillCount.ToString();
        imgSkillCooldown.enabled = false;
        btnSkill.onClick.AddListener(() =>
        {
            if (skillCount > 0 && mNextTime < Time.time)
            {
                if (onSkill != null)
                {

                    onSkill(skillId);
                }
                mNextTime = Time.time + cooldown;
                skillCount--;
                txtSkill.text = skillCount.ToString();
                StartCoroutine(_Cooldown());
            }
        });
    }

    IEnumerator _Cooldown()
    {
        imgSkillCooldown.enabled = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / cooldown;
            imgSkillCooldown.fillAmount = (1 - t);
            yield return null;
        }
        imgSkillCooldown.enabled = false;
    }

    void Update()
    {

    }
}
