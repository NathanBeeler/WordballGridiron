using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
  public int xVisibleGridSize;
  public int xFullGridSize;
  public int yGridSize;
  public float xStepSize;
  public float yStepSize;
  public float xLeftGoalLine;
  public float yOffsetScreenBottom;
  public LetterTileController letterTilePrefab;
  public GameLevelController game;

  private LetterTileController[,] visibleLetterGrid;
  private char[,] fullLetterGrid;
  private float[] _frequencies =
  {
    8.167f, 1.492f, 2.782f, 4.253f, 12.702f,
    2.228f, 2.015f, 6.094f, 6.966f, 0.153f,
    0.772f, 4.025f, 2.406f, 6.749f, 7.507f,
    1.929f, 0.095f, 5.987f, 6.327f, 9.056f,
    2.758f, 0.978f, 2.360f, 0.150f, 1.974f,
    0.074f
  };
  private int int_A = 'A';

  private char GenerateLetter()
  {
    float num = Random.Range(0.0f, 100.0f);
    for (int i = 0; i < _frequencies.Length; i++)
    {
      num -= _frequencies[i];
      //Check if we've found the number
      if (num <= 0 || i == (_frequencies.Length - 1))
      {
        return (char)(int_A + i);
      }
    }
    return 'Z'; //This should never get hit, ever. If so, something is wrong
  }

  public LetterTileController CreateLetterTile(char letter, int x, int y)
  {
    float newX = ((x + game.currentLeftSideOffset) * xStepSize) + xLeftGoalLine;
    float newY = (y * yStepSize) + yOffsetScreenBottom;
    LetterTileController lt = Instantiate(letterTilePrefab, new Vector3(newX, newY, 0), transform.rotation) as LetterTileController;
    lt.transform.position.Set(newX, newY, 0);
    lt.SetLetterTileLetter(letter, x, y);
    //lt.MakeSelectable((x + game.currentLeftSideOffset) <= game.currentLineOfScrimmage); //Letter is selectable if x is left of current line of scrimmage?
    //lt.game = game;
    return lt;
  }

  void Start()
  {
    xVisibleGridSize += 1; //This allow for one extra row to be offscreen
    visibleLetterGrid = new LetterTileController[(xVisibleGridSize), yGridSize];
    fullLetterGrid = new char[xFullGridSize, yGridSize];

    FillFullGrid();
    CreateInitialVisibleGrid(game.currentLineOfScrimmage - 2); //Fills visible grid full of letter tiles game objects
  }

  private void FillFullGrid()
  {
    for (int i = 0; i < xFullGridSize; i++)
    {
      for (int j = 0; j < yGridSize; j++)
      {
        fullLetterGrid[i, j] = GenerateLetter();
      }
    }
  }

  public void OnResetGrid(GameObject go)
  {
    foreach (LetterTileController lt in visibleLetterGrid)
    {
      lt.MakeCurrent(false);
      lt.MakeSelectable(lt.gridPositionX + game.currentLeftSideOffset <= game.currentLineOfScrimmage);
    }
  }

  private void CreateInitialVisibleGrid(int offsetLinesFromLeft)
  {
    for (int i = 0; i < xVisibleGridSize; i++) //Add one to visible grid size so there's a row offscreen
    {
      for (int j = 0; j < yGridSize; j++)
      {
        visibleLetterGrid[i, j] = CreateLetterTile(fullLetterGrid[i + offsetLinesFromLeft, j], i, j);
      }
    }
    EventManager.TriggerEvent("ResetGrid", gameObject);
  }

  void OnEnable()
  {
    EventManager.StartListening("ResetGrid", OnResetGrid);
  }

  void OnDisable()
  {
    EventManager.StopListening("ResetGrid", OnResetGrid);
  }

  //public void OnLetterSelected(GameObject tile)
  ////public void InputRegistered(LetterTileController tile)
  //{
  //  LetterTileController lt = tile.GetComponent<LetterTileController>();
  //  Debug.Log(lt.letter.ToString());
  //}

  //void OnEnable()
  //{
  //  EventManager.StartListening("OnLetterSelected", OnLetterSelected);
  //}

  //void OnDisable()
  //{
  //  EventManager.StopListening("OnLetterSelected", OnLetterSelected);
  //}
}
