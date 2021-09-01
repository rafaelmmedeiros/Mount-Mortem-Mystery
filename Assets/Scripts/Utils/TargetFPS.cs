using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
#if UNITY_EDITOR
    private void Start()
    {

        // Limit FPS to preserve GPU
        // Remove to prod
        Application.targetFrameRate = 60;
    }
#endif
}
