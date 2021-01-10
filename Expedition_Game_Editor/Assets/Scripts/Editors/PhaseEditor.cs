using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseEditor : MonoBehaviour, IEditor
{
    private PhaseData phaseData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == phaseData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(EditData).Concat(new[] { EditData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });
            
            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return phaseData.Id; }
    }

    public int DefaultRegionId
    {
        get { return phaseData.DefaultRegionId; }
        set
        {
            phaseData.DefaultRegionId = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultRegionId = value);
        }
    }
    
    public int Index
    {
        get { return phaseData.Index; }
    }

    public string Name
    {
        get { return phaseData.Name; }
        set
        {
            phaseData.Name = value;

            DataList.ForEach(x => ((PhaseElementData)x).Name = value);
        }
    }

    public float DefaultPositionX
    {
        get { return phaseData.DefaultPositionX; }
        set
        {
            phaseData.DefaultPositionX = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultPositionX = value);
        }
    }

    public float DefaultPositionY
    {
        get { return phaseData.DefaultPositionY; }
        set
        {
            phaseData.DefaultPositionY = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultPositionY = value);
        }
    }

    public float DefaultPositionZ
    {
        get { return phaseData.DefaultPositionZ; }
        set
        {
            phaseData.DefaultPositionZ = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultPositionZ = value);
        }
    }

    public int DefaultRotationX
    {
        get { return phaseData.DefaultRotationX; }
        set
        {
            phaseData.DefaultRotationX = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultRotationX = value);
        }
    }

    public int DefaultRotationY
    {
        get { return phaseData.DefaultRotationY; }
        set
        {
            phaseData.DefaultRotationY = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultRotationY = value);
        }
    }

    public int DefaultRotationZ
    {
        get { return phaseData.DefaultRotationZ; }
        set
        {
            phaseData.DefaultRotationZ = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultRotationZ = value);
        }
    }

    public int DefaultTime
    {
        get { return phaseData.DefaultTime; }
        set
        {
            phaseData.DefaultTime = value;

            DataList.ForEach(x => ((PhaseElementData)x).DefaultTime = value);
        }
    }

    public string PublicNotes
    {
        get { return phaseData.PublicNotes; }
        set
        {
            phaseData.PublicNotes = value;

            DataList.ForEach(x => ((PhaseElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return phaseData.PrivateNotes; }
        set
        {
            phaseData.PrivateNotes = value;

            DataList.ForEach(x => ((PhaseElementData)x).PrivateNotes = value);
        }
    }

    public int TerrainTileId
    {
        get { return phaseData.TerrainTileId; }
        set
        {
            phaseData.TerrainTileId = value;

            DataList.ForEach(x => ((PhaseElementData)x).TerrainTileId = value);
        }
    }

    public string ModelIconPath
    {
        get { return phaseData.ModelIconPath; }
        set
        {
            phaseData.ModelIconPath = value;

            DataList.ForEach(x => ((PhaseElementData)x).ModelIconPath = value);
        }
    }

    public string InteractableName
    {
        get { return phaseData.InteractableName; }
        set
        {
            phaseData.InteractableName = value;

            DataList.ForEach(x => ((PhaseElementData)x).InteractableName = value);
        }
    }

    public string LocationName
    {
        get { return phaseData.LocationName; }
        set
        {
            phaseData.LocationName = value;

            DataList.ForEach(x => ((PhaseElementData)x).LocationName = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        phaseData = (PhaseData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyPhaseChanges(dataRequest);
    }

    private void ApplyPhaseChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddPhase(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdatePhase(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemovePhase(dataRequest);
                break;
        }
    }

    private void AddPhase(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            phaseData.Id = tempData.Id;
    }

    private void UpdatePhase(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemovePhase(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ElementDataList.Where(x => x != EditData).ToList().ForEach(x => x.SetOriginalValues());
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != 0).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
