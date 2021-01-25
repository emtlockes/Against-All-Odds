using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();

        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();

        Node currentNode = nodes[start];

        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighborPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    if (LevelManager.Instance.InBounds(neighborPos) && LevelManager.Instance.Tiles[neighborPos].Walkable && neighborPos != currentNode.GridPosition)
                    {
                        int gCost = 0;



                        if (Math.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        { 
                            gCost = 150;
                        }
                        Node neighbor = nodes[neighborPos];



                        if (openList.Contains(neighbor))
                        {
                            if (currentNode.G + gCost < neighbor.G)
                            {
                                neighbor.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }
                        else if (!closedList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                            neighbor.CalcValues(currentNode, nodes[goal], gCost);
                        }
                    }


                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)
            {
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;

                }
                break;
            }
        }

        return finalPath;
        //***ONLY FOR DEBUGGING, REMOVE LATER
        //GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
    }
}
