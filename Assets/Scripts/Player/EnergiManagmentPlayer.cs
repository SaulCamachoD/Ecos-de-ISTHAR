using UnityEngine;

public class EnergiManagmentPlayer : MonoBehaviour
{
    [SerializeField] private Transform energyCapsule; // Cápsula de energía
    [SerializeField] private float maxEnergy = 100f;
    private float currentEnergy;

    private Renderer capsuleRenderer;

    void Start()
    {
        currentEnergy = maxEnergy;
        capsuleRenderer = energyCapsule.GetComponent<Renderer>();
    }

    public void UpdateEnergy(float energy)
    {
        // Limitar la energía entre 0 y el máximo
        currentEnergy = Mathf.Clamp(energy, 0, maxEnergy);

        // Escala de la cápsula (eje Y)
        float scale = currentEnergy / maxEnergy;
        energyCapsule.localScale = new Vector3(1, scale, 1); // Ajusta el eje según tu configuración

        // Cambiar color según el nivel de energía
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = Color.Lerp(Color.red, Color.green, currentEnergy / maxEnergy);
        capsuleRenderer.material.color = color;
    }
}
