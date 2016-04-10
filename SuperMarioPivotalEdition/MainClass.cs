﻿using System;

namespace SuperMarioPivotalEdition
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var serverAddress = args[0];
            var slackToken = args[1];
            var pivotalApiKey = args[2];
            var bitlyApiKey = args[3];
            var catApiKey = args[4];
            var slackListener = new SlackListener(new DatabaseClient("Mario"), serverAddress, slackToken, pivotalApiKey, bitlyApiKey, catApiKey);
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();
        }
    }
}