using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;      // Recebe o tom da nota que deverá ser spawnada na lane. O DryWet.NoteName contém um "enum" (Datatype de Classe que agrupa várias constantes) que armazena os tons de nota possíveis: A, ASharp, B, C, CSharp, ..., GSharp
    public KeyCode input;                                   // Armazena a tecla que deve ser pressionada nessa lane
    public GameObject notePrefab;                           // Armazena o "prefab" (modelo) da nota a ser spawnada na Lane
    List<Note> notes = new List<Note>();                    // Cria uma lista vazia que aceitará apenas as instâncias, ou objetos, da classe Note
    public List<double> timeStamps = new List<double>();    // Cria uma lista vazia que aceitará apenas doubles com os Timestamps (um timestamp é o valor numérico em segundos em que a nota especifica é gerada)

    int spawnIndex = 0;                                     // Armazena o índice da nota a ser spawnada
    int inputIndex = 0;                                     // Armazena o índice da nota que o player está tentando acertar

    void Start()
    {
        
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // Recebe um array com as instancias das Notas
    {
        foreach (var note in array)                         // Para cada nota no array de Notas do parâmetro,
        {
            if (note.NoteName == noteRestriction)           // Se o nome da nota iterada for o mesmo que o tom de nota aceito pela Lane
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());                    // Converte o tempo da nota no MIDI para um sistema METRICO. Importante destacar que as notas lidas no MIDI têm o tempo medido em TICKS, se for para utiliza-los com segundos, é necessária a conversão. O TempoMap é 
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);    // Cálculo do Timestamp da nota em segundos: primeiro pegam-se os minutos e multiplica por 60 e, a este valor, acrescenta-se a quantidade de milissegundos dividida por mil, ou seja Timestamp = MINUTOS (em segundos) + SEGUNDOS + MILISSEGUNDOS (em segundos)
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count)                  // Enquanto o índice não percorrer toda a lista de notas,   
        {
            double timeStamp = timeStamps[spawnIndex];      // Coleta-se o timestamp da nota a ser gerada
            double marginOfError = SongManager.Instance.marginOfError;  // Coleta-se a margem de erro permitida em segundos
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);         // Armazena o tempo da música já ajustada ao delay do input nas notas. O cálculo é o seguinte: extrai-se a minutagem da música em segundos e subtrai-se o valor, também em segundos, do delay. Retornando então quanto tempo será necessário para processar o input após a nota ter chegado a área de TAP/TOQUE.

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)    // Verifica se o usuário apertou a tecla no tempo devido. O cálculo é o seguinte: [TEMPO ATUAL DA MÚSICA] - [TIMESTAMP DA NOTA] = [absoluta diferença, em segundos, entre a minutagem do playback e a da nota]. Se a distância em segundos for menor que a margem de erro, então foi um HIT
                {
                    Hit();
                    print($"Hit on {notes[inputIndex]} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;                           // O inputIndex é incrementado para localizar a próxima nota
                } else                                      // Se for maior que a margem de erro, então o usuário apertou muito cedo ou muito tarde para ser considerado um HIT
                {
                    print($"Hit inaccurate on {notes[inputIndex]} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)     // Código que valida se uma nota for perdida. Uma nota só é considerada perdida se o audio time for maior que o timestamp e a margem de erro juntos
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    private void Miss()
    {
        ScoreManager.Miss();
    }

    private void Hit()
    {
        ScoreManager.Hit();
    }
}
