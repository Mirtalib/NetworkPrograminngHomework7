using Server.Models;
using System.Net;
using System.Text.Json;

var listener = new HttpListener();

using var client = new HttpClient();
Dictionary<int, string> requestCounts = new();

listener.Prefixes.Add(@"http://localhost:27001/");


listener.Start();
Console.WriteLine("Listener...");

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

                var value = request.QueryString["key"];

                if (key == null)
                    continue;


                if (!requestCounts.ContainsKey(key))
                    requestCounts[key] = 0;

                requestCounts[key]++;

                if (requestCounts[key] >= 3)
                {
                    var clientResponse = await client.GetAsync($"http://localhost:27002/?key={key}");

                    if (clientResponse.StatusCode == HttpStatusCode.OK)
                    {
                        response.ContentType = "application/json";

                        response.StatusCode = (int)HttpStatusCode.OK;



                        var jsonStr = await clientResponse.Content.ReadAsStringAsync();

                        var writer = new StreamWriter(response.OutputStream);
                        await writer.WriteAsync(jsonStr);
                        writer.Flush();

                    }
                    else
                    {
                        var dbContext = new AppkeyvalueContext();

                        var x = dbContext.Find<KeyValue>(key);

                        if (x is not null)
                        {
                            response.ContentType = "application/json";

                            response.StatusCode = (int)HttpStatusCode.OK;

                            var keyValue = x;
                            var jsonStr = JsonSerializer.Serialize(x);

                            var writer = new StreamWriter(response.OutputStream);
                            await writer.WriteAsync(jsonStr);
                            writer.Flush();

                            var content = new StringContent(jsonStr);
                            await client.PostAsync("http://localhost:27002/", content);
                        }
                        else
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    var dbContext = new AppkeyvalueContext();

                    var temp = dbContext.Find<KeyValue>(key);

                    if (temp is not null)
                    {
                        response.ContentType = "application/json";

                        response.StatusCode = (int)HttpStatusCode.OK;

                        var keyValue = temp;
                        var jsonStr = JsonSerializer.Serialize(keyValue);

                        var writer = new StreamWriter(response.OutputStream);
                        await writer.WriteAsync(jsonStr);
                        writer.Flush();

                    }
                    else
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                }

                response.Close();

                break;
            }
        case "POST":
            {
                var stream = request.InputStream;
                var reader = new StreamReader(stream);

                var jsonStr = reader.ReadToEnd();

                Console.WriteLine(jsonStr);

                var keyValue = JsonSerializer.Deserialize<KeyValue>(jsonStr);

                if (keyValue is null)
                    continue;

                var response = context.Response;

                var dbContext = new AppkeyvalueContext();
                var value = keyValue.Value;

                if (dbContext.Find<KeyValue>(value) == null)
                {
                    if (!requestCounts.ContainsKey(value))
                        requestCounts[value] = 0;

                    requestCounts[value]++;

                    dbContext.Add(keyValue);
                    dbContext.SaveChanges();
                    response.StatusCode = (int)HttpStatusCode.OK;

                }
                else
                    response.StatusCode = (int)HttpStatusCode.Found;

                response.Close();

                break;
            }

        case "PUT":
            {
                var stream = request.InputStream;
                var reader = new StreamReader(stream);

                var jsonStr = reader.ReadToEnd();

                Console.WriteLine(jsonStr);

                var keyValue = JsonSerializer.Deserialize<KeyValue>(jsonStr);

                var response = context.Response;

                var dbContext = new AppkeyvalueContext();

                var temp = dbContext.Find<KeyValue>(keyValue.Key);

                if (temp != null)
                {
                    if (requestCounts[temp.Key] >= 3)
                    {
                        var content = new StringContent(jsonStr);
                        await client.PutAsync("http://localhost:27002/", content);
                    }

                    temp.Value = keyValue.Value;

                    dbContext.SaveChanges();
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                    response.StatusCode = (int)HttpStatusCode.NotFound;

                response.Close();

                break;
            }

    }

}

