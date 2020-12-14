using UnityEngine;
using System.Linq;

static public class InteractionManager
{
    static public GameWorldInteractableElementData interactionDelayTarget;
    static public GameWorldInteractableElementData interactionTarget;

    static public GameOutcomeElementData activeOutcome;

    static public float interactionDelay;
    static public ExLoadingBar loadingBar;

    static public void SetInteractionDelay(GameWorldInteractableElementData selectionTarget)
    {
        //Only set the interaction when there is currently no interaction target set
        if (interactionDelayTarget == selectionTarget) return;

        CancelInteractionDelay();

        SetInteractionDelayTarget(selectionTarget);
    }

    static private void SetInteractionDelayTarget(GameWorldInteractableElementData selectionTarget)
    {
        //Set the selection target as the interaction target if it hasn't already been set
        if (interactionDelayTarget == selectionTarget) return;
        
        interactionDelayTarget = selectionTarget;

        PlayerControlManager.instance.UpdateControls();

        if (interactionDelayTarget != null)
        {
            InitializeInteraction();
        }
    }

    static private void InitializeInteraction()
    {
        interactionDelay = interactionDelayTarget.Interaction.DelayDuration;

        //Delay duration is at least 0.1s if active
        if (interactionDelayTarget.Interaction.DelayMethod != Enums.DelayMethod.Instant)
        {
            if(!interactionDelayTarget.Interaction.HideDelayIndicator)
                loadingBar = GameManager.instance.Organizer.OverlayManger.GameOverlay.SpawnLoadingBar(interactionDelayTarget.Interaction.DelayMethod);

            //Play an animation based on the delay method

        } else {

            //Interact immediately if the delay method is nothing
            Interact();
        }
    }

    static public void UpdateLoadingBar()
    {
        if (loadingBar == null) return;

        var delayDuration = interactionDelayTarget.Interaction.DelayDuration;
        var value = Mathf.Clamp01((delayDuration - interactionDelay) / delayDuration);

        loadingBar.UpdateFiller(value);
    }

    static public void Interact()
    {
        interactionTarget = interactionDelayTarget;

        CancelInteractionDelay();

        //Selection is removed when interacting with the target
        PlayerControlManager.instance.SetSelectionTarget(null);

        SetOutcome();
    }
    
    static private void SetOutcome()
    {
        var outcomeType = CheckRequirements();

        activeOutcome = interactionTarget.Interaction.OutcomeDataList.Where(x => x.Type == (int)outcomeType).First();
        activeOutcome.SceneIndex = -1;

        PlayerControlManager.instance.UpdateControls();
        
        ScenarioManager.instance.SetScenario();
    }

    static private Enums.OutcomeType CheckRequirements()
    {
        var outcomeType = Enums.OutcomeType.Positive;

        return outcomeType;
    }

    static public void FinalizeInteraction()
    {
        Debug.Log("Finish interaction");

        CancelInteraction(true);

        GameManager.instance.CheckProgress();
    }

    static public void CancelInteractionDelay(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        if (interactionDelayTarget == gameWorldInteractableElementData)
            CancelInteractionDelay();
    }
    
    static public void CancelInteractionDelay()
    {
        if (loadingBar != null)
            PoolManager.ClosePoolObject(loadingBar);
        
        SetInteractionDelayTarget(null);
    }

    static public void CancelInteractionOnRange(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        if (interactionTarget != gameWorldInteractableElementData) return;
        
        if(activeOutcome.CancelScenarioOnRange)
        {
            CancelInteraction();
            return;
        }
    }

    static public void CheckActorSchedule(GameWorldInteractableElementData worldInteractableElementData)
    {
        //Deactivate scenario if the world interactable is an actor in any of the active scenario's scenes
        if (activeOutcome == null) return;
        
        if (activeOutcome.SceneDataList.Any(scene => scene.SceneActorDataList.Select(actor => actor.WorldInteractable).Contains(worldInteractableElementData)))
        {
            Debug.Log("Cancel scenario over actor commitments");
            CancelInteraction();
        }
    }

    static public void CancelInteraction(bool finished = false)
    {
        ScenarioManager.instance.CancelScenario(finished);

        interactionTarget = null;
        activeOutcome = null;

        PlayerControlManager.instance.UpdateControls();
    }
}
