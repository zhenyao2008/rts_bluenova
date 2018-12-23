using UnityEngine;
using System.Collections;

public class MasterSkillService
{
    //TODO need change csv and action. into the ActionManager.
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
                for (int i = 0; i < 10;i++){
                    SpawnSoilder("Soldier_Peltast_I");
                }
                break;
            case "000102":
                for (int i = 0; i < 10; i++)
                {
                    SpawnSoilder("Soldier_Gunman_I");
                }
                break;
        }
    }

    public static void SpawnSoilder(string prefabName)
    {
        Debug.Log(PlayerController_III.instance.playerIndex);
        ServerController_III.instance.SpawnUnit(prefabName, PlayerController_III.instance.playerIndex);
    }
}
