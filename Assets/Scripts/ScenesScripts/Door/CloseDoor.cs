using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public OpenDoor openDoor;
    public ButtonDoor buttonDoor;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            openDoor.Close_Door();
            buttonDoor.isDoorOpen = false; // Cambiar estado de la puerta a cerrada
        }
    }
}
