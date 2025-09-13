using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs; 
    public int[,] levelMap = 
        {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {7,4,4,3,5,5,5,3,4,4,4,4,4,4},
            {7,4,4,3,5,5,5,4,3,4,4,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
        };
        
    public int[,] levelMap1 = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {7,4,4,3,5,5,5,3,3,5,3,4,4,4},
        {7,4,4,3,5,5,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
    };

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        ClearOldLevel();
        GenerateLevel(BuildMap(), Vector2.zero, transform);
        AdjustCamera();
    }
    void ClearOldLevel()
    {
        GameObject oldLevel = GameObject.Find("Manual Level Layout");
        if (oldLevel != null)
        {
            DestroyImmediate(oldLevel);
        }
    }
    
    int[,] BuildMap()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);
        int fullRows = 2 * rows - 1;
        int fullCols = 2 * cols;

        int[,] newMap = new int[fullRows, fullCols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int v = levelMap[r, c];
                newMap[r, c] = v;
                newMap[r, (fullCols - 1) - c] = v;
                newMap[(fullRows - 1) - r, c] = v;
                newMap[(fullRows - 1) - r, (fullCols - 1) - c] = v;
            }
        }

        return newMap;
    }
    
    void GenerateLevel(int[,] map, Vector2 offset, Transform parent)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int tileId = map[r, c];
                if (tileId == 0) continue;
                Vector3 pos = new Vector3(c , -r , 0) + (Vector3)offset;
                GameObject prefab = tilePrefabs[tileId];
                Instantiate(prefab, pos, ComputeRotation(map,r,c,tileId), parent);
            }
        }
    }

    Quaternion ComputeRotation(int[,] map, int r, int c, int id)
    {
        if (id == 0 || id == 5 || id == 6) return Quaternion.identity;

        bool up = r > 0 && IsWall(map[r - 1, c]);
        bool down = r < map.GetLength(0) - 1 && IsWall(map[r + 1, c]);
        bool left = c > 0 && IsWall(map[r, c - 1]);
        bool right = c < map.GetLength(1) - 1 && IsWall(map[r, c + 1]);

        if (id == 2 || id == 4 || id == 8)
        {
            if (!up || !down) return Quaternion.identity;
            return Quaternion.Euler(0, 0, 90);
        }

        if (id == 1 || id == 3)
        {
            //right c+1 left c-1 up r-1 down r+1
            if (right && left && up && down)
            {
                if (!IsWall(map[r + 1, c + 1])) return Quaternion.Euler(0, 0, 180);
                if (!IsWall(map[r + 1, c - 1])) return Quaternion.Euler(0, 0, 90);
                if (!IsWall(map[r - 1, c - 1])) return Quaternion.identity;
                if (!IsWall(map[r - 1, c + 1])) return Quaternion.Euler(0, 0, 270);
            }
            else
            {
                if (right && down) return Quaternion.Euler(0, 0, 180);
                if (left && down) return Quaternion.Euler(0, 0, 90);
                if (left && up) return Quaternion.identity;
                if (right && up) return Quaternion.Euler(0, 0, 270);
            }
        }

        if (id == 7)
        {
            //right c+1 left c-1 up r-1 down r+1
            if (!left && up && down && right)
            {
                return !IsWall(map[r - 1, c + 1]) ? Quaternion.Euler(180, 0, 90): Quaternion.Euler(0, 0, 90);
            }

            if (!down && up && left && right)
            {
                return !IsWall(map[r - 1, c - 1]) ? Quaternion.Euler(180, 0, 0): Quaternion.Euler(0, 0, 180);
            }

            if (!right && up && down && left)
            {
                return !IsWall(map[r - 1, c - 1]) ? Quaternion.Euler(0, 0, -90): Quaternion.Euler(180, 0, -90);
            }

            if (!up && down && left && right)
            {
                return !IsWall(map[r + 1, c - 1]) ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            }
        }
        return Quaternion.identity;
    }
    
    bool IsWall(int tileId)
    {
        return (tileId == 1 || tileId == 2 || tileId == 3 || 
                tileId == 4 || tileId == 7 || tileId == 8);
    }
    
    void AdjustCamera()
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);
        Camera cam = Camera.main;
        cam.orthographic = true;

        float Height = rows +1;
        float Width = cols;

        cam.orthographicSize = Height;
        cam.transform.position = new Vector3(Width-4, -Height+2,-5);
    }
}
