using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class OptionOrganizer : MonoBehaviour
{
    public Text header;

    public List<RectTransform> options = new List<RectTransform>();

    public RectTransform main_editor_parent;

    public int wrap_limit;

    public void SortOptions()
    {
        for (int i = 0; i < options.Count; i++)
        {
            RectTransform new_option = options[i].GetComponent<RectTransform>();

            if (options.Count <= wrap_limit)
            {
                new_option.anchorMin = new Vector2(      i * (1f / wrap_limit), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / wrap_limit), 1);
            } else {
                new_option.anchorMin = new Vector2(      i * (1f / options.Count), 1);
                new_option.anchorMax = new Vector2((i + 1) * (1f / options.Count), 1);
            }

            new_option.offsetMin = new Vector2(-2.5f, new_option.offsetMin.y);
            new_option.offsetMax = new Vector2( 2.5f, new_option.offsetMax.y);
        }

        if(options.Count > 0)
            SetEditorSize(true);
    }

    public void SetEditorSize(bool collapse)
    {
        if(collapse)
        {
            main_editor_parent.anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, GetComponent<RectTransform>().anchorMax.y);
        } else {
            main_editor_parent.anchorMin = Vector2.zero;
        }  
    }

    public void CloseOptions()
    {
        for (int i = 0; i < options.Count; i++)
            options[i].gameObject.SetActive(false);

        SetEditorSize(false);

        options.Clear();
    }
}
