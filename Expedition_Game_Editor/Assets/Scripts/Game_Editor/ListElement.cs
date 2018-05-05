using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//REVIEW

public class ListElement : MonoBehaviour
{
    public Text id, header, content;

    public Button edit_button;

    private float[] offset_max = new float[] { -5, -25 };

    public void SetOffset()
    {
        RectTransform content_rect = content.GetComponent<RectTransform>();

        if (header.text == "")
            content_rect.offsetMax = new Vector2(content_rect.offsetMax.x, offset_max[0]);
        else
            content_rect.offsetMax = new Vector2(content_rect.offsetMax.x, offset_max[1]);
    }
}
