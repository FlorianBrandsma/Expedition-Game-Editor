using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var saveData = (SaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Save).ElementData;

        GameManager.instance.LoadGameSaveData(saveData);
    }

    public void OpenSegment()
    {
        GameManager.instance.OpenGame();
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
