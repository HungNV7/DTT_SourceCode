using ServerDTT_New_.DAO;
using ServerDTT_New_.DTO;
using ServerDTT_New_.ExtendedWindow;
using ServerDTT_New_.SupportClass;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerDTT_New_.User_Control
{
    /// <summary>
    /// Interaction logic for UCSubQuestions.xaml
    /// </summary>
    public partial class UCSubQuestions : UserControl
    {
        MediaAct mediaAct = new MediaAct();
        List<DTO.Question> questionList = new List<DTO.Question>();

        MainWindow mainWindow;
        EWMainWindow eWMainWindow;
        EWSubQuestions ewSubQuestions;
        List<Student> studentList;
        Dictionary<int, Student> studentChosen;
        Server server;
        int currentQuestionID = 0;
        int bellPosition;
        int orderBell = 0;
        DTO.Question currentQuestion;
        string matchID = "";

        public UCSubQuestions(MainWindow main, EWMainWindow ewMainWindow, List<Student> students, Server _server, string matchID)
        {
            InitializeComponent();

            studentList = students;
            mainWindow = main;
            server = _server;
            eWMainWindow = ewMainWindow;
            this.matchID = matchID;
            InitUC();
        }


        public void InitUC()
        {
            cbTS1.Content = studentList[0].Name;
            cbTS2.Content = studentList[1].Name;
            cbTS3.Content = studentList[2].Name;
            cbTS4.Content = studentList[3].Name;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            studentChosen = new Dictionary<int, Student>();
            if (cbTS1.IsChecked == true)
            {
                studentChosen.Add(0, studentList[0]);
            }
            if (cbTS2.IsChecked == true)
            {
                studentChosen.Add(1, studentList[1]);
            }
            if (cbTS3.IsChecked == true)
            {
                studentChosen.Add(2, studentList[2]);
            }
            if (cbTS4.IsChecked == true)
            {
                studentChosen.Add(3, studentList[3]);
            }
            ewSubQuestions = new EWSubQuestions(studentChosen.Count);
            eWMainWindow.Content = ewSubQuestions;
            questionList = QuestionDAO.Instance.GetSubQuestions(matchID);
            currentQuestionID = 0;
            //Cap nhap ID cua vong hien tai
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("Round.txt");
            streamWriter.Flush();
            streamWriter.Write("6");
            streamWriter.Close();
            server.SendTSInfo(6, studentList);
            UpdateInfoOnScreen();
        }

        //public void ShowNextQuestion(int IsCorrect)
        //{


        //    if (IsBackup)
        //    {
        //        if (currentBUQuestionID > 14)
        //        {
        //            btnTrueAnswer.IsEnabled = false;
        //            btnFalseAnswer.IsEnabled = false;
        //            return;
        //        }
        //        currentQuestion = bUQuestionList[currentBUQuestionID];
        //        currentBUQuestionID++;
        //    }
        //    else
        //    {
        //        if (currentQuestionID > 14)
        //        {
        //            btnTrueAnswer.IsEnabled = false;
        //            btnFalseAnswer.IsEnabled = false;
        //            return;
        //        }
        //        currentQuestion = questionList[currentQuestionID];
        //        currentQuestionID++;
        //    }

        //    eWStart.txtBlockQuestion.Text = txtBlockQuestion.Text = currentQuestion.Detail;
        //    txtBlockAnswer.Text = currentQuestion.Answer;
        //    eWStart.mediaQuestion.Source = null;
        //    eWStart.imgQuestion.Source = null;
        //    if (currentQuestion.QuestionImageName != string.Empty)
        //        mediaAct.Upload(eWStart.imgQuestion, currentQuestion.QuestionImageName);
        //    if (currentQuestion.QuestionVideoName != string.Empty)
        //        mediaAct.Upload(eWStart.mediaQuestion, currentQuestion.QuestionVideoName);
        //    eWStart.imgQuestion.Visibility = Visibility.Visible;
        //    eWStart.mediaQuestion.Visibility = Visibility.Visible;
        //    string message = "1_1_" + IsCorrect.ToString() + "_" + currentQuestion.Detail + "_" + currentQuestion.QuestionImageName + currentQuestion.QuestionVideoName;
        //    for (int i = 0; i < server.ClientIDList.Count(); i++)
        //        if (server.ClientIDList[i] == currentStudentID)
        //        {
        //            server.Send(server.ClientList[i], message);
        //            break;
        //        }
        //}

        public void SolveMessage(string message)
        {
            string[] messages = message.Split('_');
            int position = int.Parse(messages[0]);

            switch (messages[1])
            {
                case "0":
                    BellHandler(position);
                    break;
            }
        }

        private void BellHandler(int position)
        {
            int count = 0;
            foreach(int x in studentChosen.Keys)
            {
                if(position == x)
                {
                    orderBell++;
                    mediaAct.Pause(ewSubQuestions.videoQuestionStart);
                    mediaAct.Stop(ewSubQuestions.soundBell);
                    mediaAct.Play(ewSubQuestions.soundBell);
                    for (int i = 0; i < server.ClientList.Count; i++)
                    {
                        server.Send(server.ClientList[i], "6_3");
                    }
                    break;
                }
                count++;
            }
            bellPosition = position;
            ewSubQuestions.txtBackGroundNameList[count].Background = Brushes.DarkRed;
        }

        public void UpdateInfoOnScreen()
        {
            int count = 0;
            foreach(Student student in studentChosen.Values)
            {
                ewSubQuestions.txtBlockStudentNameList[count++].Text = student.Name;
            }
        }

        private void btnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            orderBell = 0;
            if (currentQuestionID < questionList.Count)
            {
                currentQuestion = questionList[currentQuestionID++];
                ewSubQuestions.txtBlockQuestion.Text = currentQuestion.Detail;
                ewSubQuestions.txtBlockQuestion.Visibility = Visibility.Visible;
                for (int i = 0; i < server.ClientList.Count; i++)
                {
                    server.Send(server.ClientList[i], "6_1_" + currentQuestion.Detail);
                }
            }
            var bc = new BrushConverter();
            for (int i = 0; i < studentChosen.Count; i++)
            {
                ewSubQuestions.txtBackGroundNameList[i].Background = (Brush)bc.ConvertFrom("#2a2728");
            }
        }

        private void btnFalseAnswer_Click(object sender, RoutedEventArgs e)
        {
            if(orderBell != studentChosen.Count)
            {
                mediaAct.Continue(ewSubQuestions.videoQuestionStart);
                foreach (int x in studentChosen.Keys)
                {
                    if (bellPosition != x)
                    {
                        server.Send(server.ClientList[x], "6_4");
                    }
                }
                var bc = new BrushConverter();
                for (int i = 0; i < studentChosen.Count; i++)
                {
                    ewSubQuestions.txtBackGroundNameList[i].Background = (Brush)bc.ConvertFrom("#2a2728");
                }
            }
            else
            {
                mediaAct.Stop(ewSubQuestions.videoQuestionStart);
            }
            mediaAct.Stop(ewSubQuestions.soundFalse);
            mediaAct.Play(ewSubQuestions.soundFalse);
            
        }

        private void btnTrueAnswer_Click(object sender, RoutedEventArgs e)
        {
            mediaAct.Stop(ewSubQuestions.soundTrue);
            mediaAct.Play(ewSubQuestions.soundTrue);
            mediaAct.Stop(ewSubQuestions.videoQuestionStart);
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            ewSubQuestions.HideAll();
            mediaAct.Stop(ewSubQuestions.soundFinish);
            mediaAct.Play(ewSubQuestions.soundFinish);
        }

        private void btnTimeStart_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < server.ClientList.Count; i++)
            {
                server.Send(server.ClientList[i], "6_2");
            }
            mediaAct.Stop(ewSubQuestions.videoQuestionStart);
            mediaAct.Play(ewSubQuestions.videoQuestionStart);
        }

        private void btnEliminate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
