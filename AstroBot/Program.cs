// Developer: ar9entum
// Discord bot Memories Shop created in C# using Discord.net library
// Дискорд-бот Memories Shop, созданный на языке C# с использованием библиотеки Discord.net

// Main file
// Основной файл

// Dependencies (libraries)
// Зависимости (библиотеки)
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Discord.Interactions;
using Config;
using Microsoft.VisualBasic;

namespace AstroBot 
{
    // Entry class of the program
    // Входной класс программы
    class Program 
    {
        // Bot client instance
        // Экземпляр клиента бота
        public static DiscordSocketClient client;

        // Token for bot authentication
        // Токен для аутентификации бота
        public static string token = BotConfig.getToken();

        // Command prefix
        // Префикс команд
        public static string prefix = BotConfig.prefix;
        
        // Command and interaction services
        // Сервисы команд и взаимодействий
        private CommandService commands;
        private InteractionService interactions;
        private IServiceProvider services;
        
        // Entry point of the program
        // Входная точка программы
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            // Requesting bot intents (permissions)
            // Запрос интентов (разрешений) для бота
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All
            };
            
            // Initializing services
            // Инициализация сервисов
            client = new DiscordSocketClient(config);
            commands = new CommandService();
            interactions = new InteractionService(client);
            
            // Building dependency injection container
            // Создание контейнера зависимостей
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton(interactions)
                .BuildServiceProvider();

            // Registering event handlers
            // Регистрация обработчиков событий
            client.Log += clientLog;
            client.MessageReceived += HandleCommandAsync;
            client.InteractionCreated += HandleInteractionAsync;
            client.ModalSubmitted += HandleModalSubmitted;
            client.Ready += ClientReady;
            
            // Loading command modules
            // Загрузка модулей команд
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await interactions.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            // Logging in and starting the bot
            // Авторизация и запуск бота
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            // Keeping the bot running indefinitely
            // Поддержание работы бота бесконечно
            await Task.Delay(-1);
        }
        
        // Function to register slash commands on bot startup
        // Функция для регистрации слэш-команд при запуске бота
        private async Task ClientReady() 
        {
            await interactions.RegisterCommandsGloballyAsync();
        }

        // Handling text commands
        // Обработка текстовых команд
        private async Task HandleCommandAsync(SocketMessage arg) 
        {
            // Checking if the message is from a bot or null
            // Проверка, является ли сообщение от бота или null
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;
            
            // Creating a command context
            // Создание контекста команды
            var context = new SocketCommandContext(client, message);

            int argPos = 0;
            // Checking if the message starts with the bot's prefix
            // Проверка, начинается ли сообщение с префикса бота
            if (message.HasStringPrefix(prefix, ref argPos))
            {
                // Executing the command
                // Выполнение команды
                var result = await commands.ExecuteAsync(context, argPos, services);
                
                // Logging errors if the command fails
                // Логирование ошибок, если команда не выполнена
                if (!result.IsSuccess) 
                    Console.WriteLine(result.ErrorReason);
                
                // Sending an error message to the user if the command cannot be executed
                // Отправка сообщения об ошибке пользователю, если команда не может быть выполнена
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                    await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
        
        // Handling slash commands and other interactions
        // Обработка слэш-команд и других взаимодействий
        private async Task HandleInteractionAsync(SocketInteraction interaction) 
        {
            try
            {
                // Creating an interaction context
                // Создание контекста взаимодействия
                var context = new SocketInteractionContext(client, interaction);
                
                // Executing the interaction command
                // Выполнение команды взаимодействия
                await interactions.ExecuteCommandAsync(context, services);
            }
            catch (Exception ex)
            {
                // Logging errors
                // Логирование ошибок
                Console.WriteLine($"Error handling interaction: {ex.Message}");
            }
        }
        
        // Handling modal (form-based) interactions
        // Обработка модальных (формовых) взаимодействий
        private async Task HandleModalSubmitted(SocketModal modal) 
        {
            try
            {
                // Creating a modal interaction context
                // Создание контекста модального взаимодействия
                var context = new SocketInteractionContext<SocketModal>(client, modal);
                
                // Executing the modal command
                // Выполнение команды модального взаимодействия
                await interactions.ExecuteCommandAsync(context, services);
            }
            catch (Exception ex)
            {
                // Logging errors
                // Логирование ошибок
                Console.WriteLine($"Error handling modal interaction: {ex.Message}");
            }
        }

        // Logging bot messages to the console
        // Логирование сообщений бота в консоль
        private Task clientLog(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
        
    }
}