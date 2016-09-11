using System;
using SuperMarioPivotalEdition.Listeners;

namespace SuperMarioPivotalEdition
{
    internal static class MainClass
    {
        private static void Main()
        {
            var slackListener = new SlackListener();
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();
        }
    }
}