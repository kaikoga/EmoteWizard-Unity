using System;
using UnityEngine;

namespace EmoteWizard.Scopes
{
    public class BoxLayoutScope : IDisposable
    {
        readonly IDisposable _parent;

        public BoxLayoutScope() : this(GUI.backgroundColor) { }

        public BoxLayoutScope(Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            _parent = new GUILayout.VerticalScope(GUI.skin.box);
            GUI.backgroundColor = backgroundColor;
        }

        public void Dispose()
        {
            _parent.Dispose();
        }
    }
}