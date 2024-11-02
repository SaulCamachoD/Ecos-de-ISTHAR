using UnityEngine;
using UnityEngine.UIElements;

public class HudManager : MonoBehaviour
{
    public GameObject hudChild;

    private void Start()
    {
        hudChild.SetActive(false);
    }
    public void TargetVisibility(bool visibility)
    {
        hudChild.SetActive(visibility);  
    }
    
}
