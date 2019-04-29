using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TerrainObjectEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }

    private TerrainObjectDataElement terrainObjectData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        terrainObjectData = data.element.Cast<TerrainObjectDataElement>().FirstOrDefault();

        if (!pathController.loaded)
            terrainObjectData.ClearChanges();
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateElement()
    {
        selectionElement.UpdateElement();
    }

    public void UpdateIndex(int index)
    {
        var list = data.controller.dataList.Cast<TerrainObjectDataElement>().ToList();

        list.RemoveAt(terrainObjectData.index);
        list.Insert(index, terrainObjectData);

        selectionElement.listManager.listProperties.segmentController.dataController.dataList = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void UpdateList()
    {
        selectionElement.listManager.UpdateData();
    }

    public void OpenEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        pathController.editorSection.SetActionButtons();
    }

    public bool Changed()
    {
        return terrainObjectData.changed;
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
