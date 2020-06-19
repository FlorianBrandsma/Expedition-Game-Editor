using UnityEngine;
using System.Collections;

public class LayoutContent : MonoBehaviour
{
    public RectTransform header, content, footer;

    public void SetContent()
    {
        if (header != null && header.gameObject.activeInHierarchy)
            content.offsetMax = new Vector2(content.offsetMax.x, header.offsetMin.y);

        if (footer != null && footer.gameObject.activeInHierarchy)
            content.offsetMin = new Vector2(content.offsetMin.x, footer.offsetMax.y);
    }

    public void CloseContent()
    {
        content.offsetMin = new Vector2();
        content.offsetMax = new Vector2();
    }
}
