using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace USTL.ReadmeViewer.Editor
{
    [InitializeOnLoad]
    public static class Migrator
    {
        private const string OLD_SCRIPT = "guid: b5c24c463bd2b3f43a2945b9e2ba6c3b";
        private const string NEW_SCRIPT = "guid: d0b4b82bb5de9460da301aae886a719c";
        private const string MIGRATION_SESSION_KEY = "USTL.ReadmeViewer.Migrator.HasPerformedStartupMigration";

        static Migrator()
        {
            AssetDatabase.importPackageCompleted += _ => PerformMigrate();
            EditorApplication.delayCall += PerformMigrateOncePerSession;
        }

        private static void PerformMigrateOncePerSession()
        {
            if (SessionState.GetBool(MIGRATION_SESSION_KEY, false))
            {
                return;
            }

            SessionState.SetBool(MIGRATION_SESSION_KEY, true);
            PerformMigrate();
        }

        private static void PerformMigrate()
        {
            foreach (string path in AssetDatabase.GetAllAssetPaths())
            {
                if (AssetDatabase.GetMainAssetTypeAtPath(path) != null)
                {
                    continue;
                }

                PerformMigrateByPath(path);
            }

            foreach (string guid in AssetDatabase.FindAssets("t:ReadmeAsset"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(ReadmeAsset))
                {
                    continue;
                }

                PerformMigrateByPath(path);
            }
        }

        private static void PerformMigrateByPath(string path)
        {
            if (!TryGetMigrationFilePath(path, out string filePath))
            {
                return;
            }

            string content;
            try
            {
                content = File.ReadAllText(filePath);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to read ReadmeAsset migration target at '{path}': {exception.Message}");
                return;
            }

            if (content.Contains(OLD_SCRIPT))
            {
                string newContent = content.Replace(OLD_SCRIPT, NEW_SCRIPT);

                try
                {
                    File.WriteAllText(filePath, newContent);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning($"Failed to write ReadmeAsset migration target at '{path}': {exception.Message}");
                    return;
                }

                AssetDatabase.ImportAsset(path);
            }
        }

        private static bool TryGetMigrationFilePath(string path, out string filePath)
        {
            filePath = null;

            if (string.IsNullOrEmpty(path) || !string.Equals(Path.GetExtension(path), ".asset", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (File.Exists(path))
            {
                filePath = path;
                return true;
            }

            const string assetsPrefix = "Assets/";
            if (!path.StartsWith(assetsPrefix, StringComparison.Ordinal))
            {
                return false;
            }

            string assetsRelativePath = path[assetsPrefix.Length..];
            string assetsFilePath = Path.Combine(Application.dataPath, assetsRelativePath);
            if (!File.Exists(assetsFilePath))
            {
                return false;
            }

            filePath = assetsFilePath;
            return true;
        }
    }
}
