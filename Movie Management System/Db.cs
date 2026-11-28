using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Movie_Management_System
{

    using System;
        public static class Db
        {
            public static string ConStr
            {
                get
                {
                    return "Server=MovieDB.mssql.somee.com;Database=MovieDB;User Id=movie_SQLLogin_1;Password=efnyqamwfy;TrustServerCertificate=True;";
                }
            }
        }
    }


