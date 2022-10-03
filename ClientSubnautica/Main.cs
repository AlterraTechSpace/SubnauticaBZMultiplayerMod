using ClientSubnautica.MultiplayerManager;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Discord;

namespace ClientSubnautica
{
    [QModCore]
    public static class MainPatcher
    {
        public static string location;
        public static string modFolder;
        public static string id;
        public static string username;
        public static JObject configFile;
        public static Dictionary<string, string> player_list = new Dictionary<string, string>();

        private static int gameTimestamp = DateTime.Now.Second - new DateTime(1970, 1, 1).Second;
        public static Discord.Discord Client = new Discord.Discord(Int64.Parse("1026540000617189416"), (UInt64)Discord. CreateFlags.Default);
        public static void UpdatePresence(Discord.Discord c, string state)
        {
            var activityManager = c.GetActivityManager();
            var discordPresence = new Discord.Activity
            {
                State = state,
                Details = "Surviving on 4546B",
                Timestamps =
                {
                    Start = gameTimestamp,
                },
                Assets =
                {
                    LargeImage = "logo",
                    LargeText = "Mod by Damien"
                },
                Party =
                {
                    Id = "Soon!",
                    Size =
                    {
                        CurrentSize = player_list.Count,
                        MaxSize = 999,
                    },
                },
                Instance = true,
                Type = ActivityType.Playing
            };
            activityManager.UpdateActivity(discordPresence, result =>
            {
                Logger.Log(Logger.Level.Debug, $"Update activity {result}");
            });
        }


        [QModPatch]
        public static void Patch()
        {
            location = AppDomain.CurrentDomain.BaseDirectory;
            modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Loading the user configs.
            configFile = LoadParam(Path.Combine(modFolder, "player.json"));
            string playerID = configFile["playerID"].ToString();
            id = configFile["playerID"].ToString();
            username = configFile["nickname"].ToString();

            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = "dam_" + executingAssembly.GetName().Name;
            new Harmony(text).PatchAll(executingAssembly);
            UpdatePresence(Client, "Into menu");

            Logger.Log(Logger.Level.Info, playerID + " - Username: " + username);
        }


        /// <summary>
        /// Loads a JSON file and parse it, if none is found, one is created. NOT UNIVERSAL.
        /// </summary>
        /// <param name="path">Path to the file (including the file)</param>
        /// <returns>A parsed JSON object.</returns>
        public static JObject LoadParam(string path)
        {
            if (File.Exists(path))
            {
                return JObject.Parse(File.ReadAllText(path));
            }
            else if (path.EndsWith("player.json"))
            {
                var id = GenerateID();
                File.WriteAllText(path,
@"{
    ""WARNING"": ""DO NOT CHANGE OR DELETE THE ID OR YOU WILL LOSE ALL YOUR PROGRESSIONS ON EVERY GAMES"",
    ""playerID"": """ + id + @""",
    ""nickname"": ""Player" + id + @"""
}");
                return JObject.Parse(File.ReadAllText(path));
            }
            else throw new Exception("The file you're trying to access does not exist, and has no default value.");
        }
        public static string GenerateID()
        {
            var tid = Process.GetCurrentProcess().Id.ToString() + ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            return tid;
        }
    }
}