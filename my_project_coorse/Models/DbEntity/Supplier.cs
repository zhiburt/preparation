using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models.Account;

namespace preparation.Models.DbEntity
{
    public class Supplier
    {
        [Key]
        public string Id { get; set; }
        public User User { get; set; }
        public string Companys { get; set; }

        public IQueryable<string> GetCompanysID()
        {
            return this.Companys?.Split('~')?.AsQueryable();
        }

        public void AddCompanysID(string id)
        {
            if (this.Companys != "")
            {
                this.Companys += "~" + id;
            }
            else
            {
                this.Companys += id;
            }
        }
    }
}
