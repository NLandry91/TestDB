using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTConnect;

namespace MTConnectData
{
    class RunMTConnectClient
    {
        private MTConnect.Clients.MTConnectClient client;
        // The base address for the MTConnect Agent
        private string baseUrl = "";

        public RunMTConnectClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public void Start()
        {
            // Create a new MTConnectClient using the baseUrl
            client = new MTConnect.Clients.MTConnectClient(baseUrl);

            //var sample = new MTConnect.Clients.Sample(baseUrl);
            //var stream = new MTConnect.Clients.Stream(baseUrl);

            // Start the MTConnectClient

            
             
            // Subscribe to the Event handlers to receive the MTConnect documents
            client.ProbeReceived += DevicesSuccessful;
            client.CurrentReceived += StreamsSuccessful;
            client.SampleReceived += StreamsSuccessful;

            client.Start();
        }

        public void Stop()
        {
            client.Stop();
        }

        // --- Event Handlers ---

        void DevicesSuccessful(MTConnect.MTConnectDevices.Document document)
        {
            foreach (var device in document.Devices)
            {
                var dataItems = device.GetDataItems();
                foreach (var dataItem in dataItems) Console.WriteLine(dataItem.Id + " : " + dataItem.Name);
            }
        }

        void StreamsSuccessful(MTConnect.MTConnectStreams.Document document)
        {
            foreach (var deviceStream in document.DeviceStreams)
            {
                foreach (var dataItem in deviceStream.DataItems) Console.WriteLine(dataItem.DataItemId + " = " + dataItem.CDATA);
            }
        }
    }
}
