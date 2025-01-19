using UnityEngine;
using UnityEngine.UIElements;

public class RestartPlayer : MonoBehaviour
{
    private Transform currentposition;

    public GameObject Player;
    public GameObject Inicialposition;

    public void Start()
    {
        currentposition = Inicialposition.transform;
    }

    public void AsignementPosition(Transform transform)
    {
        currentposition.position = transform.position;
    }

    public void RestarInCurrentPosition()
    { 
        RestartInPointPlayer(currentposition);
    }

    public void RestartInPointPlayer(Transform transform)
    {
        Player.transform.position = transform.position;
    }
        
}
