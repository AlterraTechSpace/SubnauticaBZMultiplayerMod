using Story;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubnauticaMPMod
{
    internal class ClientData
    {
        public struct Client
        {
            /// <summary>
            /// The username that is shown on nameplates in the game.
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// The ID of the player, it mustn't be changed, neither by the player, neither by the code !
            /// </summary>
            public string ID { get; set; }

            /// <summary>
            /// Servers of the player.
            /// </summary>
            public ServerData[] servers { get; set; }
        }
        public Client User { get; set; }
        ClientData()
        {
            if (playerConfigFileExists() == false)
            {
                var id = Utils.GenerateSFID();
                var username = $"Player{id.Substring(0, 6)}";
                Client c = new Client()
                {
                    ID = id,
                    Username = username,
                };
                createPlayerConfigFile();
            } else
            {

            }
        }

        private bool playerConfigFileExists()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var filename = "player.json";
            return File.Exists(Path.Combine(location, filename));
        }

        private void createPlayerConfigFile()
        {
            StringBuilder filestring = new StringBuilder();
            filestring.AppendLine("{");
            filestring.AppendLine($"    \"Username\": \"{this.Username}\",");
            filestring.AppendLine($"    \"ID\": \"{this.ID}\",");
            filestring.AppendLine($"    \"servers\": []");
            File.WriteAllText(Path.Combine(Assembly.GetExecutingAssembly().Location, "player.json"), filestring.ToString());
        }

        private void readPlayerConfigFile()
        {
            var filetext = File.ReadAllText(Path.Combine(Assembly.GetExecutingAssembly().Location, "player.json"));
            ClientData clientData;
            clientData.Username = JsonObject.Parse(filetext);
        }

        /// <summary>
        /// Information about a server, its name, its IP, its port, if it's a favourite of the user, and its status
        /// (must refresh list to have changes)
        /// </summary>
        public struct ServerData
        {
            public string name;
            public string ip;
            public int port;
            public bool favourite;
            public ServerStatus status;
        }

        public enum ServerStatus
        {
            Offline,
            Starting,
            Online,
            Restarting,
            Maintenance
        }

        public enum ClientConnectionStatus
        {
            TestingConnection,
            Connecting,
            Connected,
            Disconnecting
        }
    }
}
