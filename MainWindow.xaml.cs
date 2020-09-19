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
using ServerDTT_New_.SupportClass;
using ServerDTT_New_.ExtendedWindow;
using ServerDTT_New_.User_Control;
using ServerDTT_New_.DTO;
using ServerDTT_New_.DAO;
using System.Data;

namespace ServerDTT_New_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TextBox> txtBoxStudentNameList = new List<TextBox>();
        public List<TextBox> txtBoxStudentPointList = new List<TextBox>();
         List<Student> studentList = new List<Student>();
        EWMainWindow eWMainWindow;
        EWPointSummarized eWPointSummarized;
        EWStart eWStart;
        EWObstacles eWObstacles;
        EWAccelerate eWAccelerate;
        EWDecode eWDecode;
        EWFinish eWFinish;

        UCStart uCStart;
        UCObstacles uCObstacles;
        UCAccelerate uCAccelerate;
        UCDecode uCDecode;
        UCFinish uCFinish;
        UCReadExcel uCReadExcel;

        Server server;
        string matchID = "";

        const int numberOfStudent = 4;
        public MainWindow()
        {
            InitializeComponent();
            GetMatch();
            server = new Server(this);
        }
       

        void InitMainWindow()
        {
            
            
            for (int i = 0; i < numberOfStudent; i++)
                {

                    TextBox txtBoxName = new TextBox { FontSize = 25, Background = Brushes.LightBlue, Text = studentList[i].Name, Margin = new Thickness(5), Width = 125, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                    txtBoxName.SetValue(Grid.RowProperty, i + 1);
                    txtBoxName.SetValue(Grid.ColumnProperty, 0);
                    gridStudentInformation.Children.Add(txtBoxName);
                    txtBoxStudentNameList.Add(txtBoxName);

                    TextBox txtBoxPoint = new TextBox { FontSize = 25, Background = Brushes.AliceBlue, Text = studentList[i].Point.ToString(), Margin = new Thickness(5), Width = 125, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, TextAlignment = TextAlignment.Center };
                    txtBoxPoint.SetValue(Grid.RowProperty, i + 1);
                    txtBoxPoint.SetValue(Grid.ColumnProperty, 1);
                    gridStudentInformation.Children.Add(txtBoxPoint);
                    txtBoxStudentPointList.Add(txtBoxPoint);

                }
           
                eWMainWindow = new EWMainWindow();
                eWMainWindow.Show();
                eWPointSummarized = new EWPointSummarized();
                eWStart = new EWStart();
                eWObstacles = new EWObstacles();
                eWAccelerate = new EWAccelerate();
                eWDecode = new EWDecode();
                eWFinish = new EWFinish();
                
            try
            {
                uCReadExcel = new UCReadExcel();
                uCStart = new UCStart(this, eWMainWindow, eWStart, studentList, server, matchID);
                uCObstacles = new UCObstacles(this, eWMainWindow, eWObstacles, studentList, server, matchID);
                uCAccelerate = new UCAccelerate(this, eWMainWindow, eWAccelerate, studentList, server, matchID);
                //uCDecode = new UCDecode(this, eWMainWindow, eWDecode, studentList, server, matchID);
                uCFinish = new UCFinish(this, eWMainWindow, eWFinish, studentList, server, matchID);
                

            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }

            
            tabMain.Items.Add(new TabItem { Content = uCStart, Header = "Khởi Động", Width = 80, Height = 20, FontSize = 10 });
            tabMain.Items.Add(new TabItem { Content = uCObstacles, Header = "VCNV", Width = 80, Height = 20, FontSize = 10 });
            tabMain.Items.Add(new TabItem { Content = uCAccelerate, Header = "Tăng Tốc", Width = 80, Height = 20, FontSize = 10 });
            tabMain.Items.Add(new TabItem { Content = uCDecode, Header = "Giải Mã", Width = 80, Height = 20, FontSize = 10 });
            tabMain.Items.Add(new TabItem { Content = uCFinish, Header = "Về Đích", Width = 80, Height = 20, FontSize = 10 });
            tabMain.Items.Add(new TabItem { Content = uCReadExcel, Header = "Read Excel", Width = 80, Height = 20, FontSize = 10 });


            //Cap nhap ID cua vong hien tai
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("Round.txt");
            streamWriter.Flush();
            streamWriter.Write("0");
            streamWriter.Close();
        }

        public void GetMatch()
        {
            Dictionary<string, string> matches = new Dictionary<string, string>();

            String command = "SELECT * FROM tblMatch";

            DataTable data = DataProvider.Instance.ExecuteQuery(command);

            cbMatch.Items.Add(new ComboBoxItem { IsSelected = true, Content="Choose a name of the match" });

            foreach(DataRow row in data.Rows)
            {
                string id = row["matchID"].ToString();
                string name = row["name"].ToString();

                matches.Add(id, name);
                cbMatch.Items.Add(name);
            }
        }

        public void SolveMessage(string message)
        {
            switch (message[0])
            {
                case '1':
                    break;
                case '2':
                    uCObstacles.SolveMessage(message.Substring(2));
                    break;
                case '3':
                    uCAccelerate.SolveMessage(message.Substring(2));
                    break;
                case '4':
                    uCFinish.SolveMessage(message.Substring(2));
                    break;
                case '5':
                    uCDecode.SolveMessage(message.Substring(2));
                    break;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StudentDAO update = new StudentDAO();
            for (int i = 0; i < numberOfStudent; i++)
            {
               int point = int.Parse(txtBoxStudentPointList[i].Text);
                update.UpdatePoint(studentList[i].StudentID,point,studentList[i].MatchID.ToString());
            }
            uCStart.UpdateInfoOnScreen();
            uCObstacles.UpdateInfoOnScreen();
            uCAccelerate.UpdateInfoOnScreen();
            uCDecode.UpdateInfoOnScreen();
            uCFinish.UpdateInfoOnScreen();

            System.IO.StreamReader stream = new System.IO.StreamReader("Round.txt");
            int round = int.Parse(stream.ReadLine());
            stream.Close();
            server.SendTSInfo(round, studentList);
        }

        private void BtnSummarizedPoint_Click(object sender, RoutedEventArgs e)
        {
            eWMainWindow.Content = eWPointSummarized;
            eWPointSummarized.PlayVideoSummarizedPoint(studentList);
        }

        private void BtnFinal_Click(object sender, RoutedEventArgs e)
        {
            eWMainWindow.Content = eWPointSummarized;
            eWPointSummarized.soundFinishAll.Play();
        }

        private void cbMatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string s = cbMatch.SelectedItem.ToString();

            //MessageBox.Show(s);

            if (!s.Contains("Choose a name of the match"))
            {
                studentList = StudentDAO.Instance.getStartStudent(s);// get student 
                InitMainWindow();
            }
        }
    }
}
