using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigolPSU
{
    public class PSUChannel
    {
        private RigolPSU owner;
        private int id;

        public PSUChannel(RigolPSU owner, int id)
        {
            this.owner = owner;
            this.id = id;
        }

        public async Task SetEnabled (bool enabled)
        {
            string commandArgument = enabled ? "ON" : "OFF";
            var response = await owner.SendCommand($":OUTP CH{id},{commandArgument}", false);
        }

        public async void SetVoltage(double voltage)
        {
            var response = await owner.SendCommand($":APPL CH{id},{voltage},{CurrentLimit}");
        }

        public async void SetCurrentLimit(double current)
        {
            var response = await owner.SendCommand($":APPL CH{id},{VoltageLimit},{current}");
        }

        public async Task Update()
        {            
            var response = await owner.SendCommand($":MEAS:ALL? CH{id}", true, true);

            if(response != string.Empty)
            {
                var parts = response.Split(',');
                try
                {
                    Voltage = Convert.ToDouble(parts[0]);
                    Current = Convert.ToDouble(parts[1]);
                    Power = Convert.ToDouble(parts[2]);
                } catch(Exception e)
                {
                    return;
                }
            }

            response = await owner.SendCommand($":OUTP? CH{id}", true, true);

            if(response != string.Empty)
            {
                if(response == "OFF")
                {
                    poweredOn = false;
                }

                if(response == "ON")
                {
                    poweredOn = true;
                }
            }

            response = await owner.SendCommand($":APPL? CH{id}", true, true);

            if(response != string.Empty)
            {
                var parts = response.Split(',');

                try
                {
                    ChannelSpec = parts[0];
                    VoltageLimit = Convert.ToDouble(parts[1]);
                    CurrentLimit = Convert.ToDouble(parts[2]);
                } catch (Exception e)
                {
                    return;
                }
            }

            ChannelUpdateReceived?.Invoke(this, new EventArgs());
        }

        public event EventHandler<EventArgs> ChannelUpdateReceived;

        public double Voltage { get; private set; }
        public double Current { get; private set; }
        public double Power { get; private set; }

        public double VoltageLimit { get; private set; }
        public double CurrentLimit { get; private set; }

        private bool poweredOn;
        public bool PoweredOn
        {
            get { return poweredOn; }
        }

        public string ChannelSpec { get; private set; }
    }
}
