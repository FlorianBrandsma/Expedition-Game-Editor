using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ExIndexSwitch : MonoBehaviour
{
    private ISegment segment;

    private List<IElementData> dataList;

    public Button minus_button;
    public Button plus_button;

    public Text index_number;

    private int index;
    private int indexLimit;

    public void InitializeSwitch(ISegment segment, int index)
    {
        this.segment = segment;
        
        this.index = index;

        indexLimit = segment.DataEditor.Data.dataList.Count - 1;

        SetSwitch();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    private void SetSwitch()
    {
        SetIndex();
    }

    public void ChangeIndex(int value)
    {
        index += value;

        if (index > indexLimit)
            index = 0;

        if (index < 0)
            index = indexLimit;

        UpdateIndex();
    }

    private void UpdateIndex()
    {
        dataList = segment.DataEditor.Data.dataList;
        
        var elementData = segment.DataEditor.ElementData;
        var elementIndex = dataList.IndexOf(elementData);

        dataList.RemoveAt(elementIndex);
        dataList.Insert(index, elementData);

        for (int i = 0; i < dataList.Count; i++)
        {
            switch (elementData.DataType)
            {
                case Enums.DataType.Item: UpdateItemIndex(i); break;

                default: Debug.Log("CASE MISSING " + elementData.DataType); break;
            }
        }

        if (dataList[elementIndex].DataElement != null)
            dataList[elementIndex].DataElement.CancelDataSelection();

        //if (elementData.SelectionStatus == Enums.SelectionStatus.None)
        //    elementData.SelectionStatus = segment.SegmentController.EditorController.PathController.route.selectionStatus;

        SelectionElementManager.UpdateElements(elementData);
        
        SetIndex();
    }

    private void UpdateItemIndex(int index)
    {
        var itemElementData = (ItemElementData)dataList[index];

        itemElementData.Index = index;

        itemElementData.UpdateIndex();
    }

    public void SetIndex()
    {
        index_number.text = (index + 1).ToString();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
