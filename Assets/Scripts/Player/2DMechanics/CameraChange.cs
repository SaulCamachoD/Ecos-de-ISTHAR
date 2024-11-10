using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondCamera;
    private Collider Collider;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        
    }
    private void Start()
    {
        secondCamera.gameObject.SetActive(false);
        Collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        MovementsPlayer player = other.GetComponent<MovementsPlayer>();
        if (other.CompareTag("Player"))
        {
            if (mainCamera != null && secondCamera != null)
            {
                mainCamera.gameObject.SetActive(false);
                secondCamera.gameObject.SetActive(true);
                player.ToggleAxesInversion(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MovementsPlayer player = other.GetComponent<MovementsPlayer>();
            if (mainCamera != null && secondCamera != null)
            {
                mainCamera.gameObject.SetActive(true);
                secondCamera.gameObject.SetActive(false);
                player.ToggleAxesInversion(false);
            }
        }
    }
}
