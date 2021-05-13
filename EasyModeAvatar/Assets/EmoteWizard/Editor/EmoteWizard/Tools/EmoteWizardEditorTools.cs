using UnityEditor;
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
    }
}