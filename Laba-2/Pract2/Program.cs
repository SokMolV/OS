using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pract2
{
    //Напечатайте каждый соответствующий пароль вместе с его хешем SHA-256.
    //Количество потоков от 2 до 7.

    class Program
    {
        /// <summary>
        /// Количество хешей и, следовательно, паролей, которые надо найти
        /// </summary>
        static readonly int nHash = 3;
        /// <summary>
        /// Количество уже найденных паролей
        /// </summary>
        static int nFoundPasswords;
        /// <summary>
        /// Словарь, где ключ - хеш, значение - пароль
        /// </summary>
        static Dictionary<string, string> dictPasswords;
        /// <summary>
        /// Алфавит, который содержит все символы, которые могут быть в пароле
        /// В данном случае, английский алфавит в строчном регистре
        /// </summary>
        static char[] alphabet;
        /// <summary>
        /// Время запуска потоков
        /// </summary>
        static DateTime dateTimeBegin;

        /// <summary>
        /// Генерирвание хеша
        /// </summary>
        /// <param name="str">Пароль</param>
        /// <returns>Хеш</returns>
        static string GetHash(string str)
        {
            StringBuilder hash = new StringBuilder();
            using (SHA256 sha256 = SHA256Managed.Create())
            {
                // Используется ASCII кодировка, как и сказано в задании 
                Encoding enc = Encoding.ASCII;
                // Хеширование треубет массив байт на вход, поэтому преобразовываем строку с паролем в массив байт
                byte[] baResult = sha256.ComputeHash(enc.GetBytes(str));
                // Хеширование возвращает массив байт, поэтому преобрзовываем хеш в строку
                foreach (byte b in baResult)
                    // b.ToString("x2") - один байт конвертируется в число 16-ой системы счисления (как и требуется для хеша)
                    hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Вызов метода для построения хеша на основании пароля. 
        /// Сравнение полученного хеша с заданными.
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Признак равенства хеша password и одного из заданных хешей</returns>
        static bool CompareHash(string password)
        {
            string hash = GetHash(password);
            if (dictPasswords.ContainsKey(hash))
            {
                dictPasswords[hash] = password.ToString();
                nFoundPasswords++;
                Console.WriteLine("Пароль: " + dictPasswords[hash] + ". Хэш: " + hash + ".");
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Выполняется полный перебор (брутфорс). 
        /// Вызывается метода сравнения хеша полученной последовательности с эталонными хешами.
        /// </summary>
        /// <param name="beginEnd">
        /// Объект, который должен являться массивом символов.
        /// Первый символ определяет, с какой буквы начинается первый пароль
        /// Второй символ определяет, на какую букву заканчивается последний пароль
        /// Третий символ определяет номер потока, в котором раотает метод
        /// </param>
        static void NextSequence(object beginEnd)
        {
            var temp = (char[])beginEnd;
            string sequence;
            for (char i1 = temp[0]; i1 <= temp[1]; i1++)
                foreach (var i2 in alphabet)
                    foreach (var i3 in alphabet)
                        foreach (var i4 in alphabet)
                            foreach (var i5 in alphabet)
                            {
                                sequence = string.Concat(i1, i2, i3, i4, i5);
                                if (CompareHash(sequence))
                                    Console.WriteLine("С запуска потоков и до нахождения пароля прошло " + Math.Round((DateTime.Now - dateTimeBegin).TotalSeconds) + " сек.");
                                if (nFoundPasswords == nHash)
                                {
                                    Console.WriteLine("Поток " + temp[2] + " завершил работу.");
                                    return;
                                }
                            }
            Console.WriteLine("Поток " + temp[2] + " завершил работу.");
        }

        static void Main(string[] args)
        {
            nFoundPasswords = 0;
            // Заполняем массив символов, которые могут быть в пароле
            alphabet = new char[26];
            for (char i = 'a'; i <= 'z'; i++)
                alphabet[i - 'a'] = i;

            // Задаем эталонные хеши
            dictPasswords = new Dictionary<string, string>();
            dictPasswords.Add("1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad", ""); // zyzzx
            dictPasswords.Add("3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b", ""); // apple
            dictPasswords.Add("74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f", ""); // mmmmm

            dateTimeBegin = DateTime.Now;
            // Объявление потоков
            Thread threadBruteForcePart1 = new Thread(new ParameterizedThreadStart(NextSequence));
            Thread threadBruteForcePart2 = new Thread(new ParameterizedThreadStart(NextSequence));
            Thread threadBruteForcePart3 = new Thread(new ParameterizedThreadStart(NextSequence));
            Thread threadBruteForcePart4 = new Thread(new ParameterizedThreadStart(NextSequence));
            Thread threadBruteForcePart5 = new Thread(new ParameterizedThreadStart(NextSequence));

            // Запуск потоков
            threadBruteForcePart1.Start(new char[] { 'a', 'e', '1' });
            threadBruteForcePart2.Start(new char[] { 'f', 'j', '2'});
            threadBruteForcePart3.Start(new char[] { 'k', 'o', '3' });
            threadBruteForcePart4.Start(new char[] { 'p', 'u', '4' });
            threadBruteForcePart5.Start(new char[] { 'v', 'z', '5' });

            Console.WriteLine("Запущены 5 потоков. Пожалуйста, ожидайте окончания их выполнения...");
            Console.ReadKey();
        }
    }
}