using UnityEngine;

public class RestartModularScene : MonoBehaviour
{
    public ModularConfigScene restartScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            restartScene.RegenerateStages();
            gameObject.SetActive(false);
        }
    }
}
