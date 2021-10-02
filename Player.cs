using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HomeworkSkillBoxIfElseGame
{
    class Player
    {
        public string Nickname { get; set; }
        public bool Winner { get; set; }
        public int Number { get; set; }

        public Player(string Nickname, bool Winner, int Number)
        {
            this.Nickname = Nickname;
            this.Winner = Winner;
            this.Number = Number;
        }
        public string RunInputUser()
        {
            Console.WriteLine("\n                                           Введите имя игрока: ");
            do
            {
                Nickname = Convert.ToString(Console.ReadLine());
            } while (!Regex.IsMatch(Nickname, "^[a-zA-Z0-9]+$"));

                return this.Nickname;
        }

        public void Step(int valueRange) // valueRange - определяет диапазон от 1 до valueRange. Диапазон в котором пользователю разрешено сделать ход.
        {
            int userTry = 0;
            bool stepComplete = false;

            do
            {
                Console.Write($"\nХод {this.Nickname} ");
                string userInput;
                do
                {
                    userInput = Console.ReadLine();
                } while (!Regex.IsMatch(userInput, @"^[0-9]+$"));
                userTry = Int32.Parse(userInput);
                

                if ((userTry <= valueRange) && (userTry > 0))
                {
                    this.Number = userTry;
                    stepComplete = true;
                    Console.WriteLine($"{this.Nickname}: {this.Number}");
                }
                else
                    Console.Write($"Введено значение недопустимого диапазона. Повтор хода.");
            } while (!stepComplete);
        }



    }

    struct Robot
    {
        public string Nickname;
        public bool Winner { get; set; }
        public int Number { get; set; }

        public Robot(string Nickname, bool Winner, int Number)
        {
            this.Nickname = Nickname;
            this.Winner = Winner;
            this.Number = Number;
        }
        public void GenerateStep(int valueRange, int currentGameNumber) // valueRange - определяет диапазон от 1 до valueRange. Диапазон в котором пользователю разрешено сделать ход.
        {
            int userTry = 0;
            bool stepComplete = false;
            var rand = new Random();

            do
            {
                //Console.Write($"\nХод {this.Nickname} ");

                if (currentGameNumber <= valueRange)
                    userTry = currentGameNumber;
                else
                    userTry = rand.Next(1, valueRange);

                    this.Number = userTry;
                    stepComplete = true;
                    Console.WriteLine($"\nХод {this.Nickname}: {this.Number}");
  
            } while (!stepComplete);
        }

    }

}
