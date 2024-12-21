using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMSCore;

public class UserJsonConverter : JsonConverter<User>
{
    public override User? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            return null;
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            return null;
        }

        string? propertyName = reader.GetString();
        if (propertyName != "UserType")
        {
            return null;
        }

        reader.Read();
        var userType = reader.GetString();
        User user = userType switch
        {
            "Admin" => new Admin("default"),
            "Developer" => new Developer("default"),
            "ProjectManager" => new ProjectManager("default"),
            "Tester" => new Tester("default"),
            _ => throw new JsonException()
        };
        user.UserType = Enum.Parse<UserType>(userType);
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return user;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "UserId":
                        var value = reader.GetString();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return null;
                        }

                        user.UserId = value;
                        break;
                    case "Permissions":
                        break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("UserType", value.UserType.ToString());
        writer.WriteString("UserId", value.UserId);
        writer.WriteStartArray("Permissions");
        foreach (var element in value.UserPermissions)
        {
            writer.WriteStringValue(element.ToString());
        }
        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}