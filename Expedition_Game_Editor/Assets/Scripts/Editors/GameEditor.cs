using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameEditor : MonoBehaviour, IEditor
{
    private GameData gameData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == gameData.Id).FirstOrDefault(); } }

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
        get { return gameData.Id; }
    }

    public string IconPath
    {
        get { return gameData.IconPath; }
    }

    public string Name
    {
        get { return gameData.Name; }
    }
    
    public int Rating
    {
        get { return gameData.Rating; }
        set
        {
            gameData.Rating = value;

            DataList.ForEach(x => ((GameElementData)x).Rating = value);
        }
    }

    public string Description
    {
        get { return gameData.Description; }
    }
    #endregion

    public void InitializeEditor()
    {
        gameData = (GameData)ElementData.Clone();
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
        ApplyGameChanges(dataRequest);
    }

    private void ApplyGameChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddGame(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateGame(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveGame(dataRequest);
                break;
        }
    }

    private void AddGame(DataRequest dataRequest)
    {
        var tempData = EditData;

        var gameElementData = (GameElementData)EditData;

        //gameElementData.SaveTime = System.DateTime.Now;

        gameElementData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            gameData.Id = tempData.Id;
    }

    private void UpdateGame(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveGame(DataRequest dataRequest)
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
