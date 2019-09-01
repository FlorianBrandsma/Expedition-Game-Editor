﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    private PhaseDataElement phaseData;

    public List<RegionDataElement> regionDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Route.Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(phaseData);

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        phaseData = (PhaseDataElement)Data.dataElement;
        regionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.dataController.DataList.Cast<PhaseDataElement>().ToList();

        list.RemoveAt(phaseData.Index);
        list.Insert(index, phaseData);

        Data.dataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        SelectionElementManager.UpdateElements(phaseData, true);
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        DataElements.ForEach(x => x.Update());

        SelectionElementManager.UpdateElements(phaseData);

        UpdateEditor();
    }

    public void CancelEdit()
    {
        EditorManager.editorManager.PreviousEditor();
    }

    public void CloseEditor()
    {

    }
}
