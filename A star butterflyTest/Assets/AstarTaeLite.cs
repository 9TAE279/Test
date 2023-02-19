using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTaeLite : MonoBehaviour
{
    int gridSizeX = 2;
    int gridSizeY = 1;

     float cellSize = 2;

    AStarNode[,] aStarNodes;

    AStarNode startNode;


  void CreateGrid()
    {
        //Allocate space in the array for the nodes
        aStarNodes = new AStarNode[gridSizeX, gridSizeY];

        //Create the grid of nodes
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                aStarNodes[x, y] = new AStarNode(new Vector2Int(x, y));

            
            }

    }


  Vector2Int ConvertWorldToGridPoint(Vector2 position)
    {
        //Calculate grid point
        Vector2Int gridPoint = new Vector2Int(Mathf.RoundToInt(position.x / cellSize + gridSizeX / 2.0f), Mathf.RoundToInt(position.y / cellSize + gridSizeY / 2.0f));

        return gridPoint;
    }


    Vector3 ConvertGridPositionToWorldPosition(AStarNode aStarNode)
    {
        return new Vector3(aStarNode.gridPosition.x * cellSize - (gridSizeX * cellSize) / 2.0f, aStarNode.gridPosition.y * cellSize - (gridSizeY * cellSize) / 2.0f, 0);
    }








    void OnDrawGizmos()
    {
        if (aStarNodes == null)
            return;

       

        //Draw a grid
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                  if(aStarNodes[x, y].isObstacle)
		                    Gizmos.color = Color.red;
                else Gizmos.color = Color.green;

                Gizmos.DrawWireCube(ConvertGridPositionToWorldPosition(aStarNodes[x, y]), new Vector3(cellSize, cellSize, cellSize));
            }


    }
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
