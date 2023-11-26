using Silksprite.EmoteWizard.UI;
using UnityEditor;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(AvatarWizard))]
    public class AvatarWizardEditor : Editor 
    {
        AvatarWizard _wizard;

        void OnEnable()
        {
            _wizard = (AvatarWizard)target;
        }

        public override void OnInspectorGUI()
        {
            var env = _wizard.CreateEnv();

            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }
        
        static string Tutorial => 
            string.Join("\n",
                "VRCAvatarDescriptorのセットアップを行います。",
                "Animatorコンポーネントが存在する場合、それを利用してアバターのアニメーションを編集できます。");
    }
}