using UnityEngine;
using System.Collections;

public class EditorSegment : MonoBehaviour
{
    public GameObject content;
    bool collapsed;

    public void OpenSegment()
    {
        GetComponent<IEditor>().OpenEditor();

        CollapseSegment(false);
    }

    public void CollapseSegment()
    {
        CollapseSegment(collapsed);
    }

    void CollapseSegment(bool collapse)
    {
        content.SetActive(!collapse);
        collapsed = collapse;
    }

    public void CloseSegment()
    {
        
    }
}
