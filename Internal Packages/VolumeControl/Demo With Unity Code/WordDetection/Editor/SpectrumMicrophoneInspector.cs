using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpectrumMicrophone))]
public class SpectrumMicrophoneInspector : Editor
{
    [MenuItem("GameObject/Create Other/Audio/Create Spectrum Microphone")]
    public static void MenuCreateMic()
    {
        GameObject go = new GameObject("SpectrumMicrophone");
        go.AddComponent<SpectrumMicrophone>();
    }

    public override void OnInspectorGUI()
    {
        SpectrumMicrophone item = target as SpectrumMicrophone;

        int captureTime = item.captureTime;
        int sampleRate = item.sampleRate;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Capture Time:");
        item.captureTime = (int)GUILayout.HorizontalSlider(item.captureTime, 1, 16);
        item.captureTime = EditorGUILayout.IntField(item.captureTime);
        int log = (int)Mathf.Log(item.captureTime, 2);
        item.captureTime = (int)Mathf.Pow(2, log);
        item.captureTime = Mathf.Min(item.captureTime, 16);
        item.captureTime = Mathf.Max(item.captureTime, 1);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sample Rate:");
        item.sampleRate = (int)GUILayout.HorizontalSlider(item.sampleRate, 1024, 65536);
        item.sampleRate = EditorGUILayout.IntField(item.sampleRate);
        log = (int)Mathf.Log(item.sampleRate, 2);
        item.sampleRate = (int)Mathf.Pow(2, log);
        item.sampleRate = Mathf.Min(item.sampleRate, 65536);
        item.sampleRate = Mathf.Max(item.sampleRate, 1024);
        GUILayout.EndHorizontal();

        if (captureTime != item.captureTime ||
            sampleRate != item.sampleRate)
        {
            if (EditorApplication.isPlaying)
            {
                item.CleanUp();
                item.InitData();
            }
        }
    }
}