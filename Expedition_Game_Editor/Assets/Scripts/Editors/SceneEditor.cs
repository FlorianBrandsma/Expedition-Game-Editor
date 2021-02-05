using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneEditor : MonoBehaviour, IEditor
{
    private SceneData sceneData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == sceneData.Id).FirstOrDefault(); } }

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

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return sceneData.Id; }
    }

    public int OutcomeId
    {
        get { return sceneData.OutcomeId; }
    }

    public int RegionId
    {
        get { return sceneData.RegionId; }
        set
        {
            sceneData.RegionId = value;
            
            DataList.ForEach(x => ((SceneElementData)x).RegionId = value);
        }
    }

    public int Index
    {
        get { return sceneData.Index; }
    }

    public string Name
    {
        get { return sceneData.Name; }
        set
        {
            sceneData.Name = value;

            DataList.ForEach(x => ((SceneElementData)x).Name = value);
        }
    }

    public bool FreezeTime
    {
        get { return sceneData.FreezeTime; }
        set
        {
            sceneData.FreezeTime = value;

            DataList.ForEach(x => ((SceneElementData)x).FreezeTime = value);
        }
    }

    public bool AutoContinue
    {
        get { return sceneData.AutoContinue; }
        set
        {
            sceneData.AutoContinue = value;

            DataList.ForEach(x => ((SceneElementData)x).AutoContinue = value);
        }
    }

    public bool SetActorsInstantly
    {
        get { return sceneData.SetActorsInstantly; }
        set
        {
            sceneData.SetActorsInstantly = value;

            DataList.ForEach(x => ((SceneElementData)x).SetActorsInstantly = value);
        }
    }

    public float SceneDuration
    {
        get { return sceneData.SceneDuration; }
        set
        {
            sceneData.SceneDuration = value;

            DataList.ForEach(x => ((SceneElementData)x).SceneDuration = value);
        }
    }

    public float ShotDuration
    {
        get { return sceneData.ShotDuration; }
        set
        {
            sceneData.ShotDuration = value;

            DataList.ForEach(x => ((SceneElementData)x).ShotDuration = value);
        }
    }

    public string EditorNotes
    {
        get { return sceneData.EditorNotes; }
        set
        {
            sceneData.EditorNotes = value;

            DataList.ForEach(x => ((SceneElementData)x).EditorNotes = value);
        }
    }

    public string GameNotes
    {
        get { return sceneData.GameNotes; }
        set
        {
            sceneData.GameNotes = value;

            DataList.ForEach(x => ((SceneElementData)x).GameNotes = value);
        }
    }

    public string RegionName
    {
        get { return sceneData.RegionName; }
        set
        {
            sceneData.RegionName = value;

            DataList.ForEach(x => ((SceneElementData)x).RegionName = value);
        }
    }

    public string TileIconPath
    {
        get { return sceneData.TileIconPath; }
        set
        {
            sceneData.TileIconPath = value;

            DataList.ForEach(x => ((SceneElementData)x).TileIconPath = value);
        }
    }  
    #endregion

    public void InitializeEditor()
    {
        sceneData = (SceneData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
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
        ApplySceneChanges(dataRequest);
    }

    private void ApplySceneChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddScene(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateScene(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveScene(dataRequest);
                break;
        }
    }

    private void AddScene(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            sceneData.Id = tempData.Id;
    }

    private void UpdateScene(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveScene(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }

    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
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

        var autoSelectId = 0;

        var defaultElement = Data.dataController.Data.dataList.Where(x => x.Id > 0 && x.ExecuteType != Enums.ExecuteType.Remove).FirstOrDefault();

        if (defaultElement != null)
            autoSelectId = defaultElement.Id;

        ((ListManager)EditData.DataElement.DisplayManager).AutoSelectElement(autoSelectId);
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
