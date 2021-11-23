using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class endSaveData : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void saveDataJS();

    // Start is called before the first frame update
    void Start()
    {
        saveDataJS();
        
    }

 
}
