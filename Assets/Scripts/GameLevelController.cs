using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelController : MonoBehaviour {

  public Text currentWordText;
  public Grid currentGrid;

  public void Hike()
  {
    currentWordText.text = "";
  }

  public void InputRegistered(char temp)
  {
    currentWordText.text += temp;
  }

  void Start()
  {
    currentWordText.text = "";
  }
}
