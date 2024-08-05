using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoModule.Models;

namespace ToDoModule.Services.Interfaces;
public interface IToDoItemService
{
    Task AddItemAsync(ToDoItems item);
    Task<ToDoItems> GetItemAsync(string partitionKey, string rowKey);
    Task<IEnumerable<ToDoItems>> GetItemsAsync();
    Task UpdateItemAsync(ToDoItems item);
    Task DeleteItemAsync(string partitionKey, string rowKey);
}
