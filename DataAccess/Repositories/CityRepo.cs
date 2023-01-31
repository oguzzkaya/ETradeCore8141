using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public abstract class CityRepoBase : RepoBase<Country>
    {
        protected CityRepoBase(ETradeContext dbContext) : base(dbContext)
        {

        }
    }
    public class CityRepo : CityRepoBase
    {
        public CityRepo(ETradeContext dbContext) : base(dbContext)
        {
        }
    }
}
