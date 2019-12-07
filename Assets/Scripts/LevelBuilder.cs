using System.Collections;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Settings")]
    public int levelSize = 5;
    [SerializeField] LayerMask roomMask;
    [SerializeField] float roomBuildDelay = 0.1f;
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
        BuildRooms();
    }

    void BuildRooms()
    {
        CleanLevel();
        BuildLevel();

        limit = roomStep * levelSize - roomStep;
        startingRoomPositions = new Transform[levelSize];

        for (int i = 0; i < startingRoomPositions.Length; i++)
            startingRoomPositions[i] = startingRoomPoints[i].transform;

        int randomStartingPostion = Random.Range(0, startingRoomPositions.Length);
        transform.position = startingRoomPositions[randomStartingPostion].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity);

        step = Random.Range(1, 6);

        StartCoroutine(Build());
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

                if (newRoomPoint.transform.position.x % roomStep == 0 && newRoomPoint.transform.position.y == -levelSize * roomStep + roomStep)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(newRoomPoint.transform.position.x, -levelSize * roomStep + 4.5f), Quaternion.identity);
                    newBorderWall.parent = borderHolder;
                }

                if (newRoomPoint.transform.position.x == 0 && newRoomPoint.transform.position.y % roomStep == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(-5.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borderHolder;
                }
                   
                if (newRoomPoint.transform.position.x == levelSize * roomStep - roomStep && newRoomPoint.transform.position.y % roomStep == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(levelSize * roomStep - 4.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borderHolder;
                }          
            }
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

    IEnumerator Build()
    {
        yield return new WaitForSeconds(roomBuildDelay);

        while (generating)
        {
            if (step == 1 || step == 2)
                if (transform.position.x < limit)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x + roomStep, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    step = Random.Range(1, 6);
                    if (step == 3)
                        step = 1;
                    else if (step == 4)
                        step = 5;
                }

                else
                    step = 5;

            else if (step == 3 || step == 4)
                if (transform.position.x > 0)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x - roomStep, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    step = Random.Range(3, 6);
                }

                else
                    step = 5;

            else if (step == 5)
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

                    step = Random.Range(1, 6);
                }

                else
                    generating = false;
            }

            yield return new WaitForSeconds(roomBuildDelay);
        }
    }
}
