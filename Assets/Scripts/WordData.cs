using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class WordData : MonoBehaviour
{

  public GameLevelController game;

  [HideInInspector]
  public Dictionary<char, List<string>> wordsMap;

  //private List<string> allWords;

  //  private List<string> allWordsUnique;


  //  public bool IsValidWord(string word)
  //  {
  //    if (!wordsMap.ContainsKey(word[0]))
  //      return false;
  //    var list = wordsMap[word[0]];
  //    if (list != null)
  //    {
  //      return list.Contains(word);

  //    }
  //    return false;
  //  }


  void Start()
  {
    wordsMap = new Dictionary<char, List<string>>();
    //StartCoroutine("LoadWordData");
    LoadWordData();
  }

  void LoadWordData()
  {

    string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "SortedWordList.txt");

    string result = null;

    result = File.ReadAllText(filePath);

    ProcessWordSource(result);

    filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "commonWords.txt");

    game.InitGame();
  }

  void ProcessWordSource(string data)
  {
    var words = data.Split('\n');
    foreach (var word in words.Where(x => x != ""))
    {
      //Stores words by their first letter in a Dictionary of lists
      var c = word[0];
      if (!wordsMap.ContainsKey(c))
      {
        wordsMap.Add(c, new List<string>());
      }
      wordsMap[c].Add(word.TrimEnd());
    }
  }

  public bool IsWordValid(string wordToTest)
  {
    //TODO change all this back for real game
    return true;
    //List<string> wordsByLetter;
    //if (wordsMap.TryGetValue(wordToTest[0], out wordsByLetter))
    //  return wordsByLetter.Contains(wordToTest);

    //return false;
  }
}