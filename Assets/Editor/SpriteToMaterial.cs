using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteToMaterial : EditorWindow
{
    Sprite sprite;
    string saveFolder = "Assets";

    [MenuItem("Tools/Slice To Material")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpriteToMaterial));
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite to Material Converter", EditorStyles.boldLabel);
        sprite = (Sprite)EditorGUILayout.ObjectField("Sprite Slice", sprite, typeof(Sprite), false);
        saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);

        if (GUILayout.Button("Convert and Save"))
        {
            if (sprite != null)
                CreateMaterialFromSprite(sprite);
            else
                Debug.LogError("No sprite selected.");
        }
    }

    void CreateMaterialFromSprite(Sprite sprite)
    {
        Texture2D sourceTex = sprite.texture;
        Rect rect = sprite.rect;

        // Cria uma nova textura com os pixels do slice
        Texture2D newTex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        newTex.filterMode = FilterMode.Point;

        Color[] pixels = sourceTex.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height
        );
        newTex.SetPixels(pixels);
        newTex.Apply();

        // Salvar a textura
        byte[] bytes = newTex.EncodeToPNG();
        string texPath = Path.Combine(saveFolder, sprite.name + "_tex.png");
        File.WriteAllBytes(texPath, bytes);
        AssetDatabase.Refresh();

        Texture2D savedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

        // Criar material
        Material mat = new Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = savedTex;

        string matPath = Path.Combine(saveFolder, sprite.name + "_mat.mat");
        AssetDatabase.CreateAsset(mat, matPath);

        Debug.Log($"Material saved to {matPath}");
    }
}
