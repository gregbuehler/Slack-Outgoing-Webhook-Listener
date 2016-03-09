using System;

namespace SuperMarioPivotalEdition
{
    class MainClass
    {
        static void Main()
        {
            var slackListener = new SlackListener(new DatabaseClient("Mario"));
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();

        }
    }
}