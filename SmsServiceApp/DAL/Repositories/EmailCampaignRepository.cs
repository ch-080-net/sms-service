using Model.Interfaces;
using WebApp.Data;
using WebApp.Models;

namespace DAL.Repositories
{
    public class EmailCampaignRepository : BaseRepository<EmailCampaign>, IEmailCampaignRepository
    {
        public EmailCampaignRepository(ApplicationDbContext mainDbContext) : base(mainDbContext)
        {
        }

        /// <summary>
        /// Insert new company to db and return id
        /// </summary>
        /// <param name="item">Company entity</param>
        /// <returns>Id</returns>
        public int InsertWithId(EmailCampaign item)
        {
            context.EmailCampaigns.Add(item);
            context.SaveChanges();
            return item.Id;
        }
    }
}
