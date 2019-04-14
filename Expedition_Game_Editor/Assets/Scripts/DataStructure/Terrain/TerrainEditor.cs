﻿using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TerrainEditor : MonoBehaviour, IEditor
{
    public Enums.DataType data_type { get { return Enums.DataType.Terrain; } }

    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }
    private TerrainDataElement terrainData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        SetList();

        data = pathController.route.data;

        terrainData = data.Cast<TerrainDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            terrainData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateElement()
    {
        selectionElement.UpdateElement();
    }

    public void UpdateIndex(int index)
    {
        UpdateList();
    }

    private void SetList()
    {
        data_list = selectionElement.listManager.listProperties.dataController.data_list;
    }

    private void UpdateList()
    {
        SetList();
        selectionElement.listManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        pathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return terrainData.changed;
    }

    public void ApplyChanges()
    {
        terrainData.Update();

        UpdateList();

        UpdateEditor();
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}
