using System;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Scopes
{
    public class InvalidValueScope : IDisposable
    {
        readonly Color _oldColor;

        public InvalidValueScope(bool invalid) : this(invalid, new Color(1f, 0.5f, 0.5f)) { }

        public InvalidValueScope(bool invalid, Color color)
        {
            _oldColor = GUI.color;
            if (!invalid) return;
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = _oldColor;
        }
    }
}