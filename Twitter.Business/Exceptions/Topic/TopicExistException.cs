using System.Runtime.Serialization;

namespace Twitter.Business.Exceptions.Topic;

public class TopicExistException : Exception
{
    public TopicExistException():base("Topic already exist")
    {
    }

    public TopicExistException(string? message) : base(message)
    {
    }

   
}
