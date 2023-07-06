using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflection : MonoBehaviour
{
    public Transform probe;
    private Transform playerCamera;

    private void Start()
    {
        playerCamera = GetComponent<Transform>();
        probe.position = new Vector3(playerCamera.position.x, playerCamera.position.y, playerCamera.position.z);
    }
    void Update()
    {
        Vector3 pos = probe.position;
        pos.y = -Mathf.Abs(playerCamera.position.y);
        probe.position = new Vector3(playerCamera.position.x, pos.y, playerCamera.position.z);
    }
}