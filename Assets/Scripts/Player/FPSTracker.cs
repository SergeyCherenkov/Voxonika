using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSTracker : MonoBehaviour
{
    [SerializeField] private Text text;

    void Update()
    {
        float fps = 1.0f / Time.deltaTime;
        
        text.text = "" + fps;
    }
}
