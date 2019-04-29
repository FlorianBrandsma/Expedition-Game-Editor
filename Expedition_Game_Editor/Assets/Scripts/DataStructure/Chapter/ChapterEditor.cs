using UnityEngine;
using System.Collections;
using System.Linq;

//This is where the selected value is stored before it's split in segments
public class ChapterEditor : MonoBehaviour, IEditor
{
    public Data data { get; set; }
    private ChapterDataElement chapterData;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public SelectionElement selectionElement { get; set; }

    public void InitializeEditor()
    {
        selectionElement = pathController.route.path.origin;

        data = pathController.route.data;

        chapterData = data.element.Cast<ChapterDataElement>().FirstOrDefault();
        
        if (!pathController.loaded)
            chapterData.ClearChanges();
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
        var list = data.controller.dataList.Cast<ChapterDataElement>().ToList();

        list.RemoveAt(chapterData.index);
        list.Insert(index, chapterData);

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
        return chapterData.changed;
    }

    public void ApplyChanges()
    {
        chapterData.Update();

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
