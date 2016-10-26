using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour {

  public Text currentWordText; //For output
  public Text messageText;
  public Grid currentGrid;
  public CameraController cameraController;
  public WordData wordData;
  public int waitSeconds = 3;
  public int averageWordLength = 4;

  public int startingLOS = 8;
  public int goalLine = 40;

  [HideInInspector]
  public int currentLineOfScrimmage;
  [HideInInspector]
  public int currentLeftSideOffset;
  [HideInInspector]
  public List<LetterTileController> selectedTiles { get; set; }
  [HideInInspector]
  public State currentState;
  [HideInInspector]
  public int lineOfScrimmageChange;


  public enum State
  {
    Waiting,
    IncorrectAnswer,
    AnimatingLetters,
    AnimatingCameraAndGrid,
    SettingUpNextPlay
  }
  //private float cameraXResetPosition = 175.0f; //118 for Y -- use this to move camera on reset


  void Awake()
  {
    SetLOSAndLeftSideOffset();
    selectedTiles = new List<LetterTileController>();
    currentWordText.text = "";
    messageText.text = "";
  }

  public void Update()
  {
    switch (currentState)
    {
      case State.Waiting:
        {
          break;
        }
      case State.IncorrectAnswer:
        {
          //Waiting for a screen touch, though could put timer element in
          if (messageText.text == "")
          {
            currentState = State.Waiting;
          }
          break;
        }
      case State.AnimatingLetters:
        {
          if (!currentGrid.IsGridAnimating())
          {
            currentState = State.AnimatingCameraAndGrid;
            EventManager.TriggerEvent("AnimateCameraAndGrid", gameObject);
          }
          break;
        }
      case State.AnimatingCameraAndGrid:
        {
          if (lineOfScrimmageChange == 0)
          {
            currentState = State.SettingUpNextPlay;
            EventManager.TriggerEvent("NextPlay", gameObject);
          }
          break;
        }
      case State.SettingUpNextPlay:
        {
          //TODO Check that next play is completed before returning to Waiting
          currentState = State.Waiting;
          break;
        }
    }
  }

  //For both hike and clearwords, need to clear away selected words
  public void HikeButtonClick()
  {
    //Let InputController know not to respond to click event
    EventManager.TriggerEvent("ButtonClick", gameObject);
    if (wordData.IsWordValid(currentWordText.text.ToLower()))
    {
      messageText.text = "Correct!";
      //Important that all correct word events happen before selected is cleared
      EventManager.TriggerEvent("CorrectWordEntered", gameObject);
    } 
    else
    {
      currentState = State.IncorrectAnswer;
      messageText.text = "Invalid Word";
      EventManager.TriggerEvent("IncorrectWordEntered", gameObject);
    }
  }

  public void ClearButtonClick()
  {
    //Let InputController know not to respond to click event
    EventManager.TriggerEvent("ButtonClick", gameObject);
    ClearWords();
  }

  public void ClearWords()
  {
    currentWordText.text = "";
    selectedTiles.Clear();
    EventManager.TriggerEvent("ResetGrid", gameObject);
  }

  public void AddLetter(LetterTileController lt)
  {
    currentWordText.text += lt.letter;
    selectedTiles.Add(lt);
  }

  public void OnCorrectWordEntered(GameObject go)
  {
    //Calculate new LOS and move
    lineOfScrimmageChange = CalculateLineDifferenceFromWordLength(selectedTiles.Count - averageWordLength);
    SetLOSAndLeftSideOffset(currentLineOfScrimmage + lineOfScrimmageChange);
  }

  private int CalculateLineDifferenceFromWordLength(int linesAboveAverage)
  {
    if (linesAboveAverage <= 1)
    {
      return linesAboveAverage;
    }
    //Could do this with a for loop, but less computing overhead to precompile (also allows it to be tweaked)
    switch (linesAboveAverage)
    {
      case 2:
        return 3;
      case 3:
        return 5;
      case 4:
        return 8;
      case 5:
        return 12;
      case 6:
        return 17;
      case 7:
        return 23;
      case 8:
        return 30;
      default:
        return goalLine;
    }

  }

  public void OnLetterSelected(GameObject tile)
  //public void InputRegistered(LetterTileController tile)
  {
    LetterTileController lt = tile.GetComponent<LetterTileController>();
    if (lt.isSelectable)
    {
      EventManager.TriggerEvent("ValidLetterSelected", tile);
      AddLetter(lt);

      //After letting letters set themselves, deselect those already chosen
      foreach (LetterTileController l in selectedTiles)
      {
        l.MakeSelectable(false);
      }
    }
    else
    {
      EventManager.TriggerEvent("InvalidLetterLocation", tile);
    }

    //if (selectedTiles.Contains(lt))
    //{
    //  ClearWords();
    //}
  }

  private void OnAnimateLetterMovement (GameObject go)
  {
    currentState = State.AnimatingLetters;
  }

  private void SetLOSAndLeftSideOffset(int los = 8)
  {
    currentLineOfScrimmage = los;
    if (currentLineOfScrimmage >= goalLine)
    {
      currentLineOfScrimmage = goalLine;
    }
    else if (currentLineOfScrimmage <= 0)
    {
      currentLineOfScrimmage = 0;
    }
    //TODO: logic to determine when left side offset is more than two away from los (at the right edge)
    currentLeftSideOffset = currentLineOfScrimmage - 2;
  }

  private void OnNextPlay(GameObject go)
  {
    //Clear out word overlay and selected word
    ClearWords();

    //Check for Touchdown or Safety
    //TODO

    //Update the down
    //TODO

    //Update the score
    //TODO
    //
  }

  private void OnCameraAndGridMoveComplete(GameObject go)
  {
    lineOfScrimmageChange = 0;
  }

  private void OnReturnToReady (GameObject go)
  {
    messageText.text = "";
  }

  public void InitGame()
  {
    currentState = State.Waiting;
  }

  public void HandleSelection(Vector2 selectionPosition)
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(selectionPosition), Vector2.zero);
    //Don't want to handle button clicks here
    if (currentState == State.IncorrectAnswer)
    {
      ClearWords();
      messageText.text = "";
      return;
    }

    if (hitInfo && hitInfo.collider.tag == "LetterTile")
    {
      messageText.text = "";
      EventManager.TriggerEvent("LetterSelected", hitInfo.collider.gameObject);
    }
  }

  void OnEnable()
  {
    EventManager.StartListening("LetterSelected", OnLetterSelected);
    EventManager.StartListening("ReturnToReady", OnReturnToReady);
    EventManager.StartListening("CorrectWordEntered", OnCorrectWordEntered);
    EventManager.StartListening("AnimateLetterMovement", OnAnimateLetterMovement);
    EventManager.StartListening("CameraAndGridMoveComplete", OnCameraAndGridMoveComplete);
    EventManager.StartListening("NextPlay", OnNextPlay);
  }
  
  void OnDisable()
  {
    EventManager.StopListening("LetterSelected", OnLetterSelected);
    EventManager.StopListening("ReturnToReady", OnReturnToReady);
    EventManager.StopListening("CorrectWordEntered", OnCorrectWordEntered);
    EventManager.StopListening("AnimateLetterMovement", OnAnimateLetterMovement);
    EventManager.StopListening("CameraAndGridMoveComplete", OnCameraAndGridMoveComplete);
    EventManager.StopListening("NextPlay", OnNextPlay);
  }
}
