using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour {

  public Text currentWordText;
  public Grid currentGrid;
  public WordData wordData;

  private List<char> _currentWord { get; set; }

  public GameLevelController()
  {
    _currentWord = new List<char>();
  }

  //For both hike and clearwords, need to clear away selected words
  public void Hike()
  {
    if (wordData.IsWordValid(currentWordText.text.ToLower()))
    {
      currentWordText.text = "Correct!";
    } 
    else
    {
      currentWordText.text = "WRONG!";
    }
  }

  public void ClearWords()
  {
    currentWordText.text = "";
  }

  public void InputRegistered(LetterTileController tile)
  {
    currentWordText.text += tile.letter;
  }

  public void InitGame()
  {
    currentWordText.text = "";
  }

  public void HandleSelection(Vector2 selectionPosition)
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(selectionPosition), Vector2.zero);
    if (hitInfo && hitInfo.collider.tag == "LetterTile")
    {
      InputRegistered(hitInfo.collider.GetComponent<LetterTileController>());
    }
  }
}
