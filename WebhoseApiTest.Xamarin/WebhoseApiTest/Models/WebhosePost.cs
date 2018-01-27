using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhoseApiTest.Models
{
    public class WebhosePost
    {
        public string url { get; set; } = "";
        public string title { get; set; } = "";
        public string author { get; set; } = "";
        public string text { get; set; } = "";
        public string published { get; set; } = "";
        public string crawled { get; set; } = "";
        public string ordInThread { get; set; } = "";
        public string languages { get; set; } = "";
        public List<string> persons;
        public List<string> locations;
        public List<string> organizations;
        public ThreadToken thread;

        public WebhosePost()
        {

        }

        public WebhosePost(JToken post)
        {
            url = (string)post["url"];
            title = (string)post["title"];
            author = (string)post["author"];
            text = (string)post["text"];
            published = (string)post["published"];
            crawled = (string)post["crawled"];
            ordInThread = (string)post["ord_in_thread"];
            languages = (string)post["language"];

            persons = new List<string>();
            if (post["persons"] != null)
            {
                foreach (var person in post["persons"])
                {
                    persons.Add((string)person);
                }
            }

            locations = new List<string>();
            if (post["locations"] != null)
            {
                foreach (var location in post["locations"])
                {
                    locations.Add((string)location);
                }
            }

            organizations = new List<string>();
            if (post["organizations"] != null)
            {
                foreach (var organization in post["organizations"])
                {
                    organizations.Add((string)organization);
                }
            }

            thread = new ThreadToken(post["thread"]);
        }

        public override string ToString()
        {
            string print = "url: " + url + "\n" +
                           "title: " + title + "\n" +
                           "author: " + author + "\n" +
                           "text: " + text + "\n" +
                           "published: " + published + "\n" +
                           "crawled: " + crawled + "\n" +
                           "languages: " + languages + "\n" +
                           "ord_in_thread: " + ordInThread + "\n" +
                           "persons: " + listToString(persons) + "\n" +
                           "locations: " + listToString(locations) + "\n" +
                           "organizations: " + listToString(organizations) + "\n" +
                           "thread:\n" +
                           "{\n" + thread + "\n}";

            return print;
        }

        private string listToString(List<string> list)
        {
            int countItems = 0;
            string listString = "[";
            foreach (var item in list)
            {
                listString += "\n";
                if (countItems < list.Count - 1)
                    listString += "\"" + ((string)item) + "\",";
                else
                    listString += "\"" + ((string)item) + "\"";
                countItems++;
            }
            listString += "\n],";
            return listString;
        }

    }
}
