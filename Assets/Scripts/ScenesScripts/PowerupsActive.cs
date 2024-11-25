using Unity.VisualScripting;
using UnityEngine;

public class PowerupsActive : MonoBehaviour
{
    public GameObject ResetPoint;

    public void SetResetPoint(GameObject resetPoint)
    {
        ResetPoint = resetPoint;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            ResetPoint.SetActive(true);
            print("Se Activo el powerUp");
            Destroy(gameObject);
        }
    }
}
