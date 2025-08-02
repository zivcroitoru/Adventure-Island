using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 followPos = player.position + offset;
        followPos.z = transform.position.z; // keep Z fixed
        transform.position = followPos;
    }
}
