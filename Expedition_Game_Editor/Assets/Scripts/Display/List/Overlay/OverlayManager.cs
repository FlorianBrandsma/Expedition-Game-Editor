using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayManager : MonoBehaviour
{
    private RectTransform   mainList;

    public RectTransform    horizontal_min,
                            vertical_min,
                            horizontal_max,
                            vertical_max,
                            content;

    public RectTransform[] layer;

    public NumberOverlay NumberOverlay { get; set; }
    public SliderOverlay SliderOverlay { get; set; }
    public PagingOverlay PagingOverlay { get; set; }
    public HeaderOverlay HeaderOverlay { get; set; }

    public TrackingElementOverlay TrackingElementOverlay    { get; set; }
    public TerrainHeaderOverlay TerrainHeaderOverlay        { get; set; }
    public GameOverlay GameOverlay                          { get; set; }
    public TouchOverlay TouchOverlay                        { get; set; }
    public CameraFilterOverlay CameraFilterOverlay          { get; set; }
    
    public IDisplayManager DisplayManager { get; set; }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        DisplayManager = displayManager;

        mainList = DisplayManager.RectTransform;

        if (horizontal_min != null)
            horizontal_min.GetComponent<OverlayBorder>().Activate();

        if (horizontal_max != null)
            horizontal_max.GetComponent<OverlayBorder>().Activate();

        if (vertical_min != null)
            vertical_min.GetComponent<OverlayBorder>().Activate();

        if (vertical_max != null)
            vertical_max.GetComponent<OverlayBorder>().Activate();
    }

    public void SetOverlayProperties(IDisplay displayProperties)
    {
        switch (displayProperties.DisplayType)
        {
            case Enums.DisplayType.List:    SetListOverlayProperties(displayProperties);    break;
            case Enums.DisplayType.Camera:  SetCameraOverlayProperties(displayProperties);  break;
        }

        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.InitializeOverlay(DisplayManager);
    }

    private void SetListOverlayProperties(IDisplay displayProperties)
    {
        var listProperties = (ListProperties)displayProperties;

        if (listProperties.enableNumbers)
            NumberOverlay = gameObject.AddComponent<NumberOverlay>();

        if (listProperties.enableSliders)
            SliderOverlay = gameObject.AddComponent<SliderOverlay>();

        if (listProperties.enablePaging)
            PagingOverlay = gameObject.AddComponent<PagingOverlay>();

        if (listProperties.headerText.Length > 0)
            HeaderOverlay = gameObject.AddComponent<HeaderOverlay>();
    }

    private void SetCameraOverlayProperties(IDisplay displayProperties)
    {
        var cameraProperties = (CameraProperties)displayProperties;

        if (cameraProperties.enableTrackingElements)
            TrackingElementOverlay = gameObject.AddComponent<TrackingElementOverlay>();

        if (cameraProperties.enableTerrainInfo)
            TerrainHeaderOverlay = gameObject.AddComponent<TerrainHeaderOverlay>();

        if (cameraProperties.enableGameUI)
        {
            GameOverlay = gameObject.AddComponent<GameOverlay>();

            if (PlayerControlManager.instance.ControlType == Enums.ControlType.Touch)
                TouchOverlay = gameObject.AddComponent<TouchOverlay>();
        }

        if (cameraProperties.enableCameraFilters)
            CameraFilterOverlay = gameObject.AddComponent<CameraFilterOverlay>();
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.ActivateOverlay(organizer);
    }

    public void SetOverlaySize()
    {
        if (horizontal_min.gameObject.activeInHierarchy)
        {
            vertical_min.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_min.offsetMax.x, -horizontal_min.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_max.offsetMax.x, -horizontal_min.rect.height);
        } 

        if (horizontal_max.gameObject.activeInHierarchy)
        {
            vertical_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.offsetMin.x, horizontal_max.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_max.offsetMin.x, horizontal_max.rect.height);
        }

        if (vertical_min.gameObject.activeInHierarchy)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_min.offsetMin.y);
            horizontal_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_max.offsetMin.y);
        }

        if (vertical_max.gameObject.activeInHierarchy)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_min.offsetMax.y);
            horizontal_max.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_max.offsetMax.y);
        }

        SetListSize();
    }

    public void SetListSize()
    {
        if (horizontal_max.gameObject.activeInHierarchy)
            mainList.offsetMin = new Vector2(mainList.offsetMax.x, horizontal_max.rect.height);

        if (vertical_max.gameObject.activeInHierarchy)
            mainList.offsetMax = new Vector2(-vertical_max.rect.width, mainList.offsetMax.y);
    }

    public void SetOverlay()
    {
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.SetOverlay();
    }

    public void UpdateOverlay()
    {
        foreach(IOverlay overlay in GetComponents<IOverlay>())
            overlay.UpdateOverlay();  
    }

    public void CloseOverlay()
    {
        mainList.offsetMin = Vector2.zero;
        mainList.offsetMax = Vector2.zero;

        if (horizontal_min != null)
            horizontal_min.GetComponent<OverlayBorder>().Deactivate();

        if (horizontal_max != null)
            horizontal_max.GetComponent<OverlayBorder>().Deactivate();

        if (vertical_min != null)
            vertical_min.GetComponent<OverlayBorder>().Deactivate();

        if (vertical_max != null)
            vertical_max.GetComponent<OverlayBorder>().Deactivate();
  
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.CloseOverlay();    
    }  
}
