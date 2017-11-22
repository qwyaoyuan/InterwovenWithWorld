using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class CustomDTJsonConverter : JsonConverter
{

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.Integer)
        {
            throw new Exception(
                String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                    reader.TokenType));
        }

        var ticks = (long)reader.Value;


        var date = DateTime.FromBinary(ticks);

        return date;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime);
    }

    public override void WriteJson(JsonWriter writer, object value,
        JsonSerializer serializer)
    {
        long ticks;
        if (value is DateTime)
        {

            ticks = ((DateTime)value).ToBinary();
        }
        else
        {
            throw new Exception("Expected date object value.");
        }
        writer.WriteValue(ticks);
    }
}
