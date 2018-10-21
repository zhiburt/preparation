using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Models.DbEntity;

namespace preparation.Services.Messenger
{
    public class Messenger : IMessenger
    {
        private readonly UserManager<User> _userManager;
        private readonly MessengerContext _messengerContext;

        public Messenger(UserManager<User> userManager, MessengerContext messengerContext)
        {
            this._userManager = userManager;
            _messengerContext = messengerContext;
        }

        public void Send(Message message, User to)
        {
            throw new NotImplementedException();
        }

        public async Task Send<T>(T message, User to, User from) where T : class
        {
            var json = JsonConvert.SerializeObject(message);

            var user = await _userManager.Users.FirstOrDefaultAsync( u => u == to);
            if (user != null)
            {
                var newMessage = await _messengerContext.AddAsync(new Message()
                {
                    From = from.Id,
                    Level = MessageLevel.Normal,
                    Test = json
                });
                user.AddMessagesID(newMessage.Entity.Id);

                await _messengerContext.SaveChangesAsync();
                await _userManager.UpdateAsync(user);

            }
        }

        public IQueryable<Message> Recieve(User to, MessageLevel msLevel = MessageLevel.Normal)
        {
            throw new NotImplementedException();
        }
    }
}
