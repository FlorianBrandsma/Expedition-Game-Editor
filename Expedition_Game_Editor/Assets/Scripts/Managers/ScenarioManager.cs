using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScenarioManager : MonoBehaviour
{
    static public ScenarioManager instance;

    static public bool allowContinue;

    private ExCameraFilter shotStartCameraFilter;
    private ExCameraFilter shotEndCameraFilter;

    private ExSpeechTextBox speechTextBox;

    private ExSpeechBubble speechBubblePrefab;

    private void Awake()
    {
        instance = this;

        speechBubblePrefab = Resources.Load<ExSpeechBubble>("Elements/UI/SpeechBubble");
    }

    public void SetScenario()
    {
        StartNextScene();
    }
    
    public void AllowContinue(bool allowed)
    {
        allowContinue = allowed;

        if (allowContinue && (InteractionManager.activeOutcome.ActiveScene.AutoContinue || speechTextBox == null))
            StartNextScene();

        PlayerControlManager.instance.UpdateControls();
    }

    public void StartNextScene()
    {
        var nextScene = InteractionManager.activeOutcome.SceneIndex + 1;
        
        if (nextScene == InteractionManager.activeOutcome.SceneDataList.Count)
        {
            FinalizeScenario();

        } else {

            if(nextScene > 0)
                CancelScene();

            InteractionManager.activeOutcome.SceneIndex = nextScene;

            SetScene();
        }
    }

    public void SetScene()
    {
        var scene = InteractionManager.activeOutcome.ActiveScene;
        
        //-Actors must be able to move to their position instantly
        //Scene editor, "set actors instantly"

        SetRegion(scene);

        SetGameTime(scene);

        SetActors(scene);

        SetProps(scene);

        SetShot(scene);

        SetText(scene);
    }

    private void SetRegion(GameSceneElementData scene)
    {        
        GameManager.instance.ActiveRegionId = scene.RegionId;
    }

    private void SetGameTime(GameSceneElementData scene)
    {
        if (scene.FreezeTime)
            TimeManager.gameTimeSpeed = 0;
        else
            GameManager.instance.SetChapterTimeSpeed();
    }

    private void SetActors(GameSceneElementData scene)
    {
        scene.SceneActorDataList.ForEach(actor => SetActorTransform(actor, false));
    }

    private void SetActorTransform(GameSceneActorElementData actor, bool instant)
    {
        var changeTransform = false;

        actor.WorldInteractable.DestinationType = DestinationType.Scene;

        if (actor.FreezePosition)
        {
            actor.WorldInteractable.DestinationPosition = new Vector3(actor.WorldInteractable.DataElement.transform.position.x, 
                                                                      actor.WorldInteractable.DataElement.transform.position.y, 
                                                                      -actor.WorldInteractable.DataElement.transform.position.z);

            changeTransform = true;

            //Change position can only be true if "freeze position" is false
        } else if (actor.ChangePosition) {
            
            //Only set new destination if settings allow it
            actor.WorldInteractable.DestinationPosition = new Vector3(actor.PositionX, actor.PositionY, actor.PositionZ);
            actor.WorldInteractable.TerrainTileId = actor.TerrainTileId;

            changeTransform = true;
        }
        
        //Only set new rotation if settings allow it
        if(actor.ChangeRotation)
        {
            actor.WorldInteractable.ArrivalRotation = new Vector3(actor.RotationX, actor.RotationY, actor.RotationZ);
            actor.WorldInteractable.AllowRotation = true;

            changeTransform = true;
        }

        if (changeTransform)
            GameManager.instance.UpdateWorldInteractable(actor.WorldInteractable);
    }

    static private void SetProps(GameSceneElementData scene)
    {
        GameManager.instance.ScenePropDataController.Data.dataList = scene.ScenePropDataList.Cast<IElementData>().ToList();

        scene.ScenePropDataList.ForEach(prop =>
        {
            GameManager.instance.UpdateSceneProp(prop);
        });
    }

    private void SetShot(GameSceneElementData scene)
    {
        //Target functionality will be added at a later point

        //Only play shot when either shot changes position, rotation or filter
        if (!scene.SceneShotDataList.Any(x => x.ChangePosition || x.ChangeRotation || x.CameraFilterPath != "")) return;

        //Disable manual camera movement if the camera is controlled by either shot
        if (scene.SceneShotDataList.Any(x => x.ChangePosition || x.ChangeRotation)) 
            PlayerControlManager.DisableCameraMovement = true;
        
        var shotStart = scene.SceneShotDataList.Where(x => x.Type == (int)Enums.SceneShotType.Start).First();
        var shotEnd = scene.SceneShotDataList.Where(x => x.Type == (int)Enums.SceneShotType.End).First();

        StartCoroutine(PlayShot(shotStart, shotEnd, scene.ShotDuration));
    }

    private IEnumerator PlayShot(GameSceneShotElementData shotStart, GameSceneShotElementData shotEnd, float duration)
    {
        var camera = GameManager.instance.Organizer.CameraManager.cam;

        var cameraFilterOverlay = GameManager.instance.Organizer.CameraManager.overlayManager.CameraFilterOverlay;

        var startAlpha = 0f;
        var startTargetAlpha = 0f;

        var endAlpha = 0f;
        var endTargetAlpha = 0f;

        var startPosition = camera.transform.localPosition;
        var endPosition = startPosition;

        var startRotation = camera.transform.rotation;
        var endRotation = startRotation;

        //End must be spawned before start to appear in the correct order
        if (shotEnd.CameraFilterPath != "")
        {
            shotEndCameraFilter = cameraFilterOverlay.SpawnCameraFilter(Resources.Load<Texture2D>(shotEnd.CameraFilterPath));
            endAlpha = 0f;
            endTargetAlpha = 1f;
        }

        if (shotStart.CameraFilterPath != "")
        {
            shotStartCameraFilter = cameraFilterOverlay.SpawnCameraFilter(Resources.Load<Texture2D>(shotStart.CameraFilterPath));
            startAlpha = 1f;
            startTargetAlpha = 0f;
        }
        
        if (shotStart.ChangePosition)
            startPosition = new Vector3(shotStart.PositionX, shotStart.PositionY, -shotStart.PositionZ);

        if(shotEnd.ChangePosition)
            endPosition = new Vector3(shotEnd.PositionX, shotEnd.PositionY, -shotEnd.PositionZ);

        if (shotStart.ChangeRotation)
            startRotation = Quaternion.Euler(shotStart.RotationX, shotStart.RotationY, shotStart.RotationZ);
        
        if(shotEnd.ChangeRotation)
            endRotation = Quaternion.Euler(shotEnd.RotationX, shotEnd.RotationY, shotEnd.RotationZ);
        
        if (duration > 0f)
        {
            var startTime = Time.time;
            var endTime = startTime + duration;

            if(shotStartCameraFilter != null)
                shotStartCameraFilter.SetAlpha(startAlpha);

            if(shotEndCameraFilter != null)
                shotEndCameraFilter.SetAlpha(endAlpha);

            camera.transform.rotation = startRotation;
            camera.transform.localPosition = startPosition;
            
            yield return null;

            while (Time.time < endTime)
            {
                var progress = (Time.time - startTime) / duration;

                if (shotStart.CameraFilterPath != "")
                {
                    shotStartCameraFilter.SetAlpha(Mathf.Lerp(startAlpha, startTargetAlpha, progress));

                    if (shotEnd.CameraFilterPath != "")
                        shotEndCameraFilter.SetAlpha(endTargetAlpha);
                }

                if (shotEnd.CameraFilterPath != "")
                {
                    if (shotStart.CameraFilterPath == "")
                        shotEndCameraFilter.SetAlpha(Mathf.Lerp(endAlpha, endTargetAlpha, progress));
                }

                if (shotStart.ChangePosition || shotEnd.ChangePosition)
                    PlayerControlManager.instance.MoveCamera(Vector3.Lerp(startPosition, endPosition, progress));

                if (shotStart.ChangeRotation || shotEnd.ChangeRotation)
                    camera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, progress);
                
                yield return null;
            }
        }

        if (shotStartCameraFilter != null)
            shotStartCameraFilter.SetAlpha(startTargetAlpha);

        if (shotEndCameraFilter != null)
            shotEndCameraFilter.SetAlpha(endTargetAlpha);

        camera.transform.rotation = endRotation;
        camera.transform.localPosition = endPosition;
    }

    private void SetText(GameSceneElementData scene)
    {
        scene.SceneActorDataList.Where(actor => ((Enums.SpeechMethod)actor.SpeechMethod) != Enums.SpeechMethod.None).ToList().ForEach(actor =>
        {
            if(actor.ShowTextBox)
            {
                SetSpeechTextBox(actor);           
            } else {
                SetSpeechBubble(actor);
            }
        });
    }

    private void SetSpeechTextBox(GameSceneActorElementData gameSceneActorElementData)
    {
        var gameOverlay = GameManager.instance.Organizer.CameraManager.overlayManager.GameOverlay;

        speechTextBox = gameOverlay.SpawnSpeechTextBox();

        speechTextBox.nameText.text = gameSceneActorElementData.WorldInteractable.InteractableName;
        speechTextBox.speechText.text = gameSceneActorElementData.SpeechText;

        speechTextBox.SetSpeechTextBox((Enums.SpeechMethod)gameSceneActorElementData.SpeechMethod);

        var lastScene = InteractionManager.activeOutcome.SceneIndex == InteractionManager.activeOutcome.SceneDataList.Count - 1;

        speechTextBox.moreIcon.SetActive(!lastScene);
    }

    private void SetSpeechBubble(GameSceneActorElementData gameSceneActorElementData)
    {
        var trackingElementOverlay = GameManager.instance.Organizer.CameraManager.overlayManager.TrackingElementOverlay;

        var speechBubble = trackingElementOverlay.SpawnSpeechBubble(speechBubblePrefab);
        speechBubble.TrackingElement.SetTrackingTarget(gameSceneActorElementData.WorldInteractable.DataElement);

        speechBubble.speechText.text = gameSceneActorElementData.SpeechText;

        gameSceneActorElementData.WorldInteractable.DataElement.GetComponent<GameElement>().SpeechBubble = speechBubble;
    }

    public void CancelScene()
    {
        AllowContinue(false);

        CancelShot();

        CloseSceneProps();

        CloseActors();
    }

    private void CancelShot()
    {
        PlayerControlManager.DisableCameraMovement = false;

        if(shotStartCameraFilter != null)
            PoolManager.ClosePoolObject(shotStartCameraFilter);

        if(shotEndCameraFilter != null)
            PoolManager.ClosePoolObject(shotEndCameraFilter);
        
        StopAllCoroutines();
        PlayerControlManager.instance.ResetCamera();
    }

    private void CloseSceneProps()
    {
        InteractionManager.activeOutcome.ActiveScene.ScenePropDataList.ToList().ForEach(x =>
        {
            GameManager.instance.CloseSceneProp(x);
        });

        GameManager.instance.ScenePropDataController.Data.dataList = new List<IElementData>();
    }

    private void CloseActors()
    {
        //Close speech bubbles of all speaking actors whose text appears in a speech bubble
        InteractionManager.activeOutcome.ActiveScene.SceneActorDataList.Where(actor => ((Enums.SpeechMethod)actor.SpeechMethod) != Enums.SpeechMethod.None && !actor.ShowTextBox).ToList().ForEach(actor =>
        {
            actor.WorldInteractable.DataElement.GetComponent<GameElement>().CloseSpeechBubble();
        });

        if (speechTextBox != null)
            PoolManager.ClosePoolObject(speechTextBox);
    }

    public void FinalizeScenario()
    {
        Debug.Log("Finish scenario");
 
        //Should go to another outcome step, which does not yet exist
        InteractionManager.FinalizeInteraction();
    }

    public void CancelScenario(bool finished = false)
    {
        if (InteractionManager.activeOutcome == null) return;
        
        //Reset actors of the active scene if the scenario is cancelled outside of finishing
        if (!finished)
        {
            InteractionManager.activeOutcome.ActiveScene.SceneActorDataList.Select(actor => actor.WorldInteractable).ToList()
                                                                           .ForEach(worldInteractable => GameManager.instance.ResetInteractable(worldInteractable));
        }
        
        CancelScene();
        
        //Resets game time
        GameManager.instance.SetChapterTimeSpeed();
    }
}
