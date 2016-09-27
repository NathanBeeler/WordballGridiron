using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour {

  public Text currentWordText;
  public Grid currentGrid;
  public WordData wordData;

  private List<LetterTileController> selectedTiles { get; set; }

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
    selectedTiles.Clear();
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
    if (selectedTiles.Contains(lt))
    {
      ClearWords();
    }
    AddLetter(lt);
  }

  public void InitGame()
  {
    selectedTiles = new List<LetterTileController>();
    currentWordText.text = "";
  }

  public void HandleSelection(Vector2 selectionPosition)
  {
    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(selectionPosition), Vector2.zero);
    if (hitInfo && hitInfo.collider.tag == "LetterTile")
    {
      //TODO to broadcast messages to objects outside this object, need to find them -- which is fucking stupid
      //InputRegistered(hitInfo.collider.GetComponent<LetterTileController>());
      //SendMessage("OnLetterSelected", hitInfo.collider.GetComponent<LetterTileController>(), SendMessageOptions.DontRequireReceiver);
      EventManager.TriggerEvent("OnLetterSelected", hitInfo.collider.gameObject);
    }
  }

  void OnEnable()
  {
    EventManager.StartListening("OnLetterSelected", OnLetterSelected);
  }

  void OnDisable()
  {
    EventManager.StopListening("OnLetterSelected", OnLetterSelected);
  }
}
