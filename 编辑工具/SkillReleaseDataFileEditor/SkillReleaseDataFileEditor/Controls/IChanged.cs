using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillDataFileEditor
{
    interface IChanged
    {
        bool IsChangedValue { get; set; }
    }
}
