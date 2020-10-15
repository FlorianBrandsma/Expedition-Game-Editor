using UnityEngine;
using System;

public class GameOverlay : MonoBehaviour, IOverlay
{
    private ExLoadingBar loadingBarPrefab;

    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    private void Awake()
    {
        loadingBarPrefab = Resources.Load<ExLoadingBar>("Elements/UI/LoadingBar");
    }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    public ExLoadingBar SpawnLoadingBar(Enums.DelayMethod delayMethod)
    {
        var loadingBar = (ExLoadingBar)PoolManager.SpawnObject(loadingBarPrefab);
        
        loadingBar.transform.SetParent(OverlayManager.content, false);
        loadingBar.RectTransform.anchoredPosition = new Vector2(0, 100);

        loadingBar.methodText.text = Enum.GetName(typeof(Enums.DelayMethod), delayMethod);

        loadingBar.gameObject.SetActive(true);

        return loadingBar;
    }

    public void UpdateOverlay() { }

    public void SetOverlay() { }
    
    public void CloseOverlay()
    {
        DestroyImmediate(this);
    }
}
