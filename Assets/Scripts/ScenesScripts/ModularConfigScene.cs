using UnityEngine;

public class ModularConfigScene : MonoBehaviour
{
    public GameObject[] stage1;
    public GameObject[] stage2;
    public GameObject[] stage3;
    public Transform[] positions;
    public GameObject[] powerUps;
    public Transform powerUpPosition;
    private Quaternion rotation = Quaternion.Euler(0, 180, 0);

    private GameObject[] spawnedRooms;
    private GameObject spawnedPowerUp;
    void Start()
    {
        SpawnStages();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            RegenerateStages();
        }
    }

    void SpawnStages()
    {
        spawnedRooms = new GameObject[positions.Length];
        spawnedRooms[0] = InstantiateRandom(stage1, positions[0]);
        spawnedRooms[1] = InstantiateRandom(stage2, positions[1]);
        spawnedRooms[2] = InstantiateRandom(stage3, positions[2]);
    }

    public void RegenerateStages()
    {
        ClearStages();
        ClearPowerUp();
        SpawnStages();
        SpawnPowerUp();
    }


    void ClearStages()
    {
        for (int i = 0; i < spawnedRooms.Length; i++)
        {
            if (spawnedRooms[i] != null)
            {
                Destroy(spawnedRooms[i]);
            }
        }
    }

    void ClearPowerUp()
    {
        if (spawnedPowerUp != null)
        {
            Destroy(spawnedPowerUp);
        }
    }

    void SpawnPowerUp()
    {
        int randomIndex = Random.Range(0, powerUps.Length); 
        spawnedPowerUp = Instantiate(powerUps[randomIndex], powerUpPosition.position, rotation); 
    }
    GameObject InstantiateRandom(GameObject[] stageArray, Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, stageArray.Length);
        return Instantiate(stageArray[randomIndex], spawnPoint.position, rotation);
    }
}
