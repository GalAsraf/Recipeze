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
using DAL;

namespace BL.WebScraping
{
    public class GoogleSearch
    {
        public static WebClient webClient = new WebClient();
        public static Regex extractUrl = new Regex(@"[&?](?:q|url)=([^&]+)", RegexOptions.Compiled);

        #region CustomSearch function
        public static string CustomSearch(string searchText, int userId, List<string> allergiesForUser)
        {
            StringBuilder sb = new StringBuilder("http://www.google.com/search?q=");
            allergiesForUser.ForEach(a =>
            {
                //we must check if that kind of searching works, for example: "sugar free egg free cocoa free chocolate cake recipe"
                sb.Append(a.ToString() + " free ");
            });
            sb.Append(searchText + " recipe");
            return webClient.DownloadString(sb.ToString());
        }
        #endregion

        #region ParseSearchResultHtml function
        public static List<DTO.RecipeDTO> ParseSearchResultHtml(string searchLine, string html, List<string> allergiesForUser)
        {

            List<string> searchResults = new List<string>();

            List<string> recipeImages = new List<string>();

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
                if (searchResults.Contains(test))
                    continue;
                else
                    searchResults.Add(test);
                Console.WriteLine(searchResults);

                HtmlWeb hw = new HtmlWeb();
                HtmlDocument resultdoc = hw.Load(test);
            }

            List<DTO.RecipeDTO> recipesList = new List<DTO.RecipeDTO>();
            recipesList = RecipeScraping(searchLine, searchResults, allergiesForUser);
            return recipesList;
        }
        #endregion

