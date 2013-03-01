﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using MicroMvvm;

namespace WordCloud
{
    class ProjectViewModel : ObservableObject
    {
        #region Construction
        ///<summary>
        ///Constructs default instance of ProjectViewModel
        ///</summary>
        public ProjectViewModel()
        {

        }
        #endregion

        #region Members
        Project _project = new Project();
        public List<Element> elements = new List<Element>();
        #endregion

        #region Properties
        public Project Path
        {
            get { return _project; }
            set { _project = value; }
        }

        public string FullPath
        {
            get { return Path.FullPath; }
            set
            {
                if (Path.FullPath != value)
                {
                    Path.FullPath = value;
                    RaisePropertyChanged("FullPath");
                }
            }
        }

        public List<Element> Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
                RaisePropertyChanged("Elements");
                System.Diagnostics.Debug.WriteLine(" ");
                for (int i = 0; i < elements.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine(elements[i].Content + " " + elements[i].PosX + " " + elements[i].PosY + " " + elements[i].LineHeight + " " + elements[i].WordWidth);
                }

            }
        }

        #endregion

        #region Commands
        void GetPathExecute()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.ShowDialog();
            FullPath = dialog.SelectedPath;
        }
        bool CanGetPathExecute()
        {
            return true;
        }

        void StartWordCloudExecute()
        {
            Window win = Application.Current.MainWindow;
            Grid CanvasContainer = win.FindName("CanvasContainer") as Grid;
            Dummy t = new Dummy();
            Cloud c = new Cloud(Convert.ToInt32(CanvasContainer.ActualHeight), Convert.ToInt32(CanvasContainer.ActualWidth));
            c.CreateCloud(t);
            Elements = c.Holder;
        }
        bool CanStartWordCloudExecute()
        {
            return true;
        }

        void TextBlockClickExecute(object parameter)
        {
            TextBlock clickedItem = parameter as TextBlock;
            UIElement container = Application.Current.MainWindow.FindName("CanvasContainer") as UIElement;
            Point gt = clickedItem.TranslatePoint(new Point(0, 0), container);
            double Y = gt.Y;
            double X = gt.X;
            //System.Windows.Forms.MessageBox.Show(clickedItem.FontSize + " " + clickedItem.Text + " " + X.ToString() + "-" + (clickedItem.ActualWidth + X).ToString() + " , " + Y.ToString() + "-" + (Y + clickedItem.ActualWidth).ToString());
            //System.Windows.Forms.MessageBox.Show(clickedItem.Opacity.ToString());
            System.Windows.Forms.MessageBox.Show(clickedItem.FontSize.ToString());
        }
        bool CanTextBlockClickExecute()
        {
            return true;
        }

        public ICommand GetPath { get { return new RelayCommand(GetPathExecute, CanGetPathExecute); } }
        public ICommand TextBlockClick { get { return new RelayCommand<object>((param) => this.TextBlockClickExecute(param)); } }
        public ICommand StartWordCloud { get { return new RelayCommand(StartWordCloudExecute, CanStartWordCloudExecute); } }
        #endregion
    }
}
