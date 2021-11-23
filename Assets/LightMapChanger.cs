using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class LightMapChanger : MonoBehaviour
{
    MeshRenderer[] arr;

    private void Start()
    {
        arr = FindObjectsOfType<MeshRenderer>();
        foreach(MeshRenderer m in arr) 
        {
            m.scaleInLightmap = 5;
        }
    }
}
