using System.IO;
using UnityEngine;

namespace Silksprite.EmoteWizard.Tools
{
    public static class EmoteWizardTools
    {
        public static string ProjectRoot => GetDirectoryName(Application.dataPath);

        public static string GetFileName(string path) => Path.GetFileName(path);

        public static string GetDirectoryName(string path) => Path.GetDirectoryName(path).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        public static string AbsoluteToProjectPath(string absolutePath) => $"Assets{absolutePath.Substring(Application.dataPath.Length)}/";

        public static string ProjectToAbsolutePath(string projectPath) => projectPath == null ? Application.dataPath : Path.Combine(ProjectRoot, projectPath);
    }
}