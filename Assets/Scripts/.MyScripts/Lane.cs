using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;      // Recebe o tom da nota que dever� ser spawnada na lane. O DryWet.NoteName cont�m um "enum" (Datatype de Classe que agrupa v�rias constantes) que armazena os tons de nota poss�veis: A, ASharp, B, C, CSharp, ..., GSharp
    public KeyCode input;                                   // Armazena a tecla que deve ser pressionada nessa lane
    public GameObject notePrefab;                           // Armazena o "prefab" (modelo) da nota a ser spawnada na Lane
    List<Note> notes = new List<Note>();                    // Cria uma lista vazia que aceitar� apenas as inst�ncias, ou objetos, da classe Note
    public List<double> timeStamps = new List<double>();    // Cria uma lista vazia que aceitar� apenas doubles com os Timestamps (um timestamp � o valor num�rico em segundos em que a nota especifica � gerada)

    int spawnIndex = 0;                                     // Armazena o �ndice da nota a ser spawnada
    int inputIndex = 0;                                     // Armazena o �ndice da nota que o player est� tentando acertar

    void Start()
    {
        
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)       // Recebe um array com as instancias das Notas
    {
        foreach (var note in array)                         // Para cada nota no array de Notas do par�metro,
        {
            if (note.NoteName == noteRestriction)           // Se o nome da nota iterada for o mesmo que o tom de nota aceito pela Lane
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());                    // Converte o tempo da nota no MIDI para um sistema METRICO. Importante destacar que as notas lidas no MIDI t�m o tempo medido em TICKS, se for para utiliza-los com segundos, � necess�ria a convers�o. O TempoMap � 
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);    // C�lculo do Timestamp da nota em segundos: primeiro pegam-se os minutos e multiplica por 60 e, a este valor, acrescenta-se a quantidade de milissegundos dividida por mil, ou seja Timestamp = MINUTOS (em segundos) + SEGUNDOS + MILISSEGUNDOS (em segundos)
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count)                  // Enquanto o �ndice n�o percorrer toda a lista de notas,   
        {
            double timeStamp = timeStamps[spawnIndex];      // Coleta-se o timestamp da nota a ser gerada
            double marginOfError = SongManager.Instance.marginOfError;  // Coleta-se a margem de erro permitida em segundos
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);         // Armazena o tempo da m�sica j� ajustada ao delay do input nas notas. O c�lculo � o seguinte: extrai-se a minutagem da m�sica em segundos e subtrai-se o valor, tamb�m em segundos, do delay. Retornando ent�o quanto tempo ser� necess�rio para processar o input ap�s a nota ter chegado a �rea de TAP/TOQUE.

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)    // Verifica se o usu�rio apertou a tecla no tempo devido. O c�lculo � o seguinte: [TEMPO ATUAL DA M�SICA] - [TIMESTAMP DA NOTA] = [absoluta diferen�a, em segundos, entre a minutagem do playback e a da nota]. Se a dist�ncia em segundos for menor que a margem de erro, ent�o foi um HIT
                {
                    Hit();
                    print($"Hit on {notes[inputIndex]} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;                           // O inputIndex � incrementado para localizar a pr�xima nota
                } else                                      // Se for maior que a margem de erro, ent�o o usu�rio apertou muito cedo ou muito tarde para ser considerado um HIT
                {
                    print($"Hit inaccurate on {notes[inputIndex]} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)     // C�digo que valida se uma nota for perdida. Uma nota s� � considerada perdida se o audio time for maior que o timestamp e a margem de erro juntos
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
