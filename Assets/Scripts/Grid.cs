using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
  public ParticleSystem smokePuffPrefab;
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
    float num = UnityEngine.Random.Range(0.0f, 100.0f);
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
    lt.SetLetterTileLetter(letter);
    lt.SetLetterTilePositionX(x);
    lt.SetLetterTilePositioY(y);
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

  private void OnCorrectWordEntered(GameObject go)
  {
    // Correct word entered
    //TODO put a puff of smoke in each letter location
    //1. Start smoke animation on selected letters
    CreateSmokePuffs(game.selectedTiles);

    //2. Hide selected letters
    //No reason to deactivate letters, as they will just be moved
    //game.ActivateSelected(false);

    //3. Update full grid
    RemoveSelectedFromGrids(game.selectedTiles);
    //4. Move selected letters

    //5. Animate letters to fill in grid
    EventManager.TriggerEvent("AnimateLetterMovement", gameObject);

  }

  private void CreateSmokePuffs(List<LetterTileController> selectedTiles)
  {
    foreach (LetterTileController lt in selectedTiles)
    {
      ParticleSystem ps = Instantiate(smokePuffPrefab, lt.transform.position, transform.rotation) as ParticleSystem;
      StartCoroutine(ReturnToReadyAfterWait(game.WaitSeconds));
    }
  }

  private IEnumerator ReturnToReadyAfterWait(float seconds)
  {
    yield return new WaitForSeconds(seconds);
    EventManager.TriggerEvent("ReturnToReady", gameObject);
  }

  private void RemoveSelectedFromGrids(List<LetterTileController> selectedTiles)
  {
    int selectedCount;
    LetterTileController currentLetter;

    for (int row = 0; row < yGridSize; row++)
    {
      selectedCount = 0;
      for (int col = 0; col < xVisibleGridSize; col++)
      {
        currentLetter = visibleLetterGrid[col - selectedCount, row];
        if (selectedTiles.Contains(currentLetter))
        {
          //TODO: Animate puff of smoke 
          MoveLetterToEndOfVisibleGrid(currentLetter);
          selectedCount++;
          //Move selected tile to end of visible grid plus one for each previously selected
          Vector3 pos = visibleLetterGrid[xVisibleGridSize - 1, row].gameObject.transform.position;
          pos.x = ((selectedCount + xVisibleGridSize + game.currentLeftSideOffset - 1) * xStepSize) + xLeftGoalLine;
          visibleLetterGrid[xVisibleGridSize - 1, row].gameObject.transform.position = pos;
        }
      }
    }
    EventManager.TriggerEvent("ResetGrid", gameObject);
  }

  //Handles moving a letter in the visible grid (and the characters in the full grid) and sets
  //it up to move (doesn't actually move current letter, though)
  private void MoveLetterToEndOfVisibleGrid(LetterTileController currentLetter)
  {
    int col = currentLetter.gridPositionX; //These coordinates are for visible grid, relative to 0,0
    int row = currentLetter.gridPositionY;
    LetterTileController visLetter;

    try {
      for (int i = col; i < xVisibleGridSize; i++)
      {

        //Move letter from next spot in full letter grid to current
        fullLetterGrid[i + game.currentLeftSideOffset, row] = fullLetterGrid[i + game.currentLeftSideOffset + 1, row];

        //When in visible letter grid, do the same
        if (i < xVisibleGridSize - 1)
        {
          visLetter = visibleLetterGrid[i, row] = visibleLetterGrid[i + 1, row];
          visLetter.SetLetterTilePositionX(i);
          visLetter.SetToMoveToNewXPosition(((i + game.currentLeftSideOffset) * xStepSize) + xLeftGoalLine);
        }
        //Until the last space, when the current letter is placed there
        else if (i == xVisibleGridSize - 1)
        {
          visibleLetterGrid[i, row] = currentLetter;
          currentLetter.SetLetterTileLetter(fullLetterGrid[i, row]);
          currentLetter.SetLetterTilePositionX(i);
          currentLetter.SetToMoveToNewXPosition(((xVisibleGridSize + game.currentLeftSideOffset - 1) * xStepSize) + xLeftGoalLine);
        }
      }
    }
    catch (Exception e)
    {
      Debug.Log(e.ToString());
    }

    //Put a new letter in the space just past the visible grid (need to ensure full grid is always one larger than visible grid can go)
    fullLetterGrid[xVisibleGridSize + game.currentLeftSideOffset, row] = GenerateLetter();
  }

  private void CreateInitialVisibleGrid(int offsetLinesFromLeft)
  {
    for (int i = 0; i < xVisibleGridSize; i++)
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
    EventManager.StartListening("CorrectWordEntered", OnCorrectWordEntered);
  }

  void OnDisable()
  {
    EventManager.StopListening("ResetGrid", OnResetGrid);
    EventManager.StopListening("CorrectWordEntered", OnCorrectWordEntered);
  }
}
