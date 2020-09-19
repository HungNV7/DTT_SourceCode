using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDTT_New_.DTO
{
    public class DecodeQuestion
    {
        public int RowNo { get; set; }
        public int ColNo { get; set; }
        public string QuestionTypeID { get; set; }
        public String Detail { get; set; }
        public String Answer { get; set; }
        public string QuestionVideoName;
        public string QuestionImageName;


        public DecodeQuestion() { }

        public DecodeQuestion(int rowNo, int colNo, string QuestionTypeID, String detail, String answer)
        {
            this.RowNo = rowNo;
            this.ColNo = colNo;
            this.QuestionTypeID = QuestionTypeID;
            this.Detail = detail;
            this.Answer = answer;
        }

        public DecodeQuestion(DataRow row)
        {
            this.RowNo = (int)row["row"] - 1;
            this.ColNo = (int)row["col"] - 1;
            this.QuestionTypeID = row["questionTypeID"].ToString();
            this.Detail = row["detail"].ToString();
            this.Answer = row["answer"].ToString();
            QuestionImageName = row["questionImageName"].ToString();
            QuestionVideoName = row["questionVideoName"].ToString();
        }
    }
}
