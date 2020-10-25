using UnityEngine;

static public class InteractionManager
{
    static public GameWorldInteractableElementData interactionTarget;

    static public float interactionDelay;
    static public ExLoadingBar loadingBar;

    static public void SetInteraction(GameWorldInteractableElementData selectionTarget)
    {
        //Only set the interaction when there is currently no interaction target set
        if (interactionTarget != null) return;

        SetInteractionTarget(selectionTarget);
    }

    static private void SetInteractionTarget(GameWorldInteractableElementData selectionTarget)
    {
        //Set the selection target as the interaction target if it hasn't already been set
        if (interactionTarget == selectionTarget) return;
        
        interactionTarget = selectionTarget;

        PlayerControlManager.instance.UpdateControls();

        if (interactionTarget != null)
        {
            InitializeInteraction();
        }
    }

    static private void InitializeInteraction()
    {
        interactionDelay = interactionTarget.Interaction.DelayDuration;

        //Delay duration is at least 0.1s if active
        if (interactionTarget.Interaction.DelayMethod != Enums.DelayMethod.Instant)
        {
            if(!interactionTarget.Interaction.HideDelayIndicator)
                loadingBar = PlayerControlManager.instance.cameraManager.overlayManager.GameOverlay.SpawnLoadingBar(interactionTarget.Interaction.DelayMethod);

            //Play an animation based on the delay method

        } else {

            //Interact immediately if the delay method is nothing
            Interact();
        }
    }

    static public void UpdateLoadingBar()
    {
        if (loadingBar == null) return;

        var delayDuration = interactionTarget.Interaction.DelayDuration;
        var value = Mathf.Clamp01((delayDuration - interactionDelay) / delayDuration);

        loadingBar.UpdateFiller(value);
    }

    static public void Interact()
    {
        Debug.Log("INTERACT!");

        CancelInteraction();
    }

    static public void CancelInteraction(GameWorldInteractableElementData gameWorldInteractableElementData)
    {
        if (interactionTarget == gameWorldInteractableElementData)
            CancelInteraction();
    }

    static public void CancelInteraction()
    {
        if(loadingBar != null)
        {
            PoolManager.ClosePoolObject(loadingBar);
        }

        SetInteractionTarget(null);
    }
}
