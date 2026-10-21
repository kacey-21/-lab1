using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoApp
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonSerializerOptions _options;

        public JsonTaskRepository()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public void Save(IEnumerable<TodoModel> tasks, string filePath)
        {
            string json = JsonSerializer.Serialize(tasks, _options);
            File.WriteAllText(filePath, json);
        }

        public List<TodoModel> Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<TodoModel>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<TodoModel>>(json, _options) ?? new List<TodoModel>();
        }
    }
}