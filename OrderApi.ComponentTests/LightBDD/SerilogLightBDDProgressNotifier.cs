using LightBDD.Framework.Notification;
using Serilog;

namespace OrderApi.ComponentTests.LightBDD;

public class SerilogLightBDDProgressNotifier : DefaultProgressNotifier
{
    public SerilogLightBDDProgressNotifier()
        : base(message => Log.Logger.Information(message))
    {
    }
}
