using System.IO;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Tools
{
    public static class EmoteWizardTools
    {
        static string ProjectRoot => Path.GetDirectoryName(Application.dataPath);

        static string AbsoluteToProjectPath(string absolutePath) => $"Assets{absolutePath.Substring(Application.dataPath.Length)}/";

        static string ProjectToAbsolutePath(string projectPath) => projectPath == null ? Application.dataPath : Path.Combine(ProjectRoot, projectPath);

        public static bool SelectFolder(string title, ref string projectPath)
        {
            var absolutePath = EditorUtility.OpenFolderPanel(title, ProjectToAbsolutePath(projectPath), "");
            if (absolutePath == null) return false;
            if (!absolutePath.StartsWith(ProjectRoot)) return false;
            projectPath = AbsoluteToProjectPath(absolutePath);
            return true;
        }
    }
}