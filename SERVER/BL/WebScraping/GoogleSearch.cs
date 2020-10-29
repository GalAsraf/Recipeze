using Google.Apis.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing.Printing;
using HtmlAgilityPack;
using System.Web.UI;
using Nancy.Helpers;

namespace BL.WebScraping
{
    public class GoogleSearch
    {
        public static WebClient webClient = new WebClient();
        public static Regex extractUrl = new Regex(@"[&?](?:q|url)=([^&]+)", RegexOptions.Compiled);


        public static string CustomSearch(string searchText)
        {
            //string apiKey = "nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCu3UfnJ1xAPHlf\nyT7H+lipgXzodlh2T24VnT+OsVse/BTsdHvmDt9KjirtnxoXUMc1z4UUVbTytiK6\neGeUeNJYI8UGrejKELk0BAFOMyCiW0h5BPZD3avosluHOUYGCdru5nG2t26u+QWf\nglyINFTH54gCHBbDNWSzqdSSgIBuHPdpvytp/OCyCfMKxB+hBUSev/y8BWXmStcg\nCxZXM5ubRsDQ19bFniAkkea6QTe4ThMYRGlNOJdb1xc1Fcn0x86umnQ/eqE4aKFI\npWd0YnSMvHMzNUg2LiXuttHgHr8PZ3C9wd0CcZ4OgprfnncEM7OHpNZmlMV/r36Y\nHEVEqunrAgMBAAECggEACPDWKXEPUVbUdBvGS4yhjwPWy1scYlyr/yhBITrbXaQ0\noFbf7DgRj5IaNZSPahFT7/5pbfa9qm3zZYMq9RPJS7f07eMqe6/EGjZOqB0nVjsl\n8bMEXHKhUuPfQp9nuH7gDw2rjBvhzR8nwx07BKBDBlvUBuJBOlwov/fTrYHySARI\nkGk4jPlhcQ23qH4PYEymZkVZi9d+TLoDXJwqXPXsi+ZtRlmGvQKaDDnZ6UAm66bv\n+XLMoqGFx1DFYXhg4T5wy+6FGL13xro7YW8azUSAFtNo+dwXPuiMHFo89ldc7z9q\neLU2uHrHtwaagpgkMjN8N7ISKQPP/GTMrY2WkJpt8QKBgQDiIpNjVDi3CfqlVPIc\n1RPYUpMdc5Oz4BRZl680pqmz5VM+JeJONKTipJvsxJTE1udp0v1R93oskeUcWT06\njyFq1DhkX65B9Ns6GpxheBXlmXShQBIexjXDGudqnYUC2LUbJtPrh4MspQ5ZYSV4\nkd51v54hnZh4BBHMM7HDHTXEFwKBgQDF9UoHjE6DNleslH+hXuVq64JMeXraMU+p\nLzsB1+drZlaivxV4LRzlguwv4VAlt4MTYgdriOJcU/8IfXQuTI7+B2zMRsGWCNgp\noPwwAn+Ftxm5m/ZpBpdeOcho/54NRHA6uNtp8LTxc8TZLd2kgkgwE0xbn1wDCfG4\nZGmmierpTQKBgEICCbfCy9NSDGHaS9nysJpCcEL2i7TDweztA+2AgKTMWeIYONjP\nMRofJoyUTUCv4ljXh643aOg9pf0CZ4cCZKTEUbmq3DjQenWZcvBYlzuv8YVoKGHn\nRaYv4kESvdK44xSL3uwvYDDV9TxNyRxKp/8C8euqDulpdrB+nnLvwdP9AoGAD5ha\nY6vXB5lBYPQ19dWPB1RUaIfteMEHwJFa+bMzpQ9j5eBd5aDQNPiSeNcsRDxn1CAV\n64/WHWX0oouXmoonfbXCCXnNiG9b8DOhinq35yXcnfW+fNmrFR5CPptcrTjmCopD\npt3ys07mhCGL44jr/PWYP2OXkRm4dElc1WTqH8UCgYEAx2Xz26jySwBdSNWWWwVN\ncPDz7q5SBoXGOLj0RWNR4DdIDRCXRf31j3QCo6MPNgi2XXNbO+razCuveXV72RGi\n9x+9eheoUa1xQ3zCpVJomTv6TmKwIrn2XxUb3U+2T5vhFHP8JNmPvuQ7RHYH0Sa+\nvdXff8zYgzOpaqxJFGSxePQ=";
            //string cx = "e565f9cc6248f12a3";
            //string query = "cake";

            //string result =new WebClient().DownloadString(String.Format("https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}&alt=json", apiKey, cx, query));
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //Dictionary<string, object> collection = serializer.Deserialize<Dictionary<string, object>>(result);
            //foreach (Dictionary<string, object> item in (IEnumerable)collection["items"])
            //{
            //    Console.WriteLine("Title: {0}", item["title"]);
            //    Console.WriteLine("Link: {0}", item["link"]);
            //    Console.WriteLine();
            //}
            StringBuilder sb = new StringBuilder("http://www.google.com/search?q=");
            sb.Append(searchText+" recipe");

            //in addition we have to append the users allergies according to current user
            return webClient.DownloadString(sb.ToString());
        }


