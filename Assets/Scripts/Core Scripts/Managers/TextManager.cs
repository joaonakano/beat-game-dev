using UnityEngine;

public class TextManager : MonoBehaviour
{
    public GameObject tooEarlyTextPrefab;
    public GameObject missedSuperNoteTextPrefab;

    public static TextManager instance;

    private void Awake()
    {
        instance = this;
    }
}
