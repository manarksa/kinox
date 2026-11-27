using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Movie_Management_System
{
  
        public static class Db
        {
            static Db()
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);
            }

            public static string ConStr
            {
                get
                {
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MovieDB.mdf;Integrated Security=True;Connect Timeout=30";
                }
            }
        }
        }

