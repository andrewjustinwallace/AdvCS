using Microsoft.Extensions.AI;

IChatClient client =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "llama3.1");

var response = await client.CompleteAsync("What is AI?");

Console.WriteLine(response.Message);



