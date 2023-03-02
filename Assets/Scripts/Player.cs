using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Update()
    {

        if (Input.GetKey(KeyCode.W))
            Debug.Log("Pressing!");
        if (Input.GetKey(KeyCode.S))
            Debug.Log("Pressing!");
        if (Input.GetKey(KeyCode.A))
            Debug.Log("Pressing!");
        if (Input.GetKey(KeyCode.D))
            Debug.Log("Pressing!");
    }
}
