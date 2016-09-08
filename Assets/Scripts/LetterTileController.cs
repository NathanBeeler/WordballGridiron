﻿using UnityEngine;
using System.Collections;

public class LetterTileController : MonoBehaviour {

  public SpriteRenderer letterSprite;
  public GameLevelController game;

  [HideInInspector]
  public char letter;

  public void SetLetterTileLetter (char chosenLetter)
  {
    Sprite newLetterSprite = Resources.Load<Sprite>("Sprites/Letter" + chosenLetter);
    letterSprite.sprite = newLetterSprite;
    letter = chosenLetter;
  }

  void OnMouseUp()
  {
    game.InputRegistered(letter);
  }
}
