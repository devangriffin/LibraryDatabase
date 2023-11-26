﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow AddBookWindow = new AddWindow(this);
            AddBookWindow.Show();

            IsEnabled = false;
        }

        private void AddPatronButton_Click(object sender, RoutedEventArgs e)
        {
            PatronWindow NewPatronWindow = new PatronWindow();
            NewPatronWindow.Show();
        }

        public void BookAdded() { IsEnabled = true; }

        
    }
}
