using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private KeyCode input;

    private Renderer crosshairRenderer;

    private float fadeLerpConstant = 8f;

    void Start()
    {
        crosshairRenderer = GetComponent<Renderer>();
        crosshairRenderer.material.color = new Color(crosshairRenderer.material.color.r, crosshairRenderer.material.color.g, crosshairRenderer.material.color.b, 0.1f);
    }

    void Update()
    {
        float newAlpha;
        if (Input.GetKey(input))
        {
            newAlpha = Mathf.Lerp(crosshairRenderer.material.color.a, 1, fadeLerpConstant);
            crosshairRenderer.material.color = new Color(crosshairRenderer.material.color.r, crosshairRenderer.material.color.g, crosshairRenderer.material.color.b, newAlpha);
        }
        else
        {
            newAlpha = Mathf.Lerp(crosshairRenderer.material.color.a, .06f, Time.deltaTime * fadeLerpConstant);
            crosshairRenderer.material.color = new Color(crosshairRenderer.material.color.r, crosshairRenderer.material.color.g, crosshairRenderer.material.color.b, newAlpha);
        }
    }
}
