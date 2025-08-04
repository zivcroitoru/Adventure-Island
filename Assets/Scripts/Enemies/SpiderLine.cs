using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpiderLine : MonoBehaviour
{
    [SerializeField] private float topY = 10f;
    [SerializeField] private float xOffset = 0f;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.useWorldSpace = true;
    }

    private void Update()
    {
        Vector3 spiderPos = transform.position;
        Vector3 topPoint = new Vector3(spiderPos.x + xOffset, topY, spiderPos.z);

        line.SetPosition(0, spiderPos);
        line.SetPosition(1, topPoint);
    }
}
