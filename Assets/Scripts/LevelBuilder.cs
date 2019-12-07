using System.Collections;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Settings")]
    public int levelSize = 5;
    [SerializeField] LayerMask roomMask;
    [SerializeField] LayerMask wallMask;
    public float roomBuildDelay = 0.1f;
    public bool generating = true;

    [Header ("Prefabs")]
    [SerializeField] GameObject[] rooms;
    [SerializeField] RoomPoint roomPoint;
    [SerializeField] Transform borderWall;

    RoomPoint[] startingRoomPoints;
    Transform[] startingRoomPositions;

    int step;
    int limit;
    int downCounter;
    [HideInInspector] public int roomStep = 10;

    void Start()
    {
        limit = (levelSize - 1) * roomStep;

        CleanLevel();
        BuildLevel();

        StartCoroutine(Build());
    }

    public void CleanLevel()
    {
        RoomPoint[] roomPoints = FindObjectsOfType<RoomPoint>();
        GameObject[] borderWalls = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < roomPoints.Length; i++)
            DestroyImmediate(roomPoints[i].gameObject);

        for (int i = 0; i < borderWalls.Length; i++)
            DestroyImmediate(borderWalls[i]);
    }

    public void BuildLevel()
    {
        startingRoomPoints = new RoomPoint[levelSize];
        Transform pointHolder = GameObject.Find("Room Points").transform;
        Transform borderHolder = GameObject.Find("Borders").transform;

        for (int i = 0; i < levelSize; i++)
            for (int j = 0; j < levelSize; j++)
            {
                RoomPoint newRoomPoint = Instantiate(roomPoint, new Vector2(roomStep * i, -roomStep * j), Quaternion.identity);
                newRoomPoint.transform.parent = pointHolder;

                Transform newBorderWall;

                if (newRoomPoint.transform.position.x % roomStep == 0 && newRoomPoint.transform.position.y == 0)
                {
                    startingRoomPoints[i] = newRoomPoint;

                    newBorderWall = Instantiate(borderWall, new Vector3(newRoomPoint.transform.position.x, 5.5f), Quaternion.identity);
                    newBorderWall.parent = borderHolder;
                }

                if (newRoomPoint.transform.position.x % roomStep == 0 && newRoomPoint.transform.position.y == (1 - levelSize) * roomStep)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(newRoomPoint.transform.position.x, -levelSize * roomStep + 4.5f), Quaternion.identity);
                    newBorderWall.parent = borderHolder;
                }

                if (newRoomPoint.transform.position.x == 0 && newRoomPoint.transform.position.y % roomStep == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(-5.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borderHolder;
                }

                if (newRoomPoint.transform.position.x == limit && newRoomPoint.transform.position.y % roomStep == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(levelSize * roomStep - 4.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borderHolder;
                }
            }
    }

    void BuildEntrance()
    {
        startingRoomPositions = new Transform[levelSize];

        for (int i = 0; i < startingRoomPositions.Length; i++)
            startingRoomPositions[i] = startingRoomPoints[i].transform;

        int randomStartingPostion = Random.Range(0, startingRoomPositions.Length);
        transform.position = startingRoomPositions[randomStartingPostion].position;

        GameObject entrance = Instantiate(rooms[4], transform.position, Quaternion.identity);

        RaycastHit2D hit = Physics2D.Raycast(entrance.transform.position, Vector2.up, Mathf.Infinity, wallMask);
        if (hit.transform.position.x == entrance.transform.position.x)
            Destroy(hit.transform.gameObject);
    }

    IEnumerator Build()
    {
        BuildEntrance();

        step = Random.Range(0, 9);

        yield return new WaitForSeconds(roomBuildDelay);

        while (generating)
        {
            if (step == 0 || step == 1 || step == 2 || step == 3)
                if (transform.position.x > 0)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x - roomStep, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    step = Random.Range(0, 9);

                    if (step == 4 || step == 5 || step == 6 || step == 7)
                        step = 0;
                }

                else
                    step = 8;

            else if (step == 4 || step == 5 || step == 6 || step == 7)
                if (transform.position.x < limit)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x + roomStep, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    step = Random.Range(4, 9);
                }

                else
                    step = 8;

            else if (step == 8)
            {
                downCounter++;

                if (transform.position.y > -limit)
                {
                    Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, roomMask);

                    if (previousRoom.GetComponent<Room>().type != 4 && previousRoom.GetComponent<Room>().type != 2)
                        if (downCounter >= 2)
                        {
                            previousRoom.GetComponent<Room>().DestroyRoom();
                            Instantiate(rooms[4], transform.position, Quaternion.identity);
                        }

                        else
                        {
                            previousRoom.GetComponent<Room>().DestroyRoom();

                            int random = Random.Range(2, 5);

                            if (random == 3)
                                random = 2;

                            Instantiate(rooms[random], transform.position, Quaternion.identity);
                        }

                    Vector2 position = new Vector2(transform.position.x, transform.position.y - roomStep);
                    transform.position = position;

                    int randomRoom = Random.Range(3, 5);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    step = Random.Range(0, 9);
                }

                else
                    BuildExit();
            }

            yield return new WaitForSeconds(roomBuildDelay);
        }
    }

    void BuildExit()
    {
        generating = false;

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1f, roomMask);
        Destroy(room.gameObject);

        GameObject exit = Instantiate(rooms[4], transform.position, Quaternion.identity);

        RaycastHit2D hit = Physics2D.Raycast(exit.transform.position, Vector2.down, Mathf.Infinity, wallMask);
        if (hit.transform.position.x == exit.transform.position.x)
            Destroy(hit.transform.gameObject);
    }
}
