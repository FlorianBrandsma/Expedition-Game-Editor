using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractableEditor : MonoBehaviour, IEditor
{
    private InteractableData interactableData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == interactableData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }

    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
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
        get { return interactableData.Id; }
    }

    public int ModelId
    {
        get { return interactableData.ModelId; }
        set
        {
            interactableData.ModelId = value;

            DataList.ForEach(x => ((InteractableElementData)x).ModelId = value);
        }
    }

    public int Index
    {
        get { return interactableData.Index; }
    }

    public string Name
    {
        get { return interactableData.Name; }
        set
        {
            interactableData.Name = value;

            DataList.ForEach(x => ((InteractableElementData)x).Name = value);
        }
    }

    public float Scale
    {
        get { return interactableData.Scale; }
        set
        {
            interactableData.Scale = value;

            DataList.ForEach(x => ((InteractableElementData)x).Scale = value);
        }
    }

    public int Health
    {
        get { return interactableData.Health; }
        set
        {
            interactableData.Health = value;

            DataList.ForEach(x => ((InteractableElementData)x).Health = value);
        }
    }
    public int Hunger
    {
        get { return interactableData.Hunger; }
        set
        {
            interactableData.Hunger = value;

            DataList.ForEach(x => ((InteractableElementData)x).Hunger = value);
        }
    }
    public int Thirst
    {
        get { return interactableData.Thirst; }
        set
        {
            interactableData.Thirst = value;

            DataList.ForEach(x => ((InteractableElementData)x).Thirst = value);
        }
    }

    public float Weight
    {
        get { return interactableData.Weight; }
        set
        {
            interactableData.Weight = value;

            DataList.ForEach(x => ((InteractableElementData)x).Weight = value);
        }
    }
    public float Speed
    {
        get { return interactableData.Speed; }
        set
        {
            interactableData.Speed = value;

            DataList.ForEach(x => ((InteractableElementData)x).Speed = value);
        }
    }
    public float Stamina
    {
        get { return interactableData.Stamina; }
        set
        {
            interactableData.Stamina = value;

            DataList.ForEach(x => ((InteractableElementData)x).Stamina = value);
        }
    }

    public string ModelPath
    {
        get { return interactableData.ModelPath; }
        set
        {
            interactableData.ModelPath = value;

            DataList.ForEach(x => ((InteractableElementData)x).ModelPath = value);
        }
    }

    public string ModelIconPath
    {
        get { return interactableData.ModelIconPath; }
        set
        {
            interactableData.ModelIconPath = value;

            DataList.ForEach(x => ((InteractableElementData)x).ModelIconPath = value);
        }
    }

    public float Height
    {
        get { return interactableData.Height; }
        set
        {
            interactableData.Height = value;

            DataList.ForEach(x => ((InteractableElementData)x).Height = value);
        }
    }

    public float Width
    {
        get { return interactableData.Width; }
        set
        {
            interactableData.Width = value;

            DataList.ForEach(x => ((InteractableElementData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return interactableData.Depth; }
        set
        {
            interactableData.Depth = value;

            DataList.ForEach(x => ((InteractableElementData)x).Depth = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        interactableData = (InteractableData)ElementData.Clone();
    }

    public void ResetEditor()
    {
        ModelId = interactableData.ModelId;

        Name = interactableData.Name;

        ModelPath = interactableData.ModelPath;
        ModelIconPath = interactableData.ModelIconPath;

        Scale = interactableData.Scale;

        Health = interactableData.Health;
        Hunger = interactableData.Hunger;
        Thirst = interactableData.Thirst;

        Weight = interactableData.Weight;
        Speed = interactableData.Speed;
        Stamina = interactableData.Stamina;

        Height = interactableData.Height;
        Width = interactableData.Width;
        Depth = interactableData.Depth;
    }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyInteractableChanges(dataRequest);
    }

    private void ApplyInteractableChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddInteractable(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateInteractable(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveInteractable(dataRequest);
                break;
        }
    }

    private void AddInteractable(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            interactableData.Id = tempData.Id;
    }

    private void UpdateInteractable(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveInteractable(DataRequest dataRequest)
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
