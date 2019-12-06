using UnityEngine;

public class RoomPoint : MonoBehaviour {

    [SerializeField] LayerMask roomMask; 
    public GameObject closedRoom;
    public RoomBuilder roomBuilder;

    void Update () {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, roomMask);

        if (room == null && !roomBuilder.generating) 
        {
            Instantiate(closedRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}
}
