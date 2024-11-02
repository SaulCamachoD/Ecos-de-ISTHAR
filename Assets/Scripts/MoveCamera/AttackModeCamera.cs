using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class AttackModeCamera : MonoBehaviour
{
    private Vector3 inicialPosition;
    private Vector3 FinalPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    public float mx = 1.3f;
    public float mz = 0;
    public float speed = 2.0f;


    private void Start()
    {
        inicialPosition = transform.localPosition;
        currentPosition = transform.localPosition;
        FinalPosition = new Vector3(transform.localPosition.x + mx, transform.localPosition.y, transform.localPosition.z + mz);
        targetPosition = inicialPosition;
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void MoveTarget()
    {
        if (targetPosition == inicialPosition)
        {
            targetPosition = FinalPosition;
        }
        else
        {
            targetPosition = inicialPosition;
        }
    }
}
