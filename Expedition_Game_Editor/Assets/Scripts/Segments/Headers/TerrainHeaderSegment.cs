using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    private TerrainDataElement TerrainData { get { return (TerrainDataElement)DataEditor.Data.dataElement; } }

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
        var terrainData = (TerrainDataElement)DataEditor.Data.dataElement;
        terrainData.Name = inputField.text;
        
        DataEditor.UpdateEditor();
    }

    private void UpdateIcon(IconDataElement iconDataElement)
    {
        var terrainData = (TerrainDataElement)DataEditor.Data.dataElement;

        terrainData.IconId = iconDataElement.Id;
        terrainData.iconPath = iconDataElement.Path;

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

        var iconDataElement = new IconDataElement();

        iconDataElement.Id              = TerrainData.IconId;
        iconDataElement.Path            = TerrainData.iconPath;
        iconDataElement.baseIconPath    = TerrainData.baseTilePath;

        iconDataElement.DataElement = editorElement.DataElement;

        editorElement.DataElement.data.dataController.DataList = new List<IDataElement>() { iconDataElement };
        editorElement.DataElement.data.dataElement = iconDataElement;

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

    public void SetSearchResult(DataElement selectionElement)
    {
        switch (selectionElement.data.dataController.DataType)
        {
            case Enums.DataType.Icon:

                var iconDataElement = (IconDataElement)selectionElement.data.dataElement;

                UpdateIcon(iconDataElement);

                break;

            default: Debug.Log("CASE MISSING: " + selectionElement.data.dataController.DataType); break;
        }
    }
    #endregion
}
