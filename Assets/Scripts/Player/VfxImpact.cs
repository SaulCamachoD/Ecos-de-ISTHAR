using UnityEngine;

public class VfxImpact : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {

        particle.Play();
        
    }
}
