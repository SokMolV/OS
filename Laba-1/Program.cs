using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

//Зыбайло Елизавета Вадимовна БББО-06-19

namespace first_program
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            char choice_2;
            Console.WriteLine("----------------------------------------------МЕНЮ----------------------------------------------\n");
            do
            {
                
                Console.WriteLine("Информацию в консоль о логических дисках, именах, метке тома, размере типе файловой системы ---1");
                Console.WriteLine("Работа с файлами ------------------------------------------------------------------------------2");
                Console.WriteLine("Работа с форматом XML -------------------------------------------------------------------------3");
                Console.WriteLine("Работа с ZIP сжатием --------------------------------------------------------------------------4");
                Console.WriteLine("Работа с форматом JSON ------------------------------------------------------------------------5");
                Console.WriteLine("Выход из программы ----------------------------------------------------------------------------0");
                Console.Write("Номер команды: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        /*Вывести информацию в консоль о логических дисках, именах, метке тома, размере типе файловой системы.*/
                        {
                            Console.WriteLine("1. ИНФОРМАЦИЯ О ДИСКАХ");
                            DriveInfo[] drivers = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in drivers)
                            {
                                Console.WriteLine($"Название: {drive.Name}");
                                Console.WriteLine($"Тип: {drive.DriveType}");
                                if (drive.IsReady)
                                {
                                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                                }
                                Console.WriteLine();
                            }
                            break;
                        }
                    case 2:
                        /*Работа с файлами ( класс File, FileInfo, FileStream и другие)
                            Создать файл
                            Записать в файл строку
                            Прочитать файл в консоль
                            Удалить файл*/
                        {
                            Console.WriteLine("2.Работа с файлами");
                            string path = @"D:\Documents\GitHub"; 
                            Console.WriteLine("Введите строку для записи в файл:");
                            string text = Console.ReadLine();
                            // запись в файл
                            using (FileStream fstream = new FileStream($"{path}text.txt", FileMode.OpenOrCreate))
                            {
                                // преобразуем строку в байты
                                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                                // запись массива байтов в файл
                                fstream.Write(array, 0, array.Length);
                                Console.WriteLine("Текст записан в файл");
                            }
                            // чтение из файла
                            using (FileStream fstream = File.OpenRead($"{path}text.txt"))
                            {
                                // преобразуем строку в байты
                                byte[] array = new byte[fstream.Length];
                                // считываем данные
                                fstream.Read(array, 0, array.Length);
                                // декодируем байты в строку
                                string textFromFile = System.Text.Encoding.Default.GetString(array);
                                Console.WriteLine($"Текст из файла: {textFromFile}");
                            }
                            Console.WriteLine("Удалить файл? yes/no: ");    
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    if (File.Exists($"{path}text.txt"))
                                    {
                                        File.Delete($"{path}text.txt");
                                        Console.WriteLine("Файл удален!");
                                    }
                                    else Console.WriteLine("Файла не существует!");
                                    break;
                                case "no":
                                    break;
                                default:
                                    Console.WriteLine("Введено неверное значение!");
                                    break;
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 3:
                        /*Работа с форматом XML
                                Создать файл формате XML из редактора
                                Записать в файл новые данные из консоли .
                                Прочитать файл в консоль.
                                Удалить файл.*/
                        {
                            Console.WriteLine("Работа с XML:");
                            XmlDocument xDoc = new XmlDocument();
                            XDocument xdoc = new XDocument();
                            Console.Write("Сколько пользователей нужно ввести: ");
                            int count = Convert.ToInt32(Console.ReadLine());
                            XElement list = new XElement("users");
                            for (int i = 1; i <= count; i++)
                            {
                                XElement User = new XElement("user");
                                Console.Write("Введите имя пользователя: ");
                                XAttribute UserName = new XAttribute("name", Console.ReadLine());
                                Console.WriteLine();
                                Console.Write("Введите возраст пользователя: ");
                                XElement UserAge = new XElement("age", Convert.ToInt32(Console.ReadLine()));
                                Console.WriteLine();
                                Console.Write("Введите название компании: ");
                                XElement UserCompany = new XElement("company", Console.ReadLine());
                                Console.WriteLine();
                                User.Add(UserName);
                                User.Add(UserAge);
                                User.Add(UserCompany);
                                list.Add(User);
                            }
                            xdoc.Add(list);
                            xdoc.Save(@"D:\Documents\GitHub\users.xml");
                            Console.Write("Прочитать XML файл? (yes/no): ");
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    Console.WriteLine();
                                    xDoc.Load(@"D:\Documents\GitHub\users.xml");
                                    XmlElement xRoot = xDoc.DocumentElement;
                                    foreach (XmlNode xnode in xRoot)
                                    {
                                        if (xnode.Attributes.Count > 0)
                                        {
                                            XmlNode attr = xnode.Attributes.GetNamedItem("name");
                                            if (attr != null)
                                                Console.WriteLine($"Имя: {attr.Value}");
                                        }
                                        foreach (XmlNode childnode in xnode.ChildNodes)
                                        {
                                            if (childnode.Name == "age")
                                                Console.WriteLine($"Возраст: {childnode.InnerText}");
                                            if (childnode.Name == "company")
                                                Console.WriteLine($"Компания: {childnode.InnerText}");
                                        }
                                    }
                                    Console.WriteLine();
                                    break;
                                case "no":
                                    break;
                                default:
                                    Console.WriteLine("Введены неправильные данные!");
                                    break;
                            }
                            Console.Write("Удалить созданный xml файл? (yes/no): ");
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    FileInfo xmlfilecheck = new FileInfo(@"D:\Documents\GitHub\users.xml");
                                    if (xmlfilecheck.Exists)
                                    {
                                        xmlfilecheck.Delete();
                                        Console.WriteLine("Файл удален!");
                                    }
                                    else Console.WriteLine("Файла не существует!");
                                    break;
                                case "no":
                                    break;
                                default:
                                    Console.WriteLine("Введено неверное зачение!");
                                    break;
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 4:
                        /*Создание zip архива, добавление туда файла, определение размера архива
                            Создать архив в форматер zip
                            Добавить файл в архив
                            Разархивировать файл и вывести данные о нем
                            Удалить файл и архив*/
                        {
                            Console.WriteLine("Работа с ZIP:");
                            string SourceFile = @"D:\Documents\GitHub\oop.txt"; // исходный файл
                            string CompressedFile = @"D:\Documents\GitHub\bin.gz"; // сжатый файл
                            string TargetFile = @"D:\Documents\GitHub\oop1.txt"; // восстановленный файл
                            // создание сжатого файла
                            Compress(SourceFile, CompressedFile);
                            // чтение из сжатого файла
                            Decompress(CompressedFile, TargetFile);
                            Console.WriteLine("Удалить файлы? (yes/no): ");
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    if ((File.Exists(SourceFile) &&
                                         File.Exists(CompressedFile) && File.Exists(TargetFile)) == true)
                                    {
                                        File.Delete(SourceFile);
                                        File.Delete(CompressedFile);
                                        File.Delete(TargetFile);
                                        Console.WriteLine("Файлы удалены!");
                                    }
                                    else Console.WriteLine("Ошибка в удалении файлов!\n Проверьте их наличие!");
                                    break;
                                case "no":
                                    break;
                                default:
                                    Console.WriteLine("Введены неправильные данные!");
                                    break;
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 5:
                        /*Работа с форматом JSON
                            Создать файл формате JSON из редактора в
                            Создать новый объект. Выполнить сериализацию объекта в формате JSON и записать в файл.
                            Прочитать файл в консоль
                            Удалить файл*/
                        {
                            Console.WriteLine("Работа с JSON:");
                            // сохранение данных
                            using (FileStream fs = new FileStream(@"D:\Documents\GitHub\user.json", FileMode.OpenOrCreate))
                            {
                                Person liza = new Person() { Name = "Liza", Age = 19 };
                                await JsonSerializer.SerializeAsync<Person>(fs, liza);
                                Console.WriteLine("Данные были введены автоматически и они сохранены!");
                            }

                            // чтение данных
                            using (FileStream fs = new FileStream(@"D:\Documents\GitHub\user.json", FileMode.OpenOrCreate))
                            {
                                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
                                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
                            }
                            Console.Write("Удалить файл? (yes/no): ");
                            switch (Console.ReadLine())
                            {
                                case "yes":
                                    File.Delete(@"D:\Documents\GitHub\user.json");
                                    Console.WriteLine("\nФайл удален!");
                                    break;
                                case "no":
                                    break;
                            }
                            break;
                        }
                    default:
                        Console.WriteLine("\nВВЕДЕНЫ НЕПРАВИЛЬНЫЕ ДАННЫЕ!");
                        break;
                }
                Console.WriteLine("================================");
                Console.Write("\nПродолжить? y/n: ");
                choice_2 = Convert.ToChar(Console.ReadLine());
                Console.Write('\n');
            } while (choice_2 != 'n');
        }
        public static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }
        public static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
        }
    }
}