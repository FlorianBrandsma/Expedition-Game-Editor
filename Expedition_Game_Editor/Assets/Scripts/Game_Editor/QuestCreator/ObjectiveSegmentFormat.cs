using UnityEngine;
using System.Collections;

public class ObjectiveSegmentFormat : MonoBehaviour
{
    public GameObject organizer;

    public bool collapsed;
    public GameObject content;

    public int segmentSize;
    public int segmentNumber;

    private void OnEnable()
    {
        content.SetActive(!collapsed);
    }

    public void Collapse()
    {
        content.SetActive(collapsed);
        
        collapsed = !collapsed;

        //organizer.GetComponent<SubEditor>().PlaceSegments(segmentNumber);
    }
}
