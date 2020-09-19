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
        public List<Student> getStartStudent(string matchID)
        {
            string query = string.Format(
                @"SELECT * FROM tblStudent,tblDetailMatch
                WHERE tblStudent.studentID=tblDetailMatch.studentID
                      AND tblDetailMatch.matchID='CK'");
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<Student> result = new List<Student>();
            foreach (DataRow row in data.Rows)
            {
                result.Add(new Student(row));
            }
            return result;
        }
        public bool UpdatePoint(string Name, int Point)
        {
            string query = string.Format(
               "UPDATE tblDetailMatch " +
               "SET " +
               "point = N'{0}', " +
                "WHERE studentid in( select studentid from tblstudent where "+
                "name = N'{1}', "
                ,
               Name,Point);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

    }
}
