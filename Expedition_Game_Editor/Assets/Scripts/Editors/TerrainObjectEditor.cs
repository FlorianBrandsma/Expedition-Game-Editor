using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainObjectEditor : MonoBehaviour//, IEditor
{
    private TerrainObjectDataElement terrainObjectData;
    private SelectionElement selectionElement;

    private PathController PathController { get { return GetComponent<PathController>(); } }
    
    public bool subEditor;

    public bool SubEditor { get { return subEditor; } }
    public Data Data { get; set; }

    public void InitializeEditor()
    {
        selectionElement = PathController.Origin;

        Data = PathController.route.data;

        terrainObjectData = (TerrainObjectDataElement)Data.DataElement;

        if (!PathController.loaded)
            terrainObjectData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<TerrainObjectDataElement>().ToList();

        list.RemoveAt(terrainObjectData.Index);
        list.Insert(index, terrainObjectData);

        selectionElement.ListManager.listProperties.DataController.DataList = list.Cast<IDataElement>().ToList();

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
        return terrainObjectData.Changed;
    }

    public void ApplyChanges()
    {
        terrainObjectData.Update();

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
