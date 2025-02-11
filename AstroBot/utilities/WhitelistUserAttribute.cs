using Discord.Commands;
using Config;
using Discord.WebSocket;

// Creating an attribute to exclude unauthorized use of the command
// Создание атрибута для исключения несанкционированного использования команды
public class WhiteListUserAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        // Check if the user ID is in the list of allowed users
        // Проверяем, есть ли ID пользователя в списке разрешенных
        if (BotConfig.whiteListUsers.Contains(context.User.Id))
        {
            return PreconditionResult.FromSuccess(); // Success
        }
        // The user who used the command
        // Пользователь который использовал команду
        var commandUser = context.User as SocketGuildUser;
        
        // If there is no access, just return an error
        // Если доступа нет, просто возвращаем ошибку
        return PreconditionResult.FromError($"Осторожно! Пользователь {commandUser.GlobalName} ({commandUser.Id}) пытался использовать команду к которой не имеет доступа!");
    }

}