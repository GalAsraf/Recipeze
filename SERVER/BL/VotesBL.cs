using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class VotesBL
    {
        public static void addVote(string siteName)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                try
                {
                    var numOfVotes = db.Votes.Where(a => a.siteName == siteName).ToList();
                    if (numOfVotes.Count == 0)
                    {
                        VotesDTO voting = new VotesDTO() { siteName = siteName, voteNumbers = 1 };
                        db.Votes.Add(CONVERTERS.VotesConverter.ConvertVoteToDAL(voting));
                        db.SaveChanges();
                    }
                    else
                    {
                        var voteUpdate = db.Votes.Where
                        (
                           p => siteName.Contains(p.siteName)
                        );
                        foreach (Vote p in voteUpdate)
                        {
                            p.voteNumbers++;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }
        }

        public static List<Vote> CheckingNumOfVotes(List<string> sitesName)
        {
            using (RecipezeEntities db = new RecipezeEntities())
            {
                List<Vote> sites = new List<Vote>();
                sitesName.ForEach(w =>
                {
                    var amount = db.Votes.Where(n => n.siteName == w).Where(a => a.voteNumbers >= 10).ToList();
                    if (amount.Count != 0)
                        sites.Add(amount[0]);
                });
                return sites;
                //if (amount == null)
                //    return null;
                //else
                //    return amount;
              // return db.Votes.Where(a => a.vote.voteNumbers > 10).ToList();
                
            }
        }
    }

}
