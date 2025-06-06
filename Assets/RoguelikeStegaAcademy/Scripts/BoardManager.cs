using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// We indicate that we'll use specifically the Random class of the UnityEngine namespace
using Random = UnityEngine.Random;
using System;
using NUnit.Framework;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    // Subclass
    [Serializable] public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min; 
            maximum = max;
        }                
    }

    // Rows & Columns Grid number
    [SerializeField] private int columns = 8;
    [SerializeField] private int rows = 8;

    [SerializeField] private Count wallCount = new Count(5, 9);
    [SerializeField] private Count foodCount = new Count(1, 5);

    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject[] floorTiles;
    [SerializeField] private GameObject[] foodTiles;
    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject[] outerWallTiles;
    [SerializeField] private GameObject[] enemiesTiles;

    // Var to keep a clean hierarchy once the prefabs clones will be created
    private Transform boardHolder;

    // List to save all the grid positions  
    private List<Vector3> gridPositions = new List<Vector3>();

    // Reset the GridPosition List and fill it with all the pos. of the 6x6 Inner Grid
    // (The items only can be placed on this Inner Grid)
    private void InitialiseList()
    {
        // Clean the grid list to avoid problems
        gridPositions.Clear();

        for (int x = 1; x < columns-1; x++)
        {
            for (int y = 1; y < rows-1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0));
            }
        }

    }
    
    // 10x10 Game Board creation (8x8 Grid + Outer Walls)
    private void BoardSetup()
    {
        // Create a GO from scratch where we'll store all the board elements
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // Tiles selection depending on the cell position we are
                GameObject tilesInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    tilesInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                // Created the prefab clones, in func. of being on the outer walls or inside of the grid.
                GameObject instance = Instantiate(tilesInstantiate, new Vector3(x,y,0f), Quaternion.identity);

                // We'll set the prefab clones as children of the boardHolder GO
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // This method will return random positions from the 6x6 Inner Grid
    // (Once a position is returned this one is removed from the List)
    private Vector3 RandomPosition()
    {
        // Get a Random position (Vector3) from the grid
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];

        // Removes from the gridPositions List the found random Position
        // (In order to avoid getting the same grid position twice)
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    // Items generation on the board (only along the 6x6 Inner Grid)
    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        // Create a var to set the number of walls, food, soda, etc..
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tilechoice = tileArray[Random.Range(0,tileArray.Length)];

            Instantiate(tilechoice, randomPosition, Quaternion.identity);
        }
    }

    // This is the main method which will call the other methods
    public void SetupScene(int level)
    {
        // Generate the outer walls and the floor along a 10x10 Grid
        // (10x10 Grid --> 8x8 Game Board + Outer Walls)
        BoardSetup();

        // Set all the positions of the 6x6 Inner Grid on a List
        InitialiseList();

        // Generate the Inner Walls along the 6x6 Inner Grid
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        // Generate the Food along the 6x6 Inner Grid
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        // Enemy management generation along the 6x6 Inner Grid (Applying the napierian logarithm)
        // This will allow gradually increase the number of enemies in func. of the level
        int enemyCount = (int)Mathf.Log(level, 2);
        LayoutObjectAtRandom(enemiesTiles, enemyCount, enemyCount);

        // Generate the Exit Signal on the Game Board
        Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
    }
}
