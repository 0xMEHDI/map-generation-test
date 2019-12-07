using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] float followSpeed = 1f;
    [SerializeField] int edgeOffset = 2;
    [SerializeField] LevelBuilder levelBuilder;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3((levelBuilder.roomStep * levelBuilder.levelSize - levelBuilder.roomStep)/2, (-levelBuilder.roomStep * levelBuilder.levelSize + levelBuilder.roomStep) /2, -10);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);

        cam.orthographicSize = 5 * levelBuilder.levelSize + edgeOffset;
    }
}