        #region RecipeScraping function
        //RecipeScraping function gets the filtered list of links, scrapes each link; pushes the ingredients 
        //into the list 'recipes' then the directions, and continues with all links. returns list of recipes. 
        public static List<DTO.RecipeDTO> RecipeScraping(string searchLine, List<string> links, List<string> allergiesForUser)
        {
            List<DTO.RecipeDTO> recipes = new List<DTO.RecipeDTO>();

            for (var i = 0; i < links.Count ; i++)
            {
                if (links[i].Contains("bbcgoodfood") ||
                    links[i].Contains("allrecipes") ||
                    links[i].Contains("foodnetwork") ||
                    links[i].Contains("mccormick") ||
                    links[i].Contains("leitesculinaria") )
                    continue;
                var htmlurl = links[i];//the link to scrape
                //https://www.tasteofhome.com/recipes/asian-vegetable-beef-soup
                HtmlWeb web1 = new HtmlWeb();
                var htmlDoc1 = web1.Load(htmlurl);
                var titleElement = htmlDoc1.DocumentNode.SelectSingleNode("//head/title");
                string title = null;
                if (titleElement!=null)
                {
                    title = titleElement.InnerText;
                }
                else
                {
                    title = searchLine;
                }

                //have to add title to recipes, after adding prop to object
                var ingredientElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Ingredients']");
                if (ingredientElement == null)
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
                organizedIngredients = organizedIngredients.Replace("1 \n1/2", "1 1/2");
                organizedIngredients = organizedIngredients.Replace("\n\n", "\n");

                organizedIngredients = organizedIngredients.Replace("(\n1", "(1");
                organizedIngredients = organizedIngredients.Replace("(\n2", "(2");
                organizedIngredients = organizedIngredients.Replace("(\n3", "(3");
                organizedIngredients = organizedIngredients.Replace("(\n4", "(4");
                organizedIngredients = organizedIngredients.Replace("(\n5", "(5");
                organizedIngredients = organizedIngredients.Replace("(\n6", "(6");
                organizedIngredients = organizedIngredients.Replace("(\n7", "(7");
                organizedIngredients = organizedIngredients.Replace("(\n8", "(8");
                organizedIngredients = organizedIngredients.Replace("(\n9", "(9");

                organizedIngredients = organizedIngredients.Replace("Ingredients", "");


                //still didn't take care of &...;
                organizedIngredients = organizedIngredients.Replace("  ", string.Empty);
                //organizedIngredients.Replace("\n", "");

                var directionsElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Directions']");
                if (directionsElement != null)
                {
                    Console.WriteLine("Node Name: " + directionsElement.Name + "\n" + directionsElement.OuterHtml + "\n" + directionsElement.InnerText);
                }
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
                        }
                    }
                }


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
                directions = directions.Replace(".", ".\n");
                directions = directions.Replace("&nbsp;", " ");

                flag = true;
                List<string> src = new List<string>();
                string jpgSource = null;
                var flag1 = true;
                while (flag)
                {
                    if (!ingredientParentElement.InnerHtml.Contains("img")||!flag1)
                    {
                        flag = true;
                        flag1 = true;
                        ingredientParentElement = ingredientParentElement.ParentNode;
                    }
                    else
                    {
                        //flag = false;
                        //Console.WriteLine("img found!!!!!!!!!!!!!!!");
                        //HtmlNode[] nodeItem;
                        var nodeItem = ingredientParentElement.Descendants("img").ToList();

                        foreach(var item in nodeItem)
                        {
                            
                            if (item.Attributes["src"] == null)
                                src.Add(item.Attributes["data-src"].Value);
                            else
                                src.Add(item.Attributes["src"].Value);
                                                   
                            Console.WriteLine(src);
                        }
                        foreach (var item in src)
                        {
                            if(item.Contains("jpg"))
                            {
                                jpgSource = item;
                                break;
                            }
                        }
                        if (jpgSource == null)
                        {
                            flag = true;
                            flag1 = false;
                        }
                        else
                        {
                            flag = false;
                            flag1 = true;
                        }
                    }
                }
                #region image
                ////Declare the URL
               // var url = "https://joyfoodsunshine.com/the-most-amazing-chocolate-chip-cookies/";
               //  //HtmlWeb - A Utility class to get HTML document from http
               // var web = new HtmlWeb();
               // //Load() Method download the specified HTML document from an Internet resource.
               //var doc3 = web.Load(url);

                //var rootNode = doc.DocumentNode;

                //var nodes = doc.DocumentNode.SelectNodes("//img");
                //foreach (var src in nodes)
                //{
                //    var imgSrc = src.Attributes["src"].Value;
                //    Console.WriteLine(imgSrc);
                //}
                //Console.ReadLine();


                //flag = true;
                //string recipeImage = null;
                //while (flag)
                //{
                //    if (ingredientParentElement.Descendants().Where(n => n.Element("img").Attributes["src"].Value == "").First().InnerText != null)
                //    //.InnerHtml.Contains("img"))

                //    {
                //        flag = true;
                //        parentDirectionsElement = parentDirectionsElement.ParentNode;
                //    }
                //    else
                //    {
                //        flag = false;
                //        recipeImage = ingredientParentElement.Attributes["src"].Value;
                //            //doc.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
                //    }
                //}

                //HtmlNodeCollection imgs = doc3.DocumentNode.SelectNodes("//img/@src");
                //if (imgs == null)
                //{
                //    Console.WriteLine("no images found");

                //}
                //foreach (HtmlNode img in imgs)
                //{
                //    if (img.Attributes["src"] == null)
                //        continue;
                //    string src = img.Attributes["src"].Value;
                //    //var src = img.Name;
                //    Console.WriteLine(src);
                //}
                #endregion

                DTO.RecipeDTO recipe = new DTO.RecipeDTO();
                recipe.Ingredients = organizedIngredients.Split('\n').ToList();
                recipe.Method = directions.Split('.').ToList();
                recipe.RecipeName = title;
                recipe.PictureSource = jpgSource;

                //inside checking if the recipe object contains allergic ingredients.
                var checkAllergy = 0;
                List<string> listOfAllergies = new List<string>();
                allergiesForUser.ForEach(a =>
                {
                    listOfAllergies.Add(a.ToString());
                });

                foreach (var r in recipe.Ingredients)
                {
                    if (listOfAllergies.Contains(r))
                    {
                        checkAllergy = 1;
                        continue;
                    }
                }

                if (checkAllergy == 0)
                    recipes.Add(recipe);
            }

            #region getting rid of empty lines in ingredients array
            //foreach(var recipe in recipes)
            //{
            //    for(int i = 0; i < recipe.Ingredients.Count; i++)
            //    {
            //        if(recipe.Ingredients[i]==" "||recipe.Ingredients[i]==""||recipe.Ingredients[i]=="\n")
            //        {
            //            //here I have to remove the element fron array
            //            for(int j = i; j < recipe.Ingredients.Count; j++)
            //            {
            //                recipe.Ingredients[j] = recipe.Ingredients[j + 1];
            //            }
            //            //the empty lines will be pushed to end of array, then in Angular don't show the emty lines that are at the end
            //        }
            //    }
            //}
            #endregion

            return recipes;
        }
        #endregion
    }
}


//tryings

#region trying - api key
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
#endregion

#region trying fillter function

//public static List<string> FilterListOfLinks(List<string> listOfLinks, string html)
// {
////var fillteredListOfLinks = null;
//List<string> fillteredListOfLinks = new List<string>();
//HtmlWeb web2 = new HtmlWeb();
//for (int i = 0; i < listOfLinks.Count; i++)
//{
//    var htmlDoc2 = web2.Load(listOfLinks[i]);
//    var count = 0;
//    var currentLink = listOfLinks[i];
//   // for (int j = i + 1; j < listOfLinks.Count; j++)
//   // {
//        //todo: The loop should go through the list of links, and each first link is tested,
//        // if they within the same grandparent div, and if there is another link leading to the same place.
//        //If so, it removes it from the list

//        //var linkElement = htmlDoc2.DocumentNode.ParentNode.SelectSingleNode("//*[text()='Ingredients']");

//   //     HtmlAgilityPack.HtmlNodeCollection nodes = htmlDoc2.DocumentNode.SelectNodes("//div[@class=\"rc\"]");//that the class that google givs as rap div to each recipe.
//   //     foreach (HtmlAgilityPack.HtmlNode node in nodes)
//   //     { 
//   //         HtmlNode hrefNode = node.SelectSingleNode("./href");
//   //         count++;
//   //     }
//   //     i += count;
//   // fillteredListOfLinks.Add(currentLink);

//   //// }
//}
//return fillteredListOfLinks;


// }
#endregion