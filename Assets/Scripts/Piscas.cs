using UnityEngine;

public class Piscas : MonoBehaviour
{
    public Light luz;
    public AudioSource som; // Referência ao AudioSource
    public int quantidadeDePiscadas = 3;
    public float intervaloEntrePiscadas = 0.2f;
    public float intervaloEntreCiclos = 5f;
    public float intensidadeMinima = 0.1f;

    private float intensidadeOriginal;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();

        if (som == null)
            som = GetComponent<AudioSource>();

        intensidadeOriginal = luz.intensity;
        StartCoroutine(CicloDePiscadas());
    }

    System.Collections.IEnumerator CicloDePiscadas()
    {
        while (true)
        {
            // Toca o som no início do ciclo
            if (som != null)
                som.Play();

            for (int i = 0; i < quantidadeDePiscadas; i++)
            {
                luz.intensity = intensidadeMinima;
                yield return new WaitForSeconds(intervaloEntrePiscadas);

                luz.intensity = intensidadeOriginal;
                yield return new WaitForSeconds(intervaloEntrePiscadas);
            }

            yield return new WaitForSeconds(intervaloEntreCiclos);
        }
    }
}
