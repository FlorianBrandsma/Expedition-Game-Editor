﻿using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TerrainTileEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }
    private TerrainTileDataElement terrainTileData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        terrainTileData = data.element.Cast<TerrainTileDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            terrainTileData.ClearChanges();
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
        var list = data.controller.DataList.Cast<TerrainTileDataElement>().ToList();

        list.RemoveAt(terrainTileData.index);
        list.Insert(index, terrainTileData);

        selectionElement.ListManager.listProperties.SegmentController.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        selectionElement.ListManager.UpdateData();
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
        return terrainTileData.changed;
    }

    public void ApplyChanges()
    {
        terrainTileData.Update();

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
