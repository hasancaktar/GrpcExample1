using Grpc.Net.Client;
using MessageService;
namespace MyClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5265");
            var messageClient = new Message.MessageClient(channel);
            //----unary yöntemi
            //MessageResponse response = messageClient.SayHello(new MessageRequest { Message = "Gönderilen Mesaj", Name = "Hasan" });


            //---server streaming
            //var response = messageClient.SendMessage(new MessageRequest
            //{
            //    Message = "Stream yöntemi",
            //    Name = "Stream"
            //});

            //----CLIENT STREAMING
            //await  response.ResponseAsync(new MessageRequest { Message = "MESAJ", Name = "İSİM" });
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
            //{

            //    Console.WriteLine(response.ResponseStream.Current.Message);
            //}


            //----CLIENT STREAMING
            //var request = messageClient.SendMessage();
            //for (int i = 0; i < 10; i++)
            //{
            //    await Task.Delay(1000);
            //    await request.RequestStream.WriteAsync(new MessageRequest { Message = "Mesaj " + i, Name = "İsim" });
            //}

            ////stream  datanın sonlandığını ifade eder
            //await request.RequestStream.CompleteAsync();

            //Console.WriteLine((await request.ResponseAsync).Message);



            var request = messageClient.SendMessage();

            //request işlemini farklı bir thread da yapıyoruz burada
            var task1 = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    await request.RequestStream.WriteAsync(new MessageRequest
                    {
                        Message = "Mesaj " + i,
                        Name = "İsim"
                    });
                }
            });
            while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
            {
                Console.WriteLine(request.ResponseStream.Current.Message);
               

                 
            }
            await task1;
            await request.RequestStream.CompleteAsync();
            

        }
    }
}