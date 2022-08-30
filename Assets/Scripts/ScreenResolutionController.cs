using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionController : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(960, 540, false);
    }
}
