using UnityEngine;

public class ButtonLaser : MonoBehaviour
{
    [SerializeField] private LaserController associatedLaser; 
    [SerializeField] private ButtonLaserGroup associatedGroup; 
    [SerializeField] private Material activatedMaterial; 
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isActivated)
        {
            isActivated = true; 
            
            if (activatedMaterial != null)
            {
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = activatedMaterial;
                }
            }
            associatedLaser?.DisableLaser();
            associatedGroup?.ButtonPressed();
        }
    }

}
