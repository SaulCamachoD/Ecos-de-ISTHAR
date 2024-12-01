using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlayerSettings : MonoBehaviour
{       
    [Header ("Player Speed")]
    public float speed;
    public float speedSprint;
    public float wallRunSpeed;

    [Header ("Player Rotation")]
    public float rotationSpeed;

    [Header ("Player Jump Force")]
    public float jumpForce;
    public float jumpForwardForce;

    [Header ("Player dash Force")]
    public float dashDistance;
    public float dashDuration;
    public float dashForce;
    


    [Header("Skills and Health")]
    public float health;
    public float healthMax = 100f;
    public float energy = 100f;
    public float DamageBulltA;
    public float DamageBulltB;


    public float wallGravity;

   
}
