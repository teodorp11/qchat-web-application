using Microsoft.AspNetCore.SignalR;
using qchat_web_application.Models;

namespace qchat_web_application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserRoomConnection> _connection;
        public ChatHub(IDictionary<string, UserRoomConnection> connection)
        {
            _connection = connection;
        }

        public async Task JoinRoom(UserRoomConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room!);
            await Clients.Group(userConnection.Room!)
                .SendAsync("ReceiveMessage", "QChat Bot", $"{userConnection.User} has joined the group.");
            await SendConnectedUser(userConnection.Room!);
        }

        public async Task SendMessage(string message)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out var userConnection))
            {
                await Clients.Group(userConnection.Room!)
                    .SendAsync("ReceiveMessage", userConnection.User, message);
            }
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (!_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection roomConnection))
            {
                return base.OnDisconnectedAsync(exception);
            }

            _connection.Remove(Context.ConnectionId);

            Clients.Group(roomConnection.Room!)
                .SendAsync("ReceiveMessage", "QChat Bot", $"{roomConnection.User} has left the group", DateTime.Now);
            
            SendConnectedUser(roomConnection.Room!);

            return base.OnDisconnectedAsync(exception);
        }

        public Task SendConnectedUser(string room)
        {
            var users = _connection.Values
                .Where(u => u.Room == room)
                .Select(u => u.User);
            return Clients.Group(room).SendAsync("Connected User", users);
        }
    }
}
