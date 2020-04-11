using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TerrainInfoManager : MonoBehaviour, IOverlay
{
    static public List<Text> textList = new List<Text>();
    private Text terrainInfoText;

    private WorldOrganizer worldOrganizer;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        worldOrganizer = (WorldOrganizer)organizer;

        terrainInfoText = SpawnText();

        terrainInfoText.transform.SetParent(overlayManager.horizontal_min, false);
        terrainInfoText.transform.localPosition = new Vector2(0, 0);
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
        terrainInfoText.text = worldOrganizer.activeTerrainData.name;
    }

    private Text SpawnText()
    {
        foreach (Text element in textList)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                return element;
            }
        }

        Text newText = Instantiate(Resources.Load<Text>("UI/Text"));
        textList.Add(newText);

        return newText;
    }

    public void ResetText()
    {
        foreach (Text element in textList)
            element.gameObject.SetActive(false);
    }

    public void CloseOverlay()
    {
        ResetText();

        DestroyImmediate(this);
    }
}
