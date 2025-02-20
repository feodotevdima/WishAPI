using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }

        public UserModel(Guid id, string name, string email, string image)
        {
            Id = id;
            Name = name;
            Email = email;
            Image = image;
        }
    }
}
