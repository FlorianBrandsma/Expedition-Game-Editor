using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScenarioManager : MonoBehaviour
{
    static public ScenarioManager instance;

    static public bool allowContinue;

    public RawImage shotStartCameraFilter;
    public RawImage shotEndCameraFilter;

    private void Awake()
    {
        instance = this;
    }

    public void SetScenario()
    {
        StartNextScene();
    }
    
    public void AllowContinue(bool allowed)
    {
        allowContinue = allowed;

        if (allowContinue && InteractionManager.activeOutcome.ActiveScene.AutoContinue)
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

        //Set actors
        SetActors(scene);

        //Set props
        SetProps(scene);

        //Set shot
        SetShot(scene);

        //Set text
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

        StartCoroutine(MoveShot(shotStart, shotEnd, scene.ShotDuration));
    }

    private IEnumerator MoveShot(GameSceneShotElementData shotStart, GameSceneShotElementData shotEnd, float duration)
    {
        var camera = GameManager.instance.Organizer.CameraManager.cam;

        var startAlpha = 0f;
        var startTargetAlpha = 0f;

        var endAlpha = 0f;
        var endTargetAlpha = 0f;

        var startPosition = camera.transform.localPosition;
        var endPosition = startPosition;

        var startRotation = camera.transform.rotation;
        var endRotation = startRotation;
        
        if (shotStart.CameraFilterPath != "")
        {
            shotStartCameraFilter.texture = Resources.Load<Texture2D>(shotStart.CameraFilterPath);
            startAlpha = 1f;
            startTargetAlpha = 0f;
        }

        if (shotEnd.CameraFilterPath != "")
        {
            shotEndCameraFilter.texture = Resources.Load<Texture2D>(shotEnd.CameraFilterPath);
            endAlpha = 0f;
            endTargetAlpha = 1f;
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

            shotStartCameraFilter.color = new Color(1, 1, 1, startAlpha);
            shotEndCameraFilter.color = new Color(1, 1, 1, endAlpha);

            camera.transform.rotation = startRotation;
            camera.transform.localPosition = startPosition;
            
            yield return null;

            while (Time.time < endTime)
            {
                var progress = (Time.time - startTime) / duration;

                if (shotStart.CameraFilterPath != "")
                {
                    shotStartCameraFilter.color = new Color(1, 1, 1, Mathf.Lerp(startAlpha, startTargetAlpha, progress));

                    if (shotEnd.CameraFilterPath != "")
                        shotEndCameraFilter.color = new Color(1, 1, 1, 1);
                }

                if (shotEnd.CameraFilterPath != "")
                {
                    if (shotStart.CameraFilterPath == "")
                        shotEndCameraFilter.color = new Color(1, 1, 1, Mathf.Lerp(endAlpha, endTargetAlpha, progress));
                }

                if (shotStart.ChangePosition || shotEnd.ChangePosition)
                    PlayerControlManager.instance.MoveCamera(Vector3.Lerp(startPosition, endPosition, progress));

                if (shotStart.ChangeRotation || shotEnd.ChangeRotation)
                    camera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, progress);
                
                yield return null;
            }
        }

        shotStartCameraFilter.color = new Color(1, 1, 1, startTargetAlpha);
        shotEndCameraFilter.color = new Color(1, 1, 1, endTargetAlpha);

        camera.transform.rotation = endRotation;
        camera.transform.localPosition = endPosition;
    }

    public void CancelScene()
    {
        AllowContinue(false);

        CancelShot();

        CloseSceneProps();
    }

    private void CancelShot()
    {
        PlayerControlManager.DisableCameraMovement = false;

        shotStartCameraFilter.color = new Color(1, 1, 1, 0);
        shotEndCameraFilter.color = new Color(1, 1, 1, 0);

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
