using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MySearcher {
    class HttpClient {
        Dictionary<string, string> uriParams = new Dictionary<string, string>();
        string jsesionid = "";
        HttpWebRequest request;
        HttpWebResponse response;
        string location;
        string body = "";
        byte[] btbody;
        string[] locationParam;
        string cookieUri;
        string resultHtm;

        public void OpenWeb() {
            string url = "http://www.webofknowledge.com";
            request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "www.webofknowledge.com";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            response = (HttpWebResponse)request.GetResponse();

            //2
            location = response.Headers.Get("Location");
            request = (HttpWebRequest)HttpWebRequest.Create(location);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "apps.webofknowledge.com";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            string cookestring = response.Headers.Get("Set-Cookie");
            request.CookieContainer = new CookieContainer();
            locationParam = response.Headers.Get("Location").ToString().Split('?')[1].Split('&');
            foreach (var str in locationParam) {
                uriParams.Add(str.Split('=')[0], str.Split('=')[1]);
            }
            cookieUri = "http://" + request.Host;
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("SID", "\"" + uriParams["SID"] + "\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("CUSTOMER", "\"CAS National Sciences Library of Chinese Academy of Sciences\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("E_GROUP_NAME", "\"CAS Library of Beijing\""));
            response = (HttpWebResponse)request.GetResponse();

            //3
            location = response.Headers.Get("Location");
            request = (HttpWebRequest)HttpWebRequest.Create(location);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "apps.webofknowledge.com";
            request.AllowAutoRedirect = false;
            request.CookieContainer = new CookieContainer();
            locationParam = response.Headers.Get("Location").ToString().Split('?')[1].Split('&');
            jsesionid = response.Cookies["JSESSIONID"].Value;
            foreach (var str in locationParam) {
                if (!uriParams.ContainsKey(str.Split('=')[0])) {
                    uriParams.Add(str.Split('=')[0], str.Split('=')[1]);
                }
                uriParams[str.Split('=')[0]] = str.Split('=')[1];
            }
            request.Headers.Add("Conection", "Keep-Alive");
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("SID", "\"" + uriParams["SID"] + "\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("CUSTOMER", "\"CAS National Sciences Library of Chinese Academy of Sciences\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("E_GROUP_NAME", "\"CAS Library of Beijing\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("JSESSIONID", jsesionid));
            response = (HttpWebResponse)request.GetResponse();


            //4
            location = response.Headers.Get("Location");
            request = (HttpWebRequest)HttpWebRequest.Create(location);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "apps.webofknowledge.com";
            request.Headers.Add("Conection", "Keep-Alive");
            request.AllowAutoRedirect = false;
            request.CookieContainer = new CookieContainer();
            locationParam = response.Headers.Get("Location").ToString().Split('?')[1].Split('&');
            jsesionid = response.Headers.Get("Set-Cookie").ToString().Split(';')[0].Split('=')[1];
            foreach (var str in locationParam) {
                if (!uriParams.ContainsKey(str.Split('=')[0])) {
                    uriParams.Add(str.Split('=')[0], str.Split('=')[1]);
                }
                uriParams[str.Split('=')[0]] = str.Split('=')[1];
            }
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("SID", "\"" + uriParams["SID"] + "\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("CUSTOMER", "\"CAS National Sciences Library of Chinese Academy of Sciences\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("E_GROUP_NAME", "\"CAS Library of Beijing\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("JSESSIONID", jsesionid));
            response = (HttpWebResponse)request.GetResponse();
        }

        public void Search() {
            location = "http://apps.webofknowledge.com/WOS_GeneralSearch.do";
            request = (HttpWebRequest)HttpWebRequest.Create(location);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = "http://apps.webofknowledge.com/WOS_GeneralSearch_input.do?product=WOS&search_mode=GeneralSearch&SID=" + uriParams["SID"] + "&preferencesSaved=";
            request.Headers.Add("Origin", "http://apps.webofknowledge.com");
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "apps.webofknowledge.com";
            request.Headers.Add("Conection", "Keep-Alive");
            request.Headers.Add("Cache-Control", "max-age=0");
            request.AllowAutoRedirect = false;
            request.CookieContainer = new CookieContainer();
            jsesionid = response.Headers.Get("Set-Cookie").ToString().Split(';')[0].Split('=')[1];
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("SID", "\"" + uriParams["SID"] + "\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("CUSTOMER", "\"CAS National Sciences Library of Chinese Academy of Sciences\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("E_GROUP_NAME", "\"CAS Library of Beijing\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("JSESSIONID", jsesionid));


            body = "fieldCount=1&action=search&product=WOS&search_mode=GeneralSearch&SID=" +
     uriParams["SID"] +
     "&max_field_count=25&max_field_notice=%E6%B3%A8%E6%84%8F%3A%20%E6%97%A0%E6%B3%95%E6%B7%BB%E5%8A%A0%E5%8F%A6%E4%B8%80%E5%AD%97%E6%AE%B5%E3%80%82&input_invalid_notice=%E6%A3%80%E7%B4%A2%E9%94%99%E8%AF%AF%3A%20%E8%AF%B7%E8%BE%93%E5%85%A5%E6%A3%80%E7%B4%A2%E8%AF%8D%E3%80%82" +
     "&exp_notice=%E6%A3%80%E7%B4%A2%E9%94%99%E8%AF%AF%3A%20%E4%B8%93%E5%88%A9%E6%A3%80%E7%B4%A2%E8%AF%8D%E5%8F%AF%E5%9C%A8%E5%A4%9A%E4%B8%AA%E5%AE%B6%E6%97%8F%E4%B8%AD%E6%89%BE%E5%88%B0%20(&+" +
     "input_invalid_notice_limits=%20%3Cbr%2F%3E%E6%B3%A8%3A%20%E6%BB%9A%E5%8A%A8%E6%A1%86%E4%B8%AD%E6%98%BE%E7%A4%BA%E7%9A%84%E5%AD%97%E6%AE%B5%E5%BF%85%E9%A1%BB%E8%87%B3%E5%B0%91%E4%B8%8E%E4%B8%80%E4%B8%AA%E5%85%B6%E4%BB%96%E6%A3%80%E7%B4%A2%E5%AD%97%E6%AE%B5%E7%9B%B8%E7%BB%84%E9%85%8D%E3%80%82" +
     "&sa_params=WOS%7C%7C" + uriParams["SID"] + "%7Chttp%3A%2F%2Fapps.webofknowledge.com%7C'&formUpdated=true&" +
     "value(input1)=oil%20spill&value(select1)=TS&value(hidInput1)=&limitStatus=collapsed&ss_lemmatization=On&ss_spellchecking=Suggest&SinceLastVisit_UTC=&SinceLastVisit_DATE=&period=Range%20Selection&range=ALL&startYear=1864&endYear=2016&update_back2search_link_param=yes&ssStatus=display%3Anone&ss_showsuggestions=ON&ss_query_language=auto&ss_numDefaultGeneralSearchFields=1&rs_sort_by=PY.D%3BLD.D%3BSO.A%3BVL.D%3BPG.A%3BAU.A&";


            btbody = Encoding.UTF8.GetBytes(body);
            request.ContentLength = btbody.Length;
            request.GetRequestStream().Write(btbody, 0, btbody.Length);
            request.GetRequestStream().Close();
            response = (HttpWebResponse)request.GetResponse();


            //get
            location = response.Headers.Get("Location");
            request = (HttpWebRequest)HttpWebRequest.Create(location);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Host = "apps.webofknowledge.com";
            request.Headers.Add("Cache-Control", "max-age=0");
            request.AllowAutoRedirect = false;
            request.CookieContainer = new CookieContainer();
            request.Referer = "http://apps.webofknowledge.com/WOS_GeneralSearch_input.do?product=WOS&search_mode=GeneralSearch&SID=" + uriParams["SID"] + "&preferencesSaved=";

            locationParam = response.Headers.Get("Location").ToString().Split('?')[1].Split('&');
            jsesionid = response.Cookies["JSESSIONID"].Value;
            foreach (var str in locationParam) {
                if (!uriParams.ContainsKey(str.Split('=')[0])) {
                    uriParams.Add(str.Split('=')[0], str.Split('=')[1]);
                }
                uriParams[str.Split('=')[0]] = str.Split('=')[1];
            }
            request.Headers.Add("Conection", "Keep-Alive");
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("SID", "\"" + uriParams["SID"] + "\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("CUSTOMER", "\"CAS National Sciences Library of Chinese Academy of Sciences\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("E_GROUP_NAME", "\"CAS Library of Beijing\""));
            request.CookieContainer.Add(new Uri(cookieUri), new Cookie("JSESSIONID", jsesionid));
            response = (HttpWebResponse)request.GetResponse();
        }

        public void GetResultHtm() {
            System.IO.StreamReader searchReader = new System.IO.StreamReader(response.GetResponseStream());
            resultHtm = searchReader.ReadToEnd();
        }
    }
}
