using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private string inputActionName;

    private Renderer crosshairRenderer;
    private float fadeLerpConstant = 8f;

    private InputAction inputAction;

    [SerializeField] private PlayerInput playerInput;

    void Start()
    {
        crosshairRenderer = GetComponent<Renderer>();
        crosshairRenderer.material.color = new Color(crosshairRenderer.material.color.r, crosshairRenderer.material.color.g, crosshairRenderer.material.color.b, 0.1f);

        if (playerInput != null)
        {
            inputAction = playerInput.actions[inputActionName];
        }
    }

    void Update()
    {
        if (inputAction == null)
            return;

        float newAlpha;
        if (inputAction.IsPressed())
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
