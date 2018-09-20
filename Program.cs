using System;
using HelloSign;

namespace HelloSignTestApp
{
    class Program
    {

        static void GetAccount(Client client) {
            var account = client.GetAccount();
            Console.WriteLine("My Account ID: " + account.AccountId);
        }

        static void CreateAccount(Client client) {
            try
            {
                var newAccount = client.CreateAccount("james@example.com");
                throw new Exception("This account already exists!" + newAccount.EmailAddress);
            }
            catch (BadRequestException)
            {
                Console.WriteLine("Was successfully blocked from creating a pre-existing account");
            }
        }

        static void SendSignatureRequest(Client client) {

            byte[] file1 = System.Text.Encoding.ASCII.GetBytes("Test document, please sign at the end");

            var request = new SignatureRequest();
            request.Title = "NDA with Acme Co";
            request.Subject = "The NDA we talked about";
            request.Message = "Please sign this NDA.";
            request.AddSigner("james@example.com", "James");
            request.AddCc("joe@example.com");
            request.AddFile(file1, "NDA.txt");
            request.AllowDecline = true;
            request.TestMode = true;
            var response = client.SendSignatureRequest(request);
            Console.WriteLine("New Signature Request ID: " + response.SignatureRequestId);
        }

        static void Main(string[] args)
        {

            // setting up the api client.
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            if (String.IsNullOrEmpty(apiKey)) {
                throw new Exception("You must provide your API Key.");
            }

            var client = new Client(apiKey);

            var menuSelection = "";

            if (args.Length > 0) {
                menuSelection = args[0];
            }

            if (menuSelection == "1") {
                 GetAccount(client);
            } else if (menuSelection == "2") {
                CreateAccount(client);
            } else if (menuSelection == "3") {
                SendSignatureRequest(client);
            } else {
                Console.WriteLine("Please make a selection: ");
                Console.WriteLine("Please select from the following options:");
                Console.WriteLine("1: Get Account");
                Console.WriteLine("2: Create Account");
                Console.WriteLine("3: Send Non-Embedded Signature Request");
            }
        }
    }
}
