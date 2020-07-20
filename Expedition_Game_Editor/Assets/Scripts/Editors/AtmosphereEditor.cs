using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AtmosphereEditor : MonoBehaviour, IEditor
{
    public AtmosphereElementData AtmosphereData { get { return (AtmosphereElementData)Data.elementData; } }

    private List<SegmentController> editorSegments = new List<SegmentController>();

    private PathController PathController { get { return GetComponent<PathController>(); } }

    public bool Loaded { get; set; }

    public Route.Data Data { get { return PathController.route.data; } }

    public List<IElementData> DataList
    {
        get { return SelectionElementManager.FindElementData(AtmosphereData).Concat(new[] { AtmosphereData }).Distinct().ToList(); }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => list.Add(x));

            return list;
        }
    }

    public List<SegmentController> EditorSegments
    {
        get { return editorSegments; }
    }

    public void InitializeEditor() { }

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
        return ElementDataList.Any(x => x.Changed) && !AtmosphereData.timeConflict;
    }

    public void ApplyChanges()
    {
        var changedTime = AtmosphereData.changedStartTime || AtmosphereData.changedEndTime;

        AtmosphereData.Update();

        //If time was changed, reset the entire editor so that the atmosphere segment may reload.
        //Elements don't need to be updated as the reset takes care of that.
        if (changedTime)
        {
            RenderManager.ResetPath(true);

        } else {

            ElementDataList.ForEach(x =>
            {
                if (((GeneralData)x).Equals(AtmosphereData))
                    x.Copy(AtmosphereData);
                else
                    x.Update();

                if (SelectionElementManager.SelectionActive(x.DataElement))
                    x.DataElement.UpdateElement();
            });

            UpdateEditor();
        }
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
