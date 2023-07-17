using Polly;
var counter = 0;

var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    10,
                    retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm:ss}" +
                            $" , Attempt {retryCount + 1} , Message = {exception.Message}");
                    }
                );

await policy.ExecuteAsync(async () => await DoAction(ref counter));

static Task DoAction(ref int counter)
{
    if (counter <= 3)
    {
        counter++;
        throw new Exception();
    }
    Console.WriteLine($"Action Done at : {DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm:ss}");
    return Task.CompletedTask;
}