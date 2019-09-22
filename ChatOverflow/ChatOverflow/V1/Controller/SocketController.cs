using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var messages = new[]
            {
                "Message1",
                "Message2",
                "Message3",
                "Message4",
                "Message5"
            };

            foreach (var message in messages)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(bytes);
                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                Thread.Sleep(2000); //sleeping so that we can see several messages are sent
            }

            await webSocket.SendAsync(new ArraySegment<byte>(null), WebSocketMessageType.Binary, false, CancellationToken.None);
        }
    }
}