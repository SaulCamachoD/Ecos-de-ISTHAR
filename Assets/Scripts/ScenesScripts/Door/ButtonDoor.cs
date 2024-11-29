using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public OpenDoor openDoor; 
    public Renderer buttonRenderer; 
    public Material buttonClosedMaterial; 
    public Material buttonOpenMaterial; 
    public float colorTransitionSpeed = 5f; 

    private float colorLerpValue = 0f; 
    public bool isDoorOpen = false; 

    public Renderer[] objectsToChangeColor; 
    public Material[] objectClosedMaterials; 
    public Material[] objectOpenMaterials; 
    public Material[] objectDefaultMaterials; 

    public Material buttonDefaultMaterial; 

    public bool isEnabled = false; 

    private void Update()
    {
        if (isEnabled)
        {
            colorLerpValue = Mathf.Lerp(colorLerpValue, isDoorOpen ? 1f : 0f, Time.deltaTime * colorTransitionSpeed);
            buttonRenderer.material.Lerp(buttonClosedMaterial, buttonOpenMaterial, colorLerpValue);

            for (int i = 0; i < objectsToChangeColor.Length; i++)
            {
                objectsToChangeColor[i].material.Lerp(objectClosedMaterials[i], objectOpenMaterials[i], colorLerpValue);
            }
        }
        else
        {
            for (int i = 0; i < objectsToChangeColor.Length; i++)
            {
                objectsToChangeColor[i].material = objectDefaultMaterials[i];
            }

            buttonRenderer.material = buttonDefaultMaterial;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && isEnabled)
        {
            openDoor.Open_Door();
            isDoorOpen = true; 
        }
    }

    // Función para activar o desactivar la puerta
    public void ToggleEnabled(bool value)
    {
        isEnabled = value;
    }

}
