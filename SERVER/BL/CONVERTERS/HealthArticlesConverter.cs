using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public class HealthArticlesConverter
    {
        public static HealthFoodArticle ConvertArticleToDAL(HealthArticlesDTO articles)
        {
            return new HealthFoodArticle
            {
                LinkName = articles.LinkName,
                LinkId = articles.LinkId,
                Link = articles.Link
            };
        } 

        public static HealthArticlesDTO ConvertArticleToDTO(HealthFoodArticle articles)
        {
            return new HealthArticlesDTO
            {
                LinkName = articles.LinkName,
                LinkId = articles.LinkId,
                Link = articles.Link
            };
        }


        public static List<HealthArticlesDTO> ConvertArticleListToDTO(IEnumerable<HealthFoodArticle> articles)
        {
            return articles.Select(c => ConvertArticleToDTO(c)).ToList();
        }
    }
}
