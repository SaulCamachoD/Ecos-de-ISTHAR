using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlashCore;

    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void PlayMuzzleFlash()
    {
        
        gameObject.SetActive(true);
        muzzleFlash.Play();
        muzzleFlashCore.Play();
        
    }

    public void StopMuzzleFlash()
    {
        
        muzzleFlash.Stop();
        muzzleFlashCore.Stop();
        gameObject.SetActive(false);
        
    }
}
