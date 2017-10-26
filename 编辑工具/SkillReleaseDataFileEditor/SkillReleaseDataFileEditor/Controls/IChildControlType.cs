using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillDataFileEditor
{
    interface IChildControlType
    {
        string ChildControlType { get; set; }

        string[] TextValues { get; set; }
    }
}
