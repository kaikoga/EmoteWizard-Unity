using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.UI
{
    public static class EmoteWizardGUILayout
    {
        static readonly Color SetupUIColor = new Color(1.0f, 0.1f, 0.6f);
        static readonly Color ConfigUIColor = new Color(0.80f, 0.80f, 0.82f);
        static readonly Color OutputUIColor = new Color(0.3f, 1.0f, 0.9f);

        public static void SetupOnlyUI(EmoteWizardBehaviour emoteWizardBehaviour, Action action)
        {
            if (!emoteWizardBehaviour.IsSetupMode) return;

            using (new BoxLayoutScope(SetupUIColor))
            {
                GUILayout.Label("Setup only zone");
                action();
            }
        }
        

        public static void ConfigUIArea(Action action)
        {
            using (new BoxLayoutScope(ConfigUIColor))
            {
                action();
            }
        }

        public static void OutputUIArea(Action action)
        {
            using (new BoxLayoutScope(OutputUIColor))
            {
                GUILayout.Label("Output zone");
                action();
            }
        }

        public static void RequireAnotherContext<TContext, TComponent>(EmoteWizardBase emoteWizardBase, Action action)
            where TContext : IBehaviourContext
            where TComponent : EmoteWizardBase
        {
            RequireAnotherContext<TContext, TComponent>(emoteWizardBase, emoteWizardBase.Environment.GetContext<TContext>(), action);
        }

        public static void RequireAnotherContext<TContext, TComponent>(EmoteWizardBase emoteWizardBase, TContext anotherContext, Action action)
            where TContext : IBehaviourContext
            where TComponent : EmoteWizardBase
        {
            if (anotherContext != null)
            {
                action();
                return;
            }

            var typeName = typeof(TComponent).Name;
            EditorGUILayout.HelpBox($"{typeName} not found. Some functions might not work.", MessageType.Error);
            using (new BoxLayoutScope(Color.magenta))
            {
                if (GUILayout.Button($"Add {typeName}"))
                {
                    emoteWizardBase.gameObject.AddComponent<TComponent>();
                }
            }
        }

        public static void Tutorial(IEmoteWizardEnvironment environment, Action action)
        {
            if (!environment.ShowTutorial) return;
            using (new BoxLayoutScope()) action();
        }
        
        public static void Tutorial(IEmoteWizardEnvironment environment, string message)
        {
            Tutorial(environment, () => EditorGUILayout.HelpBox(message.Nowrap(), MessageType.Info));
        }
    }
}