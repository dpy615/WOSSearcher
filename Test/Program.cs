using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test {
    class Program {

        class TitleList{

        }

        static void Main(string[] args) {
            string title_old = "Wave and particle in molecular interference lithography";
            string res = System.IO.File.ReadAllText("test.txt");
            int c = res.Length;
            int index = res.LastIndexOf(title_old);
            res = res.Substring(0, index);
            c = res.Length;
            while (res.ToCharArray()[index] != '>') {
                index = res.LastIndexOf(title_old);
                res = res.Substring(0, res.Length - index);
            }
        }

        public static int[] GetString(string str, List<string> list) {
            int[] values = null;
            List<int> allV = new List<int>();
            for (int i = 0; i < list.Count; i++) {
                if (str.Equals(list[i])) {
                    allV.Add(i);
                }
            }
            if (allV.Count > 0) {
                values = allV.ToArray();
            }
            return values;
        }
    }
}
