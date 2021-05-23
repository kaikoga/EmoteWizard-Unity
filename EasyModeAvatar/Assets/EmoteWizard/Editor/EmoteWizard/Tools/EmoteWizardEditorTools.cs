using System.IO;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

using static EmoteWizard.Tools.EmoteWizardTools;

namespace EmoteWizard.Tools
{
    public static class EmoteWizardEditorTools
    {
        public static bool SelectFolder(string title, ref string projectPath)
        {
            var absolutePath = EditorUtility.OpenFolderPanel(title, ProjectToAbsolutePath(projectPath), "");
            if (absolutePath == null) return false;
            if (!absolutePath.StartsWith(ProjectRoot)) return false;
            projectPath = AbsoluteToProjectPath(absolutePath);
            return true;
        }

        public static void EnsureDirectory(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}