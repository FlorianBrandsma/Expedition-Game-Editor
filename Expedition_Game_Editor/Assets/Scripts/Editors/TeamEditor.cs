using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamEditor : MonoBehaviour, IEditor
{
    private TeamData teamData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == teamData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public LayoutSection LayoutSection              { get { return PathController.layoutSection; } }
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

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return teamData.Id; }
    }

    public string IconPath
    {
        get { return teamData.IconPath; }
    }

    public string Name
    {
        get { return teamData.Name; }
    }

    public string Description
    {
        get { return teamData.Description; }
    }
    #endregion

    public void InitializeEditor()
    {
        teamData = (TeamData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        LayoutSection.SetActionButtons();
    }

    public bool Addable()
    {
        return false;
    }

    public bool Applicable()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public bool Removable()
    {
        return false;
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyTeamChanges(dataRequest);
    }

    private void ApplyTeamChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddTeam(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateTeam(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveTeam(dataRequest);
                break;
        }
    }

    private void AddTeam(DataRequest dataRequest)
    {
        var tempData = EditData;

        var teamElementData = (TeamElementData)EditData;

        teamElementData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            teamData.Id = tempData.Id;
    }

    private void UpdateTeam(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveTeam(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Remove:
                OpenDefault();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void OpenDefault()
    {
        RenderManager.loadType = Enums.LoadType.Reload;

        var defaultElement = Data.dataController.Data.dataList.Where(x => x.Id > 0 && x.ExecuteType != Enums.ExecuteType.Remove).FirstOrDefault();

        if (defaultElement != null)
        {
            ((ListManager)EditData.DataElement.DisplayManager).AutoSelectElement(defaultElement.Id);

        } else {

            RenderManager.PreviousPath();
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