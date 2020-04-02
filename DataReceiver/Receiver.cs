using Newtonsoft.Json;
using NLog;

namespace DataReceiver
{
    public class Receiver
    {
        private static readonly Logger Logger = LogManager.GetLogger("DataReceiverLogger");

        public void Receive<T>(T[] data)
        {
            var typeName = typeof(T).FullName;

            foreach (var item in data)
            {
                var json = JsonConvert.SerializeObject(item);
                Logger.Info($"Receive object with type: [{typeName}], object: [{json}]");
            }
        }
    }
}
