using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FavoriteUserEditor : MonoBehaviour, IEditor
{
    private FavoriteUserData favoriteUserData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == favoriteUserData.Id).FirstOrDefault(); } }

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
        get { return favoriteUserData.Id; }
    }

    public string IconPath
    {
        get { return favoriteUserData.IconPath; }
    }

    public string Username
    {
        get { return favoriteUserData.Username; }
    }

    public string Note
    {
        get { return favoriteUserData.Note; }
        set
        {
            favoriteUserData.Note = value;

            DataList.ForEach(x => ((FavoriteUserElementData)x).Note = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        favoriteUserData = (FavoriteUserData)ElementData.Clone();
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
        ApplyFavoriteUserChanges(dataRequest);
    }

    private void ApplyFavoriteUserChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddFavoriteUser(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateFavoriteUser(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveFavoriteUser(dataRequest);
                break;
        }
    }

    private void AddFavoriteUser(DataRequest dataRequest)
    {
        var tempData = EditData;

        var favoriteUserElementData = (FavoriteUserElementData)EditData;

        favoriteUserElementData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            favoriteUserData.Id = tempData.Id;
    }

    private void UpdateFavoriteUser(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveFavoriteUser(DataRequest dataRequest)
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
