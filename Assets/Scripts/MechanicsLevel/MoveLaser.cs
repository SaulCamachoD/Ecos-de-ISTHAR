using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    [SerializeField] private Transform pointA; 
    [SerializeField] private Transform pointB; 
    [SerializeField] private float speed = 2f;
    

    private Vector3 _target; 

    private void Start()
    {
        
        _target = pointB.position;
    }

    private void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _target = _target == pointA.position ? pointB.position : pointA.position;
        }
    }
}

