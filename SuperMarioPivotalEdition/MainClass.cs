using System;

namespace SuperMarioPivotalEdition
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var pivotalApiKey = args[0];
            var slackToken = args[1];
            var serverAddress = args[2];
            var slackListener = new SlackListener(new DatabaseClient("Mario"), pivotalApiKey, slackToken, serverAddress);
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();
        }
    }
}