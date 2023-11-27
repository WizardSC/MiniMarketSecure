﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MSSQLConnect
    {
        public SqlConnection conn = null;
        //Toàn đẹp try wizard

        string strconn = @"Data Source=LAPTOP-AEI9M0MI\WIZARDSC1;Initial Catalog = MiniMarketSecure; Integrated Security = True";

        public void Connect()
        {
            if (conn == null)
            {
                conn = new SqlConnection(strconn);
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void Disconnect()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
