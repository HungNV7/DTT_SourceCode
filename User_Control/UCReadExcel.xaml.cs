﻿using Microsoft.Win32;
using ServerDTT_New_.DAO;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ServerDTT_New_.ExtendedWindow
{
    /// <summary>
    /// Interaction logic for UCReadExcel.xaml
    /// </summary>
    public partial class UCReadExcel : UserControl
    {
        List<TextBox> txtFirstNameList = new List<TextBox>();
        List<TextBox> txtLastNameList = new List<TextBox>();
        List<TextBox> txtClassList = new List<TextBox>();
        List<Label> txtPositionList = new List<Label>();
        Dictionary<string, string> matches = new Dictionary<string, string>();
        MainWindow mainWindow;

        public UCReadExcel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            InitUC();
        }

        private void InitUC()
        {
            for (int i = 0; i < 4; i++)
            {
                TextBox txtFirstName = new TextBox { Margin = new Thickness(3), FontSize = 20, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                txtFirstName.SetValue(Grid.ColumnProperty, 1);
                txtFirstName.SetValue(Grid.RowProperty, i + 1);
                txtFirstNameList.Add(txtFirstName);
                infoStudent.Children.Add(txtFirstName);

                TextBox txtLastName = new TextBox { Margin = new Thickness(3), FontSize = 20, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                txtLastName.SetValue(Grid.ColumnProperty, 2);
                txtLastName.SetValue(Grid.RowProperty, i + 1);
                txtLastNameList.Add(txtLastName);
                infoStudent.Children.Add(txtLastName);

                TextBox txtClass = new TextBox { Margin = new Thickness(3), FontSize = 20, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                txtClass.SetValue(Grid.ColumnProperty, 3);
                txtClass.SetValue(Grid.RowProperty, i + 1);
                txtClassList.Add(txtClass);
                infoStudent.Children.Add(txtClass);

                Label txtPosition = new Label { Margin = new Thickness(3), Content = (i + 1).ToString(), FontSize = 20, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                txtPosition.SetValue(Grid.ColumnProperty, 0);
                txtPosition.SetValue(Grid.RowProperty, i + 1);
                txtPositionList.Add(txtPosition);
                infoStudent.Children.Add(txtPosition);
            }

            getMatch();
        }

        private void getMatch()
        {   

            String command = "SELECT * FROM tblMatch";

            DataTable data = DataProvider.Instance.ExecuteQuery(command);

            foreach (DataRow row in data.Rows)
            {
                string id = row["matchID"].ToString();
                string name = row["name"].ToString();

                if (!matches.ContainsKey(id))
                {
                    matches.Add(id, name);
                }
                cbMatch.Items.Add(name);
            }
        }

        private void BtnLoadQuestion_Click(object sender, RoutedEventArgs e)
        {
            txtBlockFinish.Text = "Is Loading";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "Excel(*.xlsv,*.xls,*.csv,*.xlsx)|*.xlsv;*.xls;*.csv;*.xlsx";
            DataTable data;
            Worksheet worksheet;
            Workbook workbook;
            if (openFile.ShowDialog() == true)
            {
                string fileName = openFile.FileNames[0];
                workbook = new Workbook();
                workbook.LoadFromFile(fileName);
                string command = "Alter database DTT Set multi_user with rollback immediate;\n" + "Use master\n";
                DataProvider.Instance.ExecuteQuery(command);
                //command = "Create table Question (QuestionID INT IDENTITY(1,1) PRIMARY KEY, Detail NVARCHAR(1000) NOT NULL, QuestionImageName NVARCHAR(1000), QuestionVideoName NVARCHAR(1000), Answer NVARCHAR(1000) NOT NULL, AnswerImageName NVARCHAR(1000), AnswerVideoName NVARCHAR(1000), QuestionTypeID INT Not Null,Note NVARCHAR(1000),StudentID INT Not Null)\n";
                //DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi KD
                worksheet = workbook.Worksheets[0];
                command = string.Empty;

                command = "INSERT INTO tblMatch(matchID, name) VALUES('" + txtMatch.Text + "', N'" + txtName.Text + "')";
                DataProvider.Instance.ExecuteQuery(command);

                command = string.Empty;
                command = "SELECT COUNT(questionID) FROM tblQuestion";

                int count = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command)) + 1;
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(";
                    command += "" + count + ", ";
                    command += "N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    command += "',N'" + worksheet[i, 3].Text + "','1','0', N'" + txtMatch.Text + "', 0)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi VCNV
                worksheet = workbook.Worksheets[1];
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                    command += ", N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    if (i != 2)
                        command += "',N'" + worksheet[i, 3].Text + "','2','0', N'" + txtMatch.Text + "', 0)\n";
                    else
                        command += "',N'" + worksheet[i, 3].Text + "','02','0', N'" + txtMatch.Text + "', 0)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi TT
                worksheet = workbook.Worksheets[2];
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, answerImageName, answerVideoName, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                    command += ", N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    command += "',N'" + worksheet[i, 3].Text + "',N'" + worksheet[i, 6].Text + "',N'" + worksheet[i, 7].Text + "','3','0', N'" + txtMatch.Text + "', 0)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi VD
                worksheet = workbook.Worksheets[3];
                command = string.Empty;
                for (int i = 2; i <= 35;)
                {
                    int x = i + 9;
                    for (; i < x; i++)
                    {
                        count++;
                        for (int j = 1; j <= worksheet.Columns.Length; j++)
                            if (worksheet[i, j].NumberValue.ToString() != "NaN")
                                worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                        command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                        command += ", N'" + worksheet[i, 3].Text + "', N'" + worksheet[i, 5].Text + "',N'" + worksheet[i, 6].Text;
                        command += "',N'" + worksheet[i, 4].Text + "', '4" + (int.Parse(worksheet[i, 2].Text) / 10).ToString() + "' ," + ((int)((i - 2) / 9) + 1).ToString() + ", N'" + txtMatch.Text + "', 0)\n";
                    }
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi phan GM
                worksheet = workbook.Worksheets[4];
                //command = "Create table DecodeQuestion (QuestionID INT IDENTITY(1,1) PRIMARY KEY, Row INT NOT NULL, Col INT NOT NULL, Detail NVARCHAR(1000) NOT NULL, QuestionImageName NVARCHAR(1000), QuestionVideoName NVARCHAR(1000), Answer NVARCHAR(1000) NOT NULL, QuestionTypeID INT Not Null)\n";
                //DataProvider.Instance.ExecuteQuery(command);
                command = "SELECT COUNT(questionID) FROM tblDecodeQuestion";

                count = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command)) + 1;

                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    if (i == 2)
                        command = "INSERT INTO tblDecodeQuestion(questionID, row, col, detail, answer, questionTypeID, matchID, isBackup) values(" + count + "," + worksheet[i, 4].Text + "," + worksheet[i, 5].Text + ",N'" + worksheet[2, 6].Text + "',N'" + worksheet[2, 7].Text + "', '0', '" + txtMatch.Text + "', 0)\n";
                    else
                    {
                        command = "INSERT INTO tblDecodeQuestion(questionID, row, col, detail, questionImageName, questionVideoName, answer, questionTypeID, matchID, isBackup) VALUES(" + count;
                        int questionTypeID;
                        if (worksheet[i, 2].Text.Contains("Vàng"))
                            questionTypeID = 20;
                        else if (worksheet[i, 2].Text.Contains("Xanh"))
                            questionTypeID = 10;
                        else questionTypeID = 30;
                        questionTypeID += int.Parse(worksheet[i, 3].Text);
                        command += ", " + worksheet[i, 4].Text + ",";
                        command += worksheet[i, 5].Text + ",";
                        command += "N'" + worksheet[i, 6].Text + "',N'" + worksheet[i, 8].Text + "',N'" + worksheet[i, 9].Text + "',";
                        command += "N'" + worksheet[i, 7].Text + "'," + questionTypeID.ToString() + ", '" + txtMatch.Text + "', 0)\n";
                    }
                    DataProvider.Instance.ExecuteQuery(command);
                }


                command = "Select * from tblQuestion;";
                data = DataProvider.Instance.ExecuteQuery(command, null);

                command = "Select * from tblDecodeQuestion;";
                data = DataProvider.Instance.ExecuteQuery(command, null);

                txtBlockFinish.Text = "Finished";

            }
            //reload combox Match
            mainWindow.GetMatch();
            getMatch();
        }

        private void BtnLoadBUQuestion_Click(object sender, RoutedEventArgs e)
        {
            txtBlockFinish.Text = "Is Loading";
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "Excel(*.xlsv,*.xls,*.csv,*.xlsx)|*.xlsv;*.xls;*.csv;*.xlsx";
            DataTable data;
            Worksheet worksheet;
            Workbook workbook;
            if (openFile.ShowDialog() == true)
            {
                string fileName = openFile.FileNames[0];
                workbook = new Workbook();
                workbook.LoadFromFile(fileName);
                string command = "Alter database DTT Set multi_user with rollback immediate;\n" + "Use master\n";
                DataProvider.Instance.ExecuteQuery(command);
                //command = "Create table Question (QuestionID INT IDENTITY(1,1) PRIMARY KEY, Detail NVARCHAR(1000) NOT NULL, QuestionImageName NVARCHAR(1000), QuestionVideoName NVARCHAR(1000), Answer NVARCHAR(1000) NOT NULL, AnswerImageName NVARCHAR(1000), AnswerVideoName NVARCHAR(1000), QuestionTypeID INT Not Null,Note NVARCHAR(1000),StudentID INT Not Null)\n";
                //DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi KD
                worksheet = workbook.Worksheets[0];

                command = string.Empty;
                command = "SELECT COUNT(questionID) FROM tblQuestion";

                int count = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command)) + 1;
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(";
                    command += "" + count + ", ";
                    command += "N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    command += "',N'" + worksheet[i, 3].Text + "','1','0', N'" + txtMatch.Text + "', 1)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi VCNV
                worksheet = workbook.Worksheets[1];
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                    command += ", N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    if (i != 2)
                        command += "',N'" + worksheet[i, 3].Text + "','2','0', N'" + txtMatch.Text + "', 1)\n";
                    else
                        command += "',N'" + worksheet[i, 3].Text + "','02','0', N'" + txtMatch.Text + "', 1)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi TT
                worksheet = workbook.Worksheets[2];
                command = string.Empty;
                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, answerImageName, answerVideoName, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                    command += ", N'" + worksheet[i, 2].Text + "',N'" + worksheet[i, 4].Text + "',N'" + worksheet[i, 5].Text;
                    command += "',N'" + worksheet[i, 3].Text + "',N'" + worksheet[i, 6].Text + "',N'" + worksheet[i, 7].Text + "','3','0', N'" + txtMatch.Text + "', 1)\n";
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi VD
                worksheet = workbook.Worksheets[3];
                command = string.Empty;
                for (int i = 2; i <= 35;)
                {
                    int x = i + 9;
                    for (; i < x; i++)
                    {
                        count++;
                        for (int j = 1; j <= worksheet.Columns.Length; j++)
                            if (worksheet[i, j].NumberValue.ToString() != "NaN")
                                worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                        command += "INSERT INTO tblQuestion(questionID, detail, questionImageName, questionVideoName, answer, questionTypeID, position, matchID, isBackup) VALUES(" + count;
                        command += ", N'" + worksheet[i, 3].Text + "', N'" + worksheet[i, 5].Text + "',N'" + worksheet[i, 6].Text;
                        command += "',N'" + worksheet[i, 4].Text + "', '4" + (int.Parse(worksheet[i, 2].Text) / 10).ToString() + "' ," + ((int)((i - 2) / 9) + 1).ToString() + ", N'" + txtMatch.Text + "', 1)\n";
                    }
                }
                DataProvider.Instance.ExecuteQuery(command);

                //Cau hoi phan GM
                worksheet = workbook.Worksheets[4];
                //command = "Create table DecodeQuestion (QuestionID INT IDENTITY(1,1) PRIMARY KEY, Row INT NOT NULL, Col INT NOT NULL, Detail NVARCHAR(1000) NOT NULL, QuestionImageName NVARCHAR(1000), QuestionVideoName NVARCHAR(1000), Answer NVARCHAR(1000) NOT NULL, QuestionTypeID INT Not Null)\n";
                //DataProvider.Instance.ExecuteQuery(command);
                command = "SELECT COUNT(questionID) FROM tblDecodeQuestion";

                count = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command)) + 1;

                for (int i = 2; i <= worksheet.Rows.Length; i++)
                {
                    count++;
                    for (int j = 1; j <= worksheet.Columns.Length; j++)
                        if (worksheet[i, j].NumberValue.ToString() != "NaN")
                            worksheet[i, j].Text = worksheet[i, j].NumberValue.ToString();
                    if (i == 2)
                        command = "INSERT INTO tblDecodeQuestion(questionID, row, col, detail, answer, questionTypeID, matchID, isBackup) values(" + count + "," + worksheet[i, 4].Text + "," + worksheet[i, 5].Text + ",N'" + worksheet[2, 6].Text + "',N'" + worksheet[2, 7].Text + "', '0', '" + txtMatch.Text + "', 1)\n";
                    else
                    {
                        command = "INSERT INTO tblDecodeQuestion(questionID, row, col, detail, questionImageName, questionVideoName, answer, questionTypeID, matchID, isBackup) VALUES(" + count;
                        int questionTypeID;
                        if (worksheet[i, 2].Text.Contains("Vàng"))
                            questionTypeID = 20;
                        else if (worksheet[i, 2].Text.Contains("Xanh"))
                            questionTypeID = 10;
                        else questionTypeID = 30;
                        questionTypeID += int.Parse(worksheet[i, 3].Text);
                        command += ", " + worksheet[i, 4].Text + ",";
                        command += worksheet[i, 5].Text + ",";
                        command += "N'" + worksheet[i, 6].Text + "',N'" + worksheet[i, 8].Text + "',N'" + worksheet[i, 9].Text + "',";
                        command += "N'" + worksheet[i, 7].Text + "'," + questionTypeID.ToString() + ", '" + txtMatch.Text + "', 1)\n";
                    }
                    DataProvider.Instance.ExecuteQuery(command);
                }


                command = "Select * from tblQuestion;";
                data = DataProvider.Instance.ExecuteQuery(command, null);

                command = "Select * from tblDecodeQuestion;";
                data = DataProvider.Instance.ExecuteQuery(command, null);

                txtBlockFinish.Text = "Finished";

            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string matchID = "";
            string s = cbMatch.SelectedItem.ToString();
            foreach (string id in matches.Keys)
            {
                if (s == matches[id])
                {
                    matchID = id;
                }
            }

            string command = "SELECT COUNT(studentID) FROM tblStudent";
            int count = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command));

            command = "SELECT COUNT(No) FROM tblDetailMatch";
            int no = Convert.ToInt32(DataProvider.Instance.ExecuteScalar(command));
            for (int i = 0; i < 4; i++)
            {
                no++;
                //command = String.Format("SELECT studentID FROM tblStudent WHERE firstName = N'{0} AND lastName = N'{1} AND class = N'{2}'", txtFirstNameList[i].Text, txtLastNameList[i].Text, txtClassList[i].Text);
                string studentId = StudentDAO.Instance.checkStudent(txtFirstNameList[i].Text, txtLastNameList[i].Text, txtClassList[i].Text);
                if(studentId.Equals(""))
                {
                    count++;
                    studentId = count.ToString();
                    command = String.Format("INSERT INTO tblStudent(studentID, firstName, lastName, class) VALUES({0}, N'{1}', N'{2}', '{3}')", count, txtFirstNameList[i].Text, txtLastNameList[i].Text, txtClassList[i].Text);
                    DataProvider.Instance.ExecuteNonQuery(command);
                }

                command = String.Format("INSERT INTO tblDetailMatch(No, studentID, matchID, position, point) VALUES({0}, {1}, N'{2}', {3}, 0)", no, studentId, matchID, i+1);
                DataProvider.Instance.ExecuteNonQuery(command);
            }
        }

        private void btnCreateDB_Click(object sender, RoutedEventArgs e)
        {
            String command = String.Empty;

            command = String.Format(" USE DTT\n");
            //command += String.Format("CREATE TABLE tblMatch(matchID nvarchar(50) PRIMARY KEY NOT NULL,name nvarchar(50) NOT NULL)\n");
            command += String.Format("CREATE TABLE tblQuestionType(questionTypeID nvarchar(50) PRIMARY KEY NOT NULL, name nvarchar(50) NOT NULL)\n");
            command += String.Format("CREATE TABLE tblStudent(studentID int PRIMARY KEY NOT NULL, firstName nvarchar(50) NULL, lastName nvarchar(50) NOT NULL, class nvarchar(50) NULL)\n");
            command += String.Format("CREATE TABLE tblQuestion(questionID int PRIMARY KEY NOT NULL, detail nvarchar(MAX) NOT NULL,questionImageName nvarchar(MAX) NULL,"
                + "questionVideoName nvarchar(MAX) NULL, answer nvarchar(MAX) NOT NULL,"
                + "answerImageName nvarchar(MAX) NULL, answerVideoName nvarchar(MAX) NULL, questionTypeID nvarchar(50) NOT NULL REFERENCES tblQuestionType(questionTypeID),"
                + "position int NULL, matchID nvarchar(50) NOT NULL REFERENCES tblMatch(matchID), isBackup bit NOT NULL, note nvarchar(MAX) NULL)");
            command += String.Format("CREATE TABLE tblDetailMatch(No int PRIMARY KEY NOT NULL, studentID int NOT NULL REFERENCES tblStudent(studentID),"
                    + "matchID nvarchar(50) NOT NULL REFERENCES tblMatch(matchID), point int NOT NULL, position int NOT NULL)");
            command += String.Format("CREATE TABLE tblDecodeQuestion(questionID int PRIMARY KEY NOT NULL, row int NOT NULL, col int NOT NULL,"
                + "detail nvarchar(MAX) NOT NULL, questionImageName nvarchar(MAX) NULL, questionVideoName nvarchar(MAX) NULL,"
                + "answer nvarchar(MAX) NOT NULL, questionTypeID nvarchar(50) NOT NULL  REFERENCES tblQuestionType(questionTypeID),"
                + "matchID nvarchar(50) NOT NULL REFERENCES tblMatch(matchID), isBackup bit NOT NULL)");
            DataProvider.Instance.ExecuteQuery(command);


            command = String.Empty;
            command = String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'0', N'Câu hỏi chính vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'02', N'Câu hỏi chính vòng vượt chướng ngại vật')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'1', N'Câu hỏi vòng khởi động')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'10', N'Gợi ý ô màu xanh vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'11', N'Câu hỏi ô màu xanh vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'2', N'Câu hỏi gợi ý vòng vượt chướng ngại vật')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'20', N'Gợi ý ô màu vàng vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'21', N'Câu hỏi ô màu vàng vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'3', N'Câu hỏi vòng tăng tốc')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'31', N'Câu hỏi ô màu đỏ vòng giải mã')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'41', N'Câu hỏi vòng về đích 10đ')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'42', N'Câu hỏi vòng về đích 20đ')");
            command += String.Format("INSERT [dbo].[tblQuestionType] ([questionTypeID], [name]) VALUES (N'43', N'Câu hỏi vòng về đích 30đ')");

            DataProvider.Instance.ExecuteNonQuery(command);
        }
    }
}