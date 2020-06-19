using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour
{
    public enum Unit
    {
        None,
        Weight,
        Speed,
        Temperature,
        Currency,
    }

    static public string[] SI_units = { "", "kg", "km/h", "°C", "Au" };
}
