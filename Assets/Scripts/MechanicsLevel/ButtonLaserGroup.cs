using UnityEngine;

public class ButtonLaserGroup : MonoBehaviour
{
    [SerializeField] private LaserController[] lasersToDisable; // Láseres que se desactivarán
    [SerializeField] private ButtonLaser[] buttons;       // Botones del grupo
    private int activatedButtons = 0;

    public void ButtonPressed()
    {
        activatedButtons++;
        Debug.Log($"Botón activado en el grupo. Total activados: {activatedButtons}/{buttons.Length}");

        // Verifica si todos los botones fueron activados
        if (activatedButtons >= buttons.Length)
        {
            DisableLasers();
        }
    }

    private void DisableLasers()
    {
        Debug.Log("Todos los botones activados. Desactivando grupo de láseres.");

        foreach (LaserController laser in lasersToDisable)
        {
            laser?.DisableLaser();
        }
    }
}

