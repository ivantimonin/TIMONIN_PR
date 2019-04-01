using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TIMONIN_PR
{
    interface IClient_action
    {
        void Put_money(double money);
        void Withdraw_money(double money);
        void Check_score();
    }    

    [Serializable]
    abstract class People
    {
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public People(string name, string login, string password)
        {
            this.name = name;
            this.login = login;
            this.password = password;
        }
    }

    delegate void Info(string message);// Объявляем делегат
       
    [Serializable]
    class Clients:People, IClient_action
    {           
        public double money { get; private set;}
        
        Info some_message;// Создаем переменную делегата
        Info some_color_message;// Создаем переменную делегата

        public Clients(string name, double money, string login, string password) :base(name, login, password)
        {            
            this.money = money;                        
        }       

        // Регистрируем делегаты
        public void Get_message(Info some_message, Info some_color_message)
        {
            this.some_message = some_message;
            this.some_color_message = some_color_message;
        }

        public void Check_score()
        {
            this.some_message($"На вашем счету {this.money} руб");
        }

        public void Put_money(double money)
        {            
            this.money += money;
            if (this.some_message != null)
            {
                this.some_message($"{name}, Вы положили {money} руб. " +
                                  $"На вашем счету {this.money} руб. ");
            }            
        }

        public void Withdraw_money(double money)
        {
            if (this.money >= money)
            {
                this.money -= money;
                if (this.some_message != null)
                {
                    this.some_message($"{name}, Вы сняли {money} руб." +
                                            $" На вашем счету {this.money} руб. ");
                }
            }
            else
            {
                if (this.some_message != null)
                {
                    this.some_color_message($"{name}, Вы пытаетесь снять {money} руб." +
                                      $" На вашем счету {this.money} руб." +
                                      $" Операция не может быть выполнена.");
                }                
            }            
        }
    }

    class My_bank
    {        
        public static void Main(string[] args)
        {               
            Reade_data_obj(out Clients[] all_clients);

            if (all_clients.Length == 0)// TO DO возможно, имеет смысл делать класс админ который будет это делать
            {
                Console.WriteLine("Создание аккаунтов....");
                Console.WriteLine("Аккаунты созданы");
                Console.WriteLine("Нажмите любую кнопку");
                Console.ReadLine();
                Console.Clear();
                all_clients=Create_clients();// тут описаны все аккаунты 
                Save_data_obj(all_clients);
                Reade_data_obj(out all_clients);                
            }

            int id = Authorization(all_clients);
            Client_Operation(all_clients[id]);            
            Save_data_obj(all_clients);            
            Console.ReadLine();
         
        }

        private static void Show_Message(string message)
        {
            Console.WriteLine(message);
        }

        private static void Show_Color_Message(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Save_data_obj(object data_obj)
        {
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data_obj);                
            }
        }

        public static  void Reade_data_obj(out Clients[] all_clients)
        {
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            
            using (FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate))
            {
                try
                {                    
                    Clients[] deserilizePeople = (Clients[])formatter.Deserialize(fs);
                    all_clients= new Clients[deserilizePeople.Length];
                    int i = 0;
                    foreach (Clients p in deserilizePeople)
                    {
                        all_clients[i] = p;
                        i++;
                    }                    
                }
                catch
                {
                    Console.WriteLine("Ошибка чтения базы данных");
                    Console.WriteLine("Не создано ни одного аккаунта");
                    all_clients = new Clients[0];                
                }
            }           
        }

        public static void Client_Operation(Clients user)
        {
            user.Get_message(Show_Message, Show_Color_Message);// какой метод будет выполнятся в классе
            Console.WriteLine($"{user.name}, добро пожаловать в личный кабинет");
            while (true)
            {
                Console.WriteLine("Сделайте Ваш выбор");
                Console.WriteLine("1.Положить деньги на счет.");
                Console.WriteLine("2.Снять деньги.");
                Console.WriteLine("3.Проверить счет.");
                int choise = Convert.ToInt32(Console.ReadLine());

                if (choise == 1)
                {
                    Console.WriteLine("Какую сумму вы хотите положить");
                    double money = Convert.ToDouble(Console.ReadLine());
                    user.Put_money(money);
                }

                if (choise == 2)
                {
                    Console.WriteLine("Какую сумму вы хотите снять");
                    double money = Convert.ToDouble(Console.ReadLine());
                    user.Withdraw_money(money);
                }

                if (choise == 3)
                {                    
                    user.Check_score();
                }
                Console.WriteLine("Завершить обслуживание (Y) для продолжения нажать любую кнопку");
                string answer = Console.ReadLine();
                if (answer == "Y")
                {
                    break;
                }
            }
        }

        public static Clients[] Create_clients()
        {
            Clients user_1 = new Clients("Иванов", 1000, "Иванов_log", "123");
            Clients user_2 = new Clients("Петров", 1000, "Петров_log", "123");
            Clients user_3 = new Clients("Сидоров", 1000, "Сидоров_log", "123");
            Clients[] all_clients = new Clients [] { user_1, user_2, user_3 };
            return all_clients;
        }

        public static int Authorization(Clients[] all_clients)
        {
            while (true)
            {
                Console.WriteLine("Введите ваш логин");
                string write_login = Console.ReadLine();
                Console.WriteLine("Введите ваш пароль");
                string write_password = Console.ReadLine();
                for (int account = 0; account < all_clients.Length; account++)
                {
                    if (write_login == all_clients[account].login && write_password == all_clients[account].password)
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
    }
}
