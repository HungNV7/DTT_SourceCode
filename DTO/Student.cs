using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ServerDTT_New_.DTO
{
    public class Student
    {
        public string Name;
        public string StudentID;
        public int Point;
        public int Position;
        public string MatchID;
        public string Class;
        public Student()
        {

        }
        public Student(string StudentID, string Name, int Point)
        {
            this.StudentID = StudentID;
            this.Name = Name;
            this.Point = Point;
        }
        public Student(DataRow row)
        {
            this.Name = row["name"].ToString();
            this.StudentID = row["studentID"].ToString();
            this.Point = (Int32)row["point"];
            this.Position = (Int32)row["position"];
            this.MatchID = row["matchID"].ToString();
            this.Class = row["class"].ToString();
        }
        public Student()
        {

        }

    }
}
