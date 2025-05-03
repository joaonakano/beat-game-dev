using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    [SerializeField]
    private float secondsToDestroy;

    void Start()
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
