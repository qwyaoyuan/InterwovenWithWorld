using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    /// <summary>
    /// 包含类型的文本输入框
    /// </summary>
    public partial class TypeTextBox : UserControl, ITextValue, ITypeTag,IChanged
    {
        public TypeTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文字值
        /// </summary>
        private string textValue = "";
        /// <summary>
        /// 文字值
        /// </summary>
        public string TextValue
        {
            get
            {
                return textValue;
            }
            set
            {
                TextBox_Main.Text = value;
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        private Type structType;
        /// <summary>
        /// 值类型
        /// </summary>
        private string typeTag;
        /// <summary>
        /// 值类型
        /// </summary>
        public string TypeTag
        {
            get { return typeTag; }
            set
            {
                typeTag = value;
                try
                {
                    structType = Type.GetType(typeTag);
                }
                catch { }
                if (!Check())
                {
                    TextBox_Main.Text = "";
                }
            }
        }

        /// <summary>
        /// 是否改变了值
        /// </summary>
        private bool isChangedValue;
        /// <summary>
        /// 是否改变了值
        /// </summary>
        public bool IsChangedValue { get => isChangedValue; set => isChangedValue = value; }

        /// <summary>
        /// 检查输入的文字是否合法
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            if (string.IsNullOrEmpty(TextBox_Main.Text) || structType == null)
                return true;
            try
            {
                if (Type.Equals(structType, typeof(string)))
                    return true;
                Convert.ChangeType(TextBox_Main.Text, structType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文本框文字发生变化
        /// </summary>
        public event EventHandler<TypeTextBoxEventArgs> TypeTextChanged;

        /// <summary>
        /// 输入文字发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Main_TextChanged(object sender, EventArgs e)
        {
            if (Check())
            {
                textValue = TextBox_Main.Text;
                isChangedValue = true;
                if (TypeTextChanged != null)
                    TypeTextChanged(this, new TypeTextBoxEventArgs() { text = TextValue });
            }
            else
            {
                TextBox_Main.Text = textValue;
            }
        }

        
    }

    /// <summary>
    /// Type文本框文字发生变化
    /// </summary>
    public class TypeTextBoxEventArgs : EventArgs
    {
        public string text;
    }
}
