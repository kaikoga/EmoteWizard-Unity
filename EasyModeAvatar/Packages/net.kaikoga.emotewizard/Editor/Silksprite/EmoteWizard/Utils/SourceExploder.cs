using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class SourceExploder
    {
        public static void Explode(EmoteWizardEnvironment environment, EmoteWizardDataSourceBase container)
        {
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
            foreach (var expressionItem in source.ToExpressionItems(environment.GetContext<ExpressionContext>()))
            {
                var child = destination.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path, expressionItem.enabled);
                child.expressionItem = SerializableUtils.Clone(expressionItem);
                child.expressionItem.enabled = true;
            }
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