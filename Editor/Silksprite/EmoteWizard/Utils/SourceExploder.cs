using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Internal.ClipBuilders;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class SourceExploder
    {
        public static void Explode(EmoteWizardDataSourceBase container)
        {
            var environment = container.CreateEnv();
            var exploded = false;

            if (container is IExpressionItemSource expressionItemSource)
            {
                ExplodeExpressionItems(environment, expressionItemSource, container);
                exploded = true;
            }
            if (container is IParameterSource parameterSource)
            {
                ExplodeParameters(parameterSource, container);
                exploded = true;
            }
            if (container is EmoteSequenceSourceBase emoteSequenceSource)
            {
                ExplodeEmoteSequences(environment, emoteSequenceSource, container);
            }

            if (!exploded) return;

            EditorApplication.delayCall += () => Object.DestroyImmediate(container); 
        }

        public static void ExplodeEmoteSequences(EmoteWizardDataSourceBase container)
        {
            var environment = container.CreateEnv();
            var exploded = false;
            
            if (container is EmoteSequenceSourceBase emoteSequenceSource)
            {
                ExplodeEmoteSequences(environment, emoteSequenceSource, container);
                exploded = true;
            }

            if (!exploded) return;

            EditorApplication.delayCall += () => Object.DestroyImmediate(container); 
        }

        static void ExplodeParameters(IParameterSource source, Component destination)
        {
            foreach (var parameterItem in source.ParameterItems)
            {
                var child = destination.FindOrCreateChildComponent<ParameterSource>(parameterItem.name, parameterItem.enabled);
                child.parameterItem = SerializableUtils.Clone(parameterItem);
                child.parameterItem.enabled = true;
            }
        }

        static void ExplodeExpressionItems(EmoteWizardEnvironment environment, IExpressionItemSource source, Component destination)
        {
            foreach (var expressionItem in source.ToExpressionItems())
            {
                var child = destination.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path, expressionItem.enabled);
                child.expressionItem = SerializableUtils.Clone(expressionItem);
                child.expressionItem.enabled = true;
            }
        }

        static void ExplodeEmoteSequences(EmoteWizardEnvironment environment, EmoteSequenceSourceBase source, Component destination)
        {
            var sequence = source.ToEmoteFactory().Build(new ClipBuilderImpl(environment));
            var gameObject = destination.gameObject;
            Object.DestroyImmediate(source);
            
            var child = gameObject.AddComponent<EmoteSequenceSource>();
            child.sequence = sequence;
        }

        static T FindOrCreateChildComponent<T>(this Component component, string path, bool enabled)
            where T : Component
        {
            var result = component.FindOrCreateChildComponent<T>(path);
            result.gameObject.SetActive(enabled);
            return result;
        }
    }
}