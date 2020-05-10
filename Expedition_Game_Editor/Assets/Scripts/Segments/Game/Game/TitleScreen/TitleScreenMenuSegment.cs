using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenMenuSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public Button continueButton;
    public Button newGameButton;
    public Button loadGameButton;
    public Button editorButton;
    public Button exitGameButton;

    public void InitializeDependencies() { }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        InitializeEditorButton();
    }

    private void InitializeEditorButton()
    {
        if (GlobalManager.programType != GlobalManager.Scenes.Editor) return;
        
        editorButton.gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        
    }

    public void NewGame()
    {

    }

    public void LoadGame()
    {
        var gameMenu = new PathManager.GameMenu();

        RenderManager.Render(gameMenu.LoadGameSave());
    }

    public void OpenEditor()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Editor);
    }

    public void ExitGame()
    {
        GlobalManager.CloseApplication();
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
