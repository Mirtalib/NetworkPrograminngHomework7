﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            /// var enumType = Enum.GetNames(typeof());
            /// 
            /// foreach (var item in enumType)
            ///     cmboxCommandName.Items.Add(item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmboxCommandName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
