using UnityEngine;
using System.Collections;

public class LayoutManager : MonoBehaviour
{
    public enum Anchor
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
    }

    public RectTransform parent_rect;

    //May need multiple siblings in order to reset the layout for Form B?
    public LayoutManager sibling_layout;
    //Position of the sibling relative to this layout
    public Anchor sibling_anchor;

    private Vector2 content_offset_min;
    private Vector2 content_offset_max;

    private Vector2 start_anchor_min = new Vector2(0, 0);
    private Vector2 start_anchor_max = new Vector2(1, 1);

    public Vector2 anchor_min { get; set; }
    public Vector2 anchor_max { get; set; }

    public RectTransform header, content, footer;

    private void Awake()
    {
        content_offset_min = content.offsetMin;
        content_offset_max = content.offsetMax;
    }

    public void InitializeLayout(Vector2 new_anchor_min, Vector2 new_anchor_max, Anchor anchor)
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (anchor == Anchor.None)
        {
            Vector2 UI_size = EditorManager.UI.rect.size;
            Vector2 parent_size = parent_rect.rect.size;

            Vector2 scaled_anchor_min = new Vector2(FixedAnchor((parent_size.x - (UI_size.x - (UI_size.x * new_anchor_min.x))) / parent_size.x),
                                                    FixedAnchor(0));

            rect.anchorMin = scaled_anchor_min;
            rect.anchorMax = new_anchor_max;

        } else {

            rect.anchorMin = new Vector2(   rect.anchorMin.x,
                                            anchor == Anchor.Top ? new_anchor_max.y : rect.anchorMin.y);

            rect.anchorMax = new Vector2(   anchor == Anchor.Left ? new_anchor_min.x : rect.anchorMax.x,
                                            rect.anchorMax.y);
        }

        anchor_min = rect.anchorMin;
        anchor_max = rect.anchorMax;

        if(sibling_layout != null)
            sibling_layout.InitializeLayout(anchor_min, anchor_max, sibling_anchor);
    }

    float FixedAnchor(float anchor)
    {
        if (anchor < 0)
            return 0;

        if (anchor > 1)
            return 1;

        return anchor;
    }

    public void SetLayout()
    {
        if (header != null && header.gameObject.activeInHierarchy)
            content.offsetMax = new Vector2(content.offsetMax.x, header.offsetMin.y);

        if (footer != null && footer.gameObject.activeInHierarchy)
            content.offsetMin = new Vector2(content.offsetMin.x, footer.offsetMax.y);

        if(sibling_layout != null)
            sibling_layout.SetLayout();
    }

    public void ResetSiblingLayout()
    {
        if (sibling_layout != null)
            sibling_layout.InitializeLayout(start_anchor_max, start_anchor_min, sibling_anchor);
    }

    public void CloseLayout()
    {
        content.offsetMin = content_offset_min;
        content.offsetMax = content_offset_max;
    }
}
