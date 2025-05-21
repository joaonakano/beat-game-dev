using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RenderTesting : MonoBehaviour
{
    [SerializeField]
    private List<Renderer> renderers = new List<Renderer>();

    [SerializeField]
    private bool activeRenderers = true;

    [SerializeField]
    private KeyCode enableRenderKey;

    void Update()
    {
        if (Input.GetKeyDown(enableRenderKey))
        {
            if (activeRenderers)
            {
                activeRenderers = false;
            }
            else
            {
                activeRenderers= true;
            }

            SetRendererNote(activeRenderers);
        }
    }

    void SetRendererNote(bool desiredState)
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = desiredState;
            }
        }
    }

}
