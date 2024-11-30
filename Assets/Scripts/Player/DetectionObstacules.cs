using UnityEngine;

public class DetectionObstacules : MonoBehaviour
{
    public MovementsPlayer movementsPlayer;
    public bool fwdDetection = false;
    public Rigidbody rb;
    private RaycastHit hit;
    void FixedUpdate()
    {
        
        // Realiza el SweepTest para detectar obstáculos.
        fwdDetection = rb.SweepTest(movementsPlayer.movementRelativeToCamera, out hit, movementsPlayer.currentSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (movementsPlayer != null && rb != null)
        {
            // Define el punto de inicio del raycast (posición del Rigidbody).
            Vector3 origin = rb.position;
            // Define la dirección y distancia del raycast.
            Vector3 direction = movementsPlayer.movementRelativeToCamera.normalized;
            float distance = movementsPlayer.currentSpeed * Time.deltaTime;

            // Dibuja el raycast en rojo si no hay colisión, verde si hay colisión.
            Gizmos.color = fwdDetection ? Color.green : Color.red;
            Gizmos.DrawRay(origin, direction * distance);

            // Si hubo colisión, dibuja una esfera en el punto de impacto.
            if (fwdDetection)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hit.point, 0.1f); // Tamaño ajustable para la esfera.
            }
        }
    }
}
