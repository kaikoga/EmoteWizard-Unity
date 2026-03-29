using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.UI
{
    public class ParametersSnapshotDrawer
    {
        readonly IPlatformFeatures _platformFeatures;
        readonly List<string> _validReferenceUsages;
        readonly List<ParameterInstanceView> _parameterItems;
        readonly List<ParameterInstanceView> _implicitParameterItems;
        readonly List<ParameterInstanceView> _defaultParameterItems;

        bool _expand;
        bool _expandValidReferenceUsagesList = true;
        bool _expandParameterItemsList = true;
        bool _expandImplicitParameterItemsList = true;
        bool _expandDefaultParameterItemsList = true;

        public ParametersSnapshotDrawer(IPlatformFeatures platformFeatures, ParametersSnapshot snapshot)
        {
            _platformFeatures = platformFeatures;
            _validReferenceUsages = snapshot.ValidReferenceUsages.ToList();
            _parameterItems = snapshot.ParameterItems.Select(item => new ParameterInstanceView(item)).ToList(); 
            _implicitParameterItems = snapshot.ImplicitParameterItems.Select(item => new ParameterInstanceView(item)).ToList(); 
            _defaultParameterItems = snapshot.DefaultParameterItems.Select(item => new ParameterInstanceView(item)).ToList(); 
        }

        [SuppressMessage("ReSharper", "AssignmentInConditionalExpression")]
        public void OnGUI()
        {
            using var _ = new LabelWidthScope(140);
            using var __ = new EditorGUI.DisabledScope(true);
            if (!(_expand = LEditorGUILayout.Foldout(_expand, Loc("ParametersConfig::debugSnapshot"))))
            {
                return;
            }
            using var ___ = new EditorGUI.IndentLevelScope();
            if (_expandValidReferenceUsagesList = EditorGUILayout.Foldout(_expandValidReferenceUsagesList, "Valid Reference Usages"))
            {
                foreach (var validReferenceUsage in _validReferenceUsages)
                {
                    DrawValidReferenceUsage(validReferenceUsage);
                }
            }
            if (_expandParameterItemsList = EditorGUILayout.Foldout(_expandParameterItemsList, "Parameters"))
            {
                foreach (var item in _parameterItems)
                {
                    DrawParameterInstance(item);
                }
            }
            if (_expandImplicitParameterItemsList = EditorGUILayout.Foldout(_expandImplicitParameterItemsList, "Implicit Parameters"))
            {
                foreach (var item in _implicitParameterItems)
                {
                    DrawParameterInstance(item);
                }
            }
            if (_expandDefaultParameterItemsList = EditorGUILayout.Foldout(_expandDefaultParameterItemsList, "Default Parameters"))
            {
                foreach (var item in _defaultParameterItems)
                {
                    DrawParameterInstance(item);
                }
            }
        }
        
        void DrawValidReferenceUsage(string value)
        {
            EditorGUILayout.TextField(value);
        }

        [SuppressMessage("ReSharper", "AssignmentInConditionalExpression")]
        void DrawParameterInstance(ParameterInstanceView view)
        {
            using var _ = new BoxLayoutScope();
            var value = view.Value;
            LEditorGUILayout.TextField(Loc("ParameterInstance::name"), value.name);
            using var __ = new EditorGUI.IndentLevelScope();
            LEditorGUILayout.EnumPopup(Loc("ParameterInstance::itemKind"), value.itemKind);
            LEditorGUILayout.Toggle(Loc("ParameterInstance::saved"), value.saved);
            ParameterValueField(LocEnum(value.defaultValue.ItemKind), value.defaultValue);
            LEditorGUILayout.Toggle(Loc("ParameterInstance::synced"), value.synced);
            if (view.ExpandUsages = LEditorGUILayout.Foldout(view.ExpandUsages, Loc("ParameterInstance::usages")))
            {
                LEditorGUILayout.LabelField(Loc("ParameterInstance::referenceUsages"), $"[{value.referenceUsages.Count}]");
                foreach (var referenceUsage in value.referenceUsages)
                {
                    DrawReferenceUsage(referenceUsage);
                }
                LEditorGUILayout.LabelField(Loc("ParameterInstance::writeUsages"), $"[{value.writeUsages.Count}]");
                foreach (var writeUsage in value.writeUsages)
                {
                    DrawWriteUsage(writeUsage);
                }
                LEditorGUILayout.LabelField(Loc("ParameterInstance::readUsages"), $"[{value.readUsages.Count}]");
                foreach (var readUsage in value.readUsages)
                {
                    DrawReadUsage(readUsage);
                }
            }
        }

        static void DrawReferenceUsage(string referenceUsage)
        {
            using var _ = new EditorGUI.IndentLevelScope();
            EditorGUILayout.TextField(referenceUsage);
        }

        void DrawWriteUsage(ParameterWriteUsage writeUsage)
        {
            using var _ = new EditorGUI.IndentLevelScope();
            using var __ = new GUILayout.HorizontalScope();
            ParameterValueField(LocEnum(writeUsage.writeUsageKind), writeUsage.Value);
            using var ___ = new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel);
            LEditorGUILayout.EnumPopup(LocEmpty(), writeUsage.writeSourceKind, GUILayout.Width(100));
        }

        void DrawReadUsage(ParameterReadUsage readUsage)
        {
            using var _ = new EditorGUI.IndentLevelScope();
            ParameterValueField(LocEnum(readUsage.Value.ItemKind), readUsage.Value);
        }

        void ParameterValueField(LocalizedContent loc, ParameterValue value)
        {
            switch (value.ItemKind)
            {
                case ParameterItemKind.HandSign:
                    LEditorGUILayout.EnumPopup(loc, value.AsHandSign());
                    break;
                default:
                    LEditorGUILayout.FloatField(loc, value.AsFloat(_platformFeatures));
                    break;
            }
        }

        class ParameterInstanceView
        {
            public readonly ParameterInstance Value;
            public bool ExpandUsages;

            public ParameterInstanceView(ParameterInstance value)
            {
                Value = value;
            }
        }
    }
}
