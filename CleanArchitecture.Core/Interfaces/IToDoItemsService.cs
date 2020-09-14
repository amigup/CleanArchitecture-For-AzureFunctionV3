using AZV3CleanArchitecture.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AZV3CleanArchitecture.Services
{
    public interface IToDoItemsService
    {
        Task<IEnumerable<ToDoItem>> GetAllToDoItems(int id);
        Task<ToDoItem> GetToDoItem(int id);

        Task<ToDoItem> CreateToDoItem(ToDoItem toDoItem);

        Task<ToDoItem> UpdateToDoItem(ToDoItem toDoItem);
    }
}