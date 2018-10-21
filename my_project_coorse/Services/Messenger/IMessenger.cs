using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models.Account;
using preparation.Models.DbEntity;
using preparation.ViewModels.AdminMessenger;

namespace preparation.Services.Messenger
{
    public interface IMessenger
    {
        void Send(Message message, User to);
        Task Send<T>(T message, User to, User @from) where T : class;
        IQueryable<Message> Recieve(User to, MessageLevel msLevel = MessageLevel.Normal);
    }
}