        public static List<String> ParseSearchResultHtml(string html)
        {
            
            List<String> searchResults = new List<string>();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var nodes = (from node in doc.DocumentNode.SelectNodes("//a")
                         let href = node.Attributes["href"]
                         where null != href
                         where href.Value.Contains("/url?") || href.Value.Contains("?url=")
                         select href.Value).ToList();

            foreach (var node in nodes)
            {
                var match = extractUrl.Match(node);
                string test = HttpUtility.UrlDecode(match.Groups[1].Value);
                searchResults.Add(test);
                Console.WriteLine(searchResults);

                HtmlWeb hw = new HtmlWeb();
                HtmlDocument resultdoc = hw.Load(test);

            }

            //  string URL = "https://www.google.com/search?safe=active&q=cookies&pli=1";
            string URL = searchResults[0];
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument difdoc = new HtmlAgilityPack.HtmlDocument();
            difdoc = web.Load(URL);
            // extracting all links
            foreach (HtmlNode link in difdoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                if (att.Value.Contains("a"))
                {
                    // showing output
                    Console.WriteLine(att.Value);
                    searchResults.Add(att.Value);
                }

            }

            Console.WriteLine(searchResults[0]);

            List<String> filteredlistOfLinks = new List<string>();
            filteredlistOfLinks = FilterListOfLinks(searchResults);
            List<String> recipesList = new List<string>();
            recipesList = RecipeScraping(filteredlistOfLinks);

            return recipesList;
        }

        public static List<string> FilterListOfLinks(List<string> listOfLinks)
        {
            //taking out links that appear twice or more
            for(var i =0; i<listOfLinks.Count;i++)
            {
                for(var j = i+1; j < listOfLinks.Count; j++)
                {
                    if (listOfLinks[i] == listOfLinks[j])
                    {
                        listOfLinks.Remove(listOfLinks[j]);
                    }
                }
            }

            //at this point RecipeScraping can be called, sending the list of links, it will return the list of recipes
            //and the we can return the list of recipes.
            
            return listOfLinks;
        }



