using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace AsyncAwait
{

    public class AsyncAwait
    {
        private static ConcurrentBag<string> listOfResults = new ConcurrentBag<string>();

        public static async Task<string> DoHeavyWork(int count, char c)
        {
            Console.WriteLine($"Hledám {count} krát {c} na vlákně Id:{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(3000);
            //Thread.Sleep(3000);
            Console.WriteLine($"Hledání {c} skončilo");
            listOfResults.Add(Thread.CurrentThread.ManagedThreadId + " " + count + c);
            return Thread.CurrentThread.ManagedThreadId + " " + count + c;
        }

        public static async Task Main()
        {

            Console.WriteLine($"Hlavní vlákno Id:{Thread.CurrentThread.ManagedThreadId}, ThreadPool vlákna:{ThreadPool.ThreadCount}");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine(await Task.Run(() => DoHeavyWork(1500, 'x')));

            Task[] tasks =
            {
                Task.Run(() => DoHeavyWork(15, 'a')),
				Task.Run(() => DoHeavyWork(10, 'b')),
				Task.Run(() => DoHeavyWork(15, 'c')),
				Task.Run(() => DoHeavyWork(10, 'd')),
				Task.Run(() => DoHeavyWork(15, 'e')),
				Task.Run(() => DoHeavyWork(10, 'f'))
            };

            Task.WaitAll(tasks);

            sw.Stop();

            Console.WriteLine($"Hlavní vlákno Id:{Thread.CurrentThread.ManagedThreadId}, ThreadPool vlákna:{ThreadPool.ThreadCount}");
            Console.WriteLine($"Zpracování trvalo {sw.ElapsedMilliseconds} ms.");

        }


    }
}
