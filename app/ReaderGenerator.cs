using App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public class ReaderGenerator
    {
        List<string[]> rows;
        public ReaderGenerator(List<string[]> rows)
        {
            this.rows = rows;
        }

        

        //public Reader GenerateUnitReader(string[] row)
        //{
        //    Reader reader = new Reader()
        //    {
        //        Reader_id = row[0],
        //        Name = row[1],
        //    };
        //    //if (row[2] == "student" || row[2] == "teacher" || row[2] == "guest")
        //    //    reader.ReaderType = (Type)row[2];
        //}

    }
}
