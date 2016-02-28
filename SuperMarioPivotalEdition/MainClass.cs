using System;

namespace SuperMarioPivotalEdition
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var slackListener = new SlackListener(new DatabaseClient("Mario"), new PivotalClient(), new GoogleCalendarClient());
            slackListener.ListenForSlackOutgoingWebhooks();
            Console.ReadLine();
        }
    }
}