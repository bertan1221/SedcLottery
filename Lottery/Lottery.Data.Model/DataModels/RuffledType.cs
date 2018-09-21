using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Data.Model.DataModels
{
    public enum RuffledType
    {
        [Description("Award that can be won once the code is send")]
        Immediate = 0,
        [Description("Award that can be won at the end of the day ruffled")]
        PerDay = 1,
        [Description("Award that can be won at the final ruffled")]
        Final = 2
    }
}
