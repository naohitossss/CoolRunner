#if UNITY_EDITOR
using UnityEditor;

public class MeshImportSettings : EditorWindow
{
    [MenuItem("Tools/Enable Read/Write on All Meshes")]
    static void EnableReadWriteOnAllMeshes()
    {
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssetPaths)
        {
            if (assetPath.EndsWith(".fbx") || assetPath.EndsWith(".obj"))
            {
                ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (importer != null && !importer.isReadable)
                {
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                   
                }
            }
        }
    }
}
#endif
