using UnityEngine;

public class EnergyBarController : MonoBehaviour
{
    public Material material;  // Material que usaremos para modificar la barra de energ�a.
    public float energyLevel = 0.5f;  // Nivel inicial de energ�a (puedes cambiarlo seg�n lo que necesites).

    public void Start()
    {
        float levelEnergyValue = material.GetFloat("_LevelEnergy");
        Debug.Log("LevelEnergy: " + levelEnergyValue);
    }
    void Update()
    {
        // Si el material no est� asignado, intentamos obtenerlo del Renderer del objeto
        if (material == null)
        {
            material = GetComponent<MeshRenderer>().sharedMaterial;  // Asignamos el material del Renderer si no se asign� previamente.
        }

        if (material != null)  // Aseguramos que el material se haya asignado correctamente.
        {
            // Aumentamos el nivel de energ�a con el tiempo (simulando la recarga o progreso de energ�a).
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
