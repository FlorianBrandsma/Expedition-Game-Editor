using UnityEngine;
using System.Linq;

public class InteractionCore : GeneralData
{
    private int taskId;

    private bool isDefault;

    private int startTime;
    private int endTime;

    private bool triggerAutomatically;
    private bool beNearDestination;
    private bool faceAgent;
    private bool facePartyLeader;
    private bool hideInteractionIndicator;

    private float interactionRange;

    private int delayMethod;
    private int delayDuration;
    private bool hideDelayIndicator;

    private bool cancelDelayOnInput;
    private bool cancelDelayOnMovement;
    private bool cancelDelayOnHit;

    private string publicNotes;
    private string privateNotes;

    //Original
    public int originalStartTime;
    public int originalEndTime;

    public bool originalTriggerAutomatically;
    public bool originalBeNearDestination;
    public bool originalFaceAgent;
    public bool originalFacePartyLeader;
    public bool originalHideInteractionIndicator;

    public float originalInteractionRange;

    public int originalDelayMethod;
    public int originalDelayDuration;
    public bool originalHideDelayIndicator;

    public bool originalCancelDelayOnInput;
    public bool originalCancelDelayOnMovement;
    public bool originalCancelDelayOnHit;

    public string originalPublicNotes;
    public string originalPrivateNotes;

    //Original
    public bool changedStartTime;
    public bool changedEndTime;

    private bool changedTriggerAutomatically;
    private bool changedBeNearDestination;
    private bool changedFaceAgent;
    private bool changedFacePartyLeader;
    private bool changedHideInteractionIndicator;

    private bool changedInteractionRange;

    private bool changedDelayMethod;
    private bool changedDelayDuration;
    private bool changedHideDelayIndicator;

    private bool changedCancelDelayOnInput;
    private bool changedCancelDelayOnMovement;
    private bool changedCancelDelayOnHit;

    private bool changedPublicNotes;
    private bool changedPrivateNotes;

    public bool Changed
    {
        get
        {
            return  changedStartTime            || changedEndTime                   || 
                    changedTriggerAutomatically || changedBeNearDestination         || changedFaceAgent             || 
                    changedFacePartyLeader      || changedHideInteractionIndicator  || changedInteractionRange      || 
                    changedDelayMethod          || changedDelayDuration             ||
                    changedHideDelayIndicator   || changedCancelDelayOnInput        || changedCancelDelayOnMovement ||
                    changedCancelDelayOnHit     ||
                    changedPublicNotes          || changedPrivateNotes;
        }
    }

    #region Properties
    public int TaskId
    {
        get { return taskId; }
        set { taskId = value; }
    }

    public bool Default
    {
        get { return isDefault; }
        set { isDefault = value; }
    }

    public int StartTime
    {
        get { return startTime; }
        set
        {
            if (value == startTime) return;

            changedStartTime = (value != originalStartTime);

            startTime = value;
        }
    }

    public int EndTime
    {
        get { return endTime; }
        set
        {
            if (value == endTime) return;

            changedEndTime = (value != originalEndTime);

            endTime = value;
        }
    }

    public bool TriggerAutomatically
    {
        get { return triggerAutomatically; }
        set
        {
            if (value == triggerAutomatically) return;

            changedTriggerAutomatically = (value != originalTriggerAutomatically);

            triggerAutomatically = value;
        }
    }

    public bool BeNearDestination
    {
        get { return beNearDestination; }
        set
        {
            if (value == beNearDestination) return;

            changedBeNearDestination = (value != originalBeNearDestination);

            beNearDestination = value;
        }
    }

    public bool FaceAgent
    {
        get { return faceAgent; }
        set
        {
            if (value == faceAgent) return;

            changedFaceAgent = (value != originalFaceAgent);

            faceAgent = value;
        }
    }

    public bool FacePartyLeader
    {
        get { return facePartyLeader; }
        set
        {
            if (value == facePartyLeader) return;

            changedFacePartyLeader = (value != originalFacePartyLeader);

            facePartyLeader = value;
        }
    }

    public bool HideInteractionIndicator
    {
        get { return hideInteractionIndicator; }
        set
        {
            if (value == hideInteractionIndicator) return;

            changedHideInteractionIndicator = (value != originalHideInteractionIndicator);

            hideInteractionIndicator = value;
        }
    }

    public float InteractionRange
    {
        get { return interactionRange; }
        set
        {
            if (value == interactionRange) return;

            changedInteractionRange = (value != originalInteractionRange);

            interactionRange = value;
        }
    }

