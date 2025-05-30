using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullscreenTestController : MonoBehaviour
{
    public static FullscreenTestController instance;

    [SerializeField] private ScriptableRendererFeature _crtEffect;

    private void Start()
    {
        instance = this;
        _crtEffect.SetActive(false);
    }

    private IEnumerator CRT(float displayTime)
    {
        _crtEffect.SetActive(true);
        Debug.Log("CRT is Active!");
        yield return new WaitForSeconds(displayTime);
        _crtEffect.SetActive(false);

        Debug.Log("CRT is Inactive!");
    }

    public static void SetCRT(float displayTime)
    {
        instance.StartCoroutine(instance.CRT(displayTime));
        Debug.Log("Trying to execute the CRT function");
    }
}
