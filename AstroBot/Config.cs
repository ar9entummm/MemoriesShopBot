using dotenv.net;

namespace Config
{
    class BotConfig
    {
        // Bot prefix
        // Префикс бота
        public const string prefix = ">";  // Используем const, так как значение не меняется

        // Developer information
        // Информация о разработчике
        public const string developerTag = "ar9entum";
        public const Int64 developerId = 922814470210850846;
        
        // Array with Discord user IDs that have access to various privileged features of the bot
        // Массив с ID пользователей Discord, имеющих доступ к различным привилегированным функциям бота
        public static Int64[] botAllowedUsers = new Int64[] { developerId, 785860773850120212 };

        // Function for obtaining a token
        // Функция для получения токена
        public static string getToken()
        {
            // Loading and retrieving variables from an .env file
            // Загрузка и получение переменных из .env файла
            DotEnv.Load();
            var envVars = DotEnv.Read();
            
            // Token return
            // Возврат токена
            if (envVars.ContainsKey("TOKEN"))
            {
                return envVars["TOKEN"];
            }
            else
            {
                throw new Exception("TOKEN is not set in the .env file.");
            }
        }
    }
}