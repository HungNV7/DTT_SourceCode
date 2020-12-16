using ServerDTT_New_.SupportClass;
using System;
using System.Collections.Generic;
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

namespace ServerDTT_New_.ExtendedWindow
{
    /// <summary>
    /// Interaction logic for EWSubQuestions.xaml
    /// </summary>
    public partial class EWSubQuestions : UserControl
    {
        MediaAct mediaAct = new MediaAct();
        public List<TextBlock> txtBlockStudentNameList = new List<TextBlock>();
        public List<TextBlock> txtBackGroundNameList = new List<TextBlock>();
        Thread thread;
        User_Control.UCStart uCStart;
        double time = 0;
        int numberOfStudent = 2;
        public EWSubQuestions(int numberOfStudent)
        {
            ImageBrush imageBrush = new ImageBrush();
            mediaAct.Upload(imageBrush, "Obstacles_ImageQuestionBox.png");
            this.Background = imageBrush;
            this.numberOfStudent = numberOfStudent;
            InitializeComponent();
            InitEW();
        }


        public void UpdateUC(User_Control.UCStart ucStart)
        {
            uCStart = ucStart;
        }

        void InitEW()
        {
            //tận dụng video của mấy vòng trước
            mediaAct.Upload(videoQuestionStart, "Obstacles_VideoTiming.mp4");
            mediaAct.Upload(soundFalse, "Start_SoundFalse.wav");
            mediaAct.Upload(soundTrue, "Start_SoundTrue.wav");
            mediaAct.Upload(soundFinish, "Obstacles_TrueKeySound.mp3");
            mediaAct.Upload(soundBell, "Obstacles_BellSound.mp3");

            //chia grid name
            switch (numberOfStudent)
            {
                case 2:
                    for (int i = 0; i < 4; i++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                        gridStudentName.ColumnDefinitions.Add(columnDefinition);
                    }
                    break;
                case 3:
                    ColumnDefinition firstDef = new ColumnDefinition { Width = new GridLength(80, GridUnitType.Star) };
                    gridStudentName.ColumnDefinitions.Add(firstDef);
                    for (int i = 0; i < numberOfStudent; i++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(160, GridUnitType.Star) };
                        gridStudentName.ColumnDefinitions.Add(columnDefinition);
                    }
                    ColumnDefinition lastDef = new ColumnDefinition { Width = new GridLength(160, GridUnitType.Star) };
                    gridStudentName.ColumnDefinitions.Add(lastDef);
                    break;
                case 4:
                    for (int i = 0; i < numberOfStudent; i++)
                    {
                        ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                        gridStudentName.ColumnDefinitions.Add(columnDefinition);
                    }
                    break;
            }

            var bc = new BrushConverter();
            for (int i = 0; i < numberOfStudent; i++)
            { 
                TextBlock txtBackGroundName = new TextBlock();
                txtBackGroundName.Background = (Brush)bc.ConvertFrom("#2a2728");
                txtBackGroundName.SetValue(Grid.ColumnProperty, i + 1);
                txtBackGroundNameList.Add(txtBackGroundName);
                gridStudentName.Children.Add(txtBackGroundName);

                TextBlock txtBlockStudentName = new TextBlock { Text = "Thi Sinh" + (i + 1).ToString(), FontWeight = FontWeights.DemiBold, FontFamily = new FontFamily("Open Sans"), Foreground = Brushes.White, FontSize = 35, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                txtBlockStudentName.SetValue(Grid.ColumnProperty, i + 1);
                gridStudentName.Children.Add(txtBlockStudentName);
    
                txtBlockStudentNameList.Add(txtBlockStudentName);

                Border border = new Border { BorderThickness = new Thickness(5) };
                border.BorderBrush = (Brush)bc.ConvertFrom("#f9f6f7");
                border.SetValue(Grid.ColumnProperty, i + 1);
                gridStudentName.Children.Add(border);
            }

            HideAll();
            gridStudentContest.Visibility = Visibility.Visible;
        }

        //void ThreadStart()
        //{
        //    DateTime start = DateTime.Now;
        //    DateTime end = DateTime.Now;
        //    int x = (end.Hour * 3600 + end.Minute * 60 + end.Second) - (start.Second + start.Minute * 60 + start.Hour * 3600);
        //    while (x < 4)
        //    {
        //        end = DateTime.Now;
        //        x = (end.Hour * 3600 + end.Minute * 60 + end.Second) - (start.Second + start.Minute * 60 + start.Hour * 3600);
        //    }
        //    this.Dispatcher.Invoke(() =>
        //    {
        //        gridStudentName.Visibility = Visibility.Visible;
        //        txtBlockQuestion.Visibility = Visibility.Visible;
        //        uCStart.ShowNextQuestion(0);
        //    });
        //}

        public void HideAll()
        {
            mediaAct.Stop(soundFalse);
            mediaAct.Stop(soundFinish);
            mediaAct.Stop(soundTrue);
            mediaAct.Stop(videoQuestionStart);
            mediaAct.Stop(soundBell);
            HideGridStudentContest();
        }

        public void HideGridStudentContest()
        {
            videoQuestionStart.Visibility = Visibility.Hidden;
            txtBlockQuestion.Visibility = Visibility.Hidden;
            mediaAct.Stop(videoQuestionStart);
            gridStudentContest.Visibility = Visibility.Hidden;
            time = 3;
            if (thread != null)
                thread.Abort();
        }

        //private void SoundStart_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    mediaAct.Play(videoQuestionStart);
        //    time = 2.3;
        //    thread = new Thread(ThreadStart);
        //    thread.Start();
        //}

        //private void VideoStudentStart_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    time = 0;
        //    if (thread != null)
        //        thread.Abort();
        //}
    }
}
