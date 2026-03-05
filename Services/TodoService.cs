using MongoDB.Driver;
using TodoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Services
{
    public class TodoService
    {
        private readonly IMongoCollection<TodoItem> _todoItems;

        public TodoService(IMongoClient client)
        {
            // 你可以將 "TodoDb" 替換成你希望在 MongoDB 中使用的資料庫名稱
            var database = client.GetDatabase("TodoDb");
            // 你可以將 "TodoItems" 替換成你希望在 MongoDB 中使用的集合名稱
            _todoItems = database.GetCollection<TodoItem>("TodoItems");
        }

        // 取得所有待辦事項
        public async Task<List<TodoItem>> GetAsync() =>
            await _todoItems.Find(item => true).ToListAsync();

        // 根據 ID 取得單一待辦事項
        public async Task<TodoItem?> GetAsync(string id) =>
            await _todoItems.Find(item => item.Id == id).FirstOrDefaultAsync();

        // 新增待辦事項
        public async Task CreateAsync(TodoItem newItem) =>
            await _todoItems.InsertOneAsync(newItem);

        // 更新待辦事項
        public async Task UpdateAsync(string id, TodoItem updatedItem) =>
            await _todoItems.ReplaceOneAsync(item => item.Id == id, updatedItem);

        // 刪除待辦事項
        public async Task RemoveAsync(string id) =>
            await _todoItems.DeleteOneAsync(item => item.Id == id);
    }
}