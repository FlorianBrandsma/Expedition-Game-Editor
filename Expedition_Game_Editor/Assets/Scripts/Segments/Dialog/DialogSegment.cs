using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogSegment : MonoBehaviour, ISegment
{
    public Text messageText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData() { }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        messageText.text = DataRequestManager.dataRequest.notificationList.First().Message;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
