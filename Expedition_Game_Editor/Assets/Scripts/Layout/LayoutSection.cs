using UnityEngine;
using System.Collections;

public class LayoutSection : MonoBehaviour
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

    public LayoutSection sibling_layout;

    public Anchor anchor;

    private RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

    private Vector2 initialAnchorMin, initialAnchorMax;

    private Vector2 anchorMin, anchorMax;
    
    public void InitializeAnchors()
    {
        initialAnchorMin = new Vector2(anchor == Anchor.Right ? 1 : 0, 0);
        initialAnchorMax = new Vector2(anchor == Anchor.Left ? 0 : 1, 1);

        anchorMin = initialAnchorMin;
        anchorMax = initialAnchorMax;
    }

    public void InitializeLayout(Vector2 anchorMin, Vector2 anchorMax)
    {
        this.anchorMin = anchorMin;
        this.anchorMax = anchorMax;
    }
    
    public void SetLayout()
    {
        if (!gameObject.activeInHierarchy) return;

        if (sibling_layout != null)
        {
            //anchorMin = new Vector2(RectTransform.anchorMin.x, RectTransform.anchorMin.y);
            anchorMax = new Vector2(anchor == Anchor.Left ? sibling_layout.anchorMin.x : anchorMax.x, anchorMax.y);
        }

        RectTransform.anchorMin = new Vector2(ScaledWidth(anchorMin.x), anchorMin.y);
        RectTransform.anchorMax = new Vector2(ScaledWidth(anchorMax.x), anchorMax.y);
    }

    private float ScaledWidth(float anchor)
    {
        var UISize = EditorManager.UI.rect.size;
        var parentSize = parent_rect.rect.size;

        return FixedAnchor((parentSize.x - (UISize.x - (UISize.x * anchor))) / parentSize.x);
    }

    float FixedAnchor(float anchor)
    {
        if (anchor < 0)
            return 0;

        if (anchor > 1)
            return 1;

        return anchor;
    }

    public void CloseSection()
    {
        anchorMin = initialAnchorMin;
        anchorMax = initialAnchorMax;
    }
}
