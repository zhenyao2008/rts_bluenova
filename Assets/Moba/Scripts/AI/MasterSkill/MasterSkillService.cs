using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillService
{
    //TODO need change csv and action. into the ActionManager. 
    //表現層と計算層をわけて方がいい.
    public static void DoSkill(string skillId)
    {
        switch (skillId)
        {
            case "000001":
                SpawnSoilder("Soldier_Peltast_I");
                break;
            case "000002":
                SpawnSoilder("Soldier_Gunman_I");
                break;
            case "000101":
                for (int i = 0; i < 10; i++)
                {
                    SpawnSoilder("Soldier_Peltast_I");
                }
                break;
            case "000102":
                for (int i = 0; i < 10; i++)
                {
                    SpawnSoilder("Soldier_Gunman_III");
                }
                break;
            case "001001":
                Thunder();
                break;
        }
    }

    public static void SpawnSoilder(string prefabName)
    {
        Debug.Log(PlayerController_III.instance.playerIndex);
        ServerController_III.instance.SpawnUnit(prefabName, PlayerController_III.instance.playerIndex);
    }

    public static void Thunder()
    {
        //TODO
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        List<Enemy> targetEnemys = new List<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            if (enemy.playerIndex != PlayerController_III.instance.playerIndex)
                targetEnemys.Add(enemy);
        }
        DoSingleMagic(targetEnemys, 30, ResourcesManager.SKILL_THUNDER_EFFECT);
    }

    static void DoSingleMagic(List<Enemy> targetEnemyList, int count, string prefabName)
    {
        ServerController_III.instance.StartCoroutine(_DoSingleMagic(targetEnemyList, count, prefabName));
    }

    static IEnumerator _DoSingleMagic(List<Enemy> targetEnemyList, int count, string prefabName)
    {
        count = Mathf.Min(targetEnemyList.Count, count);
        UnitBase attacker;
        if (PlayerController_III.instance.playerIndex == 0)
        {
            attacker = ServerController_III.instance.boss0;
        }
        else
        {
            attacker = ServerController_III.instance.boss1;
        }
        Camera.main.GetComponent<AudioSource>().clip = ResourcesManager.Instance.GetThunderStart();
        Camera.main.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = targetEnemyList[Random.Range(0, targetEnemyList.Count)];
            targetEnemyList.Remove(enemy);
            if (enemy != null)
            {
                enemy.Damage(attacker, 1000);
                GameObject effect = ResourcesManager.Instance.GetEffect(prefabName);
                if (effect != null)
                {
                    GameObject.Destroy(effect, 3f);
                    effect.transform.position = enemy.transform.position;
                }
                //TODO
                GameObject gameObject = new GameObject();
                gameObject.transform.SetParent(Camera.main.transform);
                gameObject.transform.localPosition = Vector3.zero;
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = ResourcesManager.Instance.GetThunder();
                audioSource.Play();
                GameObject.Destroy(gameObject, 5f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
