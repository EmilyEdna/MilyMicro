namespace Mily.RabbitMQ.AcceptFunc
{
    public interface IAccept
    {
        void AcceptMQ<T>(string msg);
    }
}