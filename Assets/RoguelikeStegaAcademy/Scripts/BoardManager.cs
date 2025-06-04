using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// We indicate that we'll use specifically the Random class of the UnityEngine namespace
using Random = UnityEngine.Random;
using System;

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
    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject[] outerWallTiles;
    [SerializeField] private GameObject[] enemiesTiles;

    // Var to keep a clean hierarchy once the prefabs clones will be created
    private Transform boardHolder;

    // List to save all the grid positions  
    private List<Vector3> gridPositions = new List<Vector3>();

}
