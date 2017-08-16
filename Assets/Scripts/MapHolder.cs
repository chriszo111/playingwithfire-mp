using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHolder : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject wall;
    [Range(0, 100)]
    public int obstacleCoverage;

    private List<GameObject> spawnedGameObjects;

    private int[,] map;

    // Use this for initialization
    void Awake ()
    {
        spawnedGameObjects = new List<GameObject>();

        // Demomap
        map = CreateDemoMap();

        // Create impassable objects at map[x,y] == 1
        // Create x obstacles randomly across free area(map[x,y] == 0).
        SpawnEnvironment();

        // Copy map to all other sectors
        CreateSymmetricWorld();
    }

    private int[,] CreateDemoMap()
    {
        int[,] demoMap = new int[8, 8];

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (x == 7 || y == 7 || (x == 2 && (y == 1 || y == 5)) || (x == 5 && (y == 1 || y == 5)))
                {
                    demoMap[x, y] = 1;
                }
                if (x == 6 && y == 6)
                {
                    demoMap[x, y] = 2;
                }
            }
        }
        return demoMap;
    }

    private void SpawnEnvironment()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                switch (map[x, y])
                {
                    case 1:
                        SpawnWall(x, y);
                        break;
                    case 2:
                        break;
                    default:
                        if (Random.Range(0, 100) < obstacleCoverage && !isNeighbourOfPlayerSpawn(x, y))
                        {
                            SpawnObstacle(x, y);
                        }
                        break;
                }
            }
        }
    }

    private bool isNeighbourOfPlayerSpawn(int x, int y)
    {
        return (x < map.GetLength(0) - 1 && map[x + 1, y] == 2) ||  // is below playerspawn
               (x > 0 && map[x - 1, y] == 2) ||                        // is above playerspawn
               (y < map.GetLength(1) - 1 && map[x, y + 1] == 2) ||     // is to the right of playerspawn            
               (y > 0 && map[x, y - 1] == 2);                          // is to the left of playerspawn
    }

    private void SpawnObject(GameObject gameObjectToSpawn, int x, int z, Vector3 offset)
    {
        Vector3 objectDimensions = gameObjectToSpawn.GetComponent<Renderer>().bounds.size;
        Vector3 coordinates = new Vector3(x + objectDimensions.x / 2 + offset.x,
            objectDimensions.y / 2,
            z + objectDimensions.z / 2 + offset.z);
        spawnedGameObjects.Add(Instantiate(gameObjectToSpawn, coordinates, Quaternion.identity, transform));
    }

    private void SpawnWall(int x, int y)
    {
        SpawnObject(wall, x, y, Vector3.zero);
    }

    private void SpawnObstacle(int x, int y)
    {
        Vector3 obstacleSize = obstacle.GetComponent<Renderer>().bounds.size;
        Vector3 offset = new Vector3((1 - obstacleSize.x) / 2, 0, (1 - obstacleSize.z) / 2);

        SpawnObject(obstacle, x, y, offset);
    }

    private void CreateSymmetricWorld()
    {
        MirrorWorld(new Vector3(1, 1, -1));
        MirrorWorld(new Vector3(-1, 1, 1));
    }

    private void MirrorWorld(Vector3 mirrorAxis)
    {
        List<GameObject> newGameObjects = new List<GameObject>();

        foreach (var go in spawnedGameObjects)
        {
            GameObject toSpawn;
            switch (go.tag)
            {
                case "wall":
                    toSpawn = wall;
                    break;
                case "obstacle":
                    toSpawn = obstacle;
                    break;
                default:
                    toSpawn = null;
                    break;
            }
            if (toSpawn != null)
            {
                newGameObjects.Add(
                    Instantiate(toSpawn,
                        Vector3.Scale(go.transform.position, mirrorAxis),
                        go.transform.rotation,
                        transform));
            }
        }

        spawnedGameObjects.AddRange(newGameObjects);
    }

    public int[,] getMap()
    {
        return map;
    }
}