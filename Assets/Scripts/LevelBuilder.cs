using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public int levelSize;
    [SerializeField] RoomPoint roomPoint;
    RoomPoint newRoomPoint;

    public RoomPoint[] startingRoomPoints;
    RoomPoint[] roomPoints;
    public RoomBuilder roomBuilder;
    Transform points;

    [SerializeField] Transform borderWall;
    Transform newBorderWall;
    GameObject[] borderWalls;
    Transform borders;

    public void BuildLevel()
    {
        startingRoomPoints = new RoomPoint[levelSize];
        points = GameObject.Find("Room Points").transform;
        borders = GameObject.Find("Borders").transform;

        for (int i = 0; i < levelSize; i++)
            for (int j = 0; j < levelSize; j++)
            {
                newRoomPoint = Instantiate(roomPoint, new Vector2(10 * i, -10 * j), Quaternion.identity);
                newRoomPoint.roomBuilder = roomBuilder;
                newRoomPoint.transform.parent = points;

                if (newRoomPoint.transform.position.x % 10 == 0 && newRoomPoint.transform.position.y == 0)
                {
                    startingRoomPoints[i] = newRoomPoint;
                    newBorderWall = Instantiate(borderWall, new Vector3(newRoomPoint.transform.position.x, 5.5f), Quaternion.identity);
                    newBorderWall.parent = borders;
                }

                if (newRoomPoint.transform.position.x % 10 == 0 && newRoomPoint.transform.position.y == -levelSize * 10 + 10)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(newRoomPoint.transform.position.x, -levelSize * 10 + 4.5f), Quaternion.identity);
                    newBorderWall.parent = borders;
                }

                if (newRoomPoint.transform.position.x == 0 && newRoomPoint.transform.position.y % 10 == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(-5.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borders;
                }
                   
                if (newRoomPoint.transform.position.x == levelSize * 10 - 10 && newRoomPoint.transform.position.y % 10 == 0)
                {
                    newBorderWall = Instantiate(borderWall, new Vector3(levelSize * 10 - 4.5f, newRoomPoint.transform.position.y), Quaternion.AngleAxis(90, Vector3.forward));
                    newBorderWall.parent = borders;
                }
                    
            }
    }

    public void DestroyLevel()
    {
        roomPoints = FindObjectsOfType<RoomPoint>();
        borderWalls = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < roomPoints.Length; i++)
            DestroyImmediate(roomPoints[i].gameObject);

        for (int i = 0; i < borderWalls.Length; i++)
            DestroyImmediate(borderWalls[i]);
    }
}
