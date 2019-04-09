using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace TIMONIN_PR
{
    interface IClient_action
    {
        void Put_money(double money);
        void Withdraw_money(double money);
        void Check_score();
    }

    interface IAdmin_action
    {
        void Delete_client(int id);
        void Add_client(string client_name, double client_money, string client_log, string client_pasword);
        void Show_all_client();
    }

    delegate void Info(string message);// Объявляем делегат

    [Serializable]
    abstract class People
    {
        public Info some_message;// Создаем переменную делегата
        public Info some_color_message;// Создаем переменную делегата
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public People(string name, string login, string password)
        {
            this.name = name;
            this.login = login;
            this.password = password;

        }
        // Регистрируем делегаты
        public void Get_message(Info some_message, Info some_color_message)
        {
            this.some_message = some_message;
            this.some_color_message = some_color_message;
        }
    }


    [Serializable]
    class Clients : People, IClient_action
    {
        public double money { get; set; }

        public Clients(string name, double money,
                       string login, string password) : base(name, login, password)
        {
            this.money = money;
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

    [Serializable]
    class Admin : People, IAdmin_action
    {
        public List<Clients> all_clients { get; set; }        

        public Admin(string name, List<Clients> all_clients,
                       string login, string password) : base(name, login, password)
        {
            this.all_clients = all_clients;
        }

        public void Add_client(string client_name, double client_money, string client_log, string client_pasword)
        {
            
            some_color_message ($"{name}, вы создаете нового пользоваетеля");
            some_message($"Имя:{client_name}\n" +
                         $"Количество денег на счету: {client_money} \n" +
                         $"Логин клиента:{client_log}\n" +
                         $"Пароль клиента:{client_pasword}\n");
            Clients user = new Clients(client_name, client_money, client_log, client_pasword);
            all_clients.Add(user);            
        }

        public void Delete_client(int id)
        {
            if (id <all_clients.Count)
            {
                some_color_message($"Имя:{all_clients[id].name}\n" +
                                   $"Логин клиента:{all_clients[id].login}\n" +
                                   $"УДАЛЕН!");
                all_clients.RemoveAt(id);
                
            }
            else
            {
                some_color_message($"Такого ID еще не создано");
            }            
        }

        public void Show_all_client()
        {
            if (all_clients.Count == 0)
            {
                some_color_message($"Пока клиентов нет!");
            }
            else
            {
                foreach (Clients client in all_clients)
                {
                    some_message($"Имя:{client.name}\n" +
                                 $"Логин клиента:{client.login}\n" +
                                 $"Пароль клиента:{client.password}\n");
                    some_color_message("-----------------------------");
                }
            }
                  
        }
    }

    static class Save_read_data
    {       
        public static void Save_data_obj<T>(List<T> object_array, string file_name)
        {            
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, object_array);
            }
        }

        public static void Reade_data_obj<T>(out List<T> object_array, string file_name)
        {  
            // создаем объект BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                    object_array = (List<T>)formatter.Deserialize(fs);
                }
                else
                {
                    object_array= new List<T>();
                }
            }
        }
    }

    class Message
    {
        public static void Show_Message(string message)
        {
            Console.WriteLine(message);
        }

        public static void Show_Color_Message(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    class Operation
    {
        public static void Client_Operation(Clients user)
        {
            user.Get_message(Message.Show_Message,Message.Show_Color_Message);// какой метод будет выполнятся в классе
            Message.Show_Color_Message($"{user.name}, добро пожаловать в личный кабинет");
            while (true)
            {
                Message.Show_Message("Сделайте Ваш выбор");
                Message.Show_Message("1.Положить деньги на счет.");
                Message.Show_Message("2.Снять деньги.");
                Message.Show_Message("3.Проверить счет.");
                int choise = Convert.ToInt32(Console.ReadLine());

                if (choise == 1)
                {
                    Message.Show_Message("Какую сумму вы хотите положить");
                    double money = Convert.ToDouble(Console.ReadLine());
                    user.Put_money(money);
                }

                if (choise == 2)
                {
                    Message.Show_Message("Какую сумму вы хотите снять");
                    double money = Convert.ToDouble(Console.ReadLine());
                    user.Withdraw_money(money);
                }

                if (choise == 3)
                {
                    user.Check_score();
                }
                Message.Show_Message("Завершить обслуживание (Y) для продолжения нажать любую кнопку");
                string answer = Console.ReadLine();
                if (answer == "Y")
                {
                    break;
                }
            }
        }

        public static void Admin_operation(Admin user)
        {
            user.Get_message(Message.Show_Message,Message.Show_Color_Message);// какой метод будет выполнятся в классе
            Message.Show_Message($"{user.name}, добро пожаловать в личный кабинет ");
            while (true)
            {
                Message.Show_Color_Message("Сделайте Ваш выбор");
                Message.Show_Message("1.Добавить клиента.");
                Message.Show_Message("2.Удалить клиента.");
                Message.Show_Message("3.Вывести список всех клиентов.");
                int choise = Convert.ToInt32(Console.ReadLine());

                if (choise == 1)
                {
                    Message.Show_Message("Как зовут клиента");
                    string client_name = Console.ReadLine();

                    Message.Show_Message("Начальная сумма на счете");
                    double client_money = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Логин клиента");
                    string client_log = Console.ReadLine();

                    Message.Show_Message("Пароль клиента");
                    string client_pasword = Console.ReadLine();

                    user.Add_client(client_name, client_money, client_log, client_pasword);
                }

                if (choise == 2)
                {
                    Message.Show_Message("Введите ID клиента для удаления");
                    int ID= Convert.ToInt32(Console.ReadLine());
                    user.Delete_client(ID);
                }

                if (choise == 3)
                {
                    user.Show_all_client();

                }
                Message.Show_Message("Завершить обслуживание (Y) для продолжения нажать любую кнопку");
                string answer = Console.ReadLine();
                if (answer == "Y")
                {
                    break;
                }
            }
        }
    }    

    class Autorization
    {        
        public static void Authorization( List<Clients>all_clients, List<Admin> all_admin) 
        {
            bool admin = false;
            bool client = false;
            
            while (admin==false && client==false)
            {
                Message.Show_Message("Введите ваш логин");
                string write_login = Console.ReadLine();
                Message.Show_Message("Введите ваш пароль");
                string write_password = Console.ReadLine();
                
                for (int account_cl = 0; account_cl < all_clients.Count; account_cl++)
                {
                    if (write_login == all_clients[account_cl].login && write_password == all_clients[account_cl].password)
                    {                       
                        Operation.Client_Operation(all_clients[account_cl]);
                        client = true;
                        admin = false;
                        break;
                    }                    
                }
                for (int account_ad = 0; account_ad < all_admin.Count; account_ad++)
                {
                    if (write_login == all_admin[account_ad].login && write_password == all_admin[account_ad].password)
                    {
                        Admin main_user = all_admin[account_ad];//например
                        all_admin.Add(main_user);
                        Operation.Admin_operation(all_admin[account_ad]);
                        admin = true;
                        client = false;
                        break;
                    }
                }
                if (admin == false && client == false)
                {
                    Message.Show_Color_Message($"Пароль или логин введен неверно");
                    Message.Show_Color_Message($"Пожалуйста, повторите попытку");
                    Message.Show_Color_Message($"Для продолжения нажимте ENTER...");
                    Console.ReadLine();
                    Console.Clear();
                }
                
            }
        }
    }

    class My_bank
    {        
        public static void Main(string[] args)
        {
            List<Clients> all_clients = new List<Clients>();
            Save_read_data.Reade_data_obj<Clients>(out all_clients,"Clients.dat");                      

            List<Admin> all_admin = new List<Admin>();           
            
            if (all_admin.Count==0)// Админ должен быть!
            {                
                Admin main_user = new Admin("Админ", all_clients, "0", "0");//например
                all_admin.Add(main_user);               
            }         
            Autorization.Authorization(all_clients, all_admin);            
            
            Save_read_data.Save_data_obj<Clients>(all_clients, "Clients.dat");
            Console.ReadLine();         
        }      
    }
}
