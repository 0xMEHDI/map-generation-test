using System.Collections;
using UnityEngine;

public class RoomPoint : MonoBehaviour {

    [SerializeField] LayerMask roomMask; 
    [SerializeField] GameObject closedRoom;

    LevelBuilder LevelBuilder;

    void Awake()
    {
        LevelBuilder = FindObjectOfType<LevelBuilder>();
    }

    void Start()
    {
        StartCoroutine(CleanRoomPoints());
    }

    void Update () {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, roomMask);

        if (room == null && !LevelBuilder.generating) 
        {
            Instantiate(closedRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    IEnumerator CleanRoomPoints()
    {
        yield return new WaitForSeconds(10f);

        if (!LevelBuilder.generating)
            Destroy(gameObject);
    }
}
