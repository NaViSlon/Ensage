using System;
using System.Linq;
using Ensage;
using Ensage.Common;
using SharpDX;
using System.Windows.Input;

namespace Save_Yorself
{
    class Save_Yorself
    {
        private const Key toggleKey = Key.G;
        private static bool active = true;
        private static Hero me;
        private static System.Collections.Generic.List<Ensage.Hero> enemies;
        private static SideMessage informationmessage;
        private static bool loaded;
        private static string toggleText;

       
        private static void Game_OnUpdate(EventArgs args)
        {
            if (!loaded)
            {
                me = ObjectMgr.LocalHero;

                if (!Game.IsInGame || Game.IsWatchingGame || me == null || Game.IsChatOpen)
                {
                    return;
                }

                loaded = true;
                toggleText = "(" + toggleKey + ") AutoSunder: On";
            }

            if (me == null || !me.IsValid)
            {
                loaded = false;
                me = ObjectMgr.LocalHero;
                active = false;
            }

            if (Game.IsPaused) return;

            if (Game.IsKeyDown(toggleKey) && Utils.SleepCheck("toggle") && !Game.IsChatOpen && !Game.IsPaused && !Game.IsWatchingGame)
            {
                if (!active)
                {
                    active = true;
                    toggleText = "(" + toggleKey + ") AutoSunder: On";
                }
                else
                {
                    active = false;
                    toggleText = "(" + toggleKey + ") AutoSunder: Off";
                }

                Utils.Sleep(200, "toggle");
            }
        }
        static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Tick;
            PrintSuccess(string.Format("> Save Yourself"));
        }
        public static void Tick(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame)
                return;
            me = ObjectMgr.LocalHero;
            if (me == null)
                return;
            enemies = ObjectMgr.GetEntities<Hero>().Where(x => me.Team != x.Team && !x.IsIllusion && x.IsAlive).ToList();
            if (enemies == null)
                return;

            foreach (var v in enemies)
            {
                if (active)
                {
                    uint x;
                    string z;
                    if (me.Player.Hero.Level < 11  && Utils.SleepCheck("1") && me.Player.Hero.IsAlive && me.ClassID == ClassID.CDOTA_Unit_Hero_Terrorblade)
                    {
                        if (me.Player.Hero.Health < 200)
                        {
                            if (v.Player.Hero.Health > me.Player.Hero.Health)
                            {
                                me.Spellbook.SpellR.UseAbility(v.Player.Hero);
                                Utils.Sleep(1000, "1");
                            }
                        }

                    }
                    if (me.Player.Hero.Level >11  && Utils.SleepCheck("2") && me.Player.Hero.IsAlive && me.ClassID == ClassID.CDOTA_Unit_Hero_Terrorblade)
                    {
                        
                        if (me.Player.Hero.Health < 400)
                        {
                            if (v.Player.Hero.Health > me.Player.Hero.Health)
                            {

                                me.Spellbook.SpellR.UseAbility(v.Player.Hero);
                                Utils.Sleep(1000, "2");
                            }
                        }

                    }

                }
            }
        }




        private static void Drawing_OnDraw(EventArgs args)
        {
            if (loaded)
            {
                Drawing.DrawText("Terror!", new Vector2(Drawing.Width * 5 / 100, Drawing.Height * 19 / 100), Color.LightGreen, FontFlags.DropShadow);
                Drawing.DrawText(toggleText,
                    new Vector2(Drawing.Width * 5 / 100, Drawing.Height * 20 / 100), Color.LightGreen, FontFlags.DropShadow);
            }
        }   


        private static void PrintSuccess(string text, params object[] arguments)
        {
            PrintEncolored(text, ConsoleColor.Green, arguments);
        }
        private static void PrintEncolored(string text, ConsoleColor color, params object[] arguments)
        {
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, arguments);
            Console.ForegroundColor = clr;
        }
    }
}
