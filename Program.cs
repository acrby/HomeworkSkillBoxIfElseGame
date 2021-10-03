using System;
using System.Collections.Generic;

namespace HomeworkSkillBoxIfElseGame
{

    class Program
    {
        static void WelcomeRules()
        {
            Console.WriteLine("                                         Добро пожаловать в игру с числами.");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Программа генерирует число в «корзину» от 12 до 120, после чего игрокам пошагово предлагается вводить числа от 1 до [указанного в настройках]");
            Console.WriteLine("После каждого шага программа вычитает введенное игроком число от предыдущего результата.");
            Console.WriteLine("Так происходит до тех пор, пока значение «корзины» не станет равно 0");
            Console.WriteLine("Победителем признается тот, чей ход оказался решающим.");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
        }

        static int SetValueRange()
        {
            int valRange = 4;
            Console.Write("                    Установите диапазон разрешенных для ввода значений игрока от 1 до: ");
            try
            {
                valRange = Int32.Parse(Console.ReadLine());
                if (valRange < 4) valRange = 4;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Знанчение по умолчанию для диапазона установлено: 4");
            }
            return valRange;
        }

        static bool PlayerVsPlayer()
        {
            bool plVsPL = false;
        RepeatInput:
            Console.WriteLine($"F1 - Игрок против Игрока\nF2 - Игрок против компьютера");
            ConsoleKeyInfo KeyPressed;
            KeyPressed = Console.ReadKey();
            if (KeyPressed.Key == ConsoleKey.F1) { plVsPL = true; }
            else if (KeyPressed.Key == ConsoleKey.F2) { plVsPL = false; }
            else { Console.WriteLine("Неверный ввод."); goto RepeatInput; }
            return plVsPL;
        }

        static int SetPlayersQty()
        {
            int playersQty = 2;
            Console.Write("                    Введите количество игроков, которые будут участвовать в игре: ");
            try
            {
                playersQty = Int32.Parse(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Значение по умолчанию для диапазона установлено: 2");
            }
            return playersQty;
        }

        static int SetBotsQty()
        {
            int playersQty = 0;
            Console.Write("                    Введите количество ботов, которые будут участвовать в игре: ");
            try
            {
                playersQty = Int32.Parse(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Значение по умолчанию для диапазона установлено: 0");
            }
            return playersQty;
        }

        static int RandomizeNumber()
        {
            var rand = new Random();
            int number = rand.Next(12, 120); // рандомим число, используя объект класса Random бибилиотеки System

            return number;
        }

        static void Congratulations(Player player, Robot bot)
        {
            if (player != null)
                Console.WriteLine($"Поздравляем {player.Nickname} с победой!");
            else
                Console.WriteLine($"Поздравляем {bot.Nickname} с победой!");
        }

        static (List<Player>, Robot[]) PlayersInitial(bool typeOfGame, int PlayersQty, int BotsQty)
        {
            bool oneVSOne = typeOfGame;
           //Player[] Players = new Player[PlayersQty];
            Robot[] Bots = new Robot[BotsQty];
            List<Player> Players = new List<Player>();

            if (oneVSOne)
            {
                for (int index = 0; index < PlayersQty; index++)
                {
                    Players.Add (new Player("", false, 0));
                    Players[index].RunInputUser();
                }
            }
            else
            {
                for (int index = 0; index < BotsQty; index++)
                    Bots[index] = new Robot("Bot_" + index.ToString());

                for (int index = 0; index < PlayersQty; index++)
                {
                    Players.Add(new Player("", false, 0));
                    Players[index].RunInputUser();
                }           
            }

            return (Players, Bots);
        }

        static void Main(string[] args)
        {
            WelcomeRules();
            bool winner = false;
            bool oneOnOne = PlayerVsPlayer(); // Определяем тип игры (Игрок VS Игрок / Игрок VS ИИ)
            int PlayersQty = 0, BotsQty = 0;

            // В зависимости от типа игры, спрашиваем у игрока количество игроков || ботов + игроков
            if (oneOnOne)
                PlayersQty = SetPlayersQty();
            else
            {
                PlayersQty = SetPlayersQty();
                BotsQty = SetBotsQty();
            }

            //инициализация игроков и создание экземпляров класса Player с частичной инициализацией конструктора.
            var Initial = PlayersInitial(oneOnOne, PlayersQty, BotsQty); // 5 строчек, включая эту, с моей точки зрения, самые сложные для понимания.
            List<Player> Players = new List<Player>(PlayersQty);
            Robot[] Bots = new Robot[BotsQty];
            Players = Initial.Item1;
            Bots = Initial.Item2;

        Rematch:
            int valueRange = SetValueRange();

            //Генерация рандомного числа
            int gameNumber = RandomizeNumber();
            Console.WriteLine($"\n                                         Программа загадала число: {gameNumber}");

            //Игра началась
            do
            {
                foreach (var player in Players)
                {
                    if (gameNumber < valueRange)
                        valueRange = gameNumber;

                    player.Step(valueRange);
                    gameNumber -= player.Number;

                    if (gameNumber == 0) { winner = true; Congratulations(player: player, bot: null); break; }
                    Console.WriteLine($"                                                  Результат: {gameNumber}");
                }

                if ((!oneOnOne) && (!winner))
                {
                    foreach (var bot in Bots)
                    {
                        if (gameNumber < valueRange)
                            valueRange = gameNumber;

                        bot.GenerateStep(valueRange, gameNumber);
                        gameNumber -= bot.Number;

                        if ((gameNumber == 0) && (!winner))
                        { winner = true; Congratulations(bot: bot, player: null); break; }

                        Console.Write($"\n                                                    Результат: {gameNumber}");

                    }
                }

            } while (gameNumber != 0);
            //Игра закончилась

            Console.Write($"                                              \n Хотите сыграть реванш? \n" +
                "                                              Да - Нажмите клавишу Enter\n" +
                "                                          Нет - нажмите любую другую клавишу");

            ConsoleKeyInfo KeyPressed;
            KeyPressed = Console.ReadKey();
            if (KeyPressed.Key == ConsoleKey.Enter) { Console.Clear(); WelcomeRules(); goto Rematch; }

            Console.ReadKey();
        }
    }
}
