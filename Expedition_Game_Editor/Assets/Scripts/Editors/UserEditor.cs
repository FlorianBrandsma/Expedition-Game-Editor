using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserEditor : MonoBehaviour, IEditor
{
    private UserData userData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == userData.Id).FirstOrDefault(); } }

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
        get { return userData.Id; }
    }

    public string IconPath
    {
        get { return userData.IconPath; }
    }

    public string Username
    {
        get { return userData.Username; }
    }
    #endregion

    public void InitializeEditor()
    {
        userData = (UserData)ElementData.Clone();
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
        ApplyUserChanges(dataRequest);
    }

    private void ApplyUserChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddUser(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateUser(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveUser(dataRequest);
                break;
        }
    }

    private void AddUser(DataRequest dataRequest)
    {
        var tempData = EditData;

        var userElementData = (UserElementData)EditData;

        userElementData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            userData.Id = tempData.Id;
    }

    private void UpdateUser(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveUser(DataRequest dataRequest)
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
