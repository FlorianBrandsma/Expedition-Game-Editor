using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneActorActBehaviourTargetSegment : MonoBehaviour, ISegment
{
    public EditorElement sceneActorButton;

    private SceneActorDataController SceneActorDataController   { get { return GetComponent<SceneActorDataController>(); } }
    private SceneActorElementData SceneActorElementData         { get { return (SceneActorElementData)sceneActorButton.DataElement.ElementData; } }
    
    public SegmentController SegmentController                  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                                   { get; set; }

    private SceneActorEditor SceneActorEditor                   { get { return (SceneActorEditor)DataEditor; } }
    
    #region Data properties
    private int Id
    {
        get { return SceneActorEditor.Id; }
    }

    private int TargetSceneActorId
    {
        get { return SceneActorEditor.TargetSceneActorId; }
        set { SceneActorEditor.TargetSceneActorId = value; }
    }

    private int SceneId
    {
        get { return SceneActorEditor.SceneId; }
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
        InitializeSceneButton();
    }

    private void InitializeSceneButton()
    {
        //Get scene actor data
        var searchProperties = new SearchProperties(Enums.DataType.SceneActor);
        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneActor>().First();

        searchParameters.id = new List<int>() { TargetSceneActorId };

        SceneActorDataController.GetData(searchProperties);
        
        var sceneActorElementData = SceneActorDataController.Data.dataList.FirstOrDefault();

        if (sceneActorElementData == null)
        {
            sceneActorElementData = new SceneActorElementData()
            {
                InteractableName = "None"
            };

            sceneActorElementData.SetOriginalValues();

            SceneActorDataController.Data.dataList = new List<IElementData>() { sceneActorElementData };
        }

        sceneActorElementData.DataElement = sceneActorButton.DataElement;

        //Assign data
        var sceneActorData = new Data()
        {
            dataController = SceneActorDataController,
            dataList = new List<IElementData>() { sceneActorElementData },
            searchProperties = SceneActorDataController.SearchProperties
        };

        sceneActorElementData.DataElement.Data = sceneActorData;
        sceneActorElementData.DataElement.Id = TargetSceneActorId;

        sceneActorElementData.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        SetSceneActorData();

        sceneActorButton.DataElement.InitializeElement();
        sceneActorButton.GetComponent<ExPanel>().InitializeChildElement();
    }
    
    private void SetSceneActorData()
    {
        SceneActorElementData.Id = TargetSceneActorId;
        sceneActorButton.child.DataElement.Id = TargetSceneActorId;

        InitializeSearchParameters();
    }

    private void InitializeSearchParameters()
    {
        var searchParameters = SceneActorDataController.SearchProperties.searchParameters.Cast<Search.SceneActor>().First();

        searchParameters.excludeId = new List<int>() { Id, TargetSceneActorId };
        searchParameters.sceneId = new List<int>() { SceneId };

        searchParameters.includeEmptyElement = TargetSceneActorId != 0;
    }

    public void OpenSegment()
    {
        SelectionElementManager.Add(sceneActorButton);
        SelectionElementManager.Add(sceneActorButton.child);
        SelectionManager.SelectData(sceneActorButton.DataElement.Data.dataList);

        SetSceneActorButton();
    }

    private void SetSceneActorButton()
    {
        sceneActorButton.DataElement.SetElement();
    }

    public void SetSearchResult(IElementData elementData)
    {
        switch (elementData.DataType)
        {
            case Enums.DataType.SceneActor:

                var sceneActorElementData = (SceneActorElementData)elementData;
                UpdateSceneActor(sceneActorElementData);

                break;

            default: Debug.Log("CASE MISSING: " + elementData.DataType); break;
        }
    }

    public void UpdateSceneActor(SceneActorElementData sceneActorElementData)
    {
        TargetSceneActorId = sceneActorElementData.Id;

        SetSceneActorData();

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment()
    {
        SetSceneActorButton();
    }

    public void CloseSegment()
    {
        SelectionElementManager.elementPool.Remove(sceneActorButton);
        SelectionElementManager.elementPool.Remove(sceneActorButton.child);
    }
}
