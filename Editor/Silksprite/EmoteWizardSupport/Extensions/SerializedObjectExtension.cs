using Silksprite.EmoteWizardSupport.L10n;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class SerializedObjectExtension
    {
        public static LocalizedProperty Lop(this SerializedObject self, string propertyPath, LocalizedContent loc)
        {
            return new LocalizedProperty(self.FindProperty(propertyPath), loc);
        }

        public static LocalizedProperty Lop(this SerializedProperty self, string propertyPath, LocalizedContent loc)
        {
            return new LocalizedProperty(self.FindPropertyRelative(propertyPath), loc);
        }

        public static LocalizedProperty Lop(this LocalizedProperty self, string propertyPath, LocalizedContent loc)
        {
            return new LocalizedProperty(self.Property.FindPropertyRelative(propertyPath), loc);
        }

        public static LocalizedProperty GetArrayElementAtIndex(this LocalizedProperty self, int index, LocalizedContent loc)
        {
            return new LocalizedProperty(self.Property.GetArrayElementAtIndex(index), loc);
        }
    }
}