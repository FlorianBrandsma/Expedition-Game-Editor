using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideshowManager : MonoBehaviour, IOverlay
{
    private Color active_color      = Color.white;
    private Color inactive_color    = Color.gray;

    private RectTransform   main_list,
                            list_parent;

    OverlayManager          overlayManager;

    public void InitializeOverlay(RectTransform new_main_list, RectTransform new_list_parent)
    {
        overlayManager = GetComponent<OverlayManager>();

        main_list = new_main_list;
        list_parent = new_list_parent;
    }

    public void SetOverlay()
    {

    }
    public void UpdateOverlay()
    {

    }
    public void CloseOverlay()
    {
        DestroyImmediate(this);
    }
}
