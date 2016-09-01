using UnityEngine;
using System.Collections;

public class GameLevelController : MonoBehaviour {

  public LetterTileController letterTilePrefab;

  public void CreateLetterTile (char letterType)
  {
    LetterTileController lt = Instantiate(letterTilePrefab) as LetterTileController;
    Sprite letter = Resources.Load<Sprite>("Sprites/Letter" + letterType);
    lt.SetLetterSprite(letter);
    lt.typeOfLetter = letterType;
  }

  void Start()
  {
    CreateLetterTile('B');
  }
}
