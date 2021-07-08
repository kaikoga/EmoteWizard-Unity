using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : Editor
    {
        EmoteWizardRoot emoteWizardRoot;

        void OnEnable()
        {
            emoteWizardRoot = target as EmoteWizardRoot;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            using (new GUILayout.HorizontalScope())
            {
                emoteWizardRoot.generatedAssetRoot =
                    EditorGUILayout.TextField("Generated Assets Root", emoteWizardRoot.generatedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                }
            }

            EditorGUILayout.PropertyField(serializedObj.FindProperty("generatedAssetPrefix"));
            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("emptyClip"), () => emoteWizardRoot.ProvideEmptyClip());

            EmoteWizardGUILayout.ConfigUIArea(() => { emoteWizardRoot.listDisplayMode = (ListDisplayMode)EditorGUILayout.EnumPopup("List Display", emoteWizardRoot.listDisplayMode); });

            if (!emoteWizardRoot.GetComponent<SetupWizard>())
            {
                if (GUILayout.Button("Setup"))
                {
                    emoteWizardRoot.EnsureComponent<SetupWizard>();
                }
            }

            serializedObj.ApplyModifiedProperties();
        }
    }
}