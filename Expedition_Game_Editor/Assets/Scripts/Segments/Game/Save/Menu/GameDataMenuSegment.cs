using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataMenuSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public Button titleScreenButton;

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        InitializeGameButton();
    }

    private void InitializeGameButton()
    {
        titleScreenButton.onClick.AddListener(delegate { OpenGame(); });
    }

    private void OpenGame()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Game);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        titleScreenButton.onClick.RemoveAllListeners();
    }
}
