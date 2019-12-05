using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour {

    [SerializeField] Transform[] startingPositions;
    public GameObject[] rooms;
    [SerializeField] LayerMask roomMask;

    int direction;
    int downCounter;
    public bool generating = true;

    void Start()
    {
        StartCoroutine(BuildRooms());
    }

    IEnumerator BuildRooms()
    {
        int randomStartingPostion = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randomStartingPostion].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        yield return new WaitForSeconds(0.25f);

        while (generating)
        {
            if (direction == 1 || direction == 2)
            {
                if (transform.position.x < 15)
                {
                    downCounter = 0;

                    Vector2 pos = new Vector2(transform.position.x + 10, transform.position.y);
                    transform.position = pos;

                    int randRoom = Random.Range(1, 4);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);
                    if (direction == 3)
                        direction = 1;
                    else if (direction == 4)
                        direction = 5;
                }

                else
                    direction = 5;
            }

            else if (direction == 3 || direction == 4)
            {
                if (transform.position.x > -15)
                {
                    downCounter = 0;

                    Vector2 pos = new Vector2(transform.position.x - 10, transform.position.y);
                    transform.position = pos;

                    int randRoom = Random.Range(1, 4);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(3, 6);
                }

                else
                    direction = 5;
            }

            else if (direction == 5)
            {
                downCounter++;

                if (transform.position.y > -15)
                {
                    Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, roomMask);

                    if (previousRoom.GetComponent<Room>().type != 4 && previousRoom.GetComponent<Room>().type != 2)
                    {
                        if (downCounter >= 2)
                        {
                            previousRoom.GetComponent<Room>().DestroyRoom();
                            Instantiate(rooms[4], transform.position, Quaternion.identity);
                        }

                        else
                        {
                            previousRoom.GetComponent<Room>().DestroyRoom();

                            int randRoomDownOpening = Random.Range(2, 5);

                            if (randRoomDownOpening == 3)
                                randRoomDownOpening = 2;

                            Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
                        }
                    }

                    Vector2 pos = new Vector2(transform.position.x, transform.position.y - 10);
                    transform.position = pos;

                    int randRoom = Random.Range(3, 5);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);
                }

                else
                    generating = false;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
