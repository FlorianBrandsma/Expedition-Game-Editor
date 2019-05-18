using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainElementEditor : MonoBehaviour//, IEditor
{
    private TerrainElementDataElement terrainElementData;
    private SelectionElement selectionElement;

    private PathController PathController { get { return GetComponent<PathController>(); } }
    
    public bool subEditor;

    public bool SubEditor { get { return subEditor; } }
    public Data Data { get; set; }

    public void InitializeEditor()
    {
        selectionElement = PathController.Origin;

        Data = PathController.route.data;

        terrainElementData = Data.ElementData.Cast<TerrainElementDataElement>().FirstOrDefault();

        if (!PathController.loaded)
            terrainElementData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<TerrainElementDataElement>().ToList();

        list.RemoveAt(terrainElementData.Index);
        list.Insert(index, terrainElementData);

        selectionElement.ListManager.listProperties.DataController.DataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        selectionElement.ListManager.UpdateData();
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
        return terrainElementData.changed;
    }

    public void ApplyChanges()
    {
        terrainElementData.Update();

        UpdateList();

        UpdateEditor();
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}
