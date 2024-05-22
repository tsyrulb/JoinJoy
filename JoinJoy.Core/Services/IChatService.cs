using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IChatService
    {
        Task<string> ProcessMessageAsync(ChatMessageRequest request);
    }
}