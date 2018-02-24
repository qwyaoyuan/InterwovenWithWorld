using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AINew
{
    /// <summary>
    /// AI的接口
    /// </summary>
    public interface IAIInterface
    {
        void Init(IAIState aiState);

        void Update();
    }
}
