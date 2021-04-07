using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public class VotesConverter
    {

        public static Vote ConvertVoteToDAL(VotesDTO vote)
        {
            return new Vote
            {
                siteName = vote.siteName,
                voteNumbers = vote.voteNumbers
            };
        }


        public static VotesDTO ConvertVoteToDTO(Vote vote)
        {
            return new VotesDTO
            {
                siteName = vote.siteName,
                voteNumbers = vote.voteNumbers
            };
        }


        public static List<VotesDTO> ConvertVoteListToDTO(IEnumerable<Vote> votes)
        {
            return votes.Select(c => ConvertVoteToDTO(c)).ToList();
        }
    }
}
