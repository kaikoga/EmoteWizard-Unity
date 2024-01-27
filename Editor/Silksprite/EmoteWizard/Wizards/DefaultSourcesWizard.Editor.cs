using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(DefaultSourcesWizard))]
    public class DefaultSourcesWizardEditor : EmoteWizardEditorBase<DefaultSourcesWizard>
    {
        LocalizedProperty _layerKind;

        void OnEnable()
        {
            _layerKind = Lop(nameof(DefaultSourcesWizard.layerKind), Loc("DefaultSourcesWizard::layerKind"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_layerKind);

            serializedObject.ApplyModifiedProperties();

            string undoLabel = $"Add Default {soleTarget.layerKind} Items";
            if (EmoteWizardGUILayout.Undoable(Loc("DefaultSourcesWizard::Add"), undoLabel) is IUndoable undoable)
            {
                soleTarget.Explode(undoable, true);
            }
        }
    }
}