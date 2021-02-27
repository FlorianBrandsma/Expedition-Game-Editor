using UnityEngine;

public class PlaceholderData
{
    public int Id { get; set; }

    public void GetOriginalValues(PlaceholderData originalData)
    {
        Id = originalData.Id;
    }

    public PlaceholderData Clone()
    {
        var data = new PlaceholderData();

        data.Id = Id;

        return data;
    }

    public virtual void Clone(PlaceholderElementData elementData)
    {
        elementData.Id = Id;
    }
}
