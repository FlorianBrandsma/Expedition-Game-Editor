using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TerrainHeaderSegment : MonoBehaviour, ISegment
{
    public IconDataController iconDataController;

    public EditorElement iconEditorElement;
    public InputField inputField;
    public Text idText;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    private TerrainEditor TerrainEditor         { get { return (TerrainEditor)DataEditor; } }

    private IconElementData IconElementData     { get { return (IconElementData)iconEditorElement.DataElement.ElementData; } }

    #region Data properties
    private int Id
    {
        get { return TerrainEditor.Id; }
    }

    private int IconId
    {
        get { return TerrainEditor.IconId; }
        set { TerrainEditor.IconId = value; }
    }

    private string Name
    {
        get { return TerrainEditor.Name; }
        set { TerrainEditor.Name = value; }
    }

    private string IconPath
    {
        get { return TerrainEditor.IconPath; }
        set { TerrainEditor.IconPath = value; }
    }

    private string BaseTilePath
    {
        get { return TerrainEditor.BaseTilePath; }
        set { TerrainEditor.BaseTilePath = value; }
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
        var iconElementData = new IconElementData()
        {
            Id = IconId,

            Path = IconPath,
            BaseIconPath = BaseTilePath,

            DataElement = iconEditorElement.DataElement
        };

        iconElementData.SetOriginalValues();

        var iconData = new Data()
        {
            dataController = iconDataController,
            dataList = new List<IElementData>() { iconElementData },
            searchProperties = iconDataController.SearchProperties
        };
        
        iconEditorElement.DataElement.Data = iconData;
        iconEditorElement.DataElement.Id = IconId;

        SetIconData();

        iconEditorElement.DataElement.InitializeElement();
    }
    
    private void SetIconData()
    {
        iconDataController.Data = iconEditorElement.DataElement.Data;

        IconElementData.Id = IconId;
        IconElementData.Path = IconPath;
        IconElementData.BaseIconPath = BaseTilePath;
    }

    public void OpenSegment()
    {
        idText.text = Id.ToString();
        inputField.text = Name;

        SelectionElementManager.Add(iconEditorElement);
        SelectionManager.SelectData(iconEditorElement.DataElement.Data.dataList);

        iconEditorElement.DataElement.SetElement();
        iconEditorElement.SetOverlay();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        switch (mergedElementData.DataType)
        {
            case Enums.DataType.Icon:

                var iconElementData = (IconElementData)mergedElementData;
                UpdateIcon(iconElementData);

                break;

            default: Debug.Log("CASE MISSING: " + mergedElementData.DataType); break;
        }
    }

    public void UpdateIcon(IconElementData iconElementData)
    {
        IconId = iconElementData.Id;
        IconPath = iconElementData.Path;

        SetIconData();

        DataEditor.UpdateEditor();
    }

    public void UpdateName()
    {
        Name = inputField.text;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        SelectionElementManager.Remove(iconEditorElement);

        gameObject.SetActive(false);
    }
}
