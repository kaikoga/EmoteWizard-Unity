using System.IO;
using UnityEngine;

namespace EmoteWizard.Tools
{
    public static class EmoteWizardTools
    {
        public static string ProjectRoot => Path.GetDirectoryName(Application.dataPath);

        public static string AbsoluteToProjectPath(string absolutePath) => $"Assets{absolutePath.Substring(Application.dataPath.Length)}/";

        public static string ProjectToAbsolutePath(string projectPath) => projectPath == null ? Application.dataPath : Path.Combine(ProjectRoot, projectPath);
    }
}