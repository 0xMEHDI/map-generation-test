using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] float followSpeed = 1f;

    LevelBuilder levelBuilder;

    void Awake()
    {
        levelBuilder = FindObjectOfType<LevelBuilder>();
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(5 * levelBuilder.levelSize - 5, -5 * levelBuilder.levelSize + 5, -10);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
