using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorMenuSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public Button gameButton;

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }
    
    public void InitializeData() { }
    
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
        RenderManager.CloseForms();
        Debug.Log("Open game");
        SceneManager.LoadScene("Game");
    }

    public void ApplySegment() { }

    public void CloseSegment()
    {
        gameButton.onClick.RemoveAllListeners();
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
