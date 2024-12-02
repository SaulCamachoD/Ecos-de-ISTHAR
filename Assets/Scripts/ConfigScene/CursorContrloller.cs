using System.Security.Cryptography;
using UnityEngine;

public class CursorContrloller : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
