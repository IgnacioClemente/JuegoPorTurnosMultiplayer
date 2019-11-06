using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class MyNetworkClient : SocketIOComponent
{
    static public string id;
    Dictionary<string, MyNetworkIdentity> connectedPlayers;
    Dictionary<string, ServerObject> spawnedObjects;
    [SerializeField] GameObject PlayerPrefab;
    ServerObjectsManager serverManager;

    public override void Start()
    {
        base.Start();
        connectedPlayers = new Dictionary<string, MyNetworkIdentity>();
        serverManager = GetComponent<ServerObjectsManager>();
        Hook();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Close();
            Debug.Log(id);
        }
    }

    private void Hook()
    {
        On("open", (connect) =>
        {
            Debug.Log("Me Conecte");
        });

        On("spawn", (spawn) =>
        {
            Debug.Log("conected");
            GameObject player = Instantiate(PlayerPrefab);
            player.GetComponent<MyNetworkIdentity>().SetID(spawn.data["id"].ToString());
            player.GetComponent<MyNetworkIdentity>().Socket = this;
            player.name = spawn.data["id"].ToString();
            connectedPlayers.Add(spawn.data["id"].ToString(), player.GetComponent<MyNetworkIdentity>());
        });

        On("onRegister", (register) =>
        {
            id = register.data["id"].ToString();
        });

        On("updatePos", (updatePosTransform) =>
        {
            float x = updatePosTransform.data["position"]["x"].f / 1000f;
            float y = updatePosTransform.data["position"]["y"].f / 1000f;
            float z = updatePosTransform.data["position"]["z"].f / 1000f;
            connectedPlayers[updatePosTransform.data["id"].ToString()].gameObject.transform.position = new Vector3(x, y, z);
        });

        On("updateRot", (updateRotTransform) =>
         {
             float x = updateRotTransform.data["position"]["x"].f / 1000f;
             float y = updateRotTransform.data["rotation"]["y"].f / 1000f;
             float z = updateRotTransform.data["rotation"]["z"].f / 1000f;
             connectedPlayers[updateRotTransform.data["id"].ToString()].gameObject.transform.eulerAngles = new Vector3(x, y, z);
         });

        On("disconnect", (disconection) =>
        {
            GameObject objectToDestroy = connectedPlayers[id].gameObject;
            Destroy(objectToDestroy);
            connectedPlayers.Remove(id);
            Debug.Log("se desconecto un player " + id);
        });

        On("playerDisconnected", (disconection) =>
         {
             string id = disconection.data["id"].ToString();
             GameObject objectToDestroy = connectedPlayers[id].gameObject;
             Destroy(objectToDestroy);
             connectedPlayers.Remove(id);
             Debug.Log("se desconecto un player");
         });

        On("ServerSpawn", (serverDisconection) =>
        {

        });


        On("ServerDestroy", (serverDestroy) =>
        {

        });

    }
}

[System.Serializable]
class PlayerData
{
    public string id;
    public Vector3Data position;
    public RotationData rotation;
    public PlayerData()
    {
        rotation = new RotationData();
        position = new Vector3Data();
    }
}

[System.Serializable]
class Vector3Data
{
    public double x;
    public double y;
    public double z;
}

[System.Serializable]
class RotationData
{
    public double x;
    public double y;
    public double z;
}