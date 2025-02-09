using dotenv.net;

namespace Config
{
    class BotConfig
    {
        // Bot prefix
        // Префикс бота
        public static readonly string prefix = ">";
        
        // Developer information
        // Информация о разработчике
        public static readonly string developerTag = "ar9entum";
        public static readonly Int64 developerId = 922814470210850846;
        
        // Array with Discord user IDs that have access to various privileged features of the bot
        // Массив с ID пользователей Discord, имеющих доступ к различным привилегированным функциям бота
        public static Int64[] botAllowedUsers = [developerId, 785860773850120212];

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
            return envVars["TOKEN"];
        }
    }
}