using System;

namespace PasswordGenerator
{
    class Program
    {
        private const string PasswordChars = "123567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string SpecialChars = "~!@#$%^&*_+-=:;<,>.?(){}[]|";
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Password Generator");
            bool cont = true;
            string input = string.Empty;
            while (cont)
            {
                int howLong;
                while (!int.TryParse(input, out howLong))
                {
                    Console.Write("How long?: ");
                    input = Console.ReadLine();
                }

                Console.Write($"Special Chars ({SpecialChars}): ");
                string include = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(include))
                {
                    include = SpecialChars;
                }

                char[] possible = GetPossibleChars(PasswordChars, include);

                string password = GeneratePassword(howLong, possible);
                Console.WriteLine("Password: {0}", password);

                Clipboard.SetText(password);

                input = string.Empty;
                while (!(input == "y" || input == "n"))
                {
                    Console.Write("Generate another (y/n)?: ");
                    input = Console.ReadLine();
                }

                cont = (input == "y");

                input = string.Empty;
            }
        }

        private static char[] GetPossibleChars(string passwordChars, string include)
        {
            char[] possible = new char[passwordChars.Length + include.Length];
            passwordChars.CopyTo(0, possible, 0, passwordChars.Length);
            include.CopyTo(0, possible, passwordChars.Length, include.Length);
            return possible;
        }
        
        private static string GeneratePassword(int howLong, char[] possible)
        {
            char[] pass = new char[howLong];
            Random random = new Random();
            for (int i = 0; i < howLong; i++)
            {
                var index = random.Next(0, possible.Length - 1);
                pass[i] = possible[index];
            }
            return new string(pass);
        }


    }
}
