using UnityEngine;

public class VfxImpact : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    public void OnTriggerEnter(Collider other)
    {

        particle.Play();
        
    }
}
