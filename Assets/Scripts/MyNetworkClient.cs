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

        On("updatePosition", (updatePosition) =>
        {
            float x = updatePosition.data["position"]["x"].f;
            float y = updatePosition.data["position"]["y"].f;
            float z = updatePosition.data["position"]["z"].f;
            connectedPlayers[updatePosition.data["id"].ToString()].gameObject.transform.position = new Vector3(x, y, z);
            Debug.Log(updatePosition.data["id"].ToString());
        });

        On("updateRotation", (updateRotation) =>
         {
             float x = updateRotation.data["rotation"]["x"].f;
             float y = updateRotation.data["rotation"]["y"].f;
             float z = updateRotation.data["rotation"]["z"].f;
             connectedPlayers[updateRotation.data["id"].ToString()].gameObject.transform.position = new Vector3(x, y, z);
         });

        On("disconnect", (disconection) =>
        {
            GameObject objectToDestroy = connectedPlayers[id].gameObject;
            Destroy(objectToDestroy);
            connectedPlayers.Remove(id);
            Debug.Log("se desconecto un player");
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
    public PlayerData()
    {
        position = new Vector3Data();
    }
}
[System.Serializable]
class Vector3Data
{
    public float x;
    public float y;
    public float z;
}