    public int DelayMethod
    {
        get { return delayMethod; }
        set
        {
            if (value == delayMethod) return;

            changedDelayMethod = (value != originalDelayMethod);

            delayMethod = value;
        }
    }

    public int DelayDuration
    {
        get { return delayDuration; }
        set
        {
            if (value == delayDuration) return;

            changedDelayDuration = (value != originalDelayDuration);

            delayDuration = value;
        }
    }

    public bool HideDelayIndicator
    {
        get { return hideDelayIndicator; }
        set
        {
            if (value == hideDelayIndicator) return;

            changedHideDelayIndicator = (value != originalHideDelayIndicator);

            hideDelayIndicator = value;
        }
    }

    public bool CancelDelayOnInput
    {
        get { return cancelDelayOnInput; }
        set
        {
            if (value == cancelDelayOnInput) return;

            changedCancelDelayOnInput = (value != originalCancelDelayOnInput);

            cancelDelayOnInput = value;
        }
    }

    public bool CancelDelayOnMovement
    {
        get { return cancelDelayOnMovement; }
        set
        {
            if (value == cancelDelayOnMovement) return;

            changedCancelDelayOnMovement = (value != originalCancelDelayOnMovement);

            cancelDelayOnMovement = value;
        }
    }

    public bool CancelDelayOnHit
    {
        get { return cancelDelayOnHit; }
        set
        {
            if (value == cancelDelayOnHit) return;

            changedCancelDelayOnHit = (value != originalCancelDelayOnHit);

            cancelDelayOnHit = value;
        }
    }
    
    public string PublicNotes
    {
        get { return publicNotes; }
        set
        {
            if (value == publicNotes) return;

            changedPublicNotes = (value != originalPublicNotes);

            publicNotes = value;
        }
    }

