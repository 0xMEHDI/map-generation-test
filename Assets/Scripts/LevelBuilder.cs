using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public int levelSize;
    [SerializeField] RoomPoint roomPoint;
    RoomPoint newRoomPoint;

    public RoomPoint[] startingRoomPoints;
    RoomPoint[] roomPoints;
    RoomBuilder roomBuilder;

    void Awake()
    {
        roomBuilder = FindObjectOfType<RoomBuilder>();
    }

    void Start()
    {
        BuildLevel();
    }

    public void BuildLevel()
    {
        startingRoomPoints = new RoomPoint[levelSize];

        for (int i = 0; i < levelSize; i++)
            for (int j = 0; j < levelSize; j++)
            {
                newRoomPoint = Instantiate(roomPoint, new Vector2(10 * i, -10 * j), Quaternion.identity);
                newRoomPoint.roomBuilder = roomBuilder;
                newRoomPoint.gameObject.transform.parent = GameObject.Find("Room Points").transform;

                if (newRoomPoint.transform.position.x % 10 == 0 && newRoomPoint.transform.position.y == 0)
                    startingRoomPoints[i] = newRoomPoint;
            }
    }

    public void DestroyLevel()
    {
        roomPoints = FindObjectsOfType<RoomPoint>();

        for (int i = 0; i < roomPoints.Length; i++)
            DestroyImmediate(roomPoints[i].gameObject);
    }
}
