using UnityEngine;

public class SounPlayerManager : MonoBehaviour
{
    public AudioClip Walk;
    public AudioClip Run;
    public AudioClip Dash;
    public AudioClip Damage;
    public AudioClip StepWall;
    public AudioClip Gun1;
    public AudioClip Gun2;


    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    // Función para reproducir un clip de audio
    public void PlaySound(string soundType)
    {
        switch (soundType)
        {
            case "Walk":
                audioSource1.clip = Walk;
                audioSource1.loop = true;  
                audioSource1.pitch = 0.8f;
                break;
            case "Run":
                audioSource1.clip = Run;
                audioSource1.loop = true;  
                audioSource1.pitch = 1.2f;
                break;
            default:
                return;
        }
        audioSource1.Play();
    }
    public void PlaySound2(string soundType)
    {
        switch (soundType)
        {
            case "Dash":
                audioSource2.clip = Dash;
                audioSource2.loop = false;  
                audioSource2.pitch = 1f;
                break;
            case "Damage":
                audioSource2.clip = Damage;
                audioSource2.loop = false;  
                audioSource2.pitch = 1f;
                break;
            default:
                return;
        }
        audioSource2.Play();
    }
    public void PlaySound3(string soundType)
    {
        switch (soundType)
        {
            case "StepWall":
                audioSource3.clip = StepWall;
                audioSource3.loop = true;
                audioSource3.pitch = 1.2f;
                break;
            case "Gun1":
                audioSource3.clip = Gun1;
                audioSource3.loop = false;  
                audioSource3.pitch = 1f;
                break;
            case "Gun2":
                audioSource3.clip = Gun2;
                audioSource3.loop = false;  
                audioSource3.pitch = 1f;
                break;
            default:
                return;
        }
        audioSource3.Play();
    }

    
    public void StopSound()
    {
        audioSource1.Stop();
    }
    public void StopSound2()
    {
        audioSource2.Stop();
    }
    public void StopSound3()
    {
        audioSource3.Stop();
    }
}
