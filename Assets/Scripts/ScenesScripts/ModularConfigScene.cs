using UnityEngine;

public class ModularConfigScene : MonoBehaviour
{
    public GameObject[] stage1;
    public GameObject[] stage2;
    public GameObject[] stage3;
    public Transform[] positions;
    public GameObject[] powerUps;
    public Transform powerUpPosition;
    public GameObject resetPoint;
    private Quaternion rotation = Quaternion.Euler(0, 180, 0);

    private GameObject[] spawnedRooms;
    private GameObject spawnedPowerUp;
    private int currentPowerUpIndex = 0;
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
        spawnedPowerUp = Instantiate(powerUps[currentPowerUpIndex], powerUpPosition.position, rotation);
        currentPowerUpIndex = (currentPowerUpIndex + 1) % powerUps.Length;
        PowerupsActive powerupScript = spawnedPowerUp.GetComponent<PowerupsActive>();
        powerupScript.SetResetPoint(resetPoint);
    }
    GameObject InstantiateRandom(GameObject[] stageArray, Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, stageArray.Length);
        return Instantiate(stageArray[randomIndex], spawnPoint.position, rotation);
    }
}
