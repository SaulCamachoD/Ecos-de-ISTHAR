using UnityEngine;

public class RestartModularScene : MonoBehaviour
{
    public ModularConfigScene restartScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Medea")
        {
            restartScene.RegenerateStages();
        }
    }
}
