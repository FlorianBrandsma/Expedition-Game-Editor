using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StructureManager : MonoBehaviour
{
    public void SetStructure(string table, int id)
    {
        Dropdown structure_dropdown = GetComponent<OptionManager>().AddDropdown();
    }
}
