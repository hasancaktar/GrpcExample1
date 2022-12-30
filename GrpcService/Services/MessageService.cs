using Grpc.Core;
using MessageService;

namespace GrpcService.Services
{
    public class MessageService : Message.MessageBase
    {
        //Server Streaming
        //public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        //{
        //    Console.WriteLine($"isim: {request.Name} | Mesaj: {request.Message}");
        //    for (int i = 0; i < 10; i++)
        //    {
        //        await Task.Delay(1000);
        //        await responseStream.WriteAsync(new MessageResponse { Message = "Merhaba " + i });
        //    }
        //}


        //Client Streamin
        //public override async Task<MessageResponse> SendMessage(IAsyncStreamReader<MessageRequest> requestStream, ServerCallContext context)
        //{
        //    while (await requestStream.MoveNext(context.CancellationToken))
        //    {
        //        Console.WriteLine($"isim: {requestStream.Current.Name} | Mesaj: {requestStream.Current.Message}");
        //    }

        //    return new MessageResponse
        //    {
        //        Message = "Veri alındı.."
        //    };
        //}


        //bi-directional streaming
        public override async Task SendMessage(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            var task1= Task.Run(async () =>
            {
                while (await requestStream.MoveNext(context.CancellationToken))
                {
                    Console.WriteLine($"isim: {requestStream.Current.Name} | Mesaj: {requestStream.Current.Message}");
                }
            });
            for (int i = 0; i < 10; i++)
            {
                 await Task.Delay(1000);
                 await responseStream.WriteAsync(new MessageResponse { Message = "Mesaj"+i });
            }

            await task1;
        }
    }
}
