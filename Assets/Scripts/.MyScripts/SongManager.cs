using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Collections;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;     // Criamos uma vari�vel est�tica (uma v�riavel que � �nica e disponibilizada a todos os componentes que utilizarem essa classe) chamada Instance que armazena a instancia dessa classe
    public AudioSource audioSource;         // Vari�vel que armazena o componente de musica
    public Lane[] lanes;                    // Vari�vel que armazena uma lista de instancias da classe Lane. Lane vai ser utilizada para as pistas de nota
    public float songDelayInSeconds;        // Vari�vel que armazena em float o delay que a m�sica deve ter
    public double marginOfError;            // Vari�vel que armazena em um double preciso um espa�o de tempo para o usuario errar ao tentar pressionar uma nota

    public int inputDelayInMilliseconds;    // Vari�vel que armazena o delay do teclado em milisegundos

    public string fileLocation;             // Vari�vel que armazena o nome do arquivo MIDI
    public float noteTime;                  // Vari�vel que armazena o tempo em segundos que a nota vai levar para sair da origem at� o destino no eixo Z
    public float noteSpawnZ;                // Vari�vel que armazena a localiza��o no eixo Z da origem das notas
    public float noteTapZ;                  // Vari�vel que armazena a localiza��o no eixo Z da �rea cuja qual as notas poder�o ser pressionadas
    public float noteDespawnZ               // Vari�vel que armazena a localiza��o no eixo Z da exclus�o das notas
    {
        get
        {
            return noteTapZ - (noteSpawnZ - noteTapZ);  // Ao solicitar o valor da vari�vel, obtemos a �rea de exclus�o sim�trica � �rea de origem das notas. Exemplo: |- - -X- - -|, EXCLUS�O, TAPPING, ORIGEM, respectivamente
        }
    }

    public static MidiFile midiFile;        // Vari�vel que armazena a Inst�ncia do arquivo MIDI de notas feitas no LMMS

    void Start()
    {
        Instance = this;                    // Padr�o Singleton: certifica que a vari�vel Instance receber� um objeto da classe SongManager
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))    // IF valida se o streamingAssetsPath come�a com alguma URL, se sim, ent�o � um sistema WEB
        {
            StartCoroutine(ReadFromWebsite());          // Inicia uma Co-Rotina de ler os dados do MIDI pelo browser. O que � co-rotina? � basicamente um m�todo que executa uma ou mais a��es a cada frame, ao inv�s de executar tudo em um mesmo frame
        }
        else
        {
            ReadFromFile();                 // Sen�o, possivelmente a inst�ncia do UNITY � local. Ent�o, l�-se o arquivo MIDI localmente
        }
    }

    private IEnumerator ReadFromWebsite()   // Declara uma corrotina de carregamento de carregamento do MIDI da StreamingAssets
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath)) // O objeto www (requisi��o da pasta streamingAssets) � jogado fora ap�s encerrar a execu��o da declara��o using. Cria a requisi��o GET para a StreamingAssets
        {
            yield return www.SendWebRequest();          // Envia a requisi��o e aguarda a conclus�o

            if (www.isNetworkError || www.isHttpError)  // Se houver falha na requisi��o,
            {
                Debug.LogError(www.error);              // Logar o erro
            }
            else
            {
                byte[] results = www.downloadHandler.data;      // Converte os dados recebidos em uma sequencia de bytes para serem enviados � mem�ria
                using (var stream = new MemoryStream(results))  // Cria uma stream usando MemoryStream com a sequencia de bytes dos dados
                {
                    midiFile = MidiFile.Read(stream);           // L�-se o arquivo MIDI da memoria
                    GetDataFromMidi();                          // Chama a fun��o que pega os dados do MIDI lido
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); // Aqui, diferente do m�todo WEB, n�o precisa passar uma stream com os bytes do MIDI, apenas a localiza��o em disco do arquivo � suficiente (xxxxYYYY.mid)
        GetDataFromMidi();  // Chamada da fun��o que extrai os dados do MIDI
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();    // Extraem-se as notas do MIDI (o resultado � uma cole��o de notas como [C-12 => time, velocity, ...])
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];    // Cria-se um array que armazenar� apenas Notes com o tamanho reservado da quantidade de notas do var notes
        notes.CopyTo(array, 0);             // Copia das notas do GetNotes() para o array, come�ando pelo indice 0

        foreach (var lane in lanes) lane.SetTimeStamps(array);  // Para cada lane no array de lanes, configurar o ??? usando a lista de Notas extra�das

        Invoke(nameof(StartSong), songDelayInSeconds);  // Executa a fun��o StartSong a partir de um tempo definido na vari�vel songDelayInSeconds. O m�todo nameof() retorna uma string do nome da fun��o informado no parametro
    }

    public void StartSong()
    {
        audioSource.Play(); // Toca a musica
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency; // Captura-se o sample atual na m�sica em um double e divide pela frequencia (sample/s) em Hertz para obter o TEMPO em segundos do sample, exemplo: a m�sica est� em 6 segundos. Tempo (segundos) = Samples / SampleRate
    }
    
    void Update()
    {
        
    }
}
