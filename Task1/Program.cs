using Task1;

RemovalAfter30Minutes.Start();

namespace Task1
{
    class RemovalAfter30Minutes
    {
        public static void Start()
        {
            Console.WriteLine("Введите путь до папки, из которой удалится всё, что старше 30 минут:");
            string path = Console.ReadLine();

            while (!Valid(path))
            {
                Console.WriteLine("\nВведите путь:");
                path = Console.ReadLine();
            }
            Console.WriteLine("Отлично, такой путь есть!");

            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                Console.WriteLine("\nПапки, подлежащие удалению:");
                string[] dirsToDelite = GetListingToDelete(dirs);
                Console.WriteLine("\nПапок для удаления {0} шт.", dirsToDelite.Length);

                Console.WriteLine("\nФайлы, подлежащие удалению:");
                string[] filesToDelite = GetListingToDelete(files);
                Console.WriteLine("\nФайлов для удаления {0} шт.", filesToDelite.Length);

                // если есть файлы или папки для удаления, запрашиваем подтверждение операции через ключевую фразу
                if (filesToDelite.Length != 0 || dirsToDelite.Length != 0)
                {
                    string codeWord = "Да, я хочу удалить это";
                    Console.WriteLine("\nДля удаления вышеперечисленных папок и файлов введите: \"{0}\"", codeWord);

                    string enteredWord = Console.ReadLine();

                    if (enteredWord == codeWord)
                    {
                        Console.WriteLine("Удаление начато...");
                        DeliteDirs(dirsToDelite);
                        DeliteFiles(filesToDelite);
                        Console.WriteLine("Удаление завершено.");
                    }
                    else
                        Console.WriteLine("Удаление отменено.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        static bool Valid(string path)
        // проверка пути
        {
            if (Directory.Exists(path))
                return true;

            Console.WriteLine("Такой директории не существует!" +
                "\nПроверьте правильность пути.");
            return false;
        }

        static string[] GetListingToDelete(string[] pathes)
        // определение папок и файлов, которые старше 30 минут - с выводом списка на консоль
        {
            List<string> list = new();

            foreach (string path in pathes)
            {
                TimeSpan timeExsits = DateTime.Now - Directory.GetLastAccessTime(path);

                if (timeExsits > TimeSpan.FromMinutes(30))
                {
                    list.Add(path);
                    Console.WriteLine("\n {0} \n Время существования: {1}", path, timeExsits);
                }
            }
            string[] temp = list.ToArray();
            return temp;
        }

        static void DeliteDirs(string[] dirsToDelite)
        // удаление папок
        {
            if (dirsToDelite.Length != 0)
            {
                foreach (string directory in dirsToDelite)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);
                    dirInfo.Delete(true);
                }
            }
        }

        static void DeliteFiles(string[] filesToDelite)
        // удаление файлов
        {
            if (filesToDelite.Length != 0)
            {
                foreach (string file in filesToDelite)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
