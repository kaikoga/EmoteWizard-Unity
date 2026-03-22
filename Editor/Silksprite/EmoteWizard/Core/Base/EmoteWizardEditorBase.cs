using System.Diagnostics.CodeAnalysis;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using Silksprite.Loch.Tools;
using UnityEditor;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardEditorBase : Editor
    {
        static EmoteWizardEnvironment? _cachedEnvironment; 
        static bool _isDrawingInnerInspectorGUI;
        
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        EmoteWizardBehaviour soleTarget => (EmoteWizardBehaviour)target;

        protected EmoteWizardEnvironment CreateEnv()
        {
            if (_isDrawingInnerInspectorGUI) return _cachedEnvironment ??= soleTarget.CreateEnv();
            return soleTarget.CreateEnv();
        }

        LocalizedContent? _lastHeader;
        protected void HeaderOnce(LocalizedContent loc)
        {
            if (_lastHeader == loc) return;
            
            LGUILayout.Heading(loc);
            _lastHeader = loc;
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
                    _lastHeader = null;
                    OnInnerInspectorGUI();
                }
                else
                {
                    var substitution = new Substitution
                    {
                        ["platform"] = env.Platform.ToSolePlatformString()
                    };
                    LEditorGUILayout.HelpBox(LochTool.Loc("EWS::IgnoredByPlatform."), MessageType.Info, substitution);
                }

                if (!target || !env.ShowTutorial)
                {
                    return;
                }
                using (new BoxLayoutScope())
                {
                    LEditorGUILayout.HelpBox(TutorialContent, MessageType.Info);
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
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected T soleTarget => (T)target;

        protected sealed override LocalizedContent TutorialContent => LochTool._Loc(typeof(T).Name + "::Tutorial.");
    }
}