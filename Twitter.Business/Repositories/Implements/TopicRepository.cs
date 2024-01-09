using Twitter.Business.Repositories.Interface;
using Twitter.Core.Entities;
using Twitter.DAL.Contexts;

namespace Twitter.Business.Repositories.Implements;
public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(TwitterContext context) : base(context)
    {
    }
}
