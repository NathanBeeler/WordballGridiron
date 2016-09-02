using UnityEngine;
using System.Collections;

public class GameLevelController : MonoBehaviour {

  public LetterTileController letterTilePrefab;
  public float xMultiplier;
  public float xOffset;
  public float yMultiplier;
  public float yOffset;
  public void CreateLetterTile (char letterType, float x, float y)
  {
    LetterTileController lt = Instantiate(letterTilePrefab, new Vector3(x, y, 0), transform.rotation) as LetterTileController;
    Sprite letter = Resources.Load<Sprite>("Sprites/Letter" + letterType);
    lt.transform.position.Set(x, y, 0);
    lt.SetLetterSprite(letter);
    lt.typeOfLetter = letterType;
  }

  void Start()
  {
    Random rand = new Random();
    for (int i = 0; i < 11; i++) 
    {
      for (int j = 0; j < 10; j++)
      {
        //Random letter for now, will be english distribution eventually
        char randomLetter = (char)('A' + Random.Range(0, 3));
        CreateLetterTile(randomLetter, (i * xMultiplier) + xOffset, (j * yMultiplier) + yOffset);
      }
    }
  }
}
