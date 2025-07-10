using localMarketingSystem.DAL.Entities;
using localMarketingSystem.DAL.Interfaces;

namespace localMarketingSystem.DAL.Repositories
{
    public class UserRepository : Repository<MUser, localMarketingSystemDBContext>, IUserRepository
    {
        public UserRepository(localMarketingSystemDBContext context) : base(context)
        {
        }
    }
}
