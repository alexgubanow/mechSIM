using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mechLIB
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PhModels
    {
        [Description("linear Hook model")]
        hook,
        [Description("Hook model, nonlin geaometry")]
        hookGeomNon,
        [Description("Mooney-Rivlin model")]
        mooneyRiv,
        maxModel
    }
}
