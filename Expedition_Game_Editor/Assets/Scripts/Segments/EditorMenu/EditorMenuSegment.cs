using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenuSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public Button gameButton;

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        InitializeGameButton();
    }

    private void InitializeGameButton()
    {
        gameButton.onClick.AddListener(delegate { OpenGame(); });
    }

    private void OpenGame()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Game);
    }

    public void ExitEditor()
    {
        GlobalManager.CloseApplication();
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameButton.onClick.RemoveAllListeners();
    }
}
