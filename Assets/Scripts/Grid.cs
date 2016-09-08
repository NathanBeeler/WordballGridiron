using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
  public int xGridSize;
  public int yGridSize;
  public float xStepSize;
  public float yStepSize;
  public float xOffsetTwentyYardLine;
  public float yOffsetScreenBottom;
  public LetterTileController letterTilePrefab;
  public GameLevelController game;

  private List<List<LetterTileController>> letterGrid;

  public LetterTileController CreateLetterTile(char letter, float x, float y)
  {
    LetterTileController lt = Instantiate(letterTilePrefab, new Vector3(x, y, 0), transform.rotation) as LetterTileController;
    lt.transform.position.Set(x, y, 0);
    lt.SetLetterTileLetter(letter);
    lt.game = game;
    return lt;
  }

  void Start()
  {
    letterGrid = new List<List<LetterTileController>>();
    var columnTiles = new List<LetterTileController>();

    for (int i = 0; i < xGridSize; i++)
    {
      for (int j = 0; j < yGridSize; j++)
      {
        //Random letter for now, will be english distribution eventually
        char randomLetter = (char)('A' + Random.Range(0, 3));
        columnTiles.Add(CreateLetterTile(randomLetter, (i * xStepSize) + xOffsetTwentyYardLine, (j * yStepSize) + yOffsetScreenBottom));
      }
      letterGrid.Add(columnTiles);
      columnTiles.Clear();
    }
  }
  //  public int ROWS = 10;
  //  public int COLUMNS = 10;

  //  public GameObject letterTileGO;

  //  public float GRID_OFFSET_X = 6.4f;
  //  public float GRID_OFFSET_Y = 10f;

  //  [HideInInspector]
  //  public List<LetterTileController> tiles;

  //  [HideInInspector]
  //  public List<List<LetterTileController>> gridTiles;

  //  private string wordSource;

  //  private int wordSourceIndex;

  //  private struct Cell
  //  {
  //    public int row;
  //    public int column;
  //  }

  //  private List<Cell> gridIndexes;


  //  void Awake()
  //  {
  //    BuildIndexes();
  //  }

  //  public void BuildGrid()
  //  {
  //    var wordData = GetComponent<WordData>();
  //    wordSource = wordData.GetRandomWord();

  //    foreach (var index in gridIndexes)
  //    {
  //      gridTiles[index.column][index.row].SetTileData(wordSource[wordSourceIndex]);
  //      wordSourceIndex++;
  //      if (wordSourceIndex == wordSource.Length)
  //      {
  //        wordSource = wordData.GetRandomWord();
  //        wordSourceIndex = 0;
  //      }
  //    }
  //  }

  //  public void CollapseGrid()
  //  {
  //    for (int column = 0; column < COLUMNS; column++)
  //    {

  //      var columnList = gridTiles[column];
  //      var newColumn = new List<LetterTile>(ROWS);
  //      var removedCnt = 0;
  //      var row = ROWS - 1;
  //      var removedTiles = columnList.FindAll((e) => { return (!e.gameObject.activeSelf); });
  //      removedTiles.Reverse();
  //      var totalRemoved = removedTiles.Count;

  //      for (var i = columnList.Count - 1; i >= 0; i--)
  //      {
  //        if (!columnList[i].gameObject.activeSelf)
  //        {
  //          columnList[i].row = removedCnt;
  //          var p = columnList[i].transform.position;
  //          p.y = columnList[0].transform.position.y + (totalRemoved - removedCnt) * 2.4f;
  //          columnList[i].transform.position = p;
  //          removedCnt++;
  //        }
  //        else
  //        {
  //          columnList[i].row = row;
  //          row--;
  //          newColumn.Insert(0, columnList[i]);
  //        }
  //      }

  //      //append removed tiles
  //      newColumn.InsertRange(0, removedTiles);

  //      //update tiles
  //      foreach (var tile in newColumn)
  //      {
  //        tile.UpdateData();
  //      }

  //      gridTiles[column] = newColumn;
  //    }
  //  }

  //  private void BuildShuffledIndexes()
  //  {
  //    tiles = new List<LetterTileController>();
  //    gridTiles = new List<List<LetterTileController>>();

  //    gridIndexes = new List<Cell>();
  //    Cell indexer;
  //    for (int column = 0; column < COLUMNS; column++)
  //    {

  //      var columnTiles = new List<LetterTileController>();

  //      for (int row = 0; row < ROWS; row++)
  //      {
  //        indexer = new Cell();
  //        indexer.column = column;
  //        indexer.row = row;
  //        gridIndexes.Add(indexer);

  //        var item = Instantiate(letterTileGO) as GameObject;
  //        var tile = item.GetComponent<LetterTileController>();
  //        tile.SetTilePosition(this, column, row);
  //        tile.transform.parent = gameObject.transform;
  //        tiles.Add(tile);
  //        columnTiles.Add(tile);
  //      }
  //      gridTiles.Add(columnTiles);
  //    }

  //    WordData.ShuffleList(gridIndexes);
  //  }

}
