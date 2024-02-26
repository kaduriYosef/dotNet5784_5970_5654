﻿using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.ManagerInterface
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManagerInterfaceMainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ManagerInterfaceMainWindow()
        {
            InitializeComponent();
        }
        private void Button_Init(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Would you like to create Initial data? (Y/N)", "Initialization data",
                                   MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (answer == MessageBoxResult.Yes) { s_bl.InitializeDB(); }
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Would you like to Reset all data? ", "Initialization data",
                                   MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (answer == MessageBoxResult.Yes) { s_bl.ResetDB(); }
        }

        private void Button_Engineers(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();

            //MessageBox.Show("it works");
        }

        private void Button_Auto_Schedule(object sender, RoutedEventArgs e)
        {
            new AutoScheduleWindow().ShowDialog();
        }

    }
}
