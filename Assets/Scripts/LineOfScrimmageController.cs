using UnityEngine;
using System.Collections;

public class LineOfScrimmageController : MonoBehaviour
{

  public float startingX;
  public float startingY;

  public float speed = 1f;

  private float _currentX;

  // Use this for initialization
  void Start()
  {
    Vector3 pos = this.gameObject.transform.position;
    pos.x = startingX;
    pos.y = startingY;
    this.gameObject.transform.position = pos;

    _currentX = startingX;
  }

  private void OnAnimateCameraAndGrid(GameObject go)
  {
    GameLevelController game = go.GetComponent<GameLevelController>();
    _currentX += game.currentGrid.xStepSize * game.lineOfScrimmageChange;
    iTween.MoveTo(gameObject, iTween.Hash("x", _currentX, "time", speed * Mathf.Abs(game.lineOfScrimmageChange), "easetype", iTween.EaseType.linear));
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
