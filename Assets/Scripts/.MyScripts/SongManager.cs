using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;     // Criamos uma variável estática (uma váriavel que é única e disponibilizada a todos os componentes que utilizarem essa classe) chamada Instance que armazena a instancia dessa classe
    public AudioSource audioSource;         // Variável que armazena o componente de musica
    public Lane[] lanes;                    // Variável que armazena uma lista de instancias da classe Lane. Lane vai ser utilizada para as pistas de nota
    public float songDelayInSeconds;        // Variável que armazena em float o delay que a música deve ter
    public double marginOfError;            // Variável que armazena em um double preciso um espaço de tempo para o usuario errar ao tentar pressionar uma nota

    public int inputDelayInMilliseconds;    // Variável que armazena o delay do teclado em milisegundos

    public string fileLocation;             // Variável que armazena o nome do arquivo MIDI
    public float noteTime;                  // Variável que armazena o tempo em segundos que a nota vai levar para sair da origem até o destino no eixo Z
    public float noteSpawnZ;                // Variável que armazena a localização no eixo Z da origem das notas
    public float noteTapZ;                  // Variável que armazena a localização no eixo Z da área cuja qual as notas poderão ser pressionadas
    public float noteDespawnZ               // Variável que armazena a localização no eixo Z da exclusão das notas
    {
        get
        {
            return noteTapZ - (noteSpawnZ - noteTapZ);  // Ao solicitar o valor da variável, obtemos a área de exclusão simétrica à área de origem das notas. Exemplo: |- - -X- - -|, EXCLUSÃO, TAPPING, ORIGEM, respectivamente
        }
    }

    public static MidiFile midiFile;        // Variável que armazena a Instância do arquivo MIDI de notas feitas no LMMS

    void Start()
    {
        Instance = this;                    // Padrão Singleton: certifica que a variável Instance receberá um objeto da classe SongManager
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))    // IF valida se o streamingAssetsPath começa com alguma URL, se sim, então é um sistema WEB
        {
            StartCoroutine(ReadFromWebsite());          // Inicia uma Co-Rotina de ler os dados do MIDI pelo browser. O que é co-rotina? É basicamente um método que executa uma ou mais ações a cada frame, ao invés de executar tudo em um mesmo frame
        }
        else
        {
            ReadFromFile();                 // Senão, possivelmente a instância do UNITY é local. Então, lê-se o arquivo MIDI localmente
        }
    }

    private IEnumerator ReadFromWebsite()   // Declara uma corrotina de carregamento de carregamento do MIDI da StreamingAssets
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath)) // O objeto www (requisição da pasta streamingAssets) é jogado fora após encerrar a execução da declaração using. Cria a requisição GET para a StreamingAssets
        {
            yield return www.SendWebRequest();          // Envia a requisição e aguarda a conclusão

            if (www.isNetworkError || www.isHttpError)  // Se houver falha na requisição,
            {
                Debug.LogError(www.error);              // Logar o erro
            }
            else
            {
                byte[] results = www.downloadHandler.data;      // Converte os dados recebidos em uma sequencia de bytes para serem enviados à memória
                using (var stream = new MemoryStream(results))  // Cria uma stream usando MemoryStream com a sequencia de bytes dos dados
                {
                    midiFile = MidiFile.Read(stream);           // Lê-se o arquivo MIDI da memoria
                    GetDataFromMidi();                          // Chama a função que pega os dados do MIDI lido
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); // Aqui, diferente do método WEB, não precisa passar uma stream com os bytes do MIDI, apenas a localização em disco do arquivo é suficiente (xxxxYYYY.mid)
        GetDataFromMidi();  // Chamada da função que extrai os dados do MIDI
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();    // Extraem-se as notas do MIDI (o resultado é uma coleção de notas como [C-12 => time, velocity, ...])
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];    // Cria-se um array que armazenará apenas Notes com o tamanho reservado da quantidade de notas do var notes
        notes.CopyTo(array, 0);             // Copia das notas do GetNotes() para o array, começando pelo indice 0

        foreach (var lane in lanes) lane.SetTimeStamps(array);  // Para cada lane no array de lanes, configurar o ??? usando a lista de Notas extraídas

        Invoke(nameof(StartSong), songDelayInSeconds);  // Executa a função StartSong a partir de um tempo definido na variável songDelayInSeconds. O método nameof() retorna uma string do nome da função informado no parametro
    }

    public void StartSong()
    {
        audioSource.Play(); // Toca a musica
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency; // Captura-se o sample atual na música em um double e divide pela frequencia (sample/s) em Hertz para obter o TEMPO em segundos do sample, exemplo: a música está em 6 segundos. Tempo (segundos) = Samples / SampleRate
    }
    
    void Update()
    {
        
    }
}
