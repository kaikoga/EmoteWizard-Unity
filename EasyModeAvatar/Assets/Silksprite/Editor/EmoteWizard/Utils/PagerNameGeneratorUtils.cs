using Silksprite.EmoteWizard.DataObjects;
using UnityEditor;

namespace Silksprite.EmoteWizard.Utils
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
        
        public static string AsEmoteName(Emote property, int index)
        {
            var gesture1 = property.gesture1;
            var gesture2 = property.gesture2;
            return Emote.BuildStateName(gesture1.mode, gesture1.handSign, gesture2.mode, gesture2.handSign);
        }
    }
}