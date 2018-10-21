using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace preparation.Models.Contexts
{
    public class UserMessanges
    {
        [Key]
        public string Id { get; set; }
        public string Messages { get; set; } //ICollection<DbEntity.Message>

        public IQueryable<string> GetMessagesID()
        {
            return this.Messages?.Split('~')?.AsQueryable();
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
    }
}