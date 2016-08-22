namespace RigolPSU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PrimS.Telnet;
    using System.Threading;

    public class RigolPSU
    {
        private Client command_client;
        private Client poll_client;
        private JobRunner packetQueue;
        private TimeSpan timeout;

        public RigolPSU(string connectionString)
        {
            command_client = new Client("10.1.0.127", 5555, new CancellationToken());
            poll_client = new Client("10.1.0.127", 5555, new CancellationToken());

            Task.Factory.StartNew(() =>
            {
                packetQueue = new JobRunner();

                packetQueue.RunLoop(new CancellationToken());
            });

            Channels = new List<PSUChannel>();
            Channels.Add(new PSUChannel(this, 1));
            Channels.Add(new PSUChannel(this, 2));
            Channels.Add(new PSUChannel(this, 3));

            timeout = TimeSpan.FromMilliseconds(150);
        }

        public void Connect()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    foreach(var channel in Channels)
                    {
                        await channel.Update();
                    }

                    await Task.Delay(50);
                }
            });
        }        

        public async Task<string> SendCommand (string command, bool awaitResponse = true, bool polling = false)
        {
            Client client = polling ? poll_client : command_client;
            string result = string.Empty;

            await packetQueue.InvokeAsync(() =>
            {
                client.WriteLine(command).Wait();

                var task = client.ReadAsync(timeout);
                task.Wait();

                result = task.Result.Trim().Split(';')[0].Split('\n')[0];
            });

            if(result == "Command error")
            {
                return string.Empty;
            }

            return result;
        }

        public List<PSUChannel> Channels
        {
            get; private set;
        }
    }
}
