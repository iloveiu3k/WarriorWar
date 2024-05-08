using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] Camera lookAtCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtCamera != null)
        {
            transform.LookAt(lookAtCamera.transform);
        }
    }
}
