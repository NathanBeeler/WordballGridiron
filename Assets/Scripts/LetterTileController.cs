using UnityEngine;
using System.Collections;

public class LetterTileController : MonoBehaviour {

  public SpriteRenderer letterSprite;
  //public GameLevelController game;

  [HideInInspector]
  public char letter;
  [HideInInspector]
  public int gridPositionX;
  [HideInInspector]
  public int gridPositionY;

  public void SetLetterTileLetter (char chosenLetter, int x, int y)
  {
    Sprite newLetterSprite = Resources.Load<Sprite>("Sprites/Letter" + chosenLetter);
    letterSprite.sprite = newLetterSprite;
    letter = chosenLetter;
    gridPositionX = x;
    gridPositionY = y;
  }

  ////Really want this on mouse up, but that oddly still selects the collision where the mouse down occurred
  //void OnMouseDown()
  //{
  //  game.InputRegistered(letter);
  //}
}
