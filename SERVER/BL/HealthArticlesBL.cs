using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class HealthArticlesBL
    {
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
