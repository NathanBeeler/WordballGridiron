using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
  public float speed = 1f;

  private float currentXPosition;
  private int stepsToAnimate;
	
  public void Start()
  {
    currentXPosition = gameObject.transform.position.x;
  }

  public void AnimateCamera(Grid grid)
  {
    //Here we deal with forward or backward
    if (stepsToAnimate > 0)
    {
      currentXPosition += grid.xStepSize;
      iTween.MoveTo(gameObject, iTween.Hash("x", currentXPosition, "time", speed, "easetype", iTween.EaseType.linear,
                                "oncomplete", "CameraMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", grid));
    }
    else if (stepsToAnimate < 0)
    {
      grid.MoveGridRowBackwards();
      currentXPosition -= grid.xStepSize;
      iTween.MoveTo(gameObject, iTween.Hash("x", currentXPosition, "time", speed, "easetype", iTween.EaseType.linear,
                                "oncomplete", "CameraMoveComplete", "oncompletetarget", gameObject, "oncompleteparams", grid));
    }
    else
    {
      //Camera and grid moves completed
      EventManager.TriggerEvent("CameraAndGridMoveComplete", gameObject);
    }
  }

  private void CameraMoveComplete(Grid grid)
  {
    if (stepsToAnimate > 0)
    {
      grid.MoveGridRowForward();
      stepsToAnimate--;
    }
    else if (stepsToAnimate < 0)
    {
      stepsToAnimate++;
    }
    AnimateCamera(grid);
  }

  public void MoveCamera(float xPosition)
  {
    currentXPosition = xPosition;
    gameObject.transform.position.Set(currentXPosition, gameObject.transform.position.y, gameObject.transform.position.z);
  }

  private void OnAnimateCameraAndGrid(GameObject go)
  {
    GameLevelController game = go.GetComponent<GameLevelController>();
    stepsToAnimate = game.lineOfScrimmageChange;
    AnimateCamera(game.currentGrid);
  }

  void OnEnable()
  {
    EventManager.StartListening("AnimateCameraAndGrid", OnAnimateCameraAndGrid);
  }

  void OnDisable()
  {
    EventManager.StopListening("AnimateCameraAndGrid", OnAnimateCameraAndGrid);
  }
}
