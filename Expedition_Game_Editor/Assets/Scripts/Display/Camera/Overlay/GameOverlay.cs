using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverlay : MonoBehaviour, IOverlay
{
    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }
    
    public void InitializeOverlay(IDisplayManager displayManager)
    {
        //listProperties = (ListProperties)displayManager.Display;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        //var prefab = Resources.Load<ExText>("Elements/UI/Text");
        //headerText = (ExText)PoolManager.SpawnObject(prefab);

        //headerText.transform.SetParent(OverlayManager.horizontal_min, false);
        //headerText.transform.localPosition = new Vector2(0, 0);

        //headerText.gameObject.SetActive(true);
    }

    public void UpdateOverlay()
    {
        //Debug.Log("Update game overlay");
    }

    public void SetOverlay()
    {
        //Debug.Log("Set game overlay");
    }
    
    public void CloseOverlay()
    {
        DestroyImmediate(this);
    }
}
