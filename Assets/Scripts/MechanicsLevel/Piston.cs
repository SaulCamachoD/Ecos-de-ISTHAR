using System;
using UnityEngine;

public class Piston : MonoBehaviour
{
    [Header("Crusher Movement Settings")]
    public float speedUp = 2f; // Velocidad al subir
    public float speedDown = 5f; // Velocidad al bajar
    public float waitTimeAtTop = 1f; // Tiempo en la posición superior
    public float waitTimeAtBottom = 0.5f; // Tiempo en la posición inferior

    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool movingUp = false;
    private bool isWaiting = false;

    [Header("Movement Points")]
    public Transform topPoint; // Posición superior
    public Transform bottomPoint; // Posición inferior

    private void Start()
    {
        startPoint = bottomPoint.position;
        endPoint = topPoint.position;
        transform.position = startPoint;
    }

    private void Update()
    {
        if (isWaiting) return;

        if (movingUp)
        {
            MoveCrusher(endPoint, speedUp, waitTimeAtTop);
        }
        else
        {
            MoveCrusher(startPoint, speedDown, waitTimeAtBottom);
        }
    }

    private void MoveCrusher(Vector3 target, float speed, float waitTime)
    {
        // Movimiento del machacador hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Verificar si llegó al objetivo
        if (Vector3.Distance(transform.position, target) <= 0.01f)
        {
            StartCoroutine(WaitBeforeSwitchingDirection(waitTime));
        }
    }

    private System.Collections.IEnumerator WaitBeforeSwitchingDirection(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Cambiar dirección
        movingUp = !movingUp;
        isWaiting = false;
    }
}
