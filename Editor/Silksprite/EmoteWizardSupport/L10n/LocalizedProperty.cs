using System;
using Silksprite.EmoteWizardSupport.L10n;
using UnityEditor;
using UnityEngine;

namespace Silksprite.Loch
{
    public readonly struct LocalizedProperty : IEquatable<LocalizedProperty>
    {
        public readonly SerializedProperty Property;
        public readonly LocalizedContent Loc;

        public GUIContent GUIContent => Loc.GUIContent;

        public LocalizedProperty(SerializedProperty property, LocalizedContent loc)
        {
            Property = property;
            Loc = loc;
        }

        public bool Equals(LocalizedProperty other)
        {
            return Equals(Property, other.Property) && Loc.Equals(other.Loc);
        }

        public override bool Equals(object obj)
        {
            return obj is LocalizedProperty other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Property, Loc);
        }

        public static bool operator ==(LocalizedProperty left, LocalizedProperty right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LocalizedProperty left, LocalizedProperty right)
        {
            return !left.Equals(right);
        }
    }
}