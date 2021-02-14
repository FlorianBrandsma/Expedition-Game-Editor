using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TitleScreenMenuSegment : MonoBehaviour, ISegment
{
    public Text headerText;

    public ExButton continueButton;
    public ExButton newGameButton;
    public ExButton loadButton;
    public ExButton editorButton;
    public ExButton exitGameButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Save);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Save>().First();

        searchParameters.includeAddElement = true;

        searchParameters.gameId = new List<int>() { 0 };
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        InitializeContinueButton();
        InitializeLoadButton();
        InitializeEditorButton();
        
        headerText.text = "Menu";
    }
    
    private void InitializeContinueButton()
    {
        var saveDataList = SegmentController.DataController.Data.dataList.Where(x => x.ExecuteType != Enums.ExecuteType.Add)
                                                                         .OrderByDescending(x => ((SaveElementData)x).SaveTime).ToList();

        continueButton.Button.interactable = saveDataList.Count > 0;

        if (saveDataList.Count == 0) return;

        var continueButtonDataElement = continueButton.EditorElement.DataElement;

        continueButtonDataElement.Id    = saveDataList.First().Id;
        continueButtonDataElement.Data  = SegmentController.DataController.Data;
        continueButtonDataElement.Path  = SegmentController.Path;
    }

    private void InitializeLoadButton()
    {
        loadButton.Button.interactable = SegmentController.DataController.Data.dataList.Where(x => x.ExecuteType != Enums.ExecuteType.Add).ToList().Count > 0;
    }

    private void InitializeEditorButton()
    {
        //if (GlobalManager.programType != GlobalManager.Scenes.Editor) return;
        
        editorButton.gameObject.SetActive(true);
    }

    public void NewGame()
    {
        var saveElementData = (SaveElementData)SegmentController.DataController.Data.dataList.Where(x => x.ExecuteType == Enums.ExecuteType.Add).First();

        saveElementData.SaveTime = System.DateTime.Now;

        saveElementData.Add(new DataRequest() { requestType = Enums.RequestType.Execute });

        var newGameButtonDataElement = newGameButton.EditorElement.DataElement;

        newGameButtonDataElement.Id     = saveElementData.Id;
        newGameButtonDataElement.Data   = SegmentController.DataController.Data;
        newGameButtonDataElement.Path   = SegmentController.Path;

        newGameButton.EditorElement.SelectElement();
    }

    public void OpenSaveMenu()
    {
        var gameMenu = new PathManager.GameMenu();

        RenderManager.Render(gameMenu.OpenSaveMenu(Enums.SaveType.Load));
    }

    public void OpenEditor()
    {
        GlobalManager.OpenScene(GlobalManager.Scenes.Editor);
    }

    public void ExitGame()
    {
        GlobalManager.CloseApplication();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
