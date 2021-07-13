using System;
using UnityEditor;

namespace Silksprite.EmoteWizardSupport.Scopes
{
    public class HideLabelsScope : IDisposable
    {
        readonly float _oldLabelWidth;

        public HideLabelsScope()
        {
            _oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 1f;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = _oldLabelWidth;
        }
    }
}