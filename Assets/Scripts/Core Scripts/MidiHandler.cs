using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using MidiNote = Melanchall.DryWetMidi.Interaction.Note;

public class NoteInfo
{
    public double time;
    public Lane lane;
    public bool isSpecial;

    public NoteInfo(double t, Lane l, bool s)
    {
        time = t;
        lane = l;
        isSpecial = s;
    }
}

public class MidiHandler : MonoBehaviour
{
    [Header("Lanes")]
    public List<Lane> lanes = new();
    
    [HideInInspector]
    public List<NoteInfo> noteDataList = new();

    public void GetDataFromMidiAndParseNotes(MidiFile file)
    {
        List<MidiNote> notes = file.GetNotes().ToList();
        LoadAndParseNotes(notes);
    }

    public void LoadAndParseNotes(List<MidiNote> array)
    {
        noteDataList = ParseNoteInfo(array, lanes);
        Debug.Log("Ended the timestamp/lane note addressment");
    }

    // Faz toda a correspondencia de NOTA, LANE e se é uma NOTA ESPECIAL
    public List<NoteInfo> ParseNoteInfo(List<MidiNote> array, List<Lane> lanes)
    {
        List<NoteInfo> parsedNotes = new();
        System.Random random = new();

        foreach (var note in array)
        {
            var noteTime = ConvertToMetricStamp(note.Time);
            if (noteTime < SongManager.Instance.noteTime) continue;

            foreach (var lane in lanes)
            {
                if (note.NoteName == lane.settings.noteRestriction)
                {
                    // Define se a nota será preta
                    bool isSpecial = random.NextDouble() < lane.settings.darkNoteChance;

                    parsedNotes.Add(new NoteInfo(noteTime, lane, isSpecial));
                    break;
                }
            }
        }
        return parsedNotes;
    }

    public int GetSongNoteCount()
    {
        return noteDataList.Count;
    }

    // Converte TICKS em SEGUNDOS de um parametro fornecido (Length, EndTime e Time)
    public double ConvertToMetricStamp(long paramToConvert)
    {
        var metricStamp = TimeConverter.ConvertTo<MetricTimeSpan>(
            paramToConvert,
            SongManager.Instance.midiFile.GetTempoMap()
        );

        return ConvertMetricTimeSpanToSeconds(metricStamp);
    }

    public double GetLastNoteTimestamp(MidiFile midiFile)
    {
        var metric = TimeConverter.ConvertTo<MetricTimeSpan>(
            midiFile.GetDuration(TimeSpanType.Metric),
            midiFile.GetTempoMap()
        );

        return System.Math.Round(ConvertMetricTimeSpanToSeconds(metric), 3);
    }

    private double ConvertMetricTimeSpanToSeconds(MetricTimeSpan metric)
    {
        return metric.Minutes * 60 + metric.Seconds + metric.Milliseconds / 1000.0;
    }
}
