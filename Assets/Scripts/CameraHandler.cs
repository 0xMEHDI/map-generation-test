using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] float followSpeed = 1f;
    [SerializeField] int edgeOffset = 2;

    Camera cam;
    LevelBuilder levelBuilder;

    void Awake()
    {
        cam = Camera.main;
        levelBuilder = FindObjectOfType<LevelBuilder>();
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(5 * levelBuilder.levelSize - 5, -5 * levelBuilder.levelSize + 5, -10);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);

        cam.orthographicSize = 5 * levelBuilder.levelSize + edgeOffset;
    }
}
