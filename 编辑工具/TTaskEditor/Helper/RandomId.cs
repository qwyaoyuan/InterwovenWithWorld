using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTaskEditor.Helper
{
    public static class RandomId
    {
        public static int currentId = 1;

        public static int GetRandomId()
        {
            return currentId++;
        }
    }
}
