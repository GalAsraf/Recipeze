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
using System.Threading;
using System.Drawing;

namespace BL.WebScraping
{
    /// <summary>
    /// GoogleSearch is a class where all the recipes retrieval actions from google take place.
    /// It is done by Web Scraping recipes from websites google gives when doing a google search. 
    /// </summary>
    public class GoogleSearch
    {
        const int count = 0;
        public static WebClient webClient = new WebClient();
        public static Regex extractUrl = new Regex(@"[&?](?:q|url)=([^&]+)", RegexOptions.Compiled);
        /// <summary>
        /// Method makes a searchline to search on google
        /// </summary>
        /// <param name="searchText" type="string"> it is the search text that is used to search on google. </param>
        /// <param name="allergiesForUser" type="List<string>"> the current user's allergies / sensitivities</param>
        /// <returns> the HTML google page of the search result that contains links of websites with recipes to scrape. </returns>
        #region CustomSearch function

        public static string CustomSearch(string searchText, List<string> allergiesForUser)
        {
            StringBuilder sb = new StringBuilder("http://www.google.com/search?q=");
            //the searchline includes each allergy the user has plus the word "free" in order not to include it in recipes ingredients.
            allergiesForUser.ForEach(a =>
            {
                sb.Append(a.ToString() + " free ");
            });
            sb.Append(searchText + " recipe");
            return webClient.DownloadString(sb.ToString());
        }
        #endregion

        /// <summary>
        /// A method that extracts all links from the google result web page and puts it in a list.
        /// the method sends the list of links to the web scraping method: RecipeScraping. 
        /// </summary>
        /// <param name="searchLine" type="string">  it is the search text that is used to search on google. </param>
        /// <param name="html" type="string"> HTML page that was returned from CustomSearch method, which from it the links are taken</param>
        /// <param name="allergiesForUser" type="List<string>"> the current user's allergies / sensitivities </param>
        /// <returns type="List<DTO.RecipeDTO>"> list of recipes which were extracted from web pages </returns>
        #region ParseSearchResultHtml function

        public static List<DTO.RecipeDTO> ParseSearchResultHtml(string searchLine, string html, List<string> allergiesForUser)
        {
            List<DTO.RecipeDTO> recipesList = new List<DTO.RecipeDTO>();
            List<List<DTO.RecipeDTO>> recipesLists = new List<List<DTO.RecipeDTO>>();
            List<string> searchResults = new List<string>();
          
            //using package called HtmlAgilityPack to extract data from a website
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            //extracts all links from google result web page's HTML, by taking the content of the href in every <a> element.
            var nodes = (from node in doc.DocumentNode.SelectNodes("//a")
                         let href = node.Attributes["href"]
                         where null != href
                         where href.Value.Contains("/url?") || href.Value.Contains("?url=")
                         select href.Value).ToList();


            //var nextLink = nextParentElement.Attributes["href"];

            //filtering multiplied links, putting links in a new list.
            foreach (var node in nodes)
            {
                var match = extractUrl.Match(node);
                string test = HttpUtility.UrlDecode(match.Groups[1].Value);
                if (searchResults.Contains(test))
                    continue;
                else
                    searchResults.Add(test);
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument resultdoc = hw.Load(test);
            }


            //calling RecipeScraping method where the process of recipe scraping- recipe extracting, is done
            recipesList = RecipeScraping(searchLine, searchResults, allergiesForUser);
        
            return recipesList;
        }
        #endregion


        /// <summary>
        /// gets the filtered list of links, extracts a recipe from each link's HTML web page
        /// </summary>
        /// <param name="searchLine" type="string"> it is the search text that is used to search on google </param>
        /// <param name="links" type="List<string>"> list of links that were extracted from google page  </param>
        /// <param name="allergiesForUser" type="List<string>"> the current user's allergies / sensitivities </param>
        /// <returns type="List<DTO.RecipeDTO>"> list of recipes from the recipe websites </returns>
        #region RecipeScraping function


        public static List<DTO.RecipeDTO> RecipeScraping(string searchLine, List<string> links, List<string> allergiesForUser)
        {
            List<DTO.RecipeDTO> recipes = new List<DTO.RecipeDTO>();
            //loops over list of links, and axtract recipe from each links HTMl page
            for (var i = 0; i < links.Count; i++)
            {
                //websites to ignore and not scrape because there HTML structure is irregular
                if (links[i].Contains("bbcgoodfood") ||
                    links[i].Contains("bbcfood") ||
                    links[i].Contains("bbc") ||
                    links[i].Contains("sugarfreemom") ||
                    links[i].Contains("veggieinspired") ||
                    links[i].Contains("hersheys") ||
                    links[i].Contains("dovesfarm") ||
                    links[i].Contains("allrecipes") ||
                    links[i].Contains("foodnetwork") ||
                    links[i].Contains("mccormick") ||
                    links[i].Contains("delish") ||
                    links[i].Contains("pinchofyum") ||
                    links[i].Contains("dadcooksdinner") ||
                    links[i].Contains("thepioneerwoman") ||
                    links[i].Contains("thekitchn") ||
                    links[i].Contains("myfoodstory") ||
                    links[i].Contains("littlehouseliving") ||
                    links[i].Contains("NatashasKitchen") ||
                    links[i].Contains("taste") ||
                    links[i].Contains("forksnflipflops") ||
                    links[i].Contains("asweetpeachef") ||
                    links[i].Contains("HERSHEY's") ||
                    links[i].Contains("myalbanianfood") ||
                    links[i].Contains("biggerbolderbaking") ||
                    links[i].Contains("wholesomeyum") ||
                    links[i].Contains("fussfreeflavours") ||
                    links[i].Contains("rasamalaysia") ||
                    links[i].Contains("leitesculinaria"))
                continue;

                var htmlurl = links[i];//the link to scrape

                HtmlWeb web1 = new HtmlWeb();
                var htmlDoc1 = web1.Load(htmlurl);
                //extracting the recipe's title from the <title> element
                var titleElement = htmlDoc1.DocumentNode.SelectSingleNode("//head/title");
                string title = null;

                //checks if <title> element exists, if does, takes it's inner text as the title
                //if not, uses the searchline as the recipe's title
                if (titleElement != null)
                {
                    title = titleElement.InnerText;
                    title = title.Replace("&amp", "&");
                    title = title.Replace("&#039", "'");
                    title = title.Replace("&#034", "");
                    title = title.Replace(";", "");
                    title = title.Replace("&ndash", "-");
                    title = title.Replace("&frasl", "/");
                    title = title.Replace("&quot", "");
                }
                else
                {
                    title = searchLine;
                }

                //finding the element which only contains the word 'Ingredients' as it's inner text
                var ingredientElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Ingredients']");
                if (ingredientElement == null)
                {
                    continue;
                }

                //ingredientParentElement gets the parent of ingredientElement
                var ingredientParentElement = ingredientElement.ParentNode;
                if (ingredientParentElement == null)
                {
                    continue;
                }
                bool flag = true;

                //fiinding the node that contains the ingredients
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

                //filtering and fixing up the ingredients which were extracted from the HTML web page as a long string

               

                string organizedIngredients;
                string[] array1 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                string[] array2 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                string[] array3 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };


                organizedIngredients = ingredients.Replace("\n"," ");
                organizedIngredients = organizedIngredients.Replace("1", "\n1");

                organizedIngredients = organizedIngredients.Replace("2", "\n2");
                organizedIngredients = organizedIngredients.Replace("3", "\n3");
                organizedIngredients = organizedIngredients.Replace("4", "\n4");
                organizedIngredients = organizedIngredients.Replace("5", "\n5");
                organizedIngredients = organizedIngredients.Replace("6", "\n6");
                organizedIngredients = organizedIngredients.Replace("7", "\n7");
                organizedIngredients = organizedIngredients.Replace("8", "\n8");
                organizedIngredients = organizedIngredients.Replace("9", "\n9");
                organizedIngredients = organizedIngredients.Replace("1 \n1/3", "1 1/3");
                organizedIngredients = organizedIngredients.Replace("/\n2", "/2");
                organizedIngredients = organizedIngredients.Replace("/\n3", "/2");
                organizedIngredients = organizedIngredients.Replace("/\n4", "/4");
                organizedIngredients = organizedIngredients.Replace("1/2", "\n1/2");
                organizedIngredients = organizedIngredients.Replace("1/3", "\n1/3");
                organizedIngredients = organizedIngredients.Replace("1/4", "\n1/4");
                organizedIngredients = organizedIngredients.Replace("½", "\n½");
                organizedIngredients = organizedIngredients.Replace("¼", "\n¼");
                organizedIngredients = organizedIngredients.Replace("⅓", "\n⅓");
                //organizedIngredients = organizedIngredients.Replace("&#32;", " ");
                organizedIngredients = organizedIngredients.Replace("&#x\n25a\n2;", "");
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

                organizedIngredients = organizedIngredients.Replace("\n1x", " ");
                organizedIngredients = organizedIngredients.Replace("\n2x", " ");
                organizedIngredients = organizedIngredients.Replace("\n3x", " ");
                organizedIngredients = organizedIngredients.Replace("\n&#\n \n \n \n ;", " ");
                organizedIngredients = organizedIngredients.Replace("&quot;", " ");


                //getting rid of unicode universal character sets from ingredients
                List<string> icons = new List<string>();
                List<char> fixedList = new List<char>();

                char[] IngredientsArray = ingredients.ToCharArray();
                for (int x = 0; x < IngredientsArray.Length; x++)
                {
                    if (IngredientsArray[x] == '&' && (IngredientsArray[x + 1] == '#'))
                    {
                        while (IngredientsArray[x] != ';')
                        {
                            fixedList.Add(IngredientsArray[x]);
                            x++;
                        }
                        fixedList.Add(IngredientsArray[x]);
                        fixedList.Add(IngredientsArray[x]);
                        fixedList.Add(IngredientsArray[x]);
                        var myString = new string(fixedList.ToArray());
                        fixedList.Clear();
                        icons.Add(myString); //icons contains all the unicodes for ex. &#7839;
                    }
                }
                //foreach icon found in ingredients checking type of icon (length, number, or letter), and removing it from ingredients so that it won't appear in results
                //knowing if number or letter by checking if has "\n" before -> if does, it's a number
                foreach (var ic in icons)
                {
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ";", "");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", "");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", "");
                    organizedIngredients = organizedIngredients.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + ic[4] + "\n" + ic[5] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ic[5] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ic[5] + "\n" + ic[6] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ic[6] + ";", " ");
                    organizedIngredients = organizedIngredients.Replace("&#" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ic[6] + ";", " ");

                }
                
                organizedIngredients = organizedIngredients.Replace("&#x", "");
                organizedIngredients = organizedIngredients.Replace("&#\n32", " ");


                foreach (var num1 in array1)
                {
                    foreach (var num2 in array2)
                    {
                        organizedIngredients = organizedIngredients.Replace("frac\n" + num1 + "\n" + num2, num1 + "/" + num2);
                        organizedIngredients = organizedIngredients.Replace("&frac" + num1 + num2 + ";", num1 + "/" + num2);
                    }

                }
                //organizedIngredients = organizedIngredients.Replace("frac\n1\n2", " 1/2");
                //organizedIngredients = organizedIngredients.Replace("frac\n3\n4", " 3/4");
                //organizedIngredients = organizedIngredients.Replace("frac\n1\n4", " 1/4");
                //organizedIngredients = organizedIngredients.Replace("frac\n1\n3", " 1/3");
                //organizedIngredients = organizedIngredients.Replace("&frac12;", " 1/2");
                //organizedIngredients = organizedIngredients.Replace("&frac12;", " 1/2");
                //organizedIngredients = organizedIngredients.Replace("&frac34;", " 3/4");
                //organizedIngredients = organizedIngredients.Replace("&frac14;", " 1/4");
                //organizedIngredients = organizedIngredients.Replace("&frac13;", " 1/3");




                // organizedIngredients = organizedIngredients.Replace("&#", "");
                organizedIngredients = organizedIngredients.Replace("&amp", " &");
                organizedIngredients = organizedIngredients.Replace(";", "");
                organizedIngredients = organizedIngredients.Replace("&nbsp", " ");
                organizedIngredients = organizedIngredients.Replace("Scale", "");




                foreach (var num1 in array1)
                {
                    foreach (var num2 in array2)
                    {
                        organizedIngredients = organizedIngredients.Replace(num1 + "\n" + num2, num1 + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n" + num2, num1 + " " + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " - \n" + num2, num1 + "-" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " -\n" + num2, num1 + "-" + num2);
                        organizedIngredients = organizedIngredients.Replace(" - \n" + num2, " -" + num2);
                        organizedIngredients = organizedIngredients.Replace(" -\n" + num2, " -" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + "- \n" + num2, num1 + "-" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + "-\n" + num2, num1 + "-" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + "\n." + num2, num1 + "." + num2);
                        organizedIngredients = organizedIngredients.Replace("\n" + num2 + ")", num2 + ")");
                        organizedIngredients = organizedIngredients.Replace("(\n" + num1, "(" + num1);
                        organizedIngredients = organizedIngredients.Replace("and \n" + num1, "and " + num1);
                        organizedIngredients = organizedIngredients.Replace("and \n" + num1 + "/" + num2, "and " + num1 + "/" + num2);
                        organizedIngredients = organizedIngredients.Replace("& \n" + num1, "& " + num1);
                        organizedIngredients = organizedIngredients.Replace("& \n" + num1 + "/" + num2, "& " + num1 + "/" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + "|\n" + num2, num1 + "|" + num2);
                        organizedIngredients = organizedIngredients.Replace("g |\n" + num2, "g |" + num2);
                        organizedIngredients = organizedIngredients.Replace("z |\n" + num2, "z |" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " |\n" + num2, num1 + "|" + num2);
                        organizedIngredients = organizedIngredients.Replace("g | \n" + num2, "g |" + num2);
                        organizedIngredients = organizedIngredients.Replace("z | \n" + num2, "z |" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " | \n" + num2, num1 + " | " + num2);
                        organizedIngredients = organizedIngredients.Replace(". | \n" + num2, ". | " + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + ".\n" + num2, num1 + "." + num2);
                        organizedIngredients = organizedIngredients.Replace("/\n" + num2, "/" + num2);
                        organizedIngredients = organizedIngredients.Replace("/ \n" + num2, "/" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n½", num1 + " ½");
                        organizedIngredients = organizedIngredients.Replace(num1 + "\n½", num1 + " ½");
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n¼", num1 + " ¼");
                        organizedIngredients = organizedIngredients.Replace(num1 + "\n¼", num1 + " ¼");
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n⅓", num1 + " ⅓");
                        organizedIngredients = organizedIngredients.Replace(num1 + "\n⅓", num1 + " ⅓");
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n" + "1/", num1 + " "  + "1/"+num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n" + "2/", num1 + " " + "2/" + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + " \n" + "3/", num1 + " " + "3/" + num2);
                        organizedIngredients = organizedIngredients.Replace(",\n"+num1, ","+num1);
                        organizedIngredients = organizedIngredients.Replace(", \n"+num1, ", "+num1);
                        organizedIngredients = organizedIngredients.Replace("to \n"+num1, "to "+num1);
                        organizedIngredients = organizedIngredients.Replace("or \n"+num1, "or "+num1);
                        organizedIngredients = organizedIngredients.Replace(num1+" x \n"+num2, num1 + " x " + num2);
                        organizedIngredients = organizedIngredients.Replace(num1 + "x\n" + num2, num1 + "x" + num2);
                        organizedIngredients = organizedIngredients.Replace("\n"+num1 + ":\n"+num2, "  "+num1 + ":"+num2);
                        organizedIngredients = organizedIngredients.Replace(" +\n"+num1, " + "+num1);
                        organizedIngredients = organizedIngredients.Replace(" +\n "+num1, " + "+num1);
                        organizedIngredients = organizedIngredients.Replace(" + \n"+num1, " + "+num1);
                        organizedIngredients = organizedIngredients.Replace(" + \n "+num1, " + "+num1);
                        organizedIngredients = organizedIngredients.Replace(" + \n", " + ");

                    }
                }
                organizedIngredients = organizedIngredients.Replace("25a2", "");
                //   organizedIngredients = organizedIngredients.Replace("25a", "");
                organizedIngredients = organizedIngredients.Replace("25a\n2", "");
                organizedIngredients = organizedIngredients.Replace("25a\n 2", "");
                organizedIngredients = organizedIngredients.Replace("25a \n 2", "");
                organizedIngredients = organizedIngredients.Replace("25a\n  2", "");
                organizedIngredients = organizedIngredients.Replace("25a \n2", "");
                organizedIngredients = organizedIngredients.Replace("25a  \n2", "");
                organizedIngredients = organizedIngredients.Replace("&frac\n1\n2", " 1/2");
                organizedIngredients = organizedIngredients.Replace("*", "");
                organizedIngredients = organizedIngredients.Replace("&ndash","-");
                organizedIngredients = organizedIngredients.Replace("&frasl", "/");
                organizedIngredients = organizedIngredients.Replace("&#\n32", " ");



                List<string> parenthesis = new List<string>();
                List<char> counting = new List<char>();

                for (int x = 0; x < IngredientsArray.Length; x++)
                {
                    if (IngredientsArray[x] == '(')
                    {
                        while (IngredientsArray[x] != ')')
                        {
                            counting.Add(IngredientsArray[x]);
                            x++;
                        }
                        counting.Add(IngredientsArray[x]);
                        counting.Add(IngredientsArray[x]);
                        counting.Add(IngredientsArray[x]);
                        parenthesis.Add(new string(counting.ToArray()));
                        //parenthesis = parenthesis[0].Replace();
                        counting.Clear();

                    }
                }
                //foreach (var count in counting)
                //{
                //    counting = counting.Replace("\n", " ");
                ////}
                //for (int z = 0; z < parenthesis.Count; z++)
                //{
                //    if (parenthesis[z] == "\n")
                //    {
                //        organizedIngredients = organizedIngredients.Replace(parenthesis[z] , "");

                //    }
                //}
                //parenthesis.Clear();
                //for (int g = 0; g < counting.Count; g++) 
                //{
                //    organizedIngredients = organizedIngredients.Replace(counting[g]+"\n", counting[g]+" ");
                //}
                



                organizedIngredients = organizedIngredients.Replace("Ingredients", "");

                organizedIngredients = organizedIngredients.Replace("  ", string.Empty);


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
                directions = directions.Replace("\n)", ")");
                directions = directions.Replace("&nbsp;", " ");
                directions = directions.Replace("&amp;", "&");
                directions = directions.Replace(";", "");
                directions = directions.Replace("Instructions", "");
                directions = directions.Replace("Method", "");
                directions = directions.Replace("&quot;", "");
                directions = directions.Replace("&#8217", "'");
                directions = directions.Replace("&#8211", "'");
                directions = directions.Replace("&#039", "'");
                directions = directions.Replace("&#39", "'");
                directions = directions.Replace("&#x", "'");
                directions = directions.Replace("&#x27", "'");
                directions = directions.Replace("25a2", "");
                foreach (var num1 in array1)
                {
                    foreach (var num2 in array2)
                    {
                        directions = directions.Replace("frac\n" + num1 + "\n" + num2, num1 + "/" + num2);
                        directions = directions.Replace("&frac" + num1 + num2 + ";", num1 + "/" + num2);
                    }
                }

                //getting rid of &#...;

                //List<string> directionicons = new List<string>();
                //List<char> directionfixedlist = new List<char>();

                //char[] directionsarray = directions.ToCharArray();
                //for (int x = 0; x < directionsarray.Length; x++)
                //{
                //    if (directionsarray[x] == '&' && (directionsarray[x + 1] == '#'))
                //    {
                //        while (directionsarray[x] != ';')
                //        {
                //            directionfixedlist.Add(directionsarray[x]);
                //            x++;
                //        }
                //        directionfixedlist.Add(directionsarray[x]);
                //        directionfixedlist.Add(directionsarray[x]);
                //        directionfixedlist.Add(directionsarray[x]);


                //        var mystring = new string(directionfixedlist.ToArray());
                //        directionfixedlist.Clear();
                //        directionicons.Add(mystring);
                //    }
                //}

                //foreach (var ic in directionicons)
                //{
                //    directions = directions.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ";", "");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", "");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", "");
                //    directions = directions.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ";", " ");
                //    directions = directions.Replace("&#" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + ic[4] + "\n" + ic[5] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + ic[4] + "\n" + ic[5] + "\n" + ic[6] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ic[5] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + ic[5] + "\n" + ic[6] + ";", " ");
                //    directions = directions.Replace("&#\n" + ic[2] + "\n" + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ic[6] + ";", " ");
                //    directions = directions.Replace("&#" + ic[2] + ic[3] + "\n" + ic[4] + "\n" + ic[5] + ic[6] + ";", " ");

                //}


                flag = true;
                List<string> src = new List<string>();
                string jpgSource = null;
                var flag1 = true;
                string prepTime=null;
                string cookTime = null;
                string totalTime = null;
                while (flag)
                {
                    if (ingredientParentElement == null)
                    {
                        continue;
                    }
                    else if(ingredientParentElement.InnerHtml!=null)
                    {
                        if (!ingredientParentElement.InnerHtml.Contains("img") || !flag1)
                        {
                            flag = true;
                            flag1 = true;
                            ingredientParentElement = ingredientParentElement.ParentNode;
                        }
                        else
                        {
                            var nodeItem = ingredientParentElement.Descendants("img").ToList();

                            foreach (var item in nodeItem)
                            {
                                if (item.Attributes["src"] == null)
                                {
                                    if (item.Attributes["data-src"] != null)
                                        src.Add(item.Attributes["data-src"].Value);
                                }
                                else
                                    src.Add(item.Attributes["src"].Value);

                                Console.WriteLine(src);
                            }
                            foreach (var item in src)
                            {
                                if (item.Contains("jpg"))
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
                }

                //extracting prep time

                var prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Prep Time']");
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Prep Time:']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Prep Time: ']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Prep Time:']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Prep']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Prep:']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='prep time']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='prep time:']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='prep']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='prep:']");
                }
                if (prepElement == null)
                {
                    prepElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='active']");
                }
                if (prepElement != null)
                {
                    Console.WriteLine(prepElement.InnerText);
                    Console.WriteLine("prep time element found");
                    var prepParentElement = prepElement.ParentNode;
                    prepTime = prepParentElement.InnerText;
                    prepTime = prepTime.Replace("&nbsp;", "");
                    Console.WriteLine(prepTime);
                }






                //extracting cook time

                var cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Cook Time']");
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Cook Time:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Cook Time: ']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Cook Time:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Cook Time']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Cook']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Cook:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='cook time']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='cook time:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='cook']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='cook:']");
                }


                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Bake Time:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Bake Time']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Bake Time: ']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Bake Time:']");
                }
               
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Bake']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Bake:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='bake time']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='bake time:']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='bake']");
                }
                if (cookElement == null)
                {
                    cookElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='bake:']");
                }
               
                if (cookElement != null)
                {
                    Console.WriteLine(cookElement.InnerText);
                    Console.WriteLine("cook time element found");
                    var cookParentElement = cookElement.ParentNode;
                    cookTime = cookParentElement.InnerText;
                    cookTime = cookTime.Replace("&nbsp;", "");
                    Console.WriteLine(cookElement);
                }





                //extracting total time

                var totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Total Time']");
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Total Time:']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Total Time: ']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()=' Total Time:']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Total']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='Total:']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='total time']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='total time:']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='total']");
                }
                if (totalElement == null)
                {
                    totalElement = htmlDoc1.DocumentNode.SelectSingleNode("//*[text()='total:']");
                }

                if (totalElement != null)
                {
                    var totalParentElement = totalElement.ParentNode;
                    totalTime = totalParentElement.InnerText;
                    totalTime = totalTime.Replace("&nbsp;", "");
                    Console.WriteLine(totalTime);
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
                recipe.PrepTime = prepTime;
                recipe.CookTime = cookTime;
                recipe.TotalTime = totalTime;
                recipe.PictureSource = jpgSource;
                recipe.Url = links[i];
                //inside checking if the recipe object contains allergic ingredients.

                var checkAllergy = 0;
                List<string> listOfAllergies = new List<string>();
                allergiesForUser.ForEach(a =>
                {
                    listOfAllergies.Add(a.ToString());
                });

                bool contains;
                foreach (var r in recipe.Ingredients)
                {
                    foreach (var allergy in listOfAllergies)
                    {
                        contains = r.IndexOf(allergy, StringComparison.OrdinalIgnoreCase) >= 0;
                        if (contains)
                        {
                            checkAllergy = 1;
                            continue;
                        }
                    }
                }



                if (checkAllergy == 0)
                    recipes.Add(recipe);
            }

            #region getting rid of empty lines in ingredients array
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = recipe.Ingredients.Except(recipe.Ingredients.Where(i => string.IsNullOrWhiteSpace(i) || i == "\n")).ToList(); 
                recipe.Method = recipe.Method.Except(recipe.Method.Where(i => string.IsNullOrWhiteSpace(i) || i == "\n")).ToList() ;

            }

            //we also have to get rid of icon numbers like &#78965;
            //what I'm trying to do here is that once there is "&#" -> emty out everything until it get's to ";"
            //int j, count;

            //char[] IngredientsArray,MethodArray;
            //foreach (var rec in recipes)//foreach recipe 
            //{
            //    for (int b = 0; b < rec.Ingredients.Count; b++)
            //    {
            //        //string sentence = "Mahesh Chand"; =ing
            //        IngredientsArray = rec.Ingredients[b].ToCharArray();
            //        for (j = 0; j < IngredientsArray.Length - 1; j++)
            //        {
            //            if (IngredientsArray[j] == '&' && (IngredientsArray[j + 1] == '#'))
            //            {
            //                count = 0;

            //                while (IngredientsArray[j] != ';')
            //                {
            //                    j++;
            //                    count++;
            //                }
            //                count++;
            //                var aStringBuilder = new StringBuilder(rec.Ingredients[b]);
            //                aStringBuilder.Remove(j, count);
            //                rec.Ingredients[b] = aStringBuilder.ToString();

            //            }
            //        }
            //    }

            //for (int b = 0; b < rec.Method.Count; b++)
            //{
            //    //string sentence = "Mahesh Chand"; =ing
            //    MethodArray = rec.Method[b].ToCharArray();
            //    for (j = 0; j < MethodArray.Length - 1; j++)
            //    {
            //        if (MethodArray[j] == '&' && (MethodArray[j + 1] == '#'))
            //        {
            //            count = 0;

            //            while (MethodArray[j] != ';')
            //            {
            //                j++;
            //                count++;
            //            }
            //            count++;
            //            var aStringBuilder = new StringBuilder(rec.Method[b]);
            //            aStringBuilder.Remove(j, count);
            //            rec.Method[b] = aStringBuilder.ToString();

            //        }
            //    }
            //    }
            //}





            #endregion

            return recipes;
        }
        #endregion
    }
}


//tryings



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