using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Pathfinder : MonoBehaviour



{
     public Tilemap tilemap;
    public Transform playerTransform;
    public Transform targetTransform;

    private Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
    private Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

    private Vector3Int startTile;
    private Vector3Int endTile;

    void Start()
    {
        startTile = tilemap.WorldToCell(playerTransform.position);
        endTile = tilemap.WorldToCell(targetTransform.position);
        gScore[startTile] = 0;
        fScore[startTile] = Heuristic(startTile, endTile);

        List<Vector3Int> openList = new List<Vector3Int>();
        openList.Add(startTile);

        while (openList.Count > 0)
        {
            Vector3Int currentTile = GetLowestFScoreTile(openList, fScore);
            if (currentTile == endTile)
            {
                Debug.Log("Path found!");
                return;
            }

            openList.Remove(currentTile);

            foreach (Vector3Int neighborTile in GetNeighborTiles(currentTile))
            {
                float tentativeGScore = gScore[currentTile] + 1;
                if (!gScore.ContainsKey(neighborTile) || tentativeGScore < gScore[neighborTile])
                {
                    gScore[neighborTile] = tentativeGScore;
                    fScore[neighborTile] = tentativeGScore + Heuristic(neighborTile, endTile);

                    if (!openList.Contains(neighborTile))
                    {
                        openList.Add(neighborTile);
                    }
                }
            }
        }

        Debug.Log("No path found!");
    }

    private List<Vector3Int> GetNeighborTiles(Vector3Int tile)
    {
        List<Vector3Int> neighborTiles = new List<Vector3Int>();

        Vector3Int[] directions = {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborTile = tile + direction;
            if (tilemap.HasTile(neighborTile))
            {
                neighborTiles.Add(neighborTile);
            }
        }

        return neighborTiles;
    }

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private Vector3Int GetLowestFScoreTile(List<Vector3Int> tileList, Dictionary<Vector3Int, float> fScore)
    {
        Vector3Int lowestTile = new Vector3Int();
        float lowestFScore = Mathf.Infinity;

        foreach (Vector3Int tile in tileList)
        {
            if (fScore.ContainsKey(tile))
            {
                float fScoreForTile = fScore[tile];
                if (fScoreForTile < lowestFScore)
                {
                    lowestFScore = fScoreForTile;
                    lowestTile = tile;
                }
            }
        }

        return lowestTile;
    }
}

