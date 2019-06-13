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
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeData()
    {
        terrainData = (TerrainDataElement)DataEditor.Data.DataElement;
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

        selectionElement.route.data.DataController.DataList = new List<IDataElement>() { iconDataElement };
        selectionElement.route.data.DataElement = iconDataElement;

        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Icon>().FirstOrDefault();

        searchParameters.category = Enum.GetValues(typeof(Enums.IconCategory)).Cast<int>().ToList();

        selectionElement.route.data.SearchParameters = new[] { searchParameters };

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
        switch (selectionElement.route.data.DataController.DataType)
        {
            case Enums.DataType.Icon:

                var iconDataElement = (IconDataElement)selectionElement.route.data.DataElement;

                UpdateIcon(iconDataElement);

                break;

            default: Debug.Log("CASE MISSING: " + selectionElement.route.data.DataController.DataType); break;
        }
    }
    #endregion
}
