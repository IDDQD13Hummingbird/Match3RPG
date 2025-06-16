using UnityEditor;
using UnityEngine;

public class AutoChangeImageType : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // Exempt files in Assets/UI/Cursor
        // if (assetPath.Replace("\\", "/").StartsWith("Assets/UI"))
        // {
        //     return;
        // }

        TextureImporter textureImporter = (TextureImporter)assetImporter;

        // Set texture type to Sprite
        textureImporter.textureType = TextureImporterType.Sprite;

        // Set sprite mode to Single
        textureImporter.spriteImportMode = SpriteImportMode.Single;
    }
}