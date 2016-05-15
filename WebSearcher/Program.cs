using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections;

namespace WebSearcher {
    class Program {
        static void Main(string[] args) {

            string[] results = File.ReadAllLines("result.csv");

            ArrayList l = new ArrayList();
            l.Add(results);
            l.Add(0);
            l.Add(1500);
            Thread t = new Thread(new ParameterizedThreadStart(GetRes));
            t.Start(l);

            l = new ArrayList();
            l.Add(results);
            l.Add(1500);
            l.Add(3000);
            t = new Thread(new ParameterizedThreadStart(GetRes));
            t.Start(l);

            l = new ArrayList();
            l.Add(results);
            l.Add(3000);
            l.Add(results.Length);
            t = new Thread(new ParameterizedThreadStart(GetRes));
            t.Start(l);




        }
        private static void GetRes(object o) {
            ArrayList l = (ArrayList)o;
            string[] results = (string[])l[0];
            int start = (int)l[1];
            int end = (int)l[2];
            string resFile = "";
            for (int i = start; i < end; i++) {
                string title_old = results[i].Split(',')[0];
                try {

                    string title = title_old.Replace(' ', '+').Replace("'", "%27");

                    //string uri = "http://eprints.soton.ac.uk/cgi/search/simple?q=" + title + "&_action_search=&_action_search=Search&_order=bytitle&basic_srchtype=ALL&_satisfyall=ALL";

                    string uri = "http://eprints.soton.ac.uk/cgi/search/archive/advanced?screen=Search&dataset=archive&documents_merge=ALL&documents=&eprintid=&title_merge=ALL&title=" + title + "&contributors_name_merge=ALL&contributors_name=&abstract_merge=ALL&abstract=&date=&keywords_merge=ALL&keywords=&subjects_merge=ANY&divisions_merge=ANY&department_merge=ALL&department=&refereed=EITHER&publication%2Fseries_name_merge=ALL&publication%2Fseries_name=&documents.date_embargo=&shelves.shelfid=&satisfyall=ALL&order=contributors_name%2F-date%2Ftitle&_action_search=Search";

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string res = reader.ReadToEnd();
                    if (res.Contains("Search has no matches")) {
                        resFile += title_old + "," + "NO" + "\r\n";
                    } else {
                        resFile += title_old + "," + "YES" + "\r\n";
                    }
                } catch (Exception) {
                    resFile += title_old + "," + "ERROR" + "\r\n";
                }
                Console.WriteLine(i);
                //File.WriteAllText("resFile.csv", resFile);
            }
            File.WriteAllText("resFile" + start + "-" + end + ".csv", resFile);
        }
    }
}
