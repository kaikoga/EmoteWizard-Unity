using System.Collections.Generic;
using JetBrains.Annotations;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Ndmf
{
    [UsedImplicitly]
    [ParameterProviderFor(typeof(ParameterSource))]
    public class ParameterSourceParameterProvider : IParameterProvider
    {
        readonly ParameterSource _source;

        public ParameterSourceParameterProvider(ParameterSource source)
        {
            _source = source;
        }

        public IEnumerable<ProvidedParameter> GetSuppliedParameters(BuildContext context = null)
        {
            yield return new ProvidedParameter(_source.parameterItem.name, ParameterNamespace.Animator, _source, EmoteWizardPlugin.Instance, null);
        }
    }

    public abstract class ExpressionItemSourceParameterProvider<T> : IParameterProvider
        where T : Component, IExpressionItemSource 
    {
        readonly T _source;

        protected ExpressionItemSourceParameterProvider(T source)
        {
            _source = source;
        }

        public IEnumerable<ProvidedParameter> GetSuppliedParameters(BuildContext context = null)
        {
            foreach (var e in _source.ToExpressionItems())
            {
                yield return new ProvidedParameter(e.parameter, ParameterNamespace.Animator, _source, EmoteWizardPlugin.Instance, null);
            }
        }
    }

    [UsedImplicitly]
    [ParameterProviderFor(typeof(ExpressionItemSource))]
    public class ExpressionItemSourceParameterProvider : ExpressionItemSourceParameterProvider<ExpressionItemSource>
    {
        public ExpressionItemSourceParameterProvider(ExpressionItemSource source) : base(source)
        {
        }
    }

    [UsedImplicitly]
    [ParameterProviderFor(typeof(EmoteItemSource))]
    public class EmoteItemSourceParameterProvider : ExpressionItemSourceParameterProvider<EmoteItemSource>
    {
        public EmoteItemSourceParameterProvider(EmoteItemSource source) : base(source)
        {
        }
    }

    [UsedImplicitly]
    [ParameterProviderFor(typeof(GenericEmoteItemSource))]
    public class GenericEmoteItemSourceParameterProvider : ExpressionItemSourceParameterProvider<GenericEmoteItemSource>
    {
        public GenericEmoteItemSourceParameterProvider(GenericEmoteItemSource source) : base(source)
        {
        }
    }
}