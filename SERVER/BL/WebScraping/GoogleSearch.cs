using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.WebScraping
{
   public class GoogleSearch
    {
        public static void CustomSearch(string[] searchTexts )
        {
            string apiKey = "nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCu3UfnJ1xAPHlf\nyT7H+lipgXzodlh2T24VnT+OsVse/BTsdHvmDt9KjirtnxoXUMc1z4UUVbTytiK6\neGeUeNJYI8UGrejKELk0BAFOMyCiW0h5BPZD3avosluHOUYGCdru5nG2t26u+QWf\nglyINFTH54gCHBbDNWSzqdSSgIBuHPdpvytp/OCyCfMKxB+hBUSev/y8BWXmStcg\nCxZXM5ubRsDQ19bFniAkkea6QTe4ThMYRGlNOJdb1xc1Fcn0x86umnQ/eqE4aKFI\npWd0YnSMvHMzNUg2LiXuttHgHr8PZ3C9wd0CcZ4OgprfnncEM7OHpNZmlMV/r36Y\nHEVEqunrAgMBAAECggEACPDWKXEPUVbUdBvGS4yhjwPWy1scYlyr/yhBITrbXaQ0\noFbf7DgRj5IaNZSPahFT7/5pbfa9qm3zZYMq9RPJS7f07eMqe6/EGjZOqB0nVjsl\n8bMEXHKhUuPfQp9nuH7gDw2rjBvhzR8nwx07BKBDBlvUBuJBOlwov/fTrYHySARI\nkGk4jPlhcQ23qH4PYEymZkVZi9d+TLoDXJwqXPXsi+ZtRlmGvQKaDDnZ6UAm66bv\n+XLMoqGFx1DFYXhg4T5wy+6FGL13xro7YW8azUSAFtNo+dwXPuiMHFo89ldc7z9q\neLU2uHrHtwaagpgkMjN8N7ISKQPP/GTMrY2WkJpt8QKBgQDiIpNjVDi3CfqlVPIc\n1RPYUpMdc5Oz4BRZl680pqmz5VM+JeJONKTipJvsxJTE1udp0v1R93oskeUcWT06\njyFq1DhkX65B9Ns6GpxheBXlmXShQBIexjXDGudqnYUC2LUbJtPrh4MspQ5ZYSV4\nkd51v54hnZh4BBHMM7HDHTXEFwKBgQDF9UoHjE6DNleslH+hXuVq64JMeXraMU+p\nLzsB1+drZlaivxV4LRzlguwv4VAlt4MTYgdriOJcU/8IfXQuTI7+B2zMRsGWCNgp\noPwwAn+Ftxm5m/ZpBpdeOcho/54NRHA6uNtp8LTxc8TZLd2kgkgwE0xbn1wDCfG4\nZGmmierpTQKBgEICCbfCy9NSDGHaS9nysJpCcEL2i7TDweztA+2AgKTMWeIYONjP\nMRofJoyUTUCv4ljXh643aOg9pf0CZ4cCZKTEUbmq3DjQenWZcvBYlzuv8YVoKGHn\nRaYv4kESvdK44xSL3uwvYDDV9TxNyRxKp/8C8euqDulpdrB+nnLvwdP9AoGAD5ha\nY6vXB5lBYPQ19dWPB1RUaIfteMEHwJFa+bMzpQ9j5eBd5aDQNPiSeNcsRDxn1CAV\n64/WHWX0oouXmoonfbXCCXnNiG9b8DOhinq35yXcnfW+fNmrFR5CPptcrTjmCopD\npt3ys07mhCGL44jr/PWYP2OXkRm4dElc1WTqH8UCgYEAx2Xz26jySwBdSNWWWwVN\ncPDz7q5SBoXGOLj0RWNR4DdIDRCXRf31j3QCo6MPNgi2XXNbO+razCuveXV72RGi\n9x+9eheoUa1xQ3zCpVJomTv6TmKwIrn2XxUb3U+2T5vhFHP8JNmPvuQ7RHYH0Sa+\nvdXff8zYgzOpaqxJFGSxePQ=";
            //how do add web reference?
            //licence key?
            com.google.api.GoogleSearchService s = new TestGoogle.com.google.api.GoogleSearchService();
            com.google.api.GoogleSearchResult r = s.doGoogleSearch("put your lisence key her ", Txt_Text.Text, 0, 10, false, "", true, "", "", "");
            int estimatedCount = r.estimatedTotalResultsCount;  //?

            DataTable dtResults = new DataTable();
            dtResults.Columns.Add(new DataColumn("Title", typeof(string)));
            dtResults.Columns.Add(new DataColumn("Summary", typeof(string)));
            dtResults.Columns.Add(new DataColumn("URL", typeof(string)));
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dtResults.NewRow();
                dr[0] = r.resultElements[i].title;
                dr[1] = r.resultElements[i].snippet;
                dr[2] = r.resultElements[i].URL;
                dtResults.Rows.Add(dr);
            }
            dataGridView1.DataSource = dtResults;

            //different code 
            Console.WriteLine("Search for:");
            string command = Console.ReadLine();
            IWebDriver driver = new FirefoxDriver();

            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Manage().Window.Maximize();

            IWebElement searchInput = driver.FindElement(By.Id("lst-ib"));
            searchInput.SendKeys(command);
            searchInput.SendKeys(Keys.Enter);


            //IWebElement searchInput = Driver.FindElement(By.Id("lst-ib"));
            //searchInput.SendKeys(command);
            //searchInput.SendKeys(Keys.Enter);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            By linkLocator = By.CssSelector("cite._Rm");
            wait.Until(ExpectedConditions.ElementToBeClickable(linkLocator));
            IReadOnlyCollection<IWebElement> links = driver.FindElements(linkLocator);
            foreach (IWebElement link in links)
            {
                Console.WriteLine(link.Text);
            }


           
    }
    }
}
