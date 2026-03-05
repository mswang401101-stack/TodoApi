using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models
{
    public class TodoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // MongoDB 自動生成的 ID

        public string? Name { get; set; } // 待辦事項的名稱

        public bool IsComplete { get; set; } // 是否已完成

        [BsonElement("Priority")] // 可以指定在 MongoDB 中儲存的欄位名稱
        public int PriorityLevel { get; set; } = 0; // 優先級別
    }
}
