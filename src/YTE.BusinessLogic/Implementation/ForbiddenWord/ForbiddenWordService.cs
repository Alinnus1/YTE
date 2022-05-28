using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ForbiddenWord.Model;
using YTE.BusinessLogic.Implementation.ForbiddenWord.Validation;
using YTE.Common.Extensions;

namespace YTE.BusinessLogic.Implementation.ForbiddenWord
{
    public class ForbiddenWordService : BaseService
    {
        private readonly ForbiddenWordValidator forbiddenWordValidator;
        public ForbiddenWordService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            this.forbiddenWordValidator = new ForbiddenWordValidator(UnitOfWork);
        }

        public List<ListForbiddenWordModel> GetForbiddenWords()
        {
            return UnitOfWork.ForbiddenWords.Get()
                    .Select(a => Mapper.Map<Entities.ForbiddenWord, ListForbiddenWordModel>(a))
                    .ToList();
        }

        public void DeleteForbiddenWord(int id)
        {
            ExecuteInTransaction(uow =>
            {
                var word = uow.ForbiddenWords.Get()
                            .FirstOrDefault(a => a.Id == id);
                if (word != null)
                {
                    uow.ForbiddenWords.Delete(word);
                }
                uow.SaveChanges();
            });
        }

        public void CreateNewForbiddenWord(CreateForbiddenWordModel model)
        {
            ExecuteInTransaction(uow =>
            {
                forbiddenWordValidator.Validate(model).ThenThrow(model);
                var word = Mapper.Map<CreateForbiddenWordModel, Entities.ForbiddenWord>(model);
                uow.ForbiddenWords.Insert(word);
                uow.SaveChanges();
            });
        }
    }
}
