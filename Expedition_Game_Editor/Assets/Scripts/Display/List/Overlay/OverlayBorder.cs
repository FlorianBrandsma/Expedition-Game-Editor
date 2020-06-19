using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverlayBorder : MonoBehaviour
{
    public bool     always_active;

    public bool     horizontal, 
                    vertical;

    public Vector2 offset_min;
    public Vector2 offset_max;

    public RectTransform scrollParent;

    public void Activate()
    {
        GetComponent<RectTransform>().offsetMin = offset_min;
        GetComponent<RectTransform>().offsetMax = offset_max;

        if (always_active)
            gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
