using Cache.Models;
using System.Net;
using System.Text.Json;

List<KeyValue> keyValues = new();

using var listener = new HttpListener();


listener.Prefixes.Add("http://localhost:27002/");

listener.Start();

while (true)
{
    var context = await listener.GetContextAsync();

    var request = context.Request;

    if (request == null)
        continue;

    switch (request.HttpMethod)
    {
        case "GET":
            {
                var response = context.Response;

                var key = request.QueryString["key"];

                var result = keyValues.FirstOrDefault(keyValues => keyValues.Key == key);

                if (result is null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Close();
                    break;
                }
                response.ContentType = "application/json";

                response.StatusCode = (int)HttpStatusCode.OK;

                var keyValue = result;
                var jsonStr = JsonSerializer.Serialize(keyValue);

                var writer = new StreamWriter(response.OutputStream);
                await writer.WriteAsync(jsonStr);
                writer.Flush();

                response.Close();
                break;
            }
        case "POST":
            {
                var response = context.Response;


                var stream = request.InputStream;
                var reader = new StreamReader(stream);

                var jsonStr = reader.ReadToEnd();

                var keyValue = JsonSerializer.Deserialize<KeyValue>(jsonStr);

                if (keyValue is null)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Close();
                    break;
                }

                keyValues.Add(keyValue);


                response.StatusCode = (int)HttpStatusCode.OK;

                response.Close();
                break;
            }
        case "PUT":
            {
                var response = context.Response;
                var stream = request.InputStream;

                var reader = new StreamReader(stream);

                var jsonStr = reader.ReadToEnd();

                var temp = JsonSerializer.Deserialize<KeyValue>(jsonStr);

                var keyValue = keyValues.Find
                    ((keyValue) =>
                        keyValue.Key == temp?.Key);

                if (keyValue is null || temp is null)
                    break;

                keyValue.Value = temp.Value;

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Close();
                break;
            }
    }
}