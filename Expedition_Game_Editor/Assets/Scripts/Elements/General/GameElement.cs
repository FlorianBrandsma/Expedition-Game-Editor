using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public DataElement DataElement { get { return GetComponent<DataElement>(); } }

    public void InitializeElement()
    {
        GetComponent<IElement>().InitializeElement();
    }
    
    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
    }

    public void UpdateElement()
    {
        SetElement();
    }
}
