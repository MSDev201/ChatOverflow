using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatOverflow.V1.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {

        [HttpGet("ws")]
        public async Task GetWebSocketAsync()
        {
            var context = ControllerContext.HttpContext;
            var isSocketRequest = context.WebSockets.IsWebSocketRequest;

            if (isSocketRequest)
            {
                WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                await GetMessages(context, ws);
            }
            else
                context.Response.StatusCode = 400;
        }

        [NonAction]
        private async Task GetMessages(HttpContext context, WebSocket webSocket)
        {

            var testObj = new
            {
                Message = "Meine Nachricht",
                User = new
                {
                    Name = "Test",
                    Id = 2
                },
                Heute = DateTime.Now
            };

            var message = JsonConvert.SerializeObject(testObj);

            var bytes = Encoding.ASCII.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes);
            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            var resBytes = new byte[result.Count];
            Array.Copy(buffer, 0, resBytes, 0, resBytes.Length);

            var resString = Encoding.ASCII.GetString(resBytes);
            var resObj = JsonConvert.DeserializeObject(resString);

            Thread.Sleep(2000); //sleeping so that we can see several messages are sent

            await webSocket.SendAsync(new ArraySegment<byte>(null), WebSocketMessageType.Binary, false, CancellationToken.None);
        }
    }
}