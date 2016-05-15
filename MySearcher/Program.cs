using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace MySearcher {
    class Program {
        static void Main(string[] args) {
            string[] strs = File.ReadAllLines("require.html");
            List<string> list = new List<string>();
            for (int i = 0; i < strs.Length; i++) {
                if (string.IsNullOrEmpty(strs[i])) {
                    continue;
                }
                string[] tmp = strs[i].Split('#');
                string str = "";
                try {
                    str = Get(tmp[0]);
                } catch (Exception) {
                }
                if (string.IsNullOrEmpty(str)) {
                    tmp[0] = "ERROR";
                //} else if (str.Contains("USO_INTERNO")) {
                } else if (str.Replace(" ","").ToLower().Contains("ifyes,checkanyifapply):")) {
                    tmp[0] = "YES";
                } else {
                    tmp[0] = "NO";
                }
                Console.WriteLine(tmp[0] + "\t" + tmp[1]);

                list.Add(tmp[0] + "\t" + tmp[1]);
            }

            File.WriteAllLines("test_en.txt", list.ToArray());
            Console.WriteLine("over");
            Console.Read();
        }

        static string Get(string uri) {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            return new StreamReader(res.GetResponseStream()).ReadToEnd();
        }
    }
}
