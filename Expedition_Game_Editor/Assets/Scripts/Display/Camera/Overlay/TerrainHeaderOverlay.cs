using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TerrainHeaderOverlay : MonoBehaviour, IOverlay
{
    private ExText terrainInfoText;

    private WorldOrganizer worldOrganizer;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer)
    {
        worldOrganizer = (WorldOrganizer)organizer;
        
        var prefab = Resources.Load<ExText>("UI/Text");
        terrainInfoText = (ExText)PoolManager.SpawnObject(0, prefab);

        terrainInfoText.transform.SetParent(overlayManager.horizontal_min, false);
        terrainInfoText.transform.localPosition = new Vector2(0, 0);

        terrainInfoText.gameObject.SetActive(true);
    }
    
    public void UpdateOverlay()
    {
        SetText();
    }

    public void SetOverlay()
    {
        SetText();
    }

    private void SetText()
    {
        //The atmosphere part is only temporary for debugging
        terrainInfoText.Text.text = worldOrganizer.activeTerrainData.name + " (Atmosphere: " + (worldOrganizer.activeTerrainData.activeAtmosphere.Default ? 
            "Default" :
            TimeManager.FormatTime(worldOrganizer.activeTerrainData.activeAtmosphere.StartTime, true) + 
            " - " + 
            TimeManager.FormatTime(worldOrganizer.activeTerrainData.activeAtmosphere.EndTime)) + ")";

        terrainInfoText.RectTransform.anchorMin = new Vector2(0, 0);
        terrainInfoText.RectTransform.anchorMax = new Vector2(1, 1);
    }

    public void ResetText()
    {
        PoolManager.ClosePoolObject(terrainInfoText);
    }

    public void CloseOverlay()
    {
        ResetText();

        DestroyImmediate(this);
    }
}
