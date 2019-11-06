using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        /*
        if (Input.GetKeyDown(KeyCode.W))
            transform.Translate(0, 0, 5);
        if (Input.GetKeyDown(KeyCode.S))
            transform.Translate(0, 0, -5);

        if (Input.GetKeyDown(KeyCode.D))
            transform.Translate(5, 0, 0);
        if (Input.GetKeyDown(KeyCode.A))
            transform.Translate(-5, 0, 0);*/
    }
}
