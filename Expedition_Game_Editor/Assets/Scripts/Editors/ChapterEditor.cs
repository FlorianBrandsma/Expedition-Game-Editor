using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterEditor : MonoBehaviour, IEditor
{
    private ChapterDataElement chapterData;
    public ElementDataElement elementDataElement;
    public List<TerrainElementDataElement> terrainElementDataList;
    public List<ChapterRegionDataElement> chapterRegionDataList;

    private DataManager dataManager = new DataManager();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(chapterData);
            list.Add(elementDataElement);
            terrainElementDataList.ForEach(x => list.Add(x));
            chapterRegionDataList.ForEach(x => list.Add(x));

            return list;
        }
    }
    
    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        chapterData = (ChapterDataElement)Data.DataElement;
        elementDataElement = new ElementDataElement();
        terrainElementDataList.Clear();
        chapterRegionDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<ChapterDataElement>().ToList();

        list.RemoveAt(chapterData.Index);
        list.Insert(index, chapterData);

        Data.DataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        if (PathController.Origin == null) return;

        PathController.Origin.ListManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed);
    }

    public void ApplyChanges()
    {
        DataElements.ForEach(x => x.Update());

        UpdateList();

        UpdateEditor();

        UpdatePhaseElements();
    }

    private void UpdatePhaseElements()
    {
        var phaseList = dataManager.GetPhaseData(chapterData.id, true);

        var phaseElementList = dataManager.GetPhaseElementData(phaseList.Select(x => x.id).Distinct().ToList());

        //Replace phase elements to match the chapter's terrain elements
        //UPDATE: TerrainElement doesn't get replaced (idiot), the element does and that gets carried over anyway
        //foreach(TerrainElementDataElement terrainElement in terrainElementDataList)
        //{
        //    var phaseElements = phaseElementList.Where(x => x.terrainElementId == terrainElement.id).ToList();
            
        //    foreach(DataManager.PhaseElementData phaseElement in phaseElements)
        //    {
        //        Fixtures.phaseElementList.Where(x => x.id == phaseElement.id).ToList().ForEach(x => 
        //        {
        //            Debug.Log(x.terrainElementId + ":" + terrainElement.id);

        //            x.terrainElementId = terrainElement.id;
        //        });
        //    }
        //}
    }

    public void CancelEdit()
    {
        
    }

    public void CloseEditor()
    {
        
    }    
}
