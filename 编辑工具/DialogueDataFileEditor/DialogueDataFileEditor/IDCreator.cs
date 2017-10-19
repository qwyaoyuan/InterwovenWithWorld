using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// ID创建器
    /// </summary>
    class IDCreator
    {
        private static IDCreator instance;
        public static IDCreator Instance
        {
            get
            {
                if (instance == null) instance = new IDCreator();
                return instance;
            }
        }
        private IDCreator() { }

        private int startID;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int startID)
        {
            this.startID = startID;
        }

        /// <summary>
        /// 获取一个id
        /// </summary>
        /// <returns></returns>
        public int GetNextID()
        {
            return startID++;
        }
    }
}
