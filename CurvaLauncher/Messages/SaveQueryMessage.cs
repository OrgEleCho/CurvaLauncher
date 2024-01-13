using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Messages
{
    public class SaveQueryMessage
    {
        private SaveQueryMessage() { }

        public static readonly SaveQueryMessage Instance = new();
    }
}
