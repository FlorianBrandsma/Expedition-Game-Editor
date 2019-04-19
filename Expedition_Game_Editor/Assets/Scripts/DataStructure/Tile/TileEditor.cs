using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class TileEditor : MonoBehaviour, IEditor
{
    public Enums.DataType data_type { get { return Enums.DataType.Tile; } }
    
    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }
    private TileDataElement tileData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        SetList();

        data = pathController.route.data;

        tileData = data.Cast<TileDataElement>().FirstOrDefault();
        
        if (!pathController.loaded)
            tileData.ClearChanges();
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
        var list = data_list.Cast<TileDataElement>().ToList();

        list.RemoveAt(tileData.index);
        list.Insert(index, tileData);

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
        return tileData.changed;
    }

    public void ApplyChanges()
    {
        tileData.Update();

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
