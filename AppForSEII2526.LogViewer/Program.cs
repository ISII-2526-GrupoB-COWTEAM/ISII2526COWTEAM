
using System;

namespace AppForSEII2526.LogViewer;

class Program
{
    static void Main(string[] args)
    {
        var suscriber = new Subscriber("localhost",5672,"guest","guest","logs");
        suscriber.StartListening();
        Console.WriteLine("Presiona Enter para salir...");
        Console.ReadLine();

    }
}
