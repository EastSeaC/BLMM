using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using System.Threading;
using TaleWorlds.Core;

namespace BLMMClient.Helpers
{
    internal class ServerHelper
    {
        private WebSocketServer _server;

        public async void StartWSServer()
        {
            _server = new WebSocketServer();
            await _server.Start("http://localhost:8848/");
        }

        public static void ReStartMission()
        {
            Mission.Current.ResetMission();
        }
    }

    public class WebSocketServer
    {
        private HttpListener _httpListener;
        private CancellationTokenSource _cancellationTokenSource;


        public async Task Start(string url)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(url);
            _httpListener.Start();

            Console.WriteLine("WebSocket server started.");

            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    ProcessWebSocketRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }

            _httpListener.Stop();
            _httpListener.Close();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async void ProcessWebSocketRequest(HttpListenerContext context)
        {
            HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);

            WebSocket webSocket = webSocketContext.WebSocket;

            byte[] buffer = new byte[1024];

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    //Console.WriteLine("Received message: " + message);
                    string[] command = message.Split(' ');
                    if (command.Length > 0)
                    {
                        string op = command[0];
                        if (op.Equals("StartGame"))
                        {
                            ServerHelper.ReStartMission();
                        }
                        else
                        {

                        }
                    }
                    // 在这里处理接收到的消息，并根据需要发送响应

                    // 示例：回复收到的消息
                    byte[] responseBuffer = Encoding.UTF8.GetBytes("Received: " + message);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
            }
        }
    }


}
