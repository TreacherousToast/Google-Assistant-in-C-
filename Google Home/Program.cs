using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Google_Home
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
			    Console.WriteLine("Ready");
			    string[]acceptableClassNames = {"V","e"};
			    string question = Console.ReadLine();
			    question = question.Replace(' ','+');
                string urlAddress = "https://google.com/search?q="+question;
			    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
			    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			    if (response.StatusCode == HttpStatusCode.OK)
			    {
  				    Stream receiveStream = response.GetResponseStream();
  				    StreamReader readStream = null;

  				    if (response.CharacterSet == null)
  				    {
     				    readStream = new StreamReader(receiveStream);
  				    }
  				    else
  				    {
     				    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
  				    }

  				    string data = readStream.ReadToEnd();

  				    response.Close();
  				    readStream.Close();
				    bool containsClass = false;
				    for (int i = 0; i < acceptableClassNames.Length; i++)
				    {
					    if (data.Contains("<div class=\""+acceptableClassNames[i]) == true)
					    {
						    int position = data.IndexOf("<div class=\""+acceptableClassNames[i]);
						    data = data.Remove(0,position-2); // <h3 class="r" doesn't exists sometimes
						    data = data.Remove(data.IndexOf("<h3 class=\"r\">"),data.Length-data.IndexOf("<h3 class=\"r\">")); // startIndex cannot be less than 0
						    data = Regex.Replace(data, @" ?\<.*?\>", " ");
						    data = Regex.Replace(data, @" ?\&.*?\;"," ");
						    data = data.Remove(0,2);
						    Console.WriteLine(data);
						    containsClass = true;
						    break;
					    }
				    }
				    if (containsClass == false)
				    {
					    Console.WriteLine("I don't understand");
				    }
			    }
            }
        }
    }
}