using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeaderOverlay : MonoBehaviour, IOverlay
{
    private ExText headerText;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    private ListProperties listProperties;

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        listProperties = (ListProperties)displayManager.Display;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        var prefab = Resources.Load<ExText>("Elements/UI/Text");
        headerText = (ExText)PoolManager.SpawnObject(prefab);

        headerText.transform.SetParent(overlayManager.horizontal_min, false);
        headerText.transform.localPosition = new Vector2(0, 0);

        headerText.gameObject.SetActive(true);
    }

    public void UpdateOverlay() { }

    public void SetOverlay()
    {
        SetText();
    }

    private void SetText()
    {
        headerText.Text.text = listProperties.headerText;

        headerText.RectTransform.anchorMin = new Vector2(0, 0);
        headerText.RectTransform.anchorMax = new Vector2(1, 1);
    }

    public void ResetText()
    {
        PoolManager.ClosePoolObject(headerText);
    }

    public void CloseOverlay()
    {
        ResetText();

        DestroyImmediate(this);
    }
}
