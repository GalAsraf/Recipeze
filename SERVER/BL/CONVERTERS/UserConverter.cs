using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.CONVERTERS
{
    public static class UserConverter
    {
        public static User ConvertUserToDAL(UserDTO user)
        {
            return new User
            {
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            };
        }
        public static UserDTO ConvertUserToDTO(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            };
        }

        public static List<UserDTO> ConvertUserListToDTO(IEnumerable<User> users)
        {
            return users.Select(c => ConvertUserToDTO(c)).ToList();
        }




    }
}
