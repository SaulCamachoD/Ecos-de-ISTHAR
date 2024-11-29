using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Vector3 openPositionOffset = new Vector3(3f, 0f, 0f); // Desplazamiento en X
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool isMoving = false;
    public Animator animator;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openPositionOffset;
    }

    void Update()
    {
        if (isMoving)
        {

            Vector3 targetPosition = isOpen ? openPosition : closedPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * openSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen; 
        isMoving = true; 
    }

 
    public void Open_Door()
    {
        if (!isOpen && !isMoving)
        {
            StartCoroutine(OpenDoorWithDelay(2f)); 
        }
    }

    public void Close_Door()
    {
        if (isOpen)
        {
            isOpen = false;
            isMoving = true;
        }
    }

    private IEnumerator OpenDoorWithDelay(float delay)
    {
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(delay); 
        isOpen = true;
        isMoving = true;
    }
}
