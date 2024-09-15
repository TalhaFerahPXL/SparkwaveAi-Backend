using System.Net.WebSockets;
using System.Text;

namespace BriskAiHeadshot
{
    public class WebSocketHandler
    {
        private readonly List<WebSocket> _sockets = new List<WebSocket>();

        public void AddSocket(WebSocket socket)
        {
            _sockets.Add(socket);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    var segment = new ArraySegment<byte>(buffer);
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            AddSocket(webSocket);

            while (webSocket.State == WebSocketState.Open)
            {
                
                var buffer = new byte[1024 * 4];
                await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

           
            _sockets.Remove(webSocket);
        }
    }

}
