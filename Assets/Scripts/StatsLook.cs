using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsLook : MonoBehaviour
{
    public Transform lookAt;

    private Transform localTrans;

    private void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    void Update()
    {
        if (lookAt)
        {
            localTrans.LookAt(2 * localTrans.position - lookAt.position);
        }
    }
}
