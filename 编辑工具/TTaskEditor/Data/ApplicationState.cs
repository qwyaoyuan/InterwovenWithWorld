using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TTaskEditor.Data
{
    public static class ApplicationState
    {

        public static MouseState MouseState = MouseState.None;

        public static Control TransationFromElement;

        public static Control TransationToElement;
    }
    public enum MouseState
    {
        None,
        MakeTranstion,
    }
}
