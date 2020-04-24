using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HeaderOverlay : MonoBehaviour, IOverlay
{
    static public List<Text> textList = new List<Text>();
    private Text headerText;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    private ListProperties listProperties;

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        listProperties = (ListProperties)displayManager.Display;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        headerText = SpawnText();

        headerText.transform.SetParent(overlayManager.horizontal_min, false);
        headerText.transform.localPosition = new Vector2(0, 0);
    }

    public void UpdateOverlay() { }

    public void SetOverlay()
    {
        SetText();
    }

    private void SetText()
    {
        headerText.text = listProperties.headerText;
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
