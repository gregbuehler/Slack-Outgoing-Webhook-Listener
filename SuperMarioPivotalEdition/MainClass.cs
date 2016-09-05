using System;
using SuperMarioPivotalEdition.Clients;
using SuperMarioPivotalEdition.Listeners;

namespace SuperMarioPivotalEdition
{
    internal static class MainClass
    {
        private static void Main(string[] args)
        {
            var serverAddress = args[0];
            var slackToken = args[1];
            var pivotalApiKey = args[2];
            var bitlyApiKey = args[3];
            var catApiKey = args[4];
            var imgurApiKey = args[5];
            var googleApiKey = args[6];
            var slackListener = new SlackListener(new DatabaseClient("Mario"), serverAddress, slackToken, pivotalApiKey, bitlyApiKey, catApiKey, imgurApiKey, googleApiKey);
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();
        }
    }
}