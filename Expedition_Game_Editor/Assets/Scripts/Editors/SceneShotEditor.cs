using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneShotEditor : MonoBehaviour, IEditor
{
    private SceneShotData sceneShotData;

    public CameraManager cameraManager;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == sceneShotData.Id).FirstOrDefault(); } }

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
        get { return sceneShotData.Id; }
    }

    public int SceneId
    {
        get { return sceneShotData.SceneId; }
    }

    public Enums.SceneShotType Type
    {
        get { return (Enums.SceneShotType)sceneShotData.Type; }
    }

    public bool ChangePosition
    {
        get { return sceneShotData.ChangePosition; }
        set
        {
            sceneShotData.ChangePosition = value;

            DataList.ForEach(x => ((SceneShotElementData)x).ChangePosition = value);
        }
    }

    public float PositionX
    {
        get { return sceneShotData.PositionX; }
        set
        {
            sceneShotData.PositionX = value;

            DataList.ForEach(x => ((SceneShotElementData)x).PositionX = value);
        }
    }

    public float PositionY
    {
        get { return sceneShotData.PositionY; }
        set
        {
            sceneShotData.PositionY = value;

            DataList.ForEach(x => ((SceneShotElementData)x).PositionY = value);
        }
    }

    public float PositionZ
    {
        get { return sceneShotData.PositionZ; }
        set
        {
            sceneShotData.PositionZ = value;

            DataList.ForEach(x => ((SceneShotElementData)x).PositionZ = value);
        }
    }

    public int PositionTargetSceneActorId
    {
        get { return sceneShotData.PositionTargetSceneActorId; }
        set
        {
            sceneShotData.PositionTargetSceneActorId = value;

            DataList.ForEach(x => ((SceneShotElementData)x).PositionTargetSceneActorId = value);
        }
    }

    public bool ChangeRotation
    {
        get { return sceneShotData.ChangeRotation; }
        set
        {
            sceneShotData.ChangeRotation = value;

            DataList.ForEach(x => ((SceneShotElementData)x).ChangeRotation = value);
        }
    }

    public int RotationX
    {
        get { return sceneShotData.RotationX; }
        set
        {
            sceneShotData.RotationX = value;

            DataList.ForEach(x => ((SceneShotElementData)x).RotationX = value);
        }
    }

    public int RotationY
    {
        get { return sceneShotData.RotationY; }
        set
        {
            sceneShotData.RotationY = value;

            DataList.ForEach(x => ((SceneShotElementData)x).RotationY = value);
        }
    }

    public int RotationZ
    {
        get { return sceneShotData.RotationZ; }
        set
        {
            sceneShotData.RotationZ = value;

            DataList.ForEach(x => ((SceneShotElementData)x).RotationZ = value);
        }
    }

    public int RotationTargetSceneActorId
    {
        get { return sceneShotData.RotationTargetSceneActorId; }
        set
        {
            sceneShotData.RotationTargetSceneActorId = value;

            DataList.ForEach(x => ((SceneShotElementData)x).RotationTargetSceneActorId = value);
        }
    }

    public int CameraFilterId
    {
        get { return sceneShotData.CameraFilterId; }
        set
        {
            sceneShotData.CameraFilterId = value;

            DataList.ForEach(x => ((SceneShotElementData)x).CameraFilterId = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        sceneShotData = (SceneShotData)ElementData.Clone();
    }

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());

        cameraManager.UpdateData();
        cameraManager.UpdateOverlay();

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
        ApplySceneShotChanges(dataRequest);
    }

    private void ApplySceneShotChanges(DataRequest dataRequest)
    {
        if (EditData.ExecuteType == Enums.ExecuteType.Update)
            UpdateSceneShot(dataRequest);
    }

    private void UpdateSceneShot(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
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
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}