﻿using System.Net;

var listener = new HttpListener();

listener.Prefixes.Add(@"http://localhost:27001/");
listener.Prefixes.Add(@"http://localhost:27002/");

listener.Start();
Console.WriteLine("Listener...");

while (true)
{
    var context = await listener.GetContextAsync();

    var request = context.Request;
    var response = context.Response;




    switch (request.HttpMethod)
    {
        case "GET":
            {

                break;
            }
        case "POST":
            {

                break;
            }
        case "PUT":
            {

                break;
            }
        default:
            break;
    }
    
    // var writer = new StreamWriter(response.OutputStream);
    // await writer.Write(,)  
}

