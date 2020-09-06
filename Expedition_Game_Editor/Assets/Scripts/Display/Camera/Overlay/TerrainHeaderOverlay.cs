using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderOverlay : MonoBehaviour, IOverlay
{
    private ExText terrainInfoText;

    private EditorWorldOrganizer worldOrganizer;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer)
    {
        worldOrganizer = (EditorWorldOrganizer)organizer;
        
        var prefab = Resources.Load<ExText>("Elements/UI/Text");
        terrainInfoText = (ExText)PoolManager.SpawnObject(prefab);

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
        var activeAtmosphere = worldOrganizer.activeTerrainData.AtmosphereDataList.Where(x => x.ContainsActiveTime).First();

        //The atmosphere part is only temporary for debugging
        terrainInfoText.Text.text = worldOrganizer.activeTerrainData.Name + " (Atmosphere: " + (activeAtmosphere.Default ? 
            "Default" :
            TimeManager.FormatTime(activeAtmosphere.StartTime) + 
            " - " + 
            TimeManager.FormatTime(activeAtmosphere.EndTime)) + ")";

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
