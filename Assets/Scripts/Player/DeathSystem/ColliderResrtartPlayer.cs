using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ColliderResrtartPlayer : MonoBehaviour
{   
    public RestartPlayer _restartPlayer;
    public GameObject _gameObject;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _restartPlayer.AsignementPosition(_gameObject.transform);
        }
    }
}
