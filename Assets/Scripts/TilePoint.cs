using UnityEngine;

public class TilePoint : MonoBehaviour {

    [SerializeField] GameObject[] objectsToSpawn;

    void Start()
    {
        int random = Random.Range(0, objectsToSpawn.Length);
        GameObject o = Instantiate(objectsToSpawn[random], transform.position, Quaternion.identity);
        o.transform.parent = transform;
    }
}
