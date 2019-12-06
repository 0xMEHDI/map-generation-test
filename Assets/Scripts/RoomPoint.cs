using System.Collections;
using UnityEngine;

public class RoomPoint : MonoBehaviour {

    [SerializeField] LayerMask roomMask; 
    public GameObject closedRoom;
    public RoomBuilder roomBuilder;

    void Start()
    {
        StartCoroutine(CleanRoomPoints());
    }

    void Update () {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, roomMask);

        if (room == null && !roomBuilder.generating) 
        {
            Instantiate(closedRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    IEnumerator CleanRoomPoints()
    {
        yield return new WaitForSeconds(10f);

        if (!roomBuilder.generating)
            Destroy(gameObject);
    }
}
