using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderGrid : MonoBehaviour
{
    public Transform startNode;
    public Transform endNode;
    public LayerMask obstacleMask;

    private Node[,] grid;
    private Vector2Int gridSize;
    private float nodeRadius;

    private void Awake()
    {
        nodeRadius = 0.5f; // set this to half the width of your grid cells
        gridSize = new Vector2Int(10, 10); // set this to the size of your grid
        grid = new Node[gridSize.x, gridSize.y];

        Vector2 bottomLeft = (Vector2)transform.position - Vector2.right * gridSize.x / 2 - Vector2.up * gridSize.y / 2;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPos = bottomLeft + Vector2.right * (x + 0.5f) + Vector2.up * (y + 0.5f);
                bool isObstacle = Physics2D.OverlapCircle(worldPos, nodeRadius, obstacleMask);
                grid[x, y] = new Node(isObstacle, worldPos, x, y);
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            List<Node> path = FindPath(startNode.position, endNode.position);
            foreach (Node node in path)
            {
                Debug.DrawLine(node.worldPos, node.worldPos + Vector2.up, Color.red, 10f);
            }
        }
    }

    private List<Node> FindPath(Vector2 startPos, Vector2 endPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node endNode = NodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedSet.Contains(neighbor) || neighbor.isObstacle)
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
         Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    private Node NodeFromWorldPoint(Vector2 worldPos)
    {
        float percentX = (worldPos.x - transform.position.x + gridSize.x / 2) / gridSize.x;
        float percentY = (worldPos.y - transform.position.y + gridSize.y / 2) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 0));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isObstacle ? Color.red : Color.white;
                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeRadius * 2));
            }
        }
    }

    private class Node
    {
        public bool isObstacle;
        public Vector2 worldPos;
        public int gridX;
        public int gridY;
        public int gCost;
        public int hCost;
        public Node parent;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Node(bool isObstacle, Vector2 worldPos, int gridX, int gridY)
        {
            this.isObstacle = isObstacle;
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}
