using System.Threading.Tasks;

namespace Hadouken.Bots
{
    public interface IBot
    {
        Task RunAsync();

        Task StopAsync();
    }
}