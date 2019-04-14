﻿using UnityEngine;
using System.Collections;

public class SegmentPlaceholder : MonoBehaviour, ISegment
{
    public IEditor dataEditor { get; set; }
    SegmentController segmentController;

    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
    }

    public void OpenSegment()
    {

    }

    public void Activate(EditorController new_controller)
    {

    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }
}