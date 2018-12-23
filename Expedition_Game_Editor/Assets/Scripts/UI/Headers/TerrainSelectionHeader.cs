using UnityEngine;
using System.Collections;

public class TerrainSelectionHeader : MonoBehaviour, IHeader
{
    //private SubController subController;

    public void Activate(SubController new_subController)
    {
        //subController = new_subController;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
