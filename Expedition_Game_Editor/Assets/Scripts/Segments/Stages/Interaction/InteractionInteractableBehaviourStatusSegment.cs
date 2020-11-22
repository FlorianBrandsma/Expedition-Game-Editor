using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InteractionInteractableBehaviourStatusSegment : MonoBehaviour, ISegment
{
    public Text nameText;
    public Text stateText;
    public Text locationText;

    public DataElement editButton;
    public RawImage buttonIcon;
    
    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private InteractionEditor InteractionEditor     { get { return (InteractionEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
