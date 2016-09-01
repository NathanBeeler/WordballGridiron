using UnityEngine;
using System.Collections;

public class LetterTileController : MonoBehaviour {

  public char typeOfLetter;
  public SpriteRenderer letter;

  public void SetLetterSprite (Sprite chosenLetter)
  {
    letter.sprite = chosenLetter;
  }
}
