using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.Common.DTOS;

namespace YTE.BusinessLogic.Implementation.Gender
{
    public class GenderService : BaseService
    {
        public GenderService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public List<ListItem<string, int>> GetGenders()
        {
            return UnitOfWork.Genders.Get()
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Name}",
                    Value = g.Id
                })
                .ToList();
        }

        public List<ListItem<string, int>> GetGenderOfUser(Guid id)
        {
            return UnitOfWork.Users.Get()
                .Where(u => u.Id == id)
                .Select(u => new ListItem<string, int>
                {
                    Text = $"{u.Gender.Name}",
                    Value = u.Gender.Id
                })
                .ToList();
        }
    }
}
