﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebSearcher {
    public class UniversityOfCambridge : Searcher {

        public override void GetRes(object o) {
            ArrayList l = (ArrayList)o;
            int start = (int)l[0];
            int end = (int)l[1];
            for (int i = start; i < end; i++) {
                if (dt.Rows[i]["isIn"].ToString().ToLower() == "yes" || dt.Rows[i]["isIn"].ToString().ToLower() == "no") {
                    over++;
                    continue;
                }
                string title_old = dt.Rows[i][0].ToString();
                try {

                    string title = title_old.Replace('&', ' ').Replace(' ', '+').Replace("-", "%E2%80%93").Replace(",","%2C");

                    string uri = "https://www.repository.cam.ac.uk/discover?scope=%2F&query="+title+"&submit=Go";


                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string res = reader.ReadToEnd();
                    res = DealStr(res);
                    if (res.ToLower().Contains((title_old.Replace("-", "") + "</a>").ToLower()) || res.ToLower().Contains((title_old.Replace("-", "") + ".</a>").ToLower())) {
                        dt.Rows[i]["isIn"] = "YES";
                    } else {
                        dt.Rows[i]["isIn"] = "NO";
                    }
                } catch (Exception) {
                    dt.Rows[i]["isIn"] = "Error";
                }

                string strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source =" + fileName + ";Extended Properties = Excel 8.0";
                OleDbConnection oleConnection = new OleDbConnection(strConnection);
                oleConnection.Open();
                string sql = "update [Sheet1$] set isIn = '" + dt.Rows[i]["isIn"] + "' where 标题 = '" + dt.Rows[i][0].ToString().Replace("'", "''") + "'";
                OleDbCommand command = new OleDbCommand(sql, oleConnection);
                command.ExecuteNonQuery();
                oleConnection.Close();

                over++;
                //Console.WriteLine(i);
            }
        }

        private string DealStr(string str_o) {
            string str = str_o;
            str = str.Replace("\r", "").Replace("\n", "").Replace("<span>", "").Replace("</span>", "").Replace("<span class=\"highlight\">", "").Replace("<sub>", "").Replace("</sub>","").Replace("-","");

            return str;
        }
    }
}
