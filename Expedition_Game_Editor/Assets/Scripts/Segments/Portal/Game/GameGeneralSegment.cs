using UnityEngine;
using UnityEngine.UI;

public class GameGeneralSegment : MonoBehaviour, ISegment
{
    public Text descriptionText;
    
    public EditorElement gameButton;
    public Text gameButtonText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    public bool Installed
    {
        get { return ((GameEditor)DataEditor).Installed; }
    }

    public string Description
    {
        get { return ((GameEditor)DataEditor).Description; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeGameButton();
    }

    private void InitializeGameButton()
    {
        gameButton.DataElement.Id   = DataEditor.EditData.Id;
        gameButton.DataElement.Data = DataEditor.Data.dataController.Data;
        gameButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        gameButton.InitializeElement();
    }

    public void SelectButton()
    {
        if (Installed)
            PlayGame();
        else
            InstallGame();
    }

    private void PlayGame()
    {
        GameManager.gameElementData = (GameElementData)gameButton.DataElement.ElementData;

        GlobalManager.OpenScene(GlobalManager.Scenes.Game);
    }

    private void InstallGame()
    {
        Debug.Log("Install game");
    }
    
    public void OpenSegment()
    {
        descriptionText.text = Description;
        gameButtonText.text = Installed ? "Play" : "Install"; 
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
