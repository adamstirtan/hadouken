using System.Threading.Tasks;

namespace Hadouken.Bots
{
    public interface IBot
    {
        Task StartAsync();

        Task StopAsync();
    }
}