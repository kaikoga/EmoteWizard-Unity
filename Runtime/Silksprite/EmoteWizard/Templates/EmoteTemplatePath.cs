using System.IO;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Utils;

namespace Silksprite.EmoteWizard.Templates
{
    public readonly struct EmoteTemplatePath
    {
        public readonly string PathFromAvatarRoot;

        public string FileName => Path.GetFileName(PathFromAvatarRoot);

        EmoteTemplatePath(string pathFromAvatarRoot)
        {
            PathFromAvatarRoot = pathFromAvatarRoot;
        }

        public static EmoteTemplatePath Context(EmoteWizardEnvironment environment, EmoteWizardBehaviour context)
        {
            var relativePath = Path.GetDirectoryName(RuntimeUtil.RelativePath(environment.AvatarRoot, context.transform))!;
            return new EmoteTemplatePath(relativePath);
        }

        public EmoteTemplatePath Join(string relativePath)
        {
            return new EmoteTemplatePath(Path.Join(PathFromAvatarRoot, relativePath));
        }

    }
}