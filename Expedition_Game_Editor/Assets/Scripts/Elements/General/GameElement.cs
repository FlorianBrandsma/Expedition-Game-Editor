using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public DataElement DataElement { get { return GetComponent<DataElement>(); } }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
    }
}
