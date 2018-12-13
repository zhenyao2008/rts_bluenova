using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleSpawnService
{

    ServerController_III mServerController_III;

    public BattleSpawnService(ServerController_III serverController_III)
    {
        this.mServerController_III = serverController_III;
    }

    public void SpawnSoilders(List<SpawnPoint> spawners,  Transform target, int layer, int targetLayer, PlayerAttribute playerAttribute, int playerIndex)
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            SpawnUnit(spawners[i],target,layer,targetLayer,playerAttribute,playerIndex);
        }
    }

    GameObject SpawnUnit(SpawnPoint spawnPoint, Transform target, int layer, int targetLayer, PlayerAttribute playerAttribute, int playerIndex)
    {

        GameObject prefab = spawnPoint.GetCurrentPrefab();
        GameObject go = null;
        if (prefab != null)
        {
            prefab.SetActive(false);
            go = GameObject.Instantiate(prefab,spawnPoint.spawnPoint.position,spawnPoint.spawnPoint.rotation) as GameObject;
            go.name = prefab.name;
            Enemy soilder = go.GetComponent<Enemy>();
            soilder.defaultTarget = target;
            soilder.pos = spawnPoint.spawnPoint.position;
            soilder.qua = spawnPoint.spawnPoint.rotation;
            go.layer = layer;
            soilder.targetLayers.Add(targetLayer);
            soilder.playerAttribute = playerAttribute;
            soilder.playerIndex = playerIndex;
            go.SetActive(true);
            mServerController_III.StartCoroutine(_DelayMove(soilder,target));
            NetworkServer.Spawn(go);
        }
        return go;
    }

    IEnumerator _DelayMove(Enemy enemy, Transform target)
    {
        UnityEngine.AI.NavMeshAgent nav = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.enabled = false;
        yield return new WaitForSeconds(1);
        nav.enabled = true;
    }

    //TODO
    public void SpawnBuilding(List<Transform> planes, List<Transform> availablePlanes, int planeIndex, int buildIndex, List<SpawnPoint> spawner, int buildingLayer, int group, List<GameObject> cBuildPrefabs)
    {
        Transform plane = planes[planeIndex];
        if (!availablePlanes.Contains(plane))
        {
            return;
        }
        availablePlanes.Remove(plane);
        Transform sp = plane.transform.Find("SpawnPoint");
        GameObject go = GameObject.Instantiate(cBuildPrefabs[buildIndex], plane.position, plane.rotation) as GameObject;
        SpawnPoint spawnPoint = go.GetComponent<SpawnPoint>();
        spawnPoint.plane = plane;
        spawnPoint.spawnPoint = sp;
        spawnPoint.gameObject.layer = buildingLayer;
        spawnPoint.group = group;
        spawner.Add(spawnPoint);
        spawnPoint.index = spawner.IndexOf(spawnPoint);
        NetworkServer.Spawn(go);
    }
}
