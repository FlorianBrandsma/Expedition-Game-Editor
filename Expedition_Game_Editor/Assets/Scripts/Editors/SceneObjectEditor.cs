﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectEditor : MonoBehaviour, IEditor
{
    public SceneObjectDataElement SceneObjectData { get { return (SceneObjectDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(SceneObjectData).Concat(new[] { SceneObjectData }).ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void InitializeEditor() { }

    public void UpdateEditor()
    {
        DataElements.ForEach(x => x.SelectionElement.UpdateElement());

        SetEditor();
    }

    public void UpdateIndex(int index) { }

    public void OpenEditor() { }

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
        SceneObjectData.Update();

        DataElements.ForEach(x =>
        {
            if (((GeneralData)x).Equals(SceneObjectData))
                x.SetOriginalValues();
            else
                x.Update();

            if (x.SelectionElement != null)
                x.SelectionElement.UpdateElement();
        });

        UpdateEditor();
    }

    public void CancelEdit()
    {
        DataElements.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}