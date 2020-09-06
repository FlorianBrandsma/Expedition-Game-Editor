using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    private TerrainElementData TerrainData { get { return (TerrainElementData)DataEditor.ElementData; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    #region UI
    public EditorElement editorElement;
    public InputField inputField;
    public Text idText;
    #endregion

    #region Methods
    public void UpdateName()
    {
        var terrainData = (TerrainElementData)DataEditor.ElementData;
        terrainData.Name = inputField.text;
        
        DataEditor.UpdateEditor();
    }

    private void UpdateIcon(IconElementData iconElementData)
    {
        var terrainData = (TerrainElementData)DataEditor.ElementData;

        terrainData.IconId = iconElementData.Id;
        terrainData.IconPath = iconElementData.Path;

        DataEditor.UpdateEditor();
    }

    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        editorElement.DataElement.InitializeElement(editorElement.GetComponent<IDataController>());
    }
    
    public void InitializeData() { }

    public void OpenSegment()
    {
        gameObject.SetActive(true);

        idText.text = TerrainData.Id.ToString();
        inputField.text = TerrainData.Name;

        var iconElementData = new IconElementData();

        iconElementData.Id              = TerrainData.IconId;
        iconElementData.Path            = TerrainData.IconPath;
        iconElementData.BaseIconPath    = TerrainData.BaseTilePath;

        iconElementData.DataElement = editorElement.DataElement;

        editorElement.DataElement.Data.dataList = new List<IElementData>() { iconElementData };
        editorElement.DataElement.Id = iconElementData.Id;

        SelectionElementManager.Add(editorElement);
        SelectionManager.SelectData(editorElement.DataElement.Data.dataList);

        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Icon>().First();
        searchParameters.category = Enum.GetValues(typeof(Enums.IconCategory)).Cast<int>().ToList();

        editorElement.DataElement.Data.searchProperties = searchProperties;

        editorElement.DataElement.SetElement();

        editorElement.SetOverlay(); 
    }

    public void CloseSegment()
    {
        SelectionElementManager.elementPool.Remove(editorElement);

        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement)
    {
        switch (dataElement.Data.dataController.DataType)
        {
            case Enums.DataType.Icon:

                var iconElementData = (IconElementData)dataElement.ElementData;

                UpdateIcon(iconElementData);

                break;

            default: Debug.Log("CASE MISSING: " + dataElement.Data.dataController.DataType); break;
        }
    }
    #endregion
}
