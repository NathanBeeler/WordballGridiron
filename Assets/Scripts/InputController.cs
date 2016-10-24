using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{

  public GameLevelController game;
  private bool _buttonClick = false;

  void Update()
  {

    if (Input.touches.Length > 0)
    {

      Touch touch = Input.touches[0];

      //if (touch.phase == TouchPhase.Began)
      //{
      //  game.HandleTouchDown(touch.position);
      //}
      //else
      if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
      {
        //Don't handle if this is a button click touch, because it's already been handled by the event
        if (_buttonClick)
        {
          _buttonClick = false;
        }
        else
        {
          game.HandleSelection(touch.position);
        }
      }
      //else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
      //{
      //  game.HandleTouchMove(touch.position);
      //}


      //game.HandleTouchMove(touch.position);

      return;
    }
    //else if (Input.GetMouseButtonDown(0))
    //{
    //  game.HandleTouchDown(Input.mousePosition);
    //}
    else if (Input.GetMouseButtonUp(0))
    {
      //Don't handle if this is a button click touch, because it's already been handled by the event
      if (_buttonClick)
      {
        _buttonClick = false;
      }
      else
      {
        //TODO replace direct calls with raised event
        game.HandleSelection(Input.mousePosition);
      }
    }
    //else
    //  game.HandleTouchMove(Input.mousePosition);

  }

  private void OnButtonClick(GameObject go)
  {
    _buttonClick = true;
  }

  void OnEnable()
  {
    EventManager.StartListening("ButtonClick", OnButtonClick);
  }

  void OnDisable()
  {
    EventManager.StopListening("ButtonClick", OnButtonClick);
  }
}