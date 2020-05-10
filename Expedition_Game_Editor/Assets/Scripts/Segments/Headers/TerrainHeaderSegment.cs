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
    public SelectionElement selectionElement;
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
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        selectionElement.InitializeElement(selectionElement.GetComponent<IDataController>());
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

        iconDataElement.SelectionElement = selectionElement;

        selectionElement.data.dataController.DataList = new List<IDataElement>() { iconDataElement };
        selectionElement.data.dataElement = iconDataElement;

        SelectionElementManager.Add(selectionElement);
        SelectionManager.SelectData(selectionElement.data.dataController.DataList);

        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Icon>().First();
        searchParameters.category = Enum.GetValues(typeof(Enums.IconCategory)).Cast<int>().ToList();

        selectionElement.data.searchProperties = searchProperties;

        selectionElement.SetElement();

        selectionElement.SetOverlay(); 
    }

    public void CloseSegment()
    {
        SelectionElementManager.elementPool.Remove(selectionElement);

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
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
