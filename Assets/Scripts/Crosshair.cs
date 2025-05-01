using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private KeyCode input;

    private Renderer renderer;

    private float fadeLerpConstant = 8f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.1f);
    }

    void Update()
    {
        float newAlpha;
        if (Input.GetKey(input))
        {
            newAlpha = Mathf.Lerp(renderer.material.color.a, 1, fadeLerpConstant);
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, newAlpha);
        }
        else
        {
            newAlpha = Mathf.Lerp(renderer.material.color.a, .06f, Time.deltaTime * fadeLerpConstant);
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, newAlpha);
        }
    }
}
