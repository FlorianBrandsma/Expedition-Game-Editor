using UnityEngine;
using UnityEngine.UI;

public class GameDataMenuSegment : MonoBehaviour, ISegment
{
    public Text headerText;

    public Button titleScreenButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        headerText.text = "Options";
    }

    public void ContinueGame()
    {
        RenderManager.PreviousPath();
    }

    public void OpenSaveGame()
    {
        var gameMenu = new PathManager.GameMenu();

        RenderManager.Render(gameMenu.OpenSaveMenu(Enums.SaveType.Save));
    }

    public void OpenLoadGame()
    {
        var gameMenu = new PathManager.GameMenu();

        RenderManager.Render(gameMenu.OpenSaveMenu(Enums.SaveType.Load));
    }

    public void OpenTitleScreen()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Game);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
