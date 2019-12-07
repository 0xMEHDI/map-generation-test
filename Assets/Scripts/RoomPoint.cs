using System.Collections;
using UnityEngine;

public class RoomPoint : MonoBehaviour {

    [SerializeField] LayerMask roomMask; 
    [SerializeField] GameObject closedRoom;

    LevelBuilder levelBuilder;
    float cleanDelay;

    void Awake()
    {
        levelBuilder = FindObjectOfType<LevelBuilder>();
    }

    void Start()
    {
        cleanDelay = Mathf.Pow(levelBuilder.levelSize, 3) * levelBuilder.roomBuildDelay;
        StartCoroutine(CleanRoomPoints());
    }

    void Update () {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, roomMask);

        if (room == null && !levelBuilder.generating) 
        {
            Instantiate(closedRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    IEnumerator CleanRoomPoints()
    {
        yield return new WaitForSeconds(cleanDelay);

        if (!levelBuilder.generating)
            Destroy(gameObject);
    }
}
