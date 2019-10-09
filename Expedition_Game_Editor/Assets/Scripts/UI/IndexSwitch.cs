using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class IndexSwitch : MonoBehaviour
{
    private ISegment segment;

    public Button minus_button;
    public Button plus_button;

    public Text index_number;

    private int index;
    private int indexLimit;

    public void InitializeSwitch(ISegment segment, int index, int indexLimit)
    {
        this.segment = segment;

        this.index = index;
        this.indexLimit = indexLimit;

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

        dataEditor.DataList.ForEach(data =>
        {
            var list = data.SelectionElement.dataController.DataList;

            list.RemoveAt(data.Index);
            list.Insert(index, data);

            dataEditor.Data.dataController.DataList = list.Cast<IDataElement>().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
                list[i].UpdateIndex();
            }

            SelectionElementManager.UpdateElements((GeneralData)data, true);
        });
        
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
