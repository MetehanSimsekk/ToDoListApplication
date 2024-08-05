using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using ToDoModule.Models;
using ToDoModule.Services.Interfaces;

namespace ToDoModule.Services;
public class ToDoItemService : IToDoItemService
{
    private readonly TableClient _tableClient;
    public ToDoItemService(string storageAccountConnectionString, string tableName)
    {
        _tableClient = new TableClient(storageAccountConnectionString, tableName);
        _tableClient.CreateIfNotExists();
    }
    public async Task AddItemAsync(ToDoItems item)
    {
        item.PartitionKey = "ToDoItems";
        item.RowKey = Guid.NewGuid().ToString();
        item.CreatedDate = DateTime.UtcNow;
        item.ModifiedDate = DateTime.UtcNow;

        await _tableClient.AddEntityAsync(item);
    }

    public async Task DeleteItemAsync(string partitionKey, string rowKey)
    {
        await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<ToDoItems> GetItemAsync(string partitionKey, string rowKey)
    {
        var response = await _tableClient.GetEntityAsync<ToDoItems>(partitionKey, rowKey);
        return response.Value;

    }

    public async Task<IEnumerable<ToDoItems>> GetItemsAsync()
    {
        var query = _tableClient.Query<ToDoItems>();
        return await Task.FromResult(query.ToList());
    }

    public async Task UpdateItemAsync(ToDoItems item)
    {
        item.ModifiedDate = DateTime.UtcNow;
        await _tableClient.UpdateEntityAsync(item, item.ETag, TableUpdateMode.Replace);
    }
}
