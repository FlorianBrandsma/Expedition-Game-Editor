using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class IndexSwitch : MonoBehaviour
{
    private IDataElement dataElement;

    private ISegment segment;

    public Button minus_button;
    public Button plus_button;

    public Text index_number;

    private int index;
    private int indexLimit;

    private List<IDataElement> dataList;

    public void InitializeSwitch(ISegment segment, int index)
    {
        this.segment = segment;

        this.index = index;

        dataElement = segment.DataEditor.Data.dataElement;
        dataList = segment.DataEditor.Data.dataController.DataList;

        indexLimit = dataList.Count - 1;

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
        var dataEditor = segment.DataEditor;
        
        dataList.RemoveAt(dataElement.Index);
        dataList.Insert(index, dataElement);

        dataEditor.Data.dataController.DataList = dataList;

        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].Index = i;
            dataList[i].UpdateIndex();
        }

        SelectionElementManager.UpdateElements(dataElement, true);

        SetIndex();
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
