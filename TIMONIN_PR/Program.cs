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

    class Clients
    {
        private string name;
        
        public Clients(int accaunt)
        {
            name = Data_base.name[accaunt];            

            string criterion_exit = "N";
            while (criterion_exit == "N")
            {
                Welcome_to_Bank(name);
                Score_of_accaunt(accaunt);
                Add_sum(accaunt);
                Take_of_sum(accaunt);
                
                Console.WriteLine("Желаете выйти? (Y/N)");
                criterion_exit = Console.ReadLine();
                Console.Clear();
            }
           
        }

        public void Welcome_to_Bank(string name)
        {
            Console.Clear();
            Console.WriteLine($"{name}, добро пожаловать в личный кабинет");
        }

        public static void Score_of_accaunt(int accaunt)
        {
            Console.WriteLine($"На вашем счету: {Data_base.money[accaunt]} руб");
        }

        public void Add_sum(int accaunt)
        {
            Console.WriteLine("Желаете пополнить ваш счет? (Y/N)");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                Console.Clear();
                Console.WriteLine("Введите сумму пополнения:");
                double sum = Convert.ToDouble(Console.ReadLine());
                Data_base.money[accaunt] += sum;
                Score_of_accaunt(accaunt);
            }
        }

        public static void Take_of_sum(int accaunt)
        {
            Console.WriteLine();
            Console.WriteLine("Желаете снять деньги? (Y/N)");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                Console.Clear();
                Console.WriteLine("Введите сумму снятия:");
                double sum = Convert.ToDouble(Console.ReadLine());
                
                if (Data_base.money[accaunt]-sum < 0)
                {
                    Console.WriteLine("Вы не можете снять больше чем у Вас есть на счету");
                }
                else
                {
                    Data_base.money[accaunt]  -= sum;
                    Score_of_accaunt(accaunt);
                }
            }
        }
    }
    
    
    class My_bank
    {        
        public static void Main(string[] args)
        {                      

            int accaunt=Authorization();
            Clients user_of_bank = new Clients(accaunt);
            Thank();
            Console.ReadLine();            
        }              
        
        public static int Authorization()
        {            
            while (true)
            {
                Console.WriteLine("Введите ваш логин");
                string write_login = Console.ReadLine();
                Console.WriteLine("Введите ваш пароль");
                string write_password = Console.ReadLine();
                for (int account = 0; account < Data_base.login.Length; account++)
                {
                    if (write_login == Data_base.login[account] && write_password == Data_base.password[account])
                    {
                        return account;
                    }                    
                }
                Console.WriteLine($"Пароль или логин введен неверно");
                Console.WriteLine($"Пожалуйста, повторите попытку");
                Console.WriteLine($"Для продолжения нажимте ENTER...");
                Console.ReadLine();
                Console.Clear();
            }      
        }      

        public static void Thank()
        {
            Console.WriteLine();
            Console.WriteLine("Спасибо, что выбрали банк  «Надежный»");
        }
    }
}
