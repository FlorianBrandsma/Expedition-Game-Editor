using UnityEngine;
using System.Collections;
using System.Linq;

public class TerrainTileEditor : MonoBehaviour//, IEditor
{
    private TerrainTileDataElement terrainTileData;
    private SelectionElement selectionElement;

    private PathController PathController { get { return GetComponent<PathController>(); } }
    
    public bool subEditor;

    public bool SubEditor { get { return subEditor; } }
    public Data Data { get; set; }

    public void InitializeEditor()
    {
        selectionElement = PathController.Origin;

        Data = PathController.route.data;

        terrainTileData = (TerrainTileDataElement)Data.DataElement;

        if (!PathController.loaded)
            terrainTileData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.DataController.DataList.Cast<TerrainTileDataElement>().ToList();

        list.RemoveAt(terrainTileData.Index);
        list.Insert(index, terrainTileData);

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
        return terrainTileData.Changed;
    }

    public void ApplyChanges()
    {
        terrainTileData.Update();

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
