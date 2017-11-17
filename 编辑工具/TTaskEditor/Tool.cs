using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTaskEditor
{
    public static class Tool
    {

        public static bool IsInt(string s)
        {
            int num;
            return int.TryParse(s, out num);
        }

        public static bool IsFloat(string s)
        {
            float num;
            return float.TryParse(s, out num);
        }


    }
}
