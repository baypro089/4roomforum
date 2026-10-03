
using Microsoft.AspNetCore.SignalR;

namespace _4roomforum.Sockett
{
    public class CommentSocket : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var postId = Context.GetHttpContext()?.Request.Query["postId"].ToString();
            if (int.TryParse(postId, out var parsedPostId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(parsedPostId));
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var postId = Context.GetHttpContext()?.Request.Query["postId"].ToString();
            if (int.TryParse(postId, out var parsedPostId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(parsedPostId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public static string GetGroupName(int postId) => $"post:{postId}";

        public async Task ReceiveComment(int postId, object comment)
        {
            await Clients.Group(GetGroupName(postId)).SendAsync("ReceiveComment", comment);
        }
    }
}
