﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FirstThreadHomeWorkWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public MyThreadClass MyTC { get; set; }

        public bool CopyBtnEnabled
        {
            get { return (bool)GetValue(CopyBtnEnabledProperty); }
            set { SetValue(CopyBtnEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CopyBtnEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CopyBtnEnabledProperty =
            DependencyProperty.Register("CopyBtnEnabled", typeof(bool), typeof(MainWindow));



        public string FilePathOne
        {
            get { return (string)GetValue(FilePathOneProperty); }
            set { SetValue(FilePathOneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePathOne.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathOneProperty =
            DependencyProperty.Register("FilePathOne", typeof(string), typeof(MainWindow));



        public string FilePathTwo
        {
            get { return (string)GetValue(FilePathTwoProperty); }
            set { SetValue(FilePathTwoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePathTwo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathTwoProperty =
            DependencyProperty.Register("FilePathTwo", typeof(string), typeof(MainWindow));




        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CopyBtnEnabled = false;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName == FilePathTwo)
                    MessageBox.Show("Cannot Select Same Txt File");
                else
                {
                    FilePathOne = openFileDialog.FileName;
                }

            }

            if (!string.IsNullOrWhiteSpace(FilePathOne) && !string.IsNullOrWhiteSpace(FilePathTwo))
                CopyBtnEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                if (dlg.FileName == FilePathOne)
                    MessageBox.Show("Cannot Select Same Txt File");
                else
                    FilePathTwo = dlg.FileName;
            }
            if (!string.IsNullOrWhiteSpace(FilePathOne) && !string.IsNullOrWhiteSpace(FilePathTwo))
                CopyBtnEnabled = true;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MyTC = new MyThreadClass(FilePathOne, FilePathTwo);
            MyTC.MyEvent += MyTC_MyEvent;

            Thread thread = new Thread(new ThreadStart(MyTC.Do));
            thread.Start();

        }

        private void MyTC_MyEvent(long Max, int Progress)
        {

            if (Max == Progress)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    PB.Maximum = 100;
                    PB.Value = 0;
                    FilePathOne = "";
                    FilePathTwo = "";
                }));
                MyTC = null;
                MessageBox.Show("Progress Succesfull");
            }
            else
                this.Dispatcher.Invoke(new Action(() =>
                {
                    PB.Maximum = Max;
                    PB.Value = Progress;
                }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyTC is not null)
                MyTC.MyEvent -= MyTC_MyEvent;
        }
    }
}
