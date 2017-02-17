using System;

namespace ResourceWithIdentityServerWithClient.Model
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        //public static implicit operator UserDto(UserDto v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}