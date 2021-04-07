using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    /// <summary>
    /// HealthArticlesConverter is in charge of converting between different layers and the database
    /// </summary>
    public class HealthArticlesConverter
    {
        public static HealthFoodArticle ConvertArticleToDAL(HealthArticlesDTO articles)
        {
            return new HealthFoodArticle
            {
                LinkName = articles.LinkName,
                LinkId = articles.LinkId,
                Link = articles.Link,
                logo = articles.Logo
                
            };
        } 

        public static HealthArticlesDTO ConvertArticleToDTO(HealthFoodArticle articles)
        {
            return new HealthArticlesDTO
            {
                LinkName = articles.LinkName,
                LinkId = articles.LinkId,
                Link = articles.Link,
                Logo = articles.logo
            };
        }


        public static List<HealthArticlesDTO> ConvertArticleListToDTO(IEnumerable<HealthFoodArticle> articles)
        {
            return articles.Select(c => ConvertArticleToDTO(c)).ToList();
        }
    }
}
