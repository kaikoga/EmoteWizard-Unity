using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Internal.ClipBuilders;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class SourceExploder
    {
        static string GetExplodePath(EmoteSequenceSourceBase source)
        {
            return string.IsNullOrWhiteSpace(source.gameObject.name) ? "Assets/EW_GeneratedClip.anim" : $"Assets/EW_GeneratedClip_{source.gameObject.name}.anim";
        }

        public static void Explode(IUndoable undoable, EmoteWizardDataSourceBase container)
        {
            var environment = container.CreateEnv();
            var exploded = false;

            if (container is IExpressionItemSource expressionItemSource)
            {
                ExplodeExpressionItems(undoable, environment, expressionItemSource, container);
                exploded = true;
            }
            if (container is IParameterSource parameterSource)
            {
                ExplodeParameters(undoable, parameterSource, container);
                exploded = true;
            }
            if (container is EmoteSequenceSourceBase emoteSequenceSource)
            {
                ExplodeEmoteSequences(undoable, environment, emoteSequenceSource, container);
            }

            if (!exploded) return;

            EditorApplication.delayCall += () => Object.DestroyImmediate(container); 
        }

        public static void ExplodeEmoteSequences(IUndoable undoable, EmoteWizardDataSourceBase container)
        {
            var environment = container.CreateEnv();
            var exploded = false;
            
            if (container is EmoteSequenceSourceBase emoteSequenceSource)
            {
                ExplodeEmoteSequences(undoable, environment, emoteSequenceSource, container);
                exploded = true;
            }

            if (!exploded) return;

            EditorApplication.delayCall += () => Object.DestroyImmediate(container); 
        }

        static void ExplodeParameters(IUndoable undoable, IParameterSource source, Component destination)
        {
            foreach (var parameterItem in source.ParameterItems)
            {
                var child = undoable.FindOrCreateChildComponent<ParameterSource>(destination, parameterItem.name, parameterItem.enabled);
                child.parameterItem = SerializableUtils.Clone(parameterItem);
                child.parameterItem.enabled = true;
            }
        }

        static void ExplodeExpressionItems(IUndoable undoable, EmoteWizardEnvironment environment, IExpressionItemSource source, Component destination)
        {
            foreach (var expressionItem in source.ToExpressionItems())
            {
                var child = undoable.FindOrCreateChildComponent<ExpressionItemSource>(destination, expressionItem.path, expressionItem.enabled);
                child.expressionItem = SerializableUtils.Clone(expressionItem);
                child.expressionItem.enabled = true;
            }
        }

        static void ExplodeEmoteSequences(IUndoable undoable, EmoteWizardEnvironment environment, EmoteSequenceSourceBase source, Component destination)
        {
            var sequence = source.ToEmoteFactoryTemplate().Build(environment, new ClipBuilderImpl(GetExplodePath(source)));
            var gameObject = destination.gameObject;
            undoable.DestroyObject(source);
            
            var child = undoable.AddComponent<EmoteSequenceSource>(gameObject);
            child.sequence = sequence;
        }

        static T FindOrCreateChildComponent<T>(this IUndoable undoable, Component component, string path, bool enabled)
            where T : Component
        {
            var result = undoable.FindOrCreateChildComponent<T>(component, path);
            result.gameObject.SetActive(enabled);
            return result;
        }
    }
}