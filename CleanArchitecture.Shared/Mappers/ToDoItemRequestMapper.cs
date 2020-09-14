using AZV3CleanArchitecture.Models;
using AZV3CleanArchitecture.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace AZV3CleanArchitecture.Mappers
{
    public static class ToDoItemRequestMapper
    {
        public static ToDoItem MapToDoItemRequest(ToDoItemCreateRequest toDoItemRequest)
        {
            return new ToDoItem() { UserId = toDoItemRequest.UserId, Completed = toDoItemRequest.Completed, Title = toDoItemRequest.Title };
        }
    }
}
