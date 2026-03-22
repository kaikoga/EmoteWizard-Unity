using System.IO;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Utils;

namespace Silksprite.EmoteWizard.Templates
{
    public readonly struct EmoteTemplatePath
    {
        public readonly string AbsolutePath;
        public readonly string RelativePath;

        public string FileName => Path.GetFileName(RelativePath);

        EmoteTemplatePath(string absolutePath, string relativePath)
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
        }

        public static EmoteTemplatePath Context(EmoteWizardEnvironment environment, EmoteWizardBehaviour context)
        {
            return new EmoteTemplatePath(RuntimeUtil.RelativePath(environment.AvatarRoot, context.transform)!, context.transform.name);
        }

        public EmoteTemplatePath Join(string relativePath)
        {
            return new EmoteTemplatePath(Path.Join(AbsolutePath, relativePath), relativePath);
        }

    }
}