using UnityEngine;
using System.Collections;

public class LayoutManager : MonoBehaviour
{
    public LayoutManager parent_rect;
    public LayoutManager sibling_rect;

    //Sibling relative position (choose one)
    public bool left, top;

    private Vector2 content_offset_min;
    private Vector2 content_offset_max;

    public Vector2 anchor_min { get; set; }
    public Vector2 anchor_max { get; set; }

    public RectTransform header, content, footer;

    private bool set;

    private void Awake()
    {
        content_offset_min = content.offsetMin;
        content_offset_max = content.offsetMax;
    }

    public void InitializeLayout(Vector2 new_anchor_min, Vector2 new_anchor_max, bool overwrite)
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (overwrite)
        {
            rect.anchorMin = new_anchor_min;
            rect.anchorMax = new_anchor_max;
        } else {

            rect.anchorMin = new Vector2(   rect.anchorMin.x,
                                            top ? new_anchor_max.y : rect.anchorMin.y);

            rect.anchorMax = new Vector2(   left ? new_anchor_min.x : rect.anchorMax.x,
                                            rect.anchorMax.y);
        }

        anchor_min = rect.anchorMin;
        anchor_max = rect.anchorMax;

        if(sibling_rect != null)
            sibling_rect.InitializeLayout(anchor_min, anchor_max, false);
    }

    public void SetLayout()
    {
        if (header != null && header.gameObject.activeInHierarchy)
            content.offsetMax = new Vector2(content.offsetMax.x, header.offsetMin.y);

        if (footer != null && footer.gameObject.activeInHierarchy)
            content.offsetMin = new Vector2(content.offsetMin.x, footer.offsetMax.y);

        set = true;

        if (sibling_rect != null)
            sibling_rect.SetLayout();
    }

    public void ResetLayout()
    {
        CloseLayout();
        SetLayout();
    }

    public void CloseLayout()
    {
        set = false;

        content.offsetMin = content_offset_min;
        content.offsetMax = content_offset_max;
    }
}
