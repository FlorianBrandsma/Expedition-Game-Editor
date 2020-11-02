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

    public string PublicNotes
    {
        get { return sceneData.PublicNotes; }
        set
        {
            sceneData.PublicNotes = value;

            DataList.ForEach(x => ((SceneElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return sceneData.PrivateNotes; }
        set
        {
            sceneData.PrivateNotes = value;

            DataList.ForEach(x => ((SceneElementData)x).PrivateNotes = value);
        }
    }

    public int PhaseId
    {
        get { return sceneData.PhaseId; }
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

    public int RegionSize
    {
        get { return sceneData.RegionSize; }
        set
        {
            sceneData.RegionSize = value;

            DataList.ForEach(x => ((SceneElementData)x).RegionSize = value);
        }
    }

    public int TerrainSize
    {
        get { return sceneData.TerrainSize; }
        set
        {
            sceneData.TerrainSize = value;

            DataList.ForEach(x => ((SceneElementData)x).TerrainSize = value);
        }
    }

    public float TileSize
    {
        get { return sceneData.TileSize; }
        set
        {
            sceneData.TileSize = value;

            DataList.ForEach(x => ((SceneElementData)x).TileSize = value);
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

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        EditData.Update();

        if (SelectionElementManager.SelectionActive(EditData.DataElement))
            EditData.DataElement.UpdateElement();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
