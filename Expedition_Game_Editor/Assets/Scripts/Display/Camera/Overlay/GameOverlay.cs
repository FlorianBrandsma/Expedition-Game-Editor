using UnityEngine;
using System;

public class GameOverlay : MonoBehaviour, IOverlay
{
    private ExLoadingBar loadingBarPrefab;
    private ExSpeechTextBox speechTextBoxPrefab;

    private ExLoadingBar loadingBar;
    private ExSpeechTextBox speechTextBox;

    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    private void Awake()
    {
        loadingBarPrefab = Resources.Load<ExLoadingBar>("Elements/UI/LoadingBar");
        speechTextBoxPrefab = Resources.Load<ExSpeechTextBox>("Elements/UI/SpeechTextBox");
    }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    public ExLoadingBar SpawnLoadingBar(Enums.DelayMethod delayMethod)
    {
        loadingBar = (ExLoadingBar)PoolManager.SpawnObject(loadingBarPrefab);
        
        loadingBar.transform.SetParent(OverlayManager.layer[2], false);
        loadingBar.RectTransform.anchoredPosition = new Vector2(0, 100);

        loadingBar.methodText.text = Enum.GetName(typeof(Enums.DelayMethod), delayMethod);

        loadingBar.gameObject.SetActive(true);

        return loadingBar;
    }

    public ExSpeechTextBox SpawnSpeechTextBox()
    {
        speechTextBox = (ExSpeechTextBox)PoolManager.SpawnObject(speechTextBoxPrefab);

        speechTextBox.transform.SetParent(OverlayManager.layer[3], false);
        speechTextBox.RectTransform.anchoredPosition = new Vector2(0, 100);

        speechTextBox.gameObject.SetActive(true);

        return speechTextBox;
    }
    
    public void UpdateOverlay() { }

    public void SetOverlay() { }
    
    private void CloseElements()
    {
        if (loadingBar != null)
            PoolManager.ClosePoolObject(loadingBar);

        if (speechTextBox != null)
            PoolManager.ClosePoolObject(speechTextBox);
    }

    public void CloseOverlay()
    {
        CloseElements();

        DestroyImmediate(this);
    }
}
