using Auth.Model;
using Auth.Request;
using AutoMapper;

namespace Auth.Profiles
{
    public class AuthProfiles:Profile
    {
        public AuthProfiles()
        {
            //user
            CreateMap<User,AddUser>().ReverseMap();

            //Product
            CreateMap<Product, AddProduct>().ReverseMap();  
          
        }

    }
}
