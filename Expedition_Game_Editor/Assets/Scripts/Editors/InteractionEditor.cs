using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionEditor : MonoBehaviour, IEditor
{
    public InteractionDataElement InteractionData { get { return (InteractionDataElement)Data.dataElement; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IDataElement> DataList
    {
        get { return SelectionElementManager.FindDataElements(InteractionData).Concat(new[] { InteractionData }).Distinct().ToList(); }
    }

    public List<IDataElement> DataElements
    {
        get
        {
            var list = new List<IDataElement>();

            DataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return DataElements.Any(x => x.Changed) && !InteractionData.timeConflict;
    }

    public void ApplyChanges()
    {
        var changedTime = InteractionData.changedStartTime || InteractionData.changedEndTime;

        InteractionData.Update();

        //If time was changed, reset the entire editor so that the interaction segment may reload.
        //Elements don't need to be updated as the reset takes care of that.
        if (changedTime)
        {
            RenderManager.ResetPath(true);

        } else {

            DataElements.ForEach(x =>
            {
                if (((GeneralData)x).Equals(InteractionData))
                    x.Copy(InteractionData);
                else
                    x.Update();

                if (x.SelectionElement != null)
                    x.SelectionElement.UpdateElement();
            });

            UpdateEditor();
        } 
    }

    public void CancelEdit()
    {
        DataElements.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
