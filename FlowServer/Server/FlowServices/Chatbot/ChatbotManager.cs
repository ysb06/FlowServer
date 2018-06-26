using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Dialogflow;
using Google.Cloud.Dialogflow.V2;

namespace FlowServer.Server.FlowServices.Chatbot
{
    class ChatbotManager
    {

        public ChatbotManager()
        {
            /* 환경변수 
             * GOOGLE_APPLICATION_CREDENTIALS
             * D:\Programs\Google\Small-Talk-ab19e1b0105f.json
             */
            var client = SessionsClient.Create();
            QueryInput tempInput = new QueryInput();
            TextInput tempText = new TextInput
            {
                Text = "야나두",
                LanguageCode = "ko"
            };
            tempInput.Text = tempText;
            var response = client.DetectIntent(new SessionName("small-talk-7a50b", "sess-01"), tempInput);

            var queryResult = response.QueryResult;

            Console.WriteLine($"Query text: {queryResult.QueryText}");
            Console.WriteLine($"Intent detected: {queryResult.Intent.DisplayName}");
            Console.WriteLine($"Intent confidence: {queryResult.IntentDetectionConfidence}");
            Console.WriteLine($"Fulfillment text: {queryResult.FulfillmentText}");
            Console.WriteLine();
        }
    }
}
