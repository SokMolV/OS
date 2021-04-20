using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pract3
{
    class Program
    {
        /// <summary>
        /// Размер очереди
        /// </summary>
        static readonly int nQueue = 200;
        /// <summary>
        /// Переменная для генерации случайных чисел
        /// </summary>
        static Random random;
        /// <summary>
        /// Очередь
        /// </summary>
        static Queue<int> queue;
        /// <summary>
        /// Массив потоков производителей
        /// </summary>
        static Thread[] generators;
        /// <summary>
        /// Массив потоков потребителей
        /// </summary>
        static Thread[] consumers;
        /// <summary>
        /// Мюьтекс для эксклюзивного владения очередью, когда в потоке идет изменение данных
        /// </summary>
        static Mutex mutex;
        /// <summary>
        /// Признак, что пора останавливать потоки производителей и запускать потоки потребителей
        /// </summary>
        static bool isOver;

        /// <summary>
        /// Метод для потоков производителей
        /// </summary>
        static void Generator()
        {
            int value;
            while (!isOver)
            {
                // Синхронизация потоков. Выполняется для корректной работы с очередью.
                // Если не использовать мьютекс, есть шанс вылета программы, из-за одновременного добавления и удаления объектов в очередь.
                // То есть без мьютекса очередь не всегда успевает реагировать на изменения сразу из нескольких потоков.
                mutex.WaitOne();
                if (queue.Count < nQueue)
                {
                    value = random.Next(1, 101);
                    queue.Enqueue(value);
                    Console.WriteLine(Thread.CurrentThread.Name + ". В конец очереди добавленно число " + value + ". Длина очереди: " + queue.Count + ".");
                }
                // Возвращение параллельной работы потоков.
                mutex.ReleaseMutex();
            }
            Console.WriteLine(Thread.CurrentThread.Name + " остановлен.");
        }

        static void Dequeue()
        {
            // Синхронизация потоков. Выполняется для корректной работы с очередью.
            // Здесь шанс вылета без использования мьютекса еще выше.
            // На момент queue.Count поток знает, допустим, что в очереди остался один элемент,
            // а на момент выполнения queue.Dequeue() другой поток уже успел удалить тот самый единственный элемент,
            // тогда происходит попытка удалить элемент из пустой очереди, что приводит к ошибке.
            mutex.WaitOne();
            if (queue.Count > 0)
            {
                int value = queue.Dequeue();
                Console.WriteLine(Thread.CurrentThread.Name + ". Из начала очереди удаленно число " + value + ". Длина очереди: " + queue.Count + ".");
            }
            // Возвращение параллельной работы потоков.
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Метод для потоков потребителей
        /// </summary>
        static void Consumer()
        {
            while (true)
            {
                // Если программа работает в обычном режиме
                if (!isOver)
                {
                    if (queue.Count >= 100 || queue.Count == 0)
                    {
                        // Приостановка потока на 1000мс
                        Thread.Sleep(1000);
                    }
                    else if (queue.Count <= 80)
                    {
                        Dequeue();
                    }
                }
                // Если пользователь ввел q и потребители должны работать до последнего элемента в очереди
                else if (queue.Count == 0)
                {
                    break;
                }
                else
                {
                    Dequeue();
                }
            }
            Console.WriteLine(Thread.CurrentThread.Name + " остановлен.");
        }

        static void Main(string[] args)
        {
            // Количество потоков производителей и потребителей
            int nGenerator = 3, nConsumer = 2;
            isOver = false;
            queue = new Queue<int>();
            mutex = new Mutex();
            random = new Random();
            generators = new Thread[nGenerator];
            consumers = new Thread[nConsumer];

            Console.WriteLine("Нажмите q, чтобы остановить потоки производителей.");

            for (int i = 0; i < nGenerator; i++)
            {
                generators[i] = new Thread(new ThreadStart(Generator));
                generators[i].Name = "Поток производителя №" + (i + 1);
                generators[i].Start();
            }
            for (int i = 0; i < nConsumer; i++)
            {
                consumers[i] = new Thread(new ThreadStart(Consumer));
                consumers[i].Name = "Поток потребителя №" + (i + 1);
                consumers[i].Start();
            }

            char key = '0';
            while (key != 'q')
            {
                key = (char)Console.Read();
            }
            isOver = true;

            Console.ReadKey();
        }
    }
}
