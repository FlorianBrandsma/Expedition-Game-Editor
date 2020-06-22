using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    private TerrainElementData TerrainData { get { return (TerrainElementData)DataEditor.Data.elementData; } }

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
        var terrainData = (TerrainElementData)DataEditor.Data.elementData;
        terrainData.Name = inputField.text;
        
        DataEditor.UpdateEditor();
    }

    private void UpdateIcon(IconElementData iconElementData)
    {
        var terrainData = (TerrainElementData)DataEditor.Data.elementData;

        terrainData.IconId = iconElementData.Id;
        terrainData.iconPath = iconElementData.Path;

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
        iconElementData.Path            = TerrainData.iconPath;
        iconElementData.baseIconPath    = TerrainData.baseTilePath;

        iconElementData.DataElement = editorElement.DataElement;

        editorElement.DataElement.data.dataController.DataList = new List<IElementData>() { iconElementData };
        editorElement.DataElement.data.elementData = iconElementData;

        SelectionElementManager.Add(editorElement);
        SelectionManager.SelectData(editorElement.DataElement.data.dataController.DataList);

        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Icon>().First();
        searchParameters.category = Enum.GetValues(typeof(Enums.IconCategory)).Cast<int>().ToList();

        editorElement.DataElement.data.searchProperties = searchProperties;

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
        switch (dataElement.data.dataController.DataType)
        {
            case Enums.DataType.Icon:

                var iconElementData = (IconElementData)dataElement.data.elementData;

                UpdateIcon(iconElementData);

                break;

            default: Debug.Log("CASE MISSING: " + dataElement.data.dataController.DataType); break;
        }
    }
    #endregion
}
