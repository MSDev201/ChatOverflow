using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatOverflow.Infrastructure.SocketProvider;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.V1.Models.ContentModels;
using ChatOverflow.V1.Models.SocketModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatOverflow.V1.Controller
{
    public class SocketController : ApiController
    {

        private readonly ISocketProvider _socket;
        private readonly IUserProvider _user;

        public SocketController(ISocketProvider socket, IUserProvider user)
        {
            _socket = socket;
            _user = user;
        }


        [Authorize]
        [HttpGet("Token")]
        public async Task<IActionResult> GetToken()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();

            var token = await _socket.GenerateAccessTokenAsync(user);
            if (token == null)
                return BadRequest();
            return Ok(new StringContent { Value = token.Token });
        }

        [HttpGet("ws")]
        public async Task GetWebSocketAsync()
        {
            var context = ControllerContext.HttpContext;
            var isSocketRequest = context.WebSockets.IsWebSocketRequest;

            if (isSocketRequest)
            {
                WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();

                if(!(await AuthenticateAsync(ws)))
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Invalide credentials", CancellationToken.None);
                }
                
                while(ws.State == WebSocketState.Open)
                {
                    await GetMessages(context, ws);
                }
                // Handle close

                
            }
            else
                context.Response.StatusCode = 400;
        }

        [NonAction]
        private async Task<bool> AuthenticateAsync(WebSocket ws)
        {
            var handshake = await ReceiveAsyc<HandshakeInput>(ws);
            if (_socket.GetUserByAccessToken(handshake.Token) == null)
                return false;
            return true;

        }

        [NonAction]
        private async Task<T> ReceiveAsyc<T>(WebSocket ws)
        {
            var message = new List<byte>();
            WebSocketReceiveResult response;
            var cancelToken = new CancellationToken();
            var buffer = new byte[4096];
            do
            {
                response = await ws.ReceiveAsync(buffer, cancelToken);
                message.AddRange(new ArraySegment<byte>(buffer, 0, response.Count));
            } while (!response.EndOfMessage);

            var resString = Encoding.ASCII.GetString(message.ToArray());
            var resObj = JsonConvert.DeserializeObject<T>(resString);

            return resObj;
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

        }

        [NonAction]
        private async Task<(WebSocketReceiveResult, IEnumerable<byte>)> ReceiveFullMessage(WebSocket socket, CancellationToken cancelToken)
        {
            WebSocketReceiveResult response;
            var message = new List<byte>();

            var buffer = new byte[4096];
            do
            {
                response = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancelToken);
                message.AddRange(new ArraySegment<byte>(buffer, 0, response.Count));
            } while (!response.EndOfMessage);

            return (response, message);
        }
    }
}