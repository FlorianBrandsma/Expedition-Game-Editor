using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveEditor : MonoBehaviour, IEditor
{
    private ObjectiveDataElement objectiveData;
    public List<SceneInteractableDataElement> sceneInteractableDataList;

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get { return PathController.loaded; } }
    public Route.Data Data { get; set; }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            list.Add(objectiveData);
            sceneInteractableDataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public void InitializeEditor()
    {
        if (Loaded) return;

        Data = PathController.route.data;

        objectiveData = (ObjectiveDataElement)Data.dataElement;
        sceneInteractableDataList.Clear();

        DataElements.ForEach(x => x.ClearChanges());
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void UpdateIndex(int index)
    {
        var list = Data.dataController.DataList.Cast<ObjectiveDataElement>().ToList();

        list.RemoveAt(objectiveData.Index);
        list.Insert(index, objectiveData);

        Data.dataController.DataList = list.Cast<IDataElement>().ToList();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Index = i;
            list[i].UpdateIndex();
        }

        SelectionElementManager.UpdateElements(objectiveData, true);
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

        SelectionElementManager.UpdateElements(objectiveData);

        UpdateEditor();
    }

    public void CancelEdit()
    {

    }

    public void CloseEditor()
    {

    }
}
