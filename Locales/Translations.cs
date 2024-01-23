using JetBrains.Annotations;

namespace Silksprite.EmoteWizard
{
    internal static class Translations
    {
        static void Tr(string _) { }

        [UsedImplicitly]
        static void Tr()
        {
            Tr("EditorLayerConfig::Tutorial.");
            Tr("AnimatorLayerConfigBase::Tutorial.");
            Tr("ExpressionConfig::Tutorial.");
            Tr("ParametersConfig::Tutorial.");
            Tr("EmoteItemSource::Tutorial.");
            Tr("EmoteSequenceSource::Tutorial.");
            Tr("ExpressionItemSource::Tutorial.");
            Tr("GenericEmoteSequenceSource::Tutorial.");
            Tr("ParameterSource::Tutorial.");
            Tr("CustomActionWizard::Tutorial.");
            Tr("DefaultSourcesWizard::Tutorial.");
            Tr("DressChangeWizard::Tutorial.");
            Tr("EmoteItemWizard::Tutorial.");
            Tr("EmoteWizardDataSourceFactory::Tutorial.");
            Tr("EmoteWizardRoot::Tutorial.");
        }
    }
}