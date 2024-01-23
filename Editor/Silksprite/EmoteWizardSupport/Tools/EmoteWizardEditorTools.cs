using System.IO;
using Silksprite.EmoteWizardSupport.L10n;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardTools;

namespace Silksprite.EmoteWizardSupport.Tools
{
    public static class EmoteWizardEditorTools
    {
        public static bool SelectFolder(LocalizedContent loc, SerializedProperty property)
        {
            var absolutePath = EditorUtility.OpenFolderPanel(loc.Tr, ProjectToAbsolutePath(property.stringValue), "");
            if (absolutePath == null) return false;
            if (!absolutePath.StartsWith(ProjectRoot)) return false;
            property.stringValue = AbsoluteToProjectPath(absolutePath);
            return true;
        }

        public static void EnsureDirectory(string path)
        {
            Directory.CreateDirectory(GetDirectoryName(path));
        }
    }
}