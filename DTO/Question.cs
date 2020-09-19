using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ServerDTT_New_.DTO
{
    class Question
    {
        public string QuestionID { get; set; }
        public string Detail { get; set; }
        public string Answer { get; set; }
        public string QuestionTypeID { get; set; }
        public string Note { get; set; }
        public string QuestionImageName;
        public string QuestionVideoName;
        public string AnswerImageName;
        public string AnswerVideoName;
        public Boolean Backup { get; set; }

        public Question() { }

        public Question(string questionID, string detail, string answer, string questionTypeID, string note, string questionImageName, string questionVideoName, string answerImageName,string answerVideoName, Boolean backup)
        {
            this.QuestionID = questionID;
            this.Detail = detail;
            this.Answer = answer;
            this.QuestionTypeID = questionTypeID;
            this.Note = note;
            this.QuestionImageName = questionImageName;
            this.QuestionVideoName = questionVideoName;
            this.AnswerImageName = answerImageName;
            this.AnswerVideoName = answerVideoName;
            this.Backup = backup;
        }

        public Question(DataRow row)
        {
            this.QuestionID = row["questionID"].ToString();
            this.Detail = row["detail"].ToString();
            this.Answer = row["answer"].ToString();
            this.QuestionTypeID = row["questionTypeID"].ToString();
            this.Note = row["note"].ToString();
            this.AnswerImageName = row["answerImageName"].ToString();
            this.AnswerVideoName = row["answerVideoName"].ToString();
            this.QuestionImageName = row["questionImageName"].ToString();
            this.QuestionVideoName = row["questionVideoName"].ToString();
            this.Backup = row["isBackup"].ToString().Equals("1")?true:false;
        }
    }
}
