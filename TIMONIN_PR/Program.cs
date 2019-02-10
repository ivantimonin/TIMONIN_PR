using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMONIN_PR
{
    class Data_base
    {
        public static string[] name = { "Селедкин В.В", "Раздолбаев И.А.", "Дурабасов А.А." };
        public static string[] login = { "User1", "User2", "User3" };
        public static string[] password = { "1", "2", "3" };
        public static double[] money = { 1000.0, 2000.0, 3000.0 };
    }
    
    class My_bank
    {        
        public static void Main(string[] args)
        {               
            int accaunt=Authorization();
            Welcome_to_Bank(accaunt);
            string criterion_exit = "N";
            while (criterion_exit =="N")
            {
                Score_of_accaunt(accaunt);
                Add_sum(accaunt);
                Take_of_sum(accaunt);
                Thank();
                Console.WriteLine("Желаете выйти? (Y/N)");
                criterion_exit = Console.ReadLine();
                Console.Clear();
            }
            Console.ReadLine();            
        }              
        
        public static int Authorization()
        {           
            int account = -1;
            while (account == -1)
            {
                Console.Clear();
                Console.WriteLine($"Введите ваш логин");
                string write_login = Console.ReadLine();
                Console.WriteLine($"Введите ваш пароль");
                string write_password = Console.ReadLine();

                for (int i = 0; i < Data_base.login.Length; i++)
                {
                    if (write_login == Data_base.login[i] && write_password == Data_base.password[i])
                    {
                        account = i;
                    }
                    if (i == Data_base.login.Length-1 && account < 0)
                    {
                        Console.WriteLine($"Пароль или логин введен неверно");
                        Console.WriteLine($"Пожалуйста, повторите попытку");
                        Console.WriteLine($"Для продолжения нажимте ENTER...");
                        Console.ReadLine();
                    }
                }
            }
            Console.Clear();
            return account;
        }

        public static void Welcome_to_Bank(int account)
        {            
            Console.WriteLine($"{Data_base.name[account]}, добро пожаловать в личный кабинет");
        }

        public static void Score_of_accaunt(int accaunt)
        {
            Console.Write($"На вашем счету: {Data_base.money[accaunt]} руб");
        }

        public static void Add_sum(int account)
        {
            Console.WriteLine();
            Console.WriteLine($"Желаете пополнить ваш счет? (Y/N)");
            string answer=Console.ReadLine();
            if (answer == "Y")
            {
                Console.Clear();
                Console.WriteLine($"Введите сумму пополнения:");
                double sum = Convert.ToDouble (Console.ReadLine());
                Data_base.money[account] = Data_base.money[account] + sum;
                My_bank.Score_of_accaunt(account);
            }
        }

        public static void Take_of_sum(int account)
        {
            Console.WriteLine();
            Console.WriteLine($"Желаете снять деньги? (Y/N)");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                Console.Clear();
                Console.WriteLine($"Введите сумму снятия:");
                double sum = Convert.ToDouble(Console.ReadLine());
                Data_base.money[account] = Data_base.money[account] - sum;
                if (Data_base.money[account] < 0)
                {
                    Console.WriteLine($"Вы не можете снять больше чем у Вас есть на счету");
                }
                else
                {
                    My_bank.Score_of_accaunt(account);
                }
            }
        }

        public static void Thank()
        {
            Console.WriteLine();
            Console.WriteLine("Спасибо что выбрали банк  «Надежный»");
        }
    }
}
