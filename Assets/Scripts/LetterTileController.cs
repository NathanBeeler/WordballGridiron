using UnityEngine;
using System.Collections;
using System;

public class LetterTileController : MonoBehaviour {

  public SpriteRenderer letterSprite;
  public SpriteRenderer footballSprite;
  //public GameLevelController game;

  [HideInInspector]
  public char letter;
  [HideInInspector]
  public int gridPositionX; //Visible grid coordinates, relative to 0,0
  [HideInInspector]
  public int gridPositionY;
  [HideInInspector]
  public bool isSelectable;
  [HideInInspector]
  public bool isCurrent;

  private bool _moveLetter = false;
  private float _newXPosition;

  public void SetToMoveToNewXPosition(float newX)
  {
    _moveLetter = true; //This will cause the letter to animate the next time AnimateLetterMovement is raised
    _newXPosition = newX;
  }

  public void OnAnimateLetterMovement(GameObject go)
  {
    if (_moveLetter)
    {
      //iTween.MoveTo(gameObject, iTween.Hash("x", _newXPosition, "speed", 40.0f, "easetype", iTween.EaseType.easeOutCirc));
      iTween.MoveTo(gameObject, iTween.Hash("x", _newXPosition, "delay", 0.1f, "time", 2.0f, "easetype", iTween.EaseType.easeOutCirc));
      _moveLetter = false;
    }
  }

  public void SetLetterTileLetter (char chosenLetter)
  {
    Sprite newLetterSprite = Resources.Load<Sprite>("Sprites/Letter" + chosenLetter);
    letterSprite.sprite = newLetterSprite;
    letter = chosenLetter;
  }

  public void SetLetterTilePositionX(int x)
  {
    gridPositionX = x;
  }

  public void SetLetterTilePositioY(int y)
  {
    gridPositionY = y;
  }

  public void MakeSelectable(bool selectable)
  {
    isSelectable = selectable;
    if (selectable)
    {
      letterSprite.color = new Color(1.0f, 1.0f, 1.0f);
    }
    else
    {
      letterSprite.color = new Color(0.75f, 0.75f, 0.75f);
    }
  }
  public void MakeCurrent(bool current)
  {
    isCurrent = current;
    if (current)
    {
      Pulse();
      MakeSelectable(false);
    }
  }

  private void Pulse()
  {
    if (isCurrent)
    {
      //iTween.ScaleFrom(gameObject, iTween.Hash("x", 1.5f, "y", 1.5f, "time", 0.5f, "easetype", iTween.EaseType.spring, "looptype", iTween.LoopType.loop, "oncomplete", "Pulse"));
      iTween.ScaleFrom(gameObject, iTween.Hash("x", 13.0f, "y", 13.0f, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine, "oncomplete", "Pulse"));
    }
  }

  //private void Shrink()
  //{
  //  iTween.ScaleTo(gameObject, iTween.Hash("x", 1.0f, "y", 1.0f, "time", 0.5f, "oncomplete", "Grow"));
  //}

  private void OnValidLetterSelected(GameObject tile)
  {
    LetterTileController lt = tile.GetComponent<LetterTileController>();
    MakeCurrent(tile == gameObject); //Sets Current tile animation if this is current, turns it off if not
    MakeSelectable(tile != gameObject && (Math.Abs(lt.gridPositionX - gridPositionX) <= 1 && Math.Abs(lt.gridPositionY - gridPositionY) <= 1)); //Sets surround tiles to selectable
  }

  void OnEnable()
  {
    EventManager.StartListening("ValidLetterSelected", OnValidLetterSelected);
    EventManager.StartListening("AnimateLetterMovement", OnAnimateLetterMovement);
  }

  void OnDisable()
  {
    EventManager.StopListening("ValidLetterSelected", OnValidLetterSelected);
    EventManager.StopListening("AnimateLetterMovement", OnAnimateLetterMovement);
  }
}
