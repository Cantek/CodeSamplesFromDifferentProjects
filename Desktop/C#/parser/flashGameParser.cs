using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using ImageProcessing.Html;

namespace FlashGame
{
    public class Program
    {
        private StreamWriter sw = new StreamWriter("sql.txt");
        private List<string> gameNames = new List<string>();

        private int DumpPage(Uri baseUri, bool dumpMax, int catId)
        {
            int max = -1;

            string sql = "insert into items (catid, name, url, imageurl, hits, hitspd, lasthitspd, width, height) values ({0}, '{1}', '{2}', '{3}', 0, 0, 0, 640, 384);";
            string sqlName = "";
            string sqlUrl = "";
            string sqlImageUrl = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            HtmlDocument html = new HtmlDocument(sr.ReadToEnd(), true);

            if (dumpMax)
            {
                HtmlNodeCollection tds = html.Nodes.FindByName("td", true);
                foreach (HtmlElement td in tds)
                {
                    HtmlAttribute width = td.Attributes["width"];
                    HtmlAttribute height = td.Attributes["height"];
                    if ((width != null) && (height != null) && (width.Value == "707") && (height.Value == "30"))
                    {
                        HtmlNodeCollection fonts = td.Nodes.FindByName("font", true);
                        if (fonts.Count > 1)
                        {
                            HtmlElement font = (HtmlElement)fonts[1];
                            max = int.Parse(font.Text);
                        }
                    }
                }
            }

            HtmlNodeCollection tables = html.Nodes.FindByName("table", true);
            foreach (HtmlElement table in tables)
            {
                HtmlAttribute width = table.Attributes["width"];
                HtmlAttribute height = table.Attributes["height"];
                if ((width != null) && (height != null) && (width.Value == "124") && (height.Value == "112"))
                {
                    HtmlNodeCollection hrefs = table.Nodes.FindByAttributeName("href", true);
                    if (hrefs.Count > 1)
                    {
                        HtmlElement href = (HtmlElement)hrefs[1];
                        Uri gameUri = new Uri(baseUri, href.Attributes["href"].Value);
                        string[] segments = gameUri.Segments;
                        if (segments.Length > 2)
                        {
                            string n = segments[2].Remove(segments[2].Length - 1, 1);
                            sqlName = href.Text.Replace("'", "");
                            if (gameNames.IndexOf(sqlName) > -1)
                                return max;
                            gameNames.Add(sqlName);
                            sqlUrl = "http://sd1224.sivit.org/random_9080/files/" + n + ".swf";
                        }
                    }
                    HtmlNodeCollection imgs = table.Nodes.FindByName("img", true);
                    if (imgs.Count > 0)
                    {
                        HtmlAttribute src = ((HtmlElement)imgs[0]).Attributes["src"];
                        if (src != null)
                        {
                            Uri imageUri = new Uri(baseUri, src.Value);
                            sqlImageUrl = imageUri.ToString();
                            sw.WriteLine(string.Format(sql, catId, sqlName, sqlUrl, sqlImageUrl));
                            Console.WriteLine("{0} - {1}", catId, sqlName);
                        }
                    }
                }
            }
            return max;
        }

        static void Main(string[] args)
        {
            Program prg = new Program();
            string[] baseUris = new string[]
            { 
                "http://www.flash-game.net/online/action-games.php",
                "http://www.flash-game.net/online/sport-games.php",
                "http://www.flash-game.net/online/adventure-games.php",
                "http://www.flash-game.net/online/arcade-games.php",
                "http://www.flash-game.net/online/shooting-games.php",
                "http://www.flash-game.net/online/skill-games.php",
                "http://www.flash-game.net/online/puzzle-games.php",
                "http://www.flash-game.net/online/classic-games.php",
                "http://www.flash-game.net/online/fighting-games.php",
                "http://www.flash-game.net/online/board-games.php",
                "http://www.flash-game.net/online/racing-games.php",
                "http://www.flash-game.net/online/card-games.php",
                "http://www.flash-game.net/online/casino-games.php",
                "http://www.flash-game.net/online/multiplayer-games.php",
                "http://www.flash-game.net/online/kid-games.php",
                "http://www.flash-game.net/online/shockwave-games.php"
            };

            int catid = 0;
            foreach (string baseUri in baseUris)
            {
                int max = prg.DumpPage(new Uri(baseUri), true, catid);
                for (int s = 24; s < max; s += 24)
                {
                    prg.DumpPage(new Uri(String.Format("{0}?start={1}&nbl=6&order=gameID&max={2}", baseUri, s, max)), false, catid);
                }
                catid++;
            }

            prg.sw.Flush();
        }
    }
}
