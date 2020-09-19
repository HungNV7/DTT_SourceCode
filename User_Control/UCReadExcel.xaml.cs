using Microsoft.Win32;
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
        public UCReadExcel()
        {
            InitializeComponent();
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

                int count = DataProvider.Instance.ExecuteNonQuery(command) + 1;
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

                count = DataProvider.Instance.ExecuteNonQuery(command) + 1;

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

                int count = DataProvider.Instance.ExecuteNonQuery(command) + 1;
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

                count = DataProvider.Instance.ExecuteNonQuery(command) + 1;

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
    }
}
