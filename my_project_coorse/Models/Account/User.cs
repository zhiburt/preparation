using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace preparation.Models.Account
{
    public class User : IdentityUser
    {
        //public User()
        //{
        //    Messages = new HashSet<DbEntity.Message>();
        //}

        public string Address { get; set; } 
        public string FirstName { get; set; } 
        public string Surname { get; set; }
        public string Country { get; set; }

        public string Messages { get; set; } //ICollection<DbEntity.Message>

        public IQueryable<string> GetMessagesID()
        {
            return this.Messages?.Split('~')?.Skip(1).AsQueryable();
        }

        public void AddMessagesID(string id)
        {
            if (this.Messages != "")
            {
                this.Messages += "~" + id;
            }
            else
            {
                this.Messages += id;
            }
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            return user != null &&
                   Address == user.Address &&
                   FirstName == user.FirstName &&
                   Surname == user.Surname &&
                   Country == user.Country &&
                   Messages == user.Messages;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Address, FirstName, Surname, Country, Messages);
        }
    }
}
