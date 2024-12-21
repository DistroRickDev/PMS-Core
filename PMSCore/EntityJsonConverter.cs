using System.Data;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMSCore;

public class EntityJsonConverter : JsonConverter<Entity>
{
        public override Entity? Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }
            
            string? propertyName = reader.GetString();
            if (propertyName != "Type")
            {
                throw new JsonException();
            }
            
            reader.Read();
            
            var entityType = reader.GetString();
            Entity entity = entityType switch
            {
                "Project" => new  Project("default", null, default, default, default),
                "Task" => new Task("default", null, default, default, default),
                _ => throw new JsonException()
            };
            entity.Type = Enum.Parse<EntityType>(entityType);
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return entity;
                }
            
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    var value = reader.GetString();
                    switch (propertyName)
                    {
                        case "Id":
                            if (value == null)
                            {
                                return null;
                            }
                            entity.Id = value;
                            break;
                        case "Description":
                            entity.Description = value;
                            break;
                        case "Status":
                            entity.Status = Enum.Parse<EntityStatus>(value!); 
                            break;
                        case "Priority":
                            entity.Priority =  Enum.Parse<EntityPriority>(value!); 
                            break;
                        case "CreatedDate":
                            entity.CreatedDate = DateTime.Parse(value!);
                            break;
                        case "StartedDate":
                            entity.StartedDate = string.IsNullOrEmpty(value) ? default : DateTime.Parse(value);
                            break;
                        case "FinishedDate":
                            entity.FinishedDate = string.IsNullOrEmpty(value) ? default : DateTime.Parse(value);
                            break;
                    }
                }
            }
            throw new JsonException();
        }

    
        public override void Write(Utf8JsonWriter writer, Entity entity, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            switch (entity)
            {
                case Project project:
                    writer.WriteString("Type", project.Type.ToString());
                    writer.WriteString("Id", project.Id);
                    writer.WriteString("Description", project.Description ?? string.Empty);
                    writer.WriteString("Status", project.Status.ToString());
                    writer.WriteString("Priority", project.Priority.ToString());
                    writer.WriteString("CreatedDate", project.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteString("StartedDate", project.StartedDate.ToString() ?? string.Empty);
                    writer.WriteString("FinishedDate", project.FinishedDate.ToString() ?? string.Empty);
                    break;
                case Task task:
                    writer.WriteString("Type", task.Type.ToString());
                    writer.WriteString("Id", task.Id);
                    writer.WriteString("Description", task.Description ?? string.Empty);
                    writer.WriteString("Status", task.Status.ToString());
                    writer.WriteString("Priority", task.Priority.ToString());
                    writer.WriteString("CreatedDate", task.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteString("StartedDate", task.StartedDate.ToString() ?? string.Empty);
                    writer.WriteString("FinishedDate", task.FinishedDate.ToString() ?? string.Empty);
                    break;
            }

            writer.WriteEndObject();
        }
}