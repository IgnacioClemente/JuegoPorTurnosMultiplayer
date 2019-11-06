using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MyNetworkIdentity))]
public class MyNetworkTransform : MonoBehaviour
{
	MyNetworkIdentity identity;
    PlayerData data;

    private void Start()
    {
        identity = GetComponent<MyNetworkIdentity>();
        if (!identity.HasControl)
        {
            enabled = false;
        }

        data = new PlayerData();
    }

    private void Update()
    {
        if (identity.HasControl)
        {
            SendData();
        }
       /* if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x - 5 * Time.deltaTime, transform.position.y);
        }*/
    }

    void SendData()
    {
        data.position.x = (double)Math.Round(transform.position.x * 1000.0) / 1000.0;
        data.position.y = (double)Math.Round(transform.position.y * 1000.0) / 1000.0;
        data.position.z = (double)Math.Round(transform.position.z * 1000.0) / 1000.0;
        data.rotation.x = (double)Math.Round(transform.rotation.x * 1000.0f) / 1000.0f;
        data.rotation.y = (double)Math.Round(transform.rotation.y * 1000.0f) / 1000.0f;
        data.id = identity.ID;
        identity.Socket.Emit("update", new JSONObject(JsonUtility.ToJson(data)));
    }
}
