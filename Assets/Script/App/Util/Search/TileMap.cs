﻿using System;
using System.Collections.Generic;
using App.View.Map;
using UnityEngine;

namespace App.Util.Search
{
    public class TileMap
    {
        public TileMap()
        {
        }
        public VTile GetTile(Vector2Int coordinate)
        {
            return Global.tileUnits[coordinate.y][coordinate.x];
        }
        public List<Vector2Int> GetNeighboringCoordinates(Vector2Int coordinate)
        {
            List<Vector2Int> coordinates = new List<Vector2Int>();
            int x = coordinate.x;
            int y = coordinate.y;
            int mapHeight = Global.tileUnits.Count;
            int mapWidth = Global.tileUnits[0].Count;
            if (y > 0)
            {
                if (y % 2 == 0)
                {
                    if (x > 0)
                    {
                        coordinates.Add(new Vector2Int(x - 1, y - 1));
                    }
                    coordinates.Add(new Vector2Int(x, y - 1));
                }
                else
                {
                    coordinates.Add(new Vector2Int(x, y - 1));
                    if (x + 1 < mapWidth)
                    {
                        coordinates.Add(new Vector2Int(x + 1, y - 1));
                    }
                }
            }
            if (x + 1 < mapWidth)
            {
                coordinates.Add(new Vector2Int(x + 1, y));
            }
            if (y + 1 < mapHeight)
            {
                if (y % 2 == 0)
                {
                    coordinates.Add(new Vector2Int(x, y + 1));
                    if (x > 0)
                    {
                        coordinates.Add(new Vector2Int(x - 1, y + 1));
                    }
                }
                else
                {
                    if (x + 1 < mapWidth)
                    {
                        coordinates.Add(new Vector2Int(x + 1, y + 1));
                    }
                    coordinates.Add(new Vector2Int(x, y + 1));
                }
            }
            if (x > 0)
            {
                coordinates.Add(new Vector2Int(x - 1, y));
            }
            return coordinates;
        }
        public int GetDistance(VTile tile1, VTile tile2){
            return GetDistance(tile1.coordinate.x, tile1.coordinate.y, tile2.coordinate.x, tile2.coordinate.y);
        }
        public int GetDistance(Vector2Int coordinate1, Vector2Int coordinate2)
        {
            return GetDistance(coordinate1.x, coordinate1.y, coordinate2.x, coordinate2.y);
        }
        public int GetDistance(int x, int y, int cx, int cy){
            if (cy == y)
            {
                return Mathf.Abs(cx - x);
            }
            int distance = 0;
            int directionY = cy > y ? 1 : -1;
            do
            {
                distance += 1;
                if (cx != x)
                {
                    if (y % 2 == 0)
                    {
                        if (cx < x)
                        {
                            x -= 1;
                        }
                    }
                    else
                    {
                        if (cx > x)
                        {
                            x += 1;
                        }
                    }
                }
                y += directionY;
            } while (cy != y);
            return Mathf.Abs(cx - x) + distance;
        }
    }
}
