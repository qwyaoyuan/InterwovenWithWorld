﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TTaskEditor.Data;

namespace TTaskEditor
{
    using FirstFloor.ModernUI.Windows.Controls;

    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Tasks.Instance.LoadTasks("Tasks.txt");
        }
    }
}
