﻿using System;

using System.Threading.Tasks;

using Discord.Commands;

namespace Hadouken.Modules.YellBarf
{
    public class YeahBoiiiModule : ModuleBase<SocketCommandContext>
    {
        [Command("yb")]
        [Summary("Yeah boooiiiiiiiiii")]
        public async Task YeahBoiiiiiAsync()
        {
            var yeahBoi = "yeah boy";
            var random = new Random(DateTime.UtcNow.Millisecond);

            for (var i = 0; i < random.Next(10, 30); i++)
            {
                yeahBoi += "-oi";
            }

            await ReplyAsync(yeahBoi, true);
        }
    }
}