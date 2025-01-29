/*using System;
using System.Text.Json;*/

namespace TodoApi.Models
{
    public interface IJsonSerializationHelper
    {
        string SerializeObject<T>(T obj);
    }
}
