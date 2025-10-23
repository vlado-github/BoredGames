using UnityEngine;

public class CardHoverHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Color originalColor;
    private Renderer objectRenderer;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse entered!");
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.yellow; // Change color on hover
        }
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse exited!");
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // Revert color
        }
    }

    // OnMouseOver is called every frame while the mouse is over the collider
    // void OnMouseOver()
    // {
    //     Debug.Log("Mouse is over!");
    // }
}
