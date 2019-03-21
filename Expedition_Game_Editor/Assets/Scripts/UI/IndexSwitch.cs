using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IndexSwitch : MonoBehaviour
{
    private ISegment segment;

    public Button minus_button;
    public Button plus_button;

    public Text index_number;

    private int index;
    private int index_limit;

    public void InitializeSwitch(ISegment segment)
    {
        this.segment = segment;
    }

    public void Activate(int index, int index_limit)
    {
        this.index = index;
        this.index_limit = index_limit;

        SetSwitch();

        gameObject.SetActive(true);
    }

    private void SetSwitch()
    {
        SetIndex();
    }

    public void ChangeIndex(int value)
    {
        index += value;

        if (index > index_limit)
            index = 0;

        if (index < 0)
            index = index_limit;

        UpdateIndex();
    }

    private void UpdateIndex()
    {
        segment.dataEditor.UpdateIndex(index);
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
