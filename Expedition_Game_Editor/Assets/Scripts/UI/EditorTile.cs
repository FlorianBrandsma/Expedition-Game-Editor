using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorTile : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private TileProperties properties;

    public RawImage icon;

    public void InitializeElement()
    {
        properties = element.listManager.listProperties.GetComponent<TileProperties>();
    }

    public void SetElement()
    {
        switch (element.data_type)
        {
            case Enums.DataType.Element:
                SetElementElement();
                break;
        }
    }

    private void SetElementElement()
    {
        ElementDataElement data = element.data.Cast<ElementDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    public void CloseElement()
    {

    }
}
