using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoModule.Models;
using ToDoModule.Services;
using ToDoModule.Services.Interfaces;

namespace ToDoModule.Controllers;

[Route("api/todo")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly IToDoItemService _toDoItemService;

    public ToDoItemsController(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
    }

    [HttpGet("GetToDoItems")]
    public async Task<ActionResult<IEnumerable<ToDoItems>>> GetToDoItems()
    {
        var items = await _toDoItemService.GetItemsAsync();
        return Ok(items);
    }

    [HttpGet("GetToDoItem/{partitionKey}/{rowKey}")]
    public async Task<ActionResult<ToDoItems>> GetToDoItem(string partitionKey, string rowKey)
    {
        var item = await _toDoItemService.GetItemAsync(partitionKey, rowKey);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost("PostToDoItem")]
    public async Task<ActionResult<ToDoItems>> PostToDoItem(ToDoItems item)
    {
        await _toDoItemService.AddItemAsync(item);
        return Created($"/api/todo/ToDoItems/{item.PartitionKey}/{item.RowKey}", item);
    }

    [HttpPut("PutToDoItem/{partitionKey}/{rowKey}")]
    public async Task<IActionResult> PutToDoItem(string partitionKey, string rowKey, ToDoItems item)
    {
        if (partitionKey != item.PartitionKey || rowKey != item.RowKey)
        {
            return BadRequest();
        }

        var existingItem = await _toDoItemService.GetItemAsync(partitionKey, rowKey);
        if (existingItem == null)
        {
            return NotFound();
        }

        await _toDoItemService.UpdateItemAsync(item);
        return NoContent();
    }

    [HttpDelete("DeleteToDoItem/{partitionKey}/{rowKey}")]
    public async Task<IActionResult> DeleteToDoItem(string partitionKey, string rowKey)
    {
        var existingItem = await _toDoItemService.GetItemAsync(partitionKey, rowKey);
        if (existingItem == null)
        {
            return NotFound();
        }

        await _toDoItemService.DeleteItemAsync(partitionKey, rowKey);
        return NoContent();
    }

}
