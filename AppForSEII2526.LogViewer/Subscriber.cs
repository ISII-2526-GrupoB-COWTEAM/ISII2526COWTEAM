using System.Collections;
using System.Composition;
using System.Threading.Channels;
using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppForSEII2526.LogViewer
{
    public class Subscriber : IDisposable
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public Subscriber(string hostName, int port, string userName, string password, string exchangeName)
        {
            _hostName = hostName ?? throw new ArgumentNullException(nameof(hostName));
            _port = port;
            _userName = userName ?? throw new ArgumentNullException(nameof(userName));
            _password = password ?? throw new ArgumentNullException(nameof(password));
            _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));

            // Crear la conexión
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar el exchange con durable = true para que coincida con el publicador
            _channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null);

            // Crear cola efímera (temporal, exclusiva y auto-eliminable)
            var tempQueue = _channel.QueueDeclare(
                queue: "",
                durable: true,
                exclusive: true,
                autoDelete: true,
                arguments: null);

            // Recuperar el nombre asignado por RabbitMQ
            _queueName = tempQueue.QueueName;

            // Enlazar la cola al exchange
            _channel.QueueBind(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: "");

            Console.WriteLine($"[*] Conectado a RabbitMQ en {_hostName}:{_port}");
            Console.WriteLine($"[*] Escuchando logs desde el exchange '{_exchangeName}'");
            Console.WriteLine($"[*] Cola creada: {_queueName}");
            Console.WriteLine("[*] Esperando mensajes. Presiona CTRL+C para salir.\n");
        }

     public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Deserializar el mensaje JSON
                    var logEntry = JsonSerializer.Deserialize<LogEntry>(message);

                    if (logEntry != null)
                    {
                        DisplayLog(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[ERROR] Error procesando mensaje: {ex.Message}");
                }
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void DisplayLog(LogEntry logEntry)
        {
            // Formatear y mostrar el log por consola con colores
            var color = GetColorForLogLevel(logEntry.LogLevel);
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"[{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] ");

            Console.ForegroundColor = color;
            Console.Write($"[{logEntry.LogLevel}] ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"[{logEntry.Category}] ");

            if (logEntry.EventId > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[EventId: {logEntry.EventId}");
                if (!string.IsNullOrEmpty(logEntry.EventName))
                {
                    Console.Write($" - {logEntry.EventName}");
                }
                Console.Write("] ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(logEntry.Message);

            if (!string.IsNullOrEmpty(logEntry.Exception))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   Exception: {logEntry.Exception}");
            }

            Console.ForegroundColor = originalColor;
            Console.WriteLine();
        }

        private static ConsoleColor GetColorForLogLevel(string? logLevel)
        {
            return logLevel switch
            {
                "Trace" => ConsoleColor.DarkGray,
                "Debug" => ConsoleColor.Gray,
                "Information" => ConsoleColor.Green,
                "Warning" => ConsoleColor.Yellow,
                "Error" => ConsoleColor.Red,
                "Critical" => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };
        }

        public void Dispose()
        {
            try
            {
                Console.WriteLine("\n[*] Cerrando conexión...");
                _channel?.Close();
                _channel?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ERROR] Error al cerrar la conexión: {ex.Message}");
            }
            GC.SuppressFinalize(this);
        }

    // Clase interna para deserializar los logs
    private class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? LogLevel { get; set; }
        public string? Category { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
    }

}
}

