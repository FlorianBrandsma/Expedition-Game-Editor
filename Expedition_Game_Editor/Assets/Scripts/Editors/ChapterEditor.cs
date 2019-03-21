using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class ChapterEditor : MonoBehaviour, IEditor
{
    public DataManager.Type data_type { get { return DataManager.Type.Chapter; } }
    public IEnumerable data { get; set; }
    public ICollection data_list { get; set; }

    private ChapterDataElement chapterData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public ButtonActionManager buttonActionManager;

    public void InitializeEditor()
    {
        selectionElement = pathController.route.origin.selectionElement;
        //Debug.Log(selectionElement.data_type);
        SetList();

        data = pathController.route.data;

        chapterData = data.Cast<ChapterDataElement>().FirstOrDefault();
        
        if (!pathController.loaded)
            chapterData.ClearChanges();

        if (buttonActionManager != null)
            buttonActionManager.InitializeButtons(this);
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
        Debug.Log(data_list);
        var list = data_list.Cast<ChapterDataElement>().ToList();

        list.RemoveAt(chapterData.index);
        list.Insert(index, chapterData);

        selectionElement.listManager.listProperties.dataController.data_list = list;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
            list[i].UpdateIndex();
        }

        UpdateList();
    }

    private void SetList()
    {
        data_list = selectionElement.listManager.listProperties.dataController.data_list;
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
        if (buttonActionManager != null)
            buttonActionManager.SetButtons(chapterData.changed);
    }

    public void ApplyChanges()
    {
        chapterData.Update();

        UpdateList();

        UpdateEditor();
    }

    public void CancelEdit()
    {
        EditorManager.editorManager.PreviousEditor();
    }

    public void CloseEditor()
    {
        if (buttonActionManager != null)
            buttonActionManager.CloseButtons();
    }  
}