        //RecipeScraping function gets the filtered list of links, scrapes each link; pushes the ingredients 
        //into the list 'recipes' then the directions, and continues with all links. returns list of recipes. 
        public static List<string> RecipeScraping(List<string> links)
        {
            List<string> recipes = new List<string>();

            //links.count
            for(var i=0; i < 4; i++)
            {
                var htmlurl = links[i];//the link to scrape

                HtmlWeb web1 = new HtmlWeb();

                var htmlDoc1 = web1.Load(htmlurl);

                var ingredientElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Ingredients']");
                if(ingredientElement==null)
                {
                    continue;//meaning-> if ingredient element wasn't found, end this round in loop and do i++
                }
                Console.WriteLine("Node Name: " + ingredientElement.Name + "\n" + ingredientElement.OuterHtml + "\n" + ingredientElement.InnerText);

                //this code doesn't work on allrecipes, foodnetwork, bbcgoodfoods.
                var ingredientParentElement = ingredientElement.ParentNode;
                bool flag = true;
                //in reality this doesn't work accurately!

                while (flag)
                {
                    if (ingredientElement.InnerText == ingredientParentElement.InnerText)
                    {
                        flag = true;
                        ingredientParentElement = ingredientParentElement.ParentNode;
                    }
                    else
                        flag = false;
                }

                string ingredients = ingredientParentElement.InnerText;
                Console.WriteLine(ingredients);
                string organizedIngredients = ingredients.Replace("1", "\n1");
                organizedIngredients = organizedIngredients.Replace("2", "\n2");
                organizedIngredients = organizedIngredients.Replace("3", "\n3");
                organizedIngredients = organizedIngredients.Replace("4", "\n4");

                organizedIngredients = organizedIngredients.Replace("1 1/3", "\n1 1/3");

                organizedIngredients = organizedIngredients.Replace("/\n2", "/2");
                organizedIngredients = organizedIngredients.Replace("/\n3", "/2");
                organizedIngredients = organizedIngredients.Replace("/\n4", "/4");

                organizedIngredients = organizedIngredients.Replace("1/2", "\n1/2");
                organizedIngredients = organizedIngredients.Replace("1/3", "\n1/3");
                organizedIngredients = organizedIngredients.Replace("1/4", "\n1/4");

                organizedIngredients = organizedIngredients.Replace("½", "\n½");
                organizedIngredients = organizedIngredients.Replace("¼", "\n¼");
                organizedIngredients = organizedIngredients.Replace("⅓", "\n⅓");

                organizedIngredients = organizedIngredients.Replace("&#x\n25a\n2;", "\n");

                //the problem here is, how can I make it print 1 1/2 ?
                organizedIngredients = organizedIngredients.Replace("1\n1/2", "1 1/2");

                //organizedIngredients = organizedIngredients.Replace("\n\n\n\n", string.Empty);
                organizedIngredients = organizedIngredients.Replace("\n\n", "\n");
                //organizedIngredients = organizedIngredients.Replace("\n\n", "\n");
                //organizedIngredients = organizedIngredients.Replace("\n\n", "\n");
                //organizedIngredients = organizedIngredients.Replace("\n\n", "\n");


                //still didn't take care of &...;
                organizedIngredients = organizedIngredients.Replace("  ", string.Empty);
                //organizedIngredients.Replace("\n", "");

                Console.WriteLine(organizedIngredients);

                var directionsElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Directions']");
                if (directionsElement != null)
                {
                    Console.WriteLine("Node Name: " + directionsElement.Name + "\n" + directionsElement.OuterHtml + "\n" + directionsElement.InnerText);
                }
                //Console.WriteLine("Node Name: " + directionsElement.Name + "\n" + directionsElement.OuterHtml + "\n" + directionsElement.InnerText);
                else
                {
                    directionsElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Instructions']");
                    if (directionsElement != null)
                    {
                        Console.WriteLine("Node Name: " + directionsElement.Name + "\n" + directionsElement.OuterHtml + "\n" + directionsElement.InnerText);
                    }
                    else
                    {
                        directionsElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Method']");
                        if (directionsElement == null)
                        {
                            Console.WriteLine("direction element not found");
                            continue; //leave this step in loop - i++
                        }
                        else
                        {
                            Console.WriteLine("Node Name: " + directionsElement.Name + "\n" + directionsElement.OuterHtml + "\n" + directionsElement.InnerText);

                            var parentDirectionsElement = directionsElement.ParentNode;

                            flag = true;

                            while (flag)
                            {
                                if (directionsElement.InnerText == parentDirectionsElement.InnerText)
                                {
                                    flag = true;
                                    parentDirectionsElement = parentDirectionsElement.ParentNode;
                                }
                                else
                                    flag = false;
                            }

                            string directions = parentDirectionsElement.InnerText;
                            Console.WriteLine(directions);

                            string organizedDirections = directions.Replace(".", ".\n");
                            Console.WriteLine(organizedDirections);


                            recipes.Add(organizedIngredients);
                            recipes.Add(organizedDirections);

                        }
                    }
                }


            }

            return recipes;
        }
    }
}


