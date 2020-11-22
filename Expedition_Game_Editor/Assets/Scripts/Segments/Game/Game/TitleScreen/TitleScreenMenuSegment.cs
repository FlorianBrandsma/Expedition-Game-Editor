using UnityEngine;
using UnityEngine.UI;

public class TitleScreenMenuSegment : MonoBehaviour, ISegment
{
    public Button continueButton;
    public Button newGameButton;
    public Button loadGameButton;
    public Button editorButton;
    public Button exitGameButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        InitializeEditorButton();
    }

    private void InitializeEditorButton()
    {
        //if (GlobalManager.programType != GlobalManager.Scenes.Editor) return;
        
        editorButton.gameObject.SetActive(true);
    }

    public void ContinueGame() { }

    public void NewGame() { }

    public void LoadGame()
    {
        var gameMenu = new PathManager.GameMenu();

        RenderManager.Render(gameMenu.LoadSave());
    }

    public void OpenEditor()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Editor);
    }

    public void ExitGame()
    {
        GlobalManager.CloseApplication();
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
