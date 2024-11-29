using UnityEngine;

public class EnergyBarController : MonoBehaviour
{
    public Material material;  // Material que usaremos para modificar la barra de energía.
    public float energyLevel = 0.5f;  // Nivel inicial de energía (puedes cambiarlo según lo que necesites).

    public void Start()
    {
        float levelEnergyValue = material.GetFloat("_LevelEnergy");
        Debug.Log("LevelEnergy: " + levelEnergyValue);
    }
    void Update()
    {
        // Si el material no está asignado, intentamos obtenerlo del Renderer del objeto
        if (material == null)
        {
            material = GetComponent<MeshRenderer>().sharedMaterial;  // Asignamos el material del Renderer si no se asignó previamente.
        }

        if (material != null)  // Aseguramos que el material se haya asignado correctamente.
        {
            // Aumentamos el nivel de energía con el tiempo (simulando la recarga o progreso de energía).
            energyLevel += Time.deltaTime * 0.1f;  // Puedes ajustar el multiplicador para controlar la velocidad.
            energyLevel = Mathf.Clamp01(energyLevel);  // Limita el valor entre 0 y 1.

            // Actualizamos el valor de la propiedad _Level_Energy en el shader.
            material.SetFloat("_Level_Energy", energyLevel);
        }
        else
        {
            Debug.LogWarning("Material no asignado en EnergyBarController.");
        }
    }
}
