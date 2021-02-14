using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDestinationEditor : MonoBehaviour, IEditor
{
    private InteractionDestinationData interactionDestinationData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == interactionDestinationData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }

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
        get { return interactionDestinationData.Id; }
    }

    public int TerrainId
    {
        get { return interactionDestinationData.TerrainId; }
        set
        {
            interactionDestinationData.TerrainId = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).TerrainId = value);
        }
    }

    public int TerrainTileId
    {
        get { return interactionDestinationData.TerrainTileId; }
        set
        {
            interactionDestinationData.TerrainTileId = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).TerrainTileId = value);
        }
    }

    public float PositionX
    {
        get { return interactionDestinationData.PositionX; }
        set
        {
            interactionDestinationData.PositionX = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).PositionX = value);
        }
    }

    public float PositionY
    {
        get { return interactionDestinationData.PositionY; }
        set
        {
            interactionDestinationData.PositionY = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).PositionY = value);
        }
    }

    public float PositionZ
    {
        get { return interactionDestinationData.PositionZ; }
        set
        {
            interactionDestinationData.PositionZ = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).PositionZ = value);
        }
    }

    public float PositionVariance
    {
        get { return interactionDestinationData.PositionVariance; }
        set
        {
            interactionDestinationData.PositionVariance = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).PositionVariance = value);
        }
    }

    public bool ChangeRotation
    {
        get { return interactionDestinationData.ChangeRotation; }
        set
        {
            interactionDestinationData.ChangeRotation = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).ChangeRotation = value);
        }
    }

    public int RotationX
    {
        get { return interactionDestinationData.RotationX; }
        set
        {
            interactionDestinationData.RotationX = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).RotationX = value);
        }
    }

    public int RotationY
    {
        get { return interactionDestinationData.RotationY; }
        set
        {
            interactionDestinationData.RotationY = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).RotationY = value);
        }
    }

    public int RotationZ
    {
        get { return interactionDestinationData.RotationZ; }
        set
        {
            interactionDestinationData.RotationZ = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).RotationZ = value);
        }
    }

    public float Patience
    {
        get { return interactionDestinationData.Patience; }
        set
        {
            interactionDestinationData.Patience = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).Patience = value);
        }
    }

    public float Scale
    {
        get { return interactionDestinationData.Scale; }
        set
        {
            interactionDestinationData.Scale = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).Scale = value);
        }
    }

    public string ModelName
    {
        get { return interactionDestinationData.InteractableName; }
        set
        {
            interactionDestinationData.InteractableName = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).InteractableName = value);
        }
    }

    public string ModelIconPath
    {
        get { return interactionDestinationData.ModelIconPath; }
        set
        {
            interactionDestinationData.ModelIconPath = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).ModelIconPath = value);
        }
    }

    public string InteractableName
    {
        get { return interactionDestinationData.InteractableName; }
        set
        {
            interactionDestinationData.InteractableName = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).InteractableName = value);
        }
    }

    public float Height
    {
        get { return interactionDestinationData.Height; }
        set
        {
            interactionDestinationData.Height = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).Height = value);
        }
    }

    public float Width
    {
        get { return interactionDestinationData.Width; }
        set
        {
            interactionDestinationData.Width = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return interactionDestinationData.Depth; }
        set
        {
            interactionDestinationData.Depth = value;

            DataList.ForEach(x => ((InteractionDestinationData)x).Depth = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        interactionDestinationData = (InteractionDestinationData)ElementData.Clone();
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

    public bool Addable()
    {
        return true;
    }

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyInteractionDestinationChanges(dataRequest);
    }

    private void ApplyInteractionDestinationChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddInteractionDestination(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateInteractionDestination(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveInteractionDestination(dataRequest);
                break;
        }
    }

    private void AddInteractionDestination(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            interactionDestinationData.Id = tempData.Id;
    }

    private void UpdateInteractionDestination(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveInteractionDestination(DataRequest dataRequest)
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
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
