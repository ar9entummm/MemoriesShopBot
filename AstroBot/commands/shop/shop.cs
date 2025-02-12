using Discord;
using Discord.Commands;
using Config;

namespace AstroBot.commands.shop
{
    public class Shop : ModuleBase<SocketCommandContext>
    {
        [Command("shop")]
        [WhiteListUser] // Is the user on the whitelist
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator)] // Is the user is guild administrator
        public async Task shopAsync()
        {
            // Creating a message with embed for the shop
            // Создание сообщения с эмбедом для магазина
            var embed = new EmbedBuilder()
                .WithTitle(":purple_heart: Рады приветствовать вас в нашем уникальном магазине Memories Shop! :purple_heart:")
                .WithAuthor("Memories Shop")
                .WithDescription("Мы предлагаем вам широкий ассортимент товаров, начиная от **подписок** заканчивая **игровыми аккаунтами** и **услугами**. Мы постоянно пополняем наш ассортимент новыми удивительными продуктами, чтобы вы всегда могли найти что-то новое и интересное. Загляните к нам и убедитесь в высоком качестве нашей продукции и первоклассном обслуживании. Мы всегда рады вам! Не упустите шанс приобрести что-то особенное по самым выгодным ценам")
                .WithColor(BotConfig.mainColor)
                .WithImageUrl("https://media.discordapp.net/attachments/1235698640140636163/1339072884487884800/Frame_14973.png?ex=67ad644e&is=67ac12ce&hm=aba89d4312c5fc2cdaf22bfd2fc8da747c5337f534fdb3c3633371bd91611620&=&format=webp&quality=lossless&width=1620&height=630")
                .Build();

            // Receive message view (button)
            // Получение вида сообщения (кнопка)
            var view = GetView();
            
            // Sending a complete message
            // Отправка полного сообщения
            await ReplyAsync(embed: embed, components: view);
        }

        // Function for creating a message view
        // Функция для создания вида сообщения
        MessageComponent GetView()
        {
            // The “buy” button
            // Кнопка "купить"
            var buyButton = new ButtonBuilder()
                .WithLabel("Купить")
                .WithEmote((Emote)"<:walletdynamiccolor:1251729922070941728>")
                .WithStyle(ButtonStyle.Primary)
                .WithCustomId("ShopMessage_BuyButton");
            
            // Assembling a complete view of the message
            // Сборка полного вида сообщения
            var view = new ComponentBuilder()
                .WithButton(buyButton);
            
            // Return of view
            // Возврат вида 
            return view.Build();
        }
    }
}