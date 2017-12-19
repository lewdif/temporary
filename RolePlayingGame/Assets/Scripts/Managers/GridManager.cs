using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    enum TileType
    {
        Plain,
        Wall
    }
    TileType[] world = null;
    int n_x;
    int n_z;

    private int rowCnt;
    private int colCnt;

    static Vector3[] corners = { new Vector3(1, 0, 0), new Vector3(0, 0, 1) };

    private GameObject player;
    private GameObject playerBody;

    Vector3 locateToCenter(Vector3 pos)
    {
        return pos + new Vector3(0.5f, 0, 0.5f);
    }

    public void BuildWorld(int n_rows, int n_cols)
    {
        int max_tiles = n_rows * n_cols;

        n_x = n_rows;
        n_z = n_cols;

        // set up the player's position to the center of the grid.
        player.transform.position = locateToCenter(new Vector3(Random.Range(1, n_x - 1), 0, Random.Range(1, n_z - 1))); // place the player to the random position.
        int player_cell = pos2Cell(player.transform.position);

        // construct a game world and assign walls.
        world = new TileType[max_tiles];
        for (int i = 0; i < max_tiles; i++)
        {
            world[i] = TileType.Plain;
            if (i == player_cell) continue; // we assign the player's location as a plain grid cell.

            
            
            if (Random.Range(0.0f, 1.0f) < 0.1) // wall is created with a probability of 10 %.
            {
                world[i] = TileType.Wall;
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.GetComponent<MeshRenderer>().material.color = Color.gray;
                wall.transform.position = locateToCenter(cell2Pos(i));
            }

            if (i / n_z < 1 || i / n_z >= 69 || i % n_x == 0 || i % n_x == n_x - 1)
            {
                world[i] = TileType.Wall;
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.GetComponent<MeshRenderer>().material.color = Color.gray;
                wall.transform.position = locateToCenter(cell2Pos(i));
            }
        }

        for (int i = 0; i < max_tiles; i++) drawRect(i, world[i] == TileType.Wall ? Color.black : Color.green);
    }

    void Start()
    {
        player = GameObject.Find("Player");
        playerBody = GameObject.Find("Player/Body");
    }

    int pos2Cell(Vector3 pos)
    {
        return ((int)pos.z) * n_x + (int)pos.x;
    }

    Vector3 cell2Pos(int cellno)
    {
        return new Vector3(cellno % n_x, 0, cellno / n_x);
    }

    void drawRect(int cellno, Color c, float duration = 10000.0f)
    {
        Vector3 lb = cell2Pos(cellno);
        Debug.DrawLine(lb, lb + corners[0], c, duration);
        Debug.DrawLine(lb, lb + corners[1], c, duration);
        Vector3 rt = lb + corners[0] + corners[1];
        Debug.DrawLine(rt, rt - corners[0], c, duration);
        Debug.DrawLine(rt, rt - corners[1], c, duration);
    }

    int[] findNeighbors(int cellno, TileType[] world)
    {
        List<int> neighbors = new List<int> { -1, 1, -n_x, n_x/*, -n_x - 1, -n_x + 1, n_x - 1, n_x + 1*/ };

        if (cellno % n_x == 0) neighbors.RemoveAll((no) => { return no == -1 || no == -1 - n_x || no == -1 + n_x; });
        if (cellno % n_x == n_x - 1) neighbors.RemoveAll((no) => { return no == 1 || no == 1 - n_x || no == 1 + n_x; });
        if (cellno / n_x == 0) neighbors.RemoveAll((no) => { return no == -n_x || no == -n_x - 1 || no == -n_x + 1; });
        if (cellno / n_x == n_z - 1) neighbors.RemoveAll((no) => { return no == n_x || no == n_x - 1 || no == n_x + 1; });

        for (int i = 0; i < neighbors.Count;)
        {
            neighbors[i] += cellno;
            if (neighbors[i] < 0 || neighbors[i] >= n_x * n_z || world[neighbors[i]] == TileType.Wall) neighbors.RemoveAt(i);
            else i++; /* increase unless removing */
        }

        // remove crossing-neighbors if they are blocked by two adjacent walls. See ppt page 41.
        Vector3 X = cell2Pos(cellno);
        for (int i = 0; i < neighbors.Count;)
        {
            Vector3 Xp = cell2Pos(neighbors[i]);
            if ((X.x - Xp.x) * (X.z - Xp.z) != 0)
            {
                if (world[pos2Cell(new Vector3(Xp.x, 0, X.z))] == TileType.Wall
                    && world[pos2Cell(new Vector3(X.x, 0, Xp.z))] == TileType.Wall)
                {
                    neighbors.RemoveAt(i);
                    continue;
                }
            }
            i++;
        }
        return neighbors.ToArray();
    }

    int[] buildPath(int[] parents, int from, int to)
    {
        if (parents == null) return null;

        List<int> path = new List<int>();
        int current = to;
        while (current != from)
        {
            path.Add(current);
            current = parents[current];
        }
        path.Add(from); // to -> ... -> ... -> from

        path.Reverse(); // from -> ... -> ... -> to
        return path.ToArray();
    }

    void drawPath(int[] path)
    {
        if (path == null) return;

        for (int i = 0; i < path.Length - 1; i++)
        {
            Debug.DrawLine(locateToCenter(cell2Pos(path[i])), locateToCenter(cell2Pos(path[i + 1])), Color.blue, 5.0f);
        }
    }

    int[] findShortestPath(int from, int to, TileType[] world)
    {
        int max_tiles = n_x * n_z;

        if (from < 0 || from >= max_tiles || to < 0 || to >= max_tiles) return null;

        // initialize the parents of all tiles to negative value, implying no tile number associated.
        int[] parents = new int[max_tiles];
        for (int i = 0; i < parents.Length; i++) parents[i] = -1;

        List<int> N = new List<int>() { from };
        while (N.Count > 0)
        {
            int current = N[0]; N.RemoveAt(0); // dequeue

            int[] neighbors = findNeighbors(current, world);
            foreach (var neighbor in neighbors)
            {
                if (neighbor == to)
                {
                    // found the destination
                    parents[neighbor] = current;
                    return buildPath(parents, from, to); // read parents array and construct the shoretest path by traversal
                }

                if (parents[neighbor] == -1) // neighbor's parent is not set yet.
                {
                    parents[neighbor] = current; // make current tile as neighbor's parent
                    N.Add(neighbor); // enqueue
                }
            }
        }
        return null; // I cannot find any path from source to destination        
    }

    public IEnumerator Move(GameObject player, Vector3 destination, float speed)
    {
        int start = pos2Cell(player.transform.position);
        int end = pos2Cell(destination);

        int[] path = findShortestPath(start, end, world);
        if (path == null) yield break;
        // path should start from "source" to "destination".

        drawPath(path);
        List<int> remaining = new List<int>(path); // convert int array to List
        remaining.RemoveAt(0); // we don't need the first one, since the first element should be same as that of source.
        while (remaining.Count > 0)
        {
            int to = remaining[0]; remaining.RemoveAt(0);

            Vector3 toLoc = locateToCenter(cell2Pos(to));
            while (player.transform.position != toLoc)
            {
                if (player.Equals(GameObject.Find("Player")))
                {
                    player.GetComponent<Character>().SetIsMoving(true);
                }
                player.transform.position = Vector3.MoveTowards(player.transform.position, toLoc, speed * Time.deltaTime);
                playerBody.transform.LookAt(toLoc);

                drawRect(pos2Cell(player.transform.position), Color.red, Time.deltaTime);
                yield return null;
            }
        }
    }

    public IEnumerator MoveInRow(GameObject monster, float distToPlayer, float speed)
    {
        int numX = 0;
        int endLeft = 0;
        int endRight = 0;

        int start = pos2Cell(monster.transform.position);

        numX = 0;
        while (world[start - numX].Equals(TileType.Plain) && start % 70 != 0)
        {
            endLeft = start - numX;
            numX++;
        }

        numX = 0;
        while (world[start + numX].Equals(TileType.Plain) && start % 70 != 69)
        {
            endRight = start + numX;
            numX++;
        }

        Vector3 toLeft = locateToCenter(cell2Pos(endLeft));
        Vector3 toRight = locateToCenter(cell2Pos(endRight));

        while (distToPlayer > 5)
        {
            if (monster.GetComponent<NormalMonster>().GetIsReached().Equals(false))
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toLeft, speed * Time.deltaTime);
                
                if (pos2Cell(monster.transform.position).Equals(endLeft)) { monster.GetComponent<NormalMonster>().SetIsReached(true); }
            }
            else
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toRight, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endRight)) { monster.GetComponent<NormalMonster>().SetIsReached(false); }
            }

            yield return null;
        }
    }

    public IEnumerator MoveInCol(GameObject monster, float distToPlayer, float speed)
    {
        int numZ = 0;
        int endUp = 0;
        int endDown = 0;
        int start = pos2Cell(monster.transform.position);

        numZ = 0;
        while (world[start - numZ].Equals(TileType.Plain) && start / 70 >= 1)
        {
            endDown = start - numZ;
            if (endDown >= 70) { numZ += 70; }
        }

        numZ = 0;
        while (world[start + numZ].Equals(TileType.Plain) && start / 70 <= 49)
        {
            endUp = start + numZ;
            if (endUp <= 4829) { numZ += 70; }
        }

        Vector3 toLeft = locateToCenter(cell2Pos(endDown));
        Vector3 toRight = locateToCenter(cell2Pos(endUp));

        while (distToPlayer > 5)
        {
            if (monster.GetComponent<NormalMonster>().GetIsReached().Equals(false))
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toLeft, speed * Time.deltaTime);
                
                if (pos2Cell(monster.transform.position).Equals(endDown)) { monster.GetComponent<NormalMonster>().SetIsReached(true); }
            }
            else
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toRight, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endUp)) { monster.GetComponent<NormalMonster>().SetIsReached(false); }
            }

            yield return null;
        }
    }

    public IEnumerator MoveRandom(GameObject monster, float distToPlayer, float speed)
    {
        int num = 0;
        int endUp = 0;
        int endDown = 0;
        int endLeft = 0;
        int endRight = 0;
        int start = pos2Cell(monster.transform.position);

        num = 0;
        while (world[start - num].Equals(TileType.Plain) && start / 70 >= 1)
        {
            endDown = start - num;
            num += 70;
        }

        num = 0;
        while (world[start + num].Equals(TileType.Plain) && start / 70 <= 69)
        {
            endUp = start + num;
            num += 70;
        }

        num = 0;
        while (world[start - num].Equals(TileType.Plain) && start % 70 != 0)
        {
            endLeft = start - num;
            num++;
        }

        num = 0;
        while (world[start + num].Equals(TileType.Plain) && start % 70 != 69)
        {
            endRight = start + num;
            num++;
        }

        Vector3 toLeft = locateToCenter(cell2Pos(endLeft));
        Vector3 toRight = locateToCenter(cell2Pos(endRight));
        Vector3 toDown = locateToCenter(cell2Pos(endDown));
        Vector3 toUp = locateToCenter(cell2Pos(endUp));

        while (distToPlayer > 8)
        {
            if (monster.GetComponent<EliteMonster>().GetIsReached() % 4 == 0)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toDown, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endDown)) { monster.GetComponent<EliteMonster>().SetIsReached(Random.Range(2, 4)); }
            }
            else if (monster.GetComponent<EliteMonster>().GetIsReached() % 4 == 1)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toUp, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endUp)) { monster.GetComponent<EliteMonster>().SetIsReached(Random.Range(2, 4)); }
            }
            else if (monster.GetComponent<EliteMonster>().GetIsReached() % 4 == 2)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toLeft, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endLeft)) { monster.GetComponent<EliteMonster>().SetIsReached(Random.Range(0, 2)); }
            }
            else if (monster.GetComponent<EliteMonster>().GetIsReached() % 4 == 3)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, toRight, speed * Time.deltaTime);

                if (pos2Cell(monster.transform.position).Equals(endRight)) { monster.GetComponent<EliteMonster>().SetIsReached(Random.Range(0, 2)); }
            }
            
            yield return null;
        }
    }
}
