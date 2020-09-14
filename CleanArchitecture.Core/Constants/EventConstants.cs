using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Constants
{
    public static class EventConstants
    {
        public static EventId GetTodoEventId = new EventId(1001, "GetTodo");
    }
}
