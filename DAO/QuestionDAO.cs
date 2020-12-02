using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerDTT_New_.DTO;
using System.Data;

namespace ServerDTT_New_.DAO
{
    class QuestionDAO
    {
        private static QuestionDAO instance;
        public static QuestionDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new QuestionDAO();
                return instance;
            }
        }
        public QuestionDAO()
        {
            //
        }

        public List<Question> getStartQuestion(String matchID, int position, int isBackup = 0)
        {
            string query = string.Format(
                @"SELECT * FROM tblQuestion q
                WHERE q.questionTypeID = '1' AND q.isBackup = {0} AND q.matchID = '{1}' AND q.position = {2}", isBackup, matchID, position);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Question> result = new List<Question>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Question(row));
            }
            return result;
        }

        public List<Question> getObstacleQuestion(string matchID, int isBackup = 0)
        {
            string query = string.Format(
                @"SELECT * FROM tblQuestion q
                WHERE (q.questionTypeID = '2' or q.questionTypeID  = '02') AND q.isBackup = {0} AND q.matchID = '{1}'", isBackup, matchID);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Question> result = new List<Question>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Question(row));
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i].Answer=result[i].Answer.ToUpper();
            }

            return result;
        }

        public List<Question> getAccelerateQuestion(string matchID, int isBackup = 0)
        {
            string query = string.Format(
                @"SELECT* FROM tblQuestion q
                WHERE q.questionTypeID = '3' AND q.isBackup = {0} AND q.matchID = '{1}'", isBackup, matchID);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Question> result = new List<Question>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Question(row));
            }
            return result;
        }

        public List<Question> getFinishQuestions(string matchID, int contestant, int q1Difficulty, int q2Difficulty, int q3Difficulty, int isBackup = 0)
        {
            string query = string.Empty;
            query = string.Format(
                @"SELECT* FROM tblQuestion q
                WHERE q.position = {0} And (q.QuestionTypeID='41' or q.QuestionTypeID='42' or q.QuestionTypeID='43') AND q.matchID = '{1}' AND q.isBackup = {2}", contestant + 1, matchID, isBackup);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Question> questionData = new List<Question>();
            foreach (DataRow row in data.Rows)
            {
                questionData.Add(new Question(row));
            }
            List<Question> result = new List<Question>();

            int[] numberOfQuestion = { 0, 0, 0 };

            int[] typeOfQuestion = { q1Difficulty, q2Difficulty, q3Difficulty };

            for (int i = 0; i < 3; i++)
            {
                int x = typeOfQuestion[i] / 10 - 1;
                result.Add(questionData[x * 3 + numberOfQuestion[x]]);
                numberOfQuestion[x]++;
            }

            return result;
        }

        public bool addQuestion(Int32 questionID, string detail, string answer, Int32 questionTypeID, string note)
        {
            string query = string.Format(
                "INSERT INTO tblQUESTION " +
                "VALUES ({0}, N'{1}', N'{2}', {3}, N'{4}')",
                questionID,
                detail,
                answer,
                questionTypeID,
                note);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool updateQuestion(Int32 questionID, string detail, string answer, Int32 questionTypeID, string note)
        {
            string query = string.Format(
                "UPDATE tblQUESTION " +
                "SET " +
                "Detail = N'{1}', " +
                "Answer = N'{2}', " +
                "QuestionTypeID = {3}, " +
                "Note = N'{4}' " +
                "WHERE QuestionID = {0}",
                questionID, detail, answer, questionTypeID, note);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool deleteQuestion(Int32 questionID)
        {
            string query = string.Format(
                "DELETE FROM tblQUESTION " +
                "WHERE QuestionID = {0}", questionID);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
