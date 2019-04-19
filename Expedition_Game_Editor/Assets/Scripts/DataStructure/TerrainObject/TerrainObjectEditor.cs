using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TerrainObjectEditor : MonoBehaviour, IEditor
{
    public Enums.DataType data_type { get { return Enums.DataType.TerrainObject; } }

    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }
    private TerrainObjectDataElement terrainObjectData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        SetList();

        data = pathController.route.data;

        terrainObjectData = data.Cast<TerrainObjectDataElement>().FirstOrDefault();

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
        var list = data_list.Cast<TerrainObjectDataElement>().ToList();

        list.RemoveAt(terrainObjectData.index);
        list.Insert(index, terrainObjectData);

        selectionElement.listManager.listProperties.segmentController.dataController.data_list = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void SetList()
    {
        data_list = selectionElement.listManager.listProperties.segmentController.dataController.data_list;
    }

    private void UpdateList()
    {
        SetList();
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
