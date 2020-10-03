using ServerDTT_New_.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDTT_New_.DAO
{
    class StudentDAO
    {
        private static StudentDAO instance;
        public static StudentDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new StudentDAO();
                return instance;
            }
        }
        public StudentDAO()
        {
            //
        }

        //
        public List<Student> getStartStudent(string name)
        {
            string query = string.Format(
                @"SELECT * FROM tblStudent, tblDetailMatch, tblMatch
                WHERE tblStudent.studentID = tblDetailMatch.studentID
				      AND tblDetailMatch.matchID = tblMatch.matchID
                      AND tblMatch.name=N'{0}'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Student> result = new List<Student>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Student(row));
            }
            return result;
        }

        public string checkStudent(string firstName, string lastName, string _class)
        {
            string studentID = "";
            string query = string.Format("SELECT * FROM tblStudent, tblDetailMatch WHERE firstName = N'{0}'  AND lastName = N'{1}' AND class = N'{2}'", firstName, lastName, _class);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Student> result = new List<Student>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Student(row));
            }
            if(result.Count != 0)
            {
                studentID = result[0].StudentID;
            }
            
            return studentID;
        }

        //
        public bool UpdatePoint(string studentID, int Point, string matchID)
        {
            string query = string.Format(
               "UPDATE tblDetailMatch SET point={1} WHERE studentID={0} AND matchID=N'{2}'",
               studentID, Point,matchID);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateName(string studentID, string name)
        {
            string query = string.Format(
               "UPDATE tblStudent SET lastName = N'{0}' WHERE studentID={1}", name, studentID);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
