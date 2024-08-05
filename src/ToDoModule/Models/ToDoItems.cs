using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace ToDoModule.Models;
public class ToDoItems : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public ETag ETag { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}
