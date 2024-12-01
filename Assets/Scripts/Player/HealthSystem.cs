using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class HealthSystem : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public Volume globalVolume;
    private UnityEngine.Rendering.Universal.Vignette vignette;
    public Slider healthBarSlider;
    public float healthRegenAmount = 5f;

    private bool isLowHealth;
    private Coroutine vignetteCoroutine; // Para almacenar la coroutine activa

    private void Awake()
    {
        playerSettings.health = playerSettings.healthMax;
        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }
    }

    private void Start()
    {
        StartCoroutine(HealthRegenRoutine());
    }

    private void Update()
    {
        if (playerSettings.health <= 20)
        {
            if (!isLowHealth)
            {
                isLowHealth = true;

                // Si ya hay una coroutine activa, detenerla antes de iniciar otra
                if (vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                }

                // Iniciar la coroutine para mantener el efecto de Vignette
                vignetteCoroutine = StartCoroutine(MaintainVignetteEffect());
            }
        }
        else
        {
            if (isLowHealth)
            {
                isLowHealth = false;

                // Detener la coroutine de mantener el efecto de Vignette
                if (vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                    vignetteCoroutine = null;
                }

                // Opcional: Restablecer la intensidad del Vignette
                //vignette.intensity.value = 0f;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        playerSettings.health -= damage;
        UpdateHeatlhLevel();
    }

    public void DecreaseHealth(float amount)
    {
        playerSettings.health = Mathf.Max(playerSettings.health - amount, 0f);
        UpdateHeatlhLevel();
        StartCoroutine(DamageVignetteEffect());
    }

    public void IncreaseHealth(float amount)
    {
        playerSettings.health = Mathf.Min(playerSettings.health + amount, playerSettings.healthMax);
        UpdateHeatlhLevel();
    }

    private void UpdateHeatlhLevel()
    {
        float HealthAmount = playerSettings.health / playerSettings.healthMax;
        healthBarSlider.value = HealthAmount;
    }

    
    private System.Collections.IEnumerator DamageVignetteEffect()
    {
        float duration = 1.5f; // Duraci�n del efecto
        float elapsed = 0f;

        // Aumenta la intensidad del Vignette
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            yield return null;
        }

        vignette.intensity.value = 0f; // Asegura que regrese a 0
    }

    private System.Collections.IEnumerator MaintainVignetteEffect()
    {
        
        while (isLowHealth)
        {
            vignette.intensity.value = 1f;
            yield return null; 
        }
    }

    private System.Collections.IEnumerator HealthRegenRoutine()
    {
        while (true)
        {
            if (playerSettings.health > 0 && playerSettings.health < playerSettings.healthMax)
            {
                IncreaseHealth(healthRegenAmount);
            }
            yield return new WaitForSeconds(1f); // Espera 1 segundo entre regeneraciones.
        }
    }
}
