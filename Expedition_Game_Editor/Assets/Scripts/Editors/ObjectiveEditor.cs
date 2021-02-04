using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveEditor : MonoBehaviour, IEditor
{
    private ObjectiveData objectiveData;

    public List<WorldInteractableElementData> worldInteractableElementDataList = new List<WorldInteractableElementData>();

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == objectiveData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }

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

            worldInteractableElementDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return objectiveData.Id; }
    }

    public int Index
    {
        get { return objectiveData.Index; }
    }

    public string Name
    {
        get { return objectiveData.Name; }
        set
        {
            objectiveData.Name = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).Name = value);
        }
    }

    public string PublicNotes
    {
        get { return objectiveData.PublicNotes; }
        set
        {
            objectiveData.PublicNotes = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return objectiveData.PrivateNotes; }
        set
        {
            objectiveData.PrivateNotes = value;

            DataList.ForEach(x => ((ObjectiveElementData)x).PrivateNotes = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        objectiveData = (ObjectiveData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return true;
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return true;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Add || EditData.ExecuteType == Enums.ExecuteType.Update)
        {
            ApplyObjectiveChanges(dataRequest);

            ApplyWorldInteractableChanges(dataRequest);
        }

        if (EditData.ExecuteType == Enums.ExecuteType.Remove)
            RemoveObjective(dataRequest);
    }

    private void ApplyObjectiveChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddObjective(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateObjective(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveObjective(dataRequest);
                break;
        }
    }

    private void AddObjective(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            objectiveData.Id = tempData.Id;

            worldInteractableElementDataList.ForEach(x => x.ObjectiveId = objectiveData.Id);
        }
    }

    private void UpdateObjective(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveObjective(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    private void ApplyWorldInteractableChanges(DataRequest dataRequest)
    {
        foreach (WorldInteractableElementData worldInteractableElementData in worldInteractableElementDataList)
        {
            switch (worldInteractableElementData.ExecuteType)
            {
                case Enums.ExecuteType.Add:
                    worldInteractableElementData.Add(dataRequest);
                    break;

                case Enums.ExecuteType.Update:
                    worldInteractableElementData.Update(dataRequest);
                    break;

                case Enums.ExecuteType.Remove:
                    worldInteractableElementData.Remove(dataRequest);
                    break;
            }
        }

        if (dataRequest.requestType == Enums.RequestType.Execute)
            worldInteractableElementDataList.RemoveAll(x => x.ExecuteType == Enums.ExecuteType.Remove);
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
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        worldInteractableElementDataList.Clear();

        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
