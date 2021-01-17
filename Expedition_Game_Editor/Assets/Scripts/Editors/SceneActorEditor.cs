using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneActorEditor : MonoBehaviour, IEditor
{
    private SceneActorData sceneActorData;

    public CameraManager cameraManager;

    public EditorWorldOrganizer EditorWorldOrganizer { get { return (EditorWorldOrganizer)cameraManager.Organizer; } }

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == sceneActorData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded                              { get; set; }
    public bool Removable                           { get { return true; } }

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
        get { return sceneActorData.Id; }
    }
    
    public int SceneId
    {
        get { return sceneActorData.SceneId; }
    }

    public int WorldInteractableId
    {
        get { return sceneActorData.WorldInteractableId; }
        set
        {
            sceneActorData.WorldInteractableId = value;

            DataList.ForEach(x => ((SceneActorElementData)x).WorldInteractableId = value);
        }
    }

    public int TerrainId
    {
        get { return sceneActorData.TerrainId; }
        set
        {
            sceneActorData.TerrainId = value;

            DataList.ForEach(x => ((SceneActorElementData)x).TerrainId = value);
        }
    }

    public int TerrainTileId
    {
        get { return sceneActorData.TerrainTileId; }
        set
        {
            sceneActorData.TerrainTileId = value;

            DataList.ForEach(x => ((SceneActorElementData)x).TerrainTileId = value);
        }
    }

    public int SpeechMethod
    {
        get { return sceneActorData.SpeechMethod; }
        set
        {
            sceneActorData.SpeechMethod = value;

            DataList.ForEach(x => ((SceneActorElementData)x).SpeechMethod = value);
        }
    }

    public string SpeechText
    {
        get { return sceneActorData.SpeechText; }
        set
        {
            sceneActorData.SpeechText = value;

            DataList.ForEach(x => ((SceneActorElementData)x).SpeechText = value);
        }
    }

    public bool ShowTextBox
    {
        get { return sceneActorData.ShowTextBox; }
        set
        {
            sceneActorData.ShowTextBox = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ShowTextBox = value);
        }
    }

    public int TargetSceneActorId
    {
        get { return sceneActorData.TargetSceneActorId; }
        set
        {
            sceneActorData.TargetSceneActorId = value;

            DataList.ForEach(x => ((SceneActorElementData)x).TargetSceneActorId = value);
        }
    }

    public bool ChangePosition
    {
        get { return sceneActorData.ChangePosition; }
        set
        {
            sceneActorData.ChangePosition = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ChangePosition = value);
        }
    }

    public bool FreezePosition
    {
        get { return sceneActorData.FreezePosition; }
        set
        {
            sceneActorData.FreezePosition = value;

            DataList.ForEach(x => ((SceneActorElementData)x).FreezePosition = value);
        }
    }

    public float PositionX
    {
        get { return sceneActorData.PositionX; }
        set
        {
            sceneActorData.PositionX = value;

            DataList.ForEach(x => ((SceneActorElementData)x).PositionX = value);
        }
    }

    public float PositionY
    {
        get { return sceneActorData.PositionY; }
        set
        {
            sceneActorData.PositionY = value;

            DataList.ForEach(x => ((SceneActorElementData)x).PositionY = value);
        }
    }

    public float PositionZ
    {
        get { return sceneActorData.PositionZ; }
        set
        {
            sceneActorData.PositionZ = value;

            DataList.ForEach(x => ((SceneActorElementData)x).PositionZ = value);
        }
    }

    public bool ChangeRotation
    {
        get { return sceneActorData.ChangeRotation; }
        set
        {
            sceneActorData.ChangeRotation = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ChangeRotation = value);
        }
    }

    public bool FaceTarget
    {
        get { return sceneActorData.FaceTarget; }
        set
        {
            sceneActorData.FaceTarget = value;

            DataList.ForEach(x => ((SceneActorElementData)x).FaceTarget = value);
        }
    }

    public int RotationX
    {
        get { return sceneActorData.RotationX; }
        set
        {
            sceneActorData.RotationX = value;

            DataList.ForEach(x => ((SceneActorElementData)x).RotationX = value);
        }
    }

    public int RotationY
    {
        get { return sceneActorData.RotationY; }
        set
        {
            sceneActorData.RotationY = value;

            DataList.ForEach(x => ((SceneActorElementData)x).RotationY = value);
        }
    }

    public int RotationZ
    {
        get { return sceneActorData.RotationZ; }
        set
        {
            sceneActorData.RotationZ = value;

            DataList.ForEach(x => ((SceneActorElementData)x).RotationZ = value);
        }
    }
    
    public int ModelId
    {
        get { return ((SceneActorData)EditData).ModelId; }
        set
        {
            sceneActorData.ModelId = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ModelId = value);
        }
    }

    public string ModelPath
    {
        get { return ((SceneActorData)EditData).ModelPath; }
        set
        {
            sceneActorData.ModelPath = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ModelPath = value);
        }
    }

    public string ModelIconPath
    {
        get { return ((SceneActorData)EditData).ModelIconPath; }
        set
        {
            sceneActorData.ModelIconPath = value;

            DataList.ForEach(x => ((SceneActorElementData)x).ModelIconPath = value);
        }
    }

    public string InteractableName
    {
        get { return ((SceneActorData)EditData).InteractableName; }
        set
        {
            sceneActorData.InteractableName = value;

            DataList.ForEach(x => ((SceneActorElementData)x).InteractableName = value);
        }
    }

    public float Height
    {
        get { return ((SceneActorData)EditData).Height; }
        set
        {
            sceneActorData.Height = value;

            DataList.ForEach(x => ((SceneActorElementData)x).Height = value);
        }
    }

    public float Width
    {
        get { return ((SceneActorData)EditData).Width; }
        set
        {
            sceneActorData.Width = value;

            DataList.ForEach(x => ((SceneActorElementData)x).Width = value);
        }
    }

    public float Depth
    {
        get { return ((SceneActorData)EditData).Depth; }
        set
        {
            sceneActorData.Depth = value;

            DataList.ForEach(x => ((SceneActorElementData)x).Depth = value);
        }
    }

    public float Scale
    {
        get { return ((SceneActorData)EditData).Scale; }
        set
        {
            sceneActorData.Scale = value;

            DataList.ForEach(x => ((SceneActorElementData)x).Scale = value);
        }
    }

    public int SpeechTextLimit
    {
        get { return sceneActorData.SpeechTextLimit; }
        set
        {
            sceneActorData.SpeechTextLimit = value;

            DataList.ForEach(x => ((SceneActorElementData)x).SpeechTextLimit = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        sceneActorData = (SceneActorData)ElementData.Clone();
    }
    
    public void ResetEditor()
    {
        TerrainId           = sceneActorData.TerrainId;
        TerrainTileId       = sceneActorData.TerrainTileId;
        
        SpeechMethod        = sceneActorData.SpeechMethod;
        SpeechText          = sceneActorData.SpeechText;
        ShowTextBox         = sceneActorData.ShowTextBox;

        TargetSceneActorId  = sceneActorData.TargetSceneActorId;

        ChangePosition      = sceneActorData.ChangePosition;
        FreezePosition      = sceneActorData.FreezePosition;

        PositionX           = sceneActorData.PositionX;
        PositionY           = sceneActorData.PositionY;
        PositionZ           = sceneActorData.PositionZ;

        ChangeRotation      = sceneActorData.ChangeRotation;
        FaceTarget          = sceneActorData.FaceTarget;

        RotationX           = sceneActorData.RotationX;
        RotationY           = sceneActorData.RotationY;
        RotationZ           = sceneActorData.RotationZ;

        ModelId             = sceneActorData.ModelId;

        ModelPath           = sceneActorData.ModelPath;
        ModelIconPath       = sceneActorData.ModelIconPath;

        InteractableName    = sceneActorData.InteractableName;

        Height              = sceneActorData.Height;
        Width               = sceneActorData.Width;
        Depth               = sceneActorData.Depth;

        Scale               = sceneActorData.Scale;

        SpeechTextLimit     = sceneActorData.SpeechTextLimit;
    }

    public void UpdateEditor()
    {
        ElementDataList.Where(x => SelectionElementManager.SelectionActive(x.DataElement)).ToList().ForEach(x => x.DataElement.UpdateElement());
        
        var targetSceneActorData = Data.dataController.Data.dataList.Cast<SceneActorElementData>().Where(x => x.TargetSceneActorId == Id).FirstOrDefault();

        var targetSceneActorElementData = SelectionElementManager.FindElementData(targetSceneActorData);
        targetSceneActorElementData.ForEach(x => x.DataElement.UpdateElement());

        cameraManager.UpdateData();
        cameraManager.UpdateOverlay();

        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed) && (SpeechText.Length <= SpeechTextLimit);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplySceneActorChanges(dataRequest);
    }

    private void ApplySceneActorChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddSceneActor(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateSceneActor(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveSceneActor(dataRequest);
                break;
        }
    }

    private void AddSceneActor(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            sceneActorData.Id = tempData.Id;
    }

    private void UpdateSceneActor(DataRequest dataRequest)
    {
        EditData.Update(dataRequest);
    }

    private void RemoveSceneActor(DataRequest dataRequest)
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
