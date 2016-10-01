﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour {

  public Text currentWordText; //For output
  public Text messageText;
  //public Grid currentGrid;
  public WordData wordData;

  [HideInInspector]
  public int currentLineOfScrimmage;
  [HideInInspector]
  public int currentLeftSideOffset;

  private List<LetterTileController> selectedTiles { get; set; }

  //For both hike and clearwords, need to clear away selected words
  public void Hike()
  {
    if (wordData.IsWordValid(currentWordText.text.ToLower()))
    {
      //TODO change these to be a gui element that isn't the current selected word
      messageText.text = "Correct!";
    } 
    else
    {
      messageText.text = "WRONG!";
    }
    //selectedTiles.Clear(); //TODO: This will have to trigger all the letters cascading (from full grid first) and animation
    ClearWords();
    //EventManager.TriggerEvent("ResetGrid", gameObject);
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

  void Awake()
  {
    SetLOSAndLeftSideOffset();
    selectedTiles = new List<LetterTileController>();
    currentWordText.text = "";
    messageText.text = "";
  }

  private void SetLOSAndLeftSideOffset(int los = 8)
  {
    currentLineOfScrimmage = los;
    //TODO: logic to determine when left side offset is more than two away from los (at the right edge)
    currentLeftSideOffset = currentLineOfScrimmage - 2;
  }

  public void InitGame()
  {
    //This is called when word data is loaded
  }

  public void HandleSelection(Vector2 selectionPosition)
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(selectionPosition), Vector2.zero);
    if (hitInfo && hitInfo.collider.tag == "LetterTile")
    {
      messageText.text = "";
      //TODO to broadcast messages to objects outside this object, need to find them -- which is fucking stupid
      //InputRegistered(hitInfo.collider.GetComponent<LetterTileController>());
      //SendMessage("OnLetterSelected", hitInfo.collider.GetComponent<LetterTileController>(), SendMessageOptions.DontRequireReceiver);
      EventManager.TriggerEvent("LetterSelected", hitInfo.collider.gameObject);
    }
  }

  void OnEnable()
  {
    EventManager.StartListening("LetterSelected", OnLetterSelected);
  }

  void OnDisable()
  {
    EventManager.StopListening("LetterSelected", OnLetterSelected);
  }
}