    public string PrivateNotes
    {
        get { return privateNotes; }
        set
        {
            if (value == privateNotes) return;

            changedPrivateNotes = (value != originalPrivateNotes);

            privateNotes = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var interactionData = Fixtures.interactionList.Where(x => x.id == Id).FirstOrDefault();

        if (changedStartTime)
            interactionData.startTime = startTime;

        if (changedEndTime)
            interactionData.endTime = endTime;

        if (changedTriggerAutomatically)
            interactionData.triggerAutomatically = triggerAutomatically;

        if (changedBeNearDestination)
            interactionData.beNearDestination = beNearDestination;

        if (changedFaceAgent)
            interactionData.faceAgent = faceAgent;

        if (changedFacePartyLeader)
            interactionData.facePartyLeader = facePartyLeader;

        if (changedHideInteractionIndicator)
            interactionData.hideInteractionIndicator = hideInteractionIndicator;

        if (changedInteractionRange)
            interactionData.interactionRange = interactionRange;

        if (changedDelayMethod)
            interactionData.delayMethod = delayMethod;

        if (changedDelayDuration)
            interactionData.delayDuration = delayDuration;

        if (changedHideDelayIndicator)
            interactionData.hideDelayIndicator = hideDelayIndicator;

        if (changedCancelDelayOnInput)
            interactionData.cancelDelayOnInput = cancelDelayOnInput;

        if (changedCancelDelayOnMovement)
            interactionData.cancelDelayOnMovement = cancelDelayOnMovement;

        if (changedCancelDelayOnHit)
            interactionData.cancelDelayOnHit = cancelDelayOnHit;

        if (changedPublicNotes)
            interactionData.publicNotes = publicNotes;

        if (changedPrivateNotes)
            interactionData.privateNotes = privateNotes;
    }

    public void UpdateSearch() { }

    public void UpdateIndex()
    {
        if (!changedIndex) return;

        changedIndex = false;
    }

    public virtual void SetOriginalValues()
    {
        originalStartTime = startTime;
        originalEndTime = endTime;

        originalTriggerAutomatically = triggerAutomatically;
        originalBeNearDestination = beNearDestination;
        originalFaceAgent = faceAgent;
        originalFacePartyLeader = facePartyLeader;
        originalHideInteractionIndicator = hideInteractionIndicator;

        originalInteractionRange = interactionRange;

        originalDelayMethod = delayMethod;
        originalDelayDuration = delayDuration;
        originalHideDelayIndicator = hideDelayIndicator;

        originalCancelDelayOnInput = cancelDelayOnInput;
        originalCancelDelayOnMovement = cancelDelayOnMovement;
        originalCancelDelayOnHit = cancelDelayOnHit;

        originalPublicNotes = publicNotes;
        originalPrivateNotes = privateNotes;
    }

    public void GetOriginalValues()
    {
        startTime = originalStartTime;
        endTime = originalEndTime;

        triggerAutomatically = originalTriggerAutomatically;
        beNearDestination = originalBeNearDestination;
        faceAgent = originalFaceAgent;
        facePartyLeader = originalFacePartyLeader;
        hideInteractionIndicator = originalHideInteractionIndicator;

        interactionRange = originalInteractionRange;

        delayMethod = originalDelayMethod;
        delayDuration = originalDelayDuration;
        hideDelayIndicator = originalHideDelayIndicator;

        cancelDelayOnInput = originalCancelDelayOnInput;
        cancelDelayOnMovement = originalCancelDelayOnMovement;
        cancelDelayOnHit = originalCancelDelayOnHit;

        publicNotes = originalPublicNotes;
        privateNotes = originalPrivateNotes;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedStartTime = false;
        changedEndTime = false;

        changedTriggerAutomatically = false;
        changedBeNearDestination = false;
        changedFaceAgent = false;
        changedFacePartyLeader = false;
        changedHideInteractionIndicator = false;

        changedInteractionRange = false;

        changedDelayMethod = false;
        changedDelayDuration = false;
        changedHideDelayIndicator = false;

        changedCancelDelayOnInput = false;
        changedCancelDelayOnMovement = false;
        changedCancelDelayOnHit = false;

        changedPublicNotes = false;
        changedPrivateNotes = false;
    }

    public void Delete() { }

    public void CloneCore(InteractionElementData elementData)
    {
        CloneGeneralData(elementData);

        elementData.taskId = taskId;

        elementData.isDefault = isDefault;

        elementData.startTime = startTime;
        elementData.endTime = endTime;

        elementData.triggerAutomatically = triggerAutomatically;
        elementData.beNearDestination = beNearDestination;
        elementData.faceAgent = faceAgent;
        elementData.facePartyLeader = facePartyLeader;
        elementData.hideInteractionIndicator = hideInteractionIndicator;

        elementData.interactionRange = interactionRange;

        elementData.delayMethod = delayMethod;
        elementData.delayDuration = delayDuration;
        elementData.hideDelayIndicator = hideDelayIndicator;

        elementData.cancelDelayOnInput = cancelDelayOnInput;
        elementData.cancelDelayOnMovement = cancelDelayOnMovement;
        elementData.cancelDelayOnHit = cancelDelayOnHit;

        elementData.publicNotes = publicNotes;
        elementData.privateNotes = privateNotes;

        //Original
        elementData.originalStartTime = originalStartTime;
        elementData.originalEndTime = originalEndTime;

        elementData.originalTriggerAutomatically = originalTriggerAutomatically;
        elementData.originalBeNearDestination = originalBeNearDestination;
        elementData.originalFaceAgent = originalFaceAgent;
        elementData.originalFacePartyLeader = originalFacePartyLeader;
        elementData.originalHideInteractionIndicator = originalHideInteractionIndicator;

        elementData.originalInteractionRange = originalInteractionRange;

        elementData.originalDelayMethod = originalDelayMethod;
        elementData.originalDelayDuration = originalDelayDuration;
        elementData.originalHideDelayIndicator = originalHideDelayIndicator;

        elementData.originalCancelDelayOnInput = originalCancelDelayOnInput;
        elementData.originalCancelDelayOnMovement = originalCancelDelayOnMovement;
        elementData.originalCancelDelayOnHit = originalCancelDelayOnHit;

        elementData.originalPublicNotes = originalPublicNotes;
        elementData.originalPrivateNotes = originalPrivateNotes;
    }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var interactionDataSource = (InteractionElementData)dataSource;

        taskId = interactionDataSource.taskId;

        isDefault = interactionDataSource.isDefault;

        startTime = interactionDataSource.startTime;
        endTime = interactionDataSource.endTime;

        triggerAutomatically = interactionDataSource.triggerAutomatically;
        beNearDestination = interactionDataSource.beNearDestination;
        faceAgent = interactionDataSource.faceAgent;
        facePartyLeader = interactionDataSource.facePartyLeader;
        hideInteractionIndicator = interactionDataSource.hideInteractionIndicator;

        interactionRange = interactionDataSource.interactionRange;

        delayMethod = interactionDataSource.delayMethod;
        delayDuration = interactionDataSource.delayDuration;
        hideDelayIndicator = interactionDataSource.hideDelayIndicator;

        cancelDelayOnInput = interactionDataSource.cancelDelayOnInput;
        cancelDelayOnMovement = interactionDataSource.cancelDelayOnMovement;
        cancelDelayOnHit = interactionDataSource.cancelDelayOnHit;

        publicNotes = interactionDataSource.publicNotes;
        privateNotes = interactionDataSource.privateNotes;
    }
}
