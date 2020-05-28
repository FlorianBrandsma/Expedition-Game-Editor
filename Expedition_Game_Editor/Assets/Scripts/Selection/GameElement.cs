using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour
{
    public IDataElement DataElement { get; set; }

    public GeneralData GeneralData { get { return (GeneralData)DataElement; } }

    public IPoolable Poolable { get { return GetComponent<IPoolable>(); } }

    public void SetElement()
    {
        GetComponent<IElement>().SetElement();
    }
}
