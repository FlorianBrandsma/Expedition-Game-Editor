using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
