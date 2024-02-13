using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardEditorBase : Editor
    {
        static EmoteWizardEnvironment _cachedEnvironment; 
        static bool _isDrawingInnerInspectorGUI;
        
        EmoteWizardBehaviour soleTarget => target as EmoteWizardBehaviour;

        protected EmoteWizardEnvironment CreateEnv()
        {
            if (_isDrawingInnerInspectorGUI) return _cachedEnvironment = _cachedEnvironment ?? soleTarget.CreateEnv();
            return soleTarget.CreateEnv();
        }
        
        public sealed override void OnInspectorGUI()
        {
            _isDrawingInnerInspectorGUI = true;
            _cachedEnvironment = null;
            var hierarchyMode = EditorGUIUtility.hierarchyMode; 
            EditorGUIUtility.hierarchyMode = false; // false because we use Headers to group things
            try
            {
                var env = CreateEnv();
                if ((env.Platform & SupportedPlatforms) != 0)
                {
                    OnInnerInspectorGUI();
                }
                else
                {
                    EmoteWizardGUILayout.HelpBox(LocalizationTool.Loc("EWS::IgnoredByPlatform."), MessageType.Info,
                        new Substitution
                        {
                            ["platform"] = env.Platform.ToSolePlatformString()
                        });
                }

                if (!target) return;
                if (env?.ShowTutorial != true) return;
                var tutorial = TutorialContent;
                if (tutorial.IsNullOrEmpty()) return;
                using (new BoxLayoutScope())
                {
                    EmoteWizardGUILayout.HelpBox(TutorialContent, MessageType.Info);
                }
            }
            finally
            {
                _isDrawingInnerInspectorGUI = false;
                _cachedEnvironment = null;
                EditorGUIUtility.hierarchyMode = hierarchyMode;
            }
        }

        protected virtual DetectedPlatform SupportedPlatforms => DetectedPlatform.Mixed;

        protected abstract void OnInnerInspectorGUI();

        protected abstract LocalizedContent TutorialContent { get; }

        protected LocalizedProperty Lop(string propertyPath, LocalizedContent loc) => serializedObject.Lop(propertyPath, loc);
    }

    public abstract class EmoteWizardEditorBase<T> : EmoteWizardEditorBase
    where T : EmoteWizardBehaviour
    {
        protected T soleTarget => target as T;

        protected sealed override LocalizedContent TutorialContent => LocalizationTool._Loc(typeof(T).Name + "::Tutorial.");
    }
}