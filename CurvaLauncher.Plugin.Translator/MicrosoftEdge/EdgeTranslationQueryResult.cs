using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Translator.MicrosoftEdge
{
    public class EdgeTranslationQueryResult : AsyncQueryResult
    {
        public override string Title => "Translate text";

        public override string Description => "Translate specified text with 'MicrosoftEdge'";

        public override float Weight => throw new NotImplementedException();

        public override ImageSource? Icon => throw new NotImplementedException();

        public override Task InvokeAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
