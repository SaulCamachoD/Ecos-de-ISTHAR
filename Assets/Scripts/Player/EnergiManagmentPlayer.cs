using UnityEngine;

public class EnergiManagmentPlayer : MonoBehaviour
{
    [SerializeField] private Transform energyCapsule; // C�psula de energ�a
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
        // Limitar la energ�a entre 0 y el m�ximo
        currentEnergy = Mathf.Clamp(energy, 0, maxEnergy);

        // Escala de la c�psula (eje Y)
        float scale = currentEnergy / maxEnergy;
        energyCapsule.localScale = new Vector3(1, scale, 1); // Ajusta el eje seg�n tu configuraci�n

        // Cambiar color seg�n el nivel de energ�a
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = Color.Lerp(Color.red, Color.green, currentEnergy / maxEnergy);
        capsuleRenderer.material.color = color;
    }
}
