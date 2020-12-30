using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// HealthArticleBl contains a method which chooses 6 random article links
    /// </summary>
    public class HealthArticlesBL
    {
        /// <summary>
        /// chooses 6 random article links from database
        /// </summary>
        /// <returns> list of health article links </returns>
        public static List<HealthArticlesDTO> GetRandomArticles()
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                var sixRandomFoos = CONVERTERS.HealthArticlesConverter.ConvertArticleListToDTO(db.HealthFoodArticles);
                return sixRandomFoos.OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            }
        }
    }
}
