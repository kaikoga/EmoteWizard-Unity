using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class SourceExploder
    {
        public static void ExplodeAll(GameObject gameObject)
        {
            foreach (var container in gameObject.GetComponents<EmoteWizardDataSourceBase>())
            {
                Explode(container);
            }
        }

        public static void Explode(EmoteWizardDataSourceBase container)
        {
            var exploded = false;

            if (container is IGestureEmoteSource gestureEmoteSource)
            {
                ExplodeEmotes<GestureEmoteSource>(gestureEmoteSource, container);
                exploded = true;
            }
            if (container is IFxEmoteSource fxEmoteSource)
            {
                ExplodeEmotes<FxEmoteSource>(fxEmoteSource, container);
                exploded = true;
            }
            if (container is IGestureParameterEmoteSource gestureParameterEmoteSource)
            {
                ExplodeParameterEmotes<GestureParameterEmoteSource>(gestureParameterEmoteSource, container);
                exploded = true;
            }
            if (container is IFxParameterEmoteSource fxParameterEmoteSource)
            {
                ExplodeParameterEmotes<FxParameterEmoteSource>(fxParameterEmoteSource, container);
                exploded = true;
            }
            if (container is IGestureAnimationMixinSource gestureAnimationMixinSource)
            {
                ExplodeAnimationMixins<GestureAnimationMixinSource>(gestureAnimationMixinSource, container);
                exploded = true;
            }
            if (container is IFxAnimationMixinSource fxAnimationMixinSource)
            {
                ExplodeAnimationMixins<FxAnimationMixinSource>(fxAnimationMixinSource, container);
                exploded = true;
            }
            if (container is IExpressionItemSource expressionItemSource)
            {
                ExplodeExpressionItems(expressionItemSource, container);
                exploded = true;
            }
            if (container is IParameterSource parameterSource)
            {
                ExplodeParameters(parameterSource, container);
                exploded = true;
            }
            if (container is IActionEmoteSource actionEmoteSource)
            {
                ExplodeActionEmotes(actionEmoteSource, container);
                exploded = true;
            }
            if (container is IAfkEmoteSource afkEmoteSource)
            {
                ExplodeAfkEmotes(afkEmoteSource, container);
                exploded = true;
            }

            if (!exploded) return;

            EditorApplication.delayCall += () => Object.DestroyImmediate(container); 
        }

        static void ExplodeEmotes<TOut>(IEmoteSourceBase source, Component destination)
            where TOut : EmoteSourceBase
        {
            foreach (var emote in source.Emotes)
            {
                var child = destination.FindOrCreateChildComponent<TOut>(emote.ToStateName());
                child.emote = SerializableUtils.Clone(emote);
                child.advancedAnimations = source.AdvancedAnimations;
            }
        }

        static void ExplodeParameterEmotes<TOut>(IParameterEmoteSourceBase source, Component destination)
            where TOut : ParameterEmoteSourceBase
        {
            foreach (var parameterEmote in source.ParameterEmotes)
            {
                var child = destination.FindOrCreateChildComponent<TOut>(parameterEmote.name, parameterEmote.enabled);
                child.parameterEmote = SerializableUtils.Clone(parameterEmote);
                child.parameterEmote.enabled = true;
            }
        }

        static void ExplodeAnimationMixins<TOut>(IAnimationMixinSourceBase source, Component destination)
            where TOut : AnimationMixinSourceBase
        {
            foreach (var baseMixin in source.Mixins)
            {
                var child = destination.FindOrCreateChildComponent<TOut>(baseMixin.name, baseMixin.enabled);
                child.mixin = SerializableUtils.Clone(baseMixin);
                child.mixin.enabled = true;
                child.isBaseMixin = true;
            }
            foreach (var mixin in source.Mixins)
            {
                var child = destination.FindOrCreateChildComponent<TOut>(mixin.name, mixin.enabled);
                child.mixin = SerializableUtils.Clone(mixin);
                child.mixin.enabled = true;
                child.isBaseMixin = false;
            }
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

        static void ExplodeExpressionItems(IExpressionItemSource source, Component destination)
        {
            foreach (var expressionItem in source.ExpressionItems)
            {
                var child = destination.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path, expressionItem.enabled);
                child.expressionItem = SerializableUtils.Clone(expressionItem);
                child.expressionItem.enabled = true;
            }
        }

        static void ExplodeActionEmotes(IActionEmoteSource source, Component destination)
        {
            foreach (var actionEmote in source.ActionEmotes)
            {
                var child = destination.FindOrCreateChildComponent<ActionEmoteSource>(actionEmote.name, actionEmote.enabled);
                child.actionEmote = SerializableUtils.Clone(actionEmote);
                child.actionEmote.enabled = true;
            }
        }

        static void ExplodeAfkEmotes(IAfkEmoteSource source, Component destination)
        {
            foreach (var afkEmote in source.AfkEmotes)
            {
                var child = destination.FindOrCreateChildComponent<AfkEmoteSource>(afkEmote.name, afkEmote.enabled);
                child.afkEmote = SerializableUtils.Clone(afkEmote);
                child.afkEmote.enabled = true;
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