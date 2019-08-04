using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    private TerrainDataElement terrainData;

    public EditorTile IconTile { get { return GetComponent<EditorTile>(); } }

    public IEditor DataEditor { get; set; }
    
    #region UI

    public SelectionElement selectionElement;
    public InputField inputField;
    public Text idText;

    #endregion

    #region Data Methods

    public void UpdateName()
    {
        terrainData.Name = inputField.text;
        DataEditor.UpdateEditor();
    }

    private void UpdateIcon(IconDataElement iconDataElement)
    {
        terrainData.IconId = iconDataElement.id;
        terrainData.iconPath = iconDataElement.Path;
        
        DataEditor.UpdateEditor();
    }

    #endregion

    #region Segment

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();

        selectionElement.InitializeElement(selectionElement.GetComponent<IDataController>());
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeData()
    {
        terrainData = (TerrainDataElement)DataEditor.Data.dataElement;
    }

    public void OpenSegment()
    {
        idText.text = terrainData.id.ToString();

        inputField.text = terrainData.Name;

        var iconDataElement = new IconDataElement();

        iconDataElement.id = terrainData.IconId;
        iconDataElement.Path = terrainData.iconPath;

        iconDataElement.baseIconPath = terrainData.baseTilePath;

        iconDataElement.SelectionElement = selectionElement;

        selectionElement.data.dataController.DataList = new List<IDataElement>() { iconDataElement };
        selectionElement.data.dataElement = iconDataElement;

        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Icon>().FirstOrDefault();

        searchParameters.category = Enum.GetValues(typeof(Enums.IconCategory)).Cast<int>().ToList();

        selectionElement.data.searchParameters = new[] { searchParameters };

        selectionElement.SetElement();

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
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
