using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerDTT_New_.SupportClass;
using ServerDTT_New_.DTO;
using ServerDTT_New_.DAO;

namespace ServerDTT_New_.DAO
{
    public class DecodeQuestionDAO
    {
        private static DecodeQuestionDAO instance;
        public static DecodeQuestionDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new DecodeQuestionDAO();
                return instance;
            }
        }
        public DecodeQuestionDAO() { }

        public List<DecodeQuestion> getQuestions(string matchID, int isBackup = 0)
        {
            string query = string.Format(@"SELECT * FROM tblDecodeQuestion WHERE matchID = '{0}' AND isBackup = {1}", matchID, isBackup);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<DecodeQuestion> result = new List<DecodeQuestion>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new DecodeQuestion(row));
            }
            return result;
        }

        public DecodeQuestion getQuestion(int row, int col, string matchID, int isBackup = 0)
        {
            string query = string.Format(@"
                           SELECT * FROM tblDECODEQUESTION
                           WHERE row = {0} AND col = {1} AND matchID = '{2}' AND isBackup = {3}", row + 1, col + 1, matchID, isBackup);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            DecodeQuestion result = new DecodeQuestion(data.Rows[0]);
            return result;
        }
    }
}
