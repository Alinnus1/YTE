using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Token
{
    public class TokenService : BaseService
    {
        public TokenService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }


        public Entities.Token CreateConfirmationEmailToken(UnitOfWork uow)
        {
            var token = new Entities.Token
            {
                Id = (Guid.NewGuid().ToString() + Guid.NewGuid().ToString()),
                TypeId = (int)TokenTypes.EmailConfirmation,
                Date = DateTime.Now

            };
            uow.Tokens.Insert(token);
            return token;
        }

        public Entities.Token CreateForgotPassEmailToken(UnitOfWork uow)
        {
            var token = new Entities.Token()
            {
                Id = (Guid.NewGuid().ToString() + Guid.NewGuid().ToString()),
                TypeId=(int)TokenTypes.ForgotPassword,
                Date=DateTime.Now
            };
            uow.Tokens.Insert(token);
            return token;
        }


        public void DeleteExpiredTokens()
        {
            ExecuteInTransaction(uow =>
            {
                var dt = DateTime.Now.AddDays(-1);
                var expiredTokens = uow.Tokens.Get()
                    .Where(t => t.Date >= dt);
            
                foreach (var token in expiredTokens)
                {
                    uow.Tokens.Delete(token);
                }
                uow.SaveChanges();
                

            });
        }
       
    }
}
