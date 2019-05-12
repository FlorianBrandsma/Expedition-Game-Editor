﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorButton : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private ButtonProperties properties;

    public Text label;
    public RawImage icon;

    public void InitializeElement()
    {
        properties = element.ListManager.listProperties.GetComponent<ButtonProperties>();

        icon.texture = Resources.Load<Texture2D>("Textures/Icons/UI/" + element.route.property.ToString());
    }

    public void SetElement()
    {
        switch (element.route.data.DataController.DataType)
        {
            case Enums.DataType.Item:       SetItemElement();       break;
            default: Debug.Log("CASE MISSING");                     break;
        }
    }

    private void SetItemElement()
    {
        ItemDataElement data = element.route.data.ElementData.Cast<ItemDataElement>().FirstOrDefault();

        label.text = data.originalName;
    }

    public void CloseElement()
    {

    }
}
