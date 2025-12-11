using System;

namespace AppForSEII2526.LogViewer;

class Program
{
    static void Main(string[] args)
    {
        // CAMBIO: si no se pasa nada, por defecto escucha todos los logs
        var topicFilter = args.Length > 0 ? args[0] : "logs.#"; // CAMBIO

        var suscriber = new Subscriber(
            "localhost",
            5672,
            "guest",
            "guest",
            "logs.topic",
            topicFilter); // CAMBIO: pasamos el topic

        suscriber.StartListening();
        Console.WriteLine("Presiona Enter para salir...");
        Console.ReadLine();
    }
}


