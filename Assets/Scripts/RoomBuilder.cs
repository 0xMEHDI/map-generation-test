using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour 
{
    [SerializeField] GameObject[] rooms;
    [SerializeField] LayerMask roomMask;

    [SerializeField] float roomBuildDelay = 0.1f;

    Transform[] startingPositions;
    LevelBuilder levelBuilder;

    int limit;
    int direction;
    int downCounter;
    public bool generating = true;

    void Awake()
    {
        levelBuilder = FindObjectOfType<LevelBuilder>();
    }

    void Start()
    {
        limit = 10 * levelBuilder.levelSize - 10;
        startingPositions = new Transform[levelBuilder.levelSize];

        for (int i = 0; i < startingPositions.Length; i++)
            startingPositions[i] = levelBuilder.startingRoomPoints[i].transform;

        StartCoroutine(BuildRooms());
    }

    IEnumerator BuildRooms()
    {
        int randomStartingPostion = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randomStartingPostion].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        yield return new WaitForSeconds(roomBuildDelay);

        while (generating)
        {
            if (direction == 1 || direction == 2)
                if (transform.position.x < limit)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x + 10, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);
                    if (direction == 3)
                        direction = 1;
                    else if (direction == 4)
                        direction = 5;
                }

                else
                    direction = 5;

            else if (direction == 3 || direction == 4)
                if (transform.position.x > 0)
                {
                    downCounter = 0;

                    Vector2 position = new Vector2(transform.position.x - 10, transform.position.y);
                    transform.position = position;

                    int randomRoom = Random.Range(1, 4);
                    Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(3, 6);
                }

                else
                    direction = 5;

            else if (direction == 5)
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

                            int randomRoom = Random.Range(2, 5);

                            if (randomRoom == 3)
                                randomRoom = 2;

                            Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);
                        }

                    Vector2 position = new Vector2(transform.position.x, transform.position.y - 10);
                    transform.position = position;

                    int randRoom = Random.Range(3, 5);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);
                }

                else
                    generating = false;
            }

            yield return new WaitForSeconds(roomBuildDelay);
        }
    }
}
