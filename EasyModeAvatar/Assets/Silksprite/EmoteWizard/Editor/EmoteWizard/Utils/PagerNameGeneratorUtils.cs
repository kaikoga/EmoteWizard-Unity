using EmoteWizard.DataObjects;
using UnityEditor;

namespace EmoteWizard.Utils
{
    public static class PagerNameGeneratorUtils
    {
        public static string AsEmoteName(SerializedProperty property, int index)
        {
            var gesture1 = property.FindPropertyRelative("gesture1");
            var gesture2 = property.FindPropertyRelative("gesture2");
            return Emote.BuildStateName(
                (GestureConditionMode) gesture1.FindPropertyRelative("mode").intValue,
                (HandSign) gesture1.FindPropertyRelative("handSign").intValue,
                (GestureConditionMode) gesture2.FindPropertyRelative("mode").intValue,
                (HandSign) gesture2.FindPropertyRelative("handSign").intValue);
        }
    }
}