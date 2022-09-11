using Task3;

ModifiedRemovalAfter30Minutes.Start();

namespace Task3
{
    class ModifiedRemovalAfter30Minutes
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
                // сначала получаем списки кандидатов для удаления,
                // иначе - при запросе размера папки, время существования обновится
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                string[] dirsToDelite = GetListingToDelete(dirs);
                string[] filesToDelite = GetListingToDelete(files);

                var sizeBefore = GetFolderSize(path);
                Console.WriteLine("Исходный размер папки: \t{0} байт", sizeBefore);

                if (filesToDelite.Length != 0 || dirsToDelite.Length != 0)
                {
                    string codeWord = "Да, я хочу удалить всё, что старше 30 минут";
                    Console.WriteLine("\nДля удаления введите: \"{0}\"", codeWord);

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

                var sizeAfter = GetFolderSize(path);
                Console.WriteLine("Освобождено: \t{0} байт", sizeBefore - sizeAfter);
                Console.WriteLine("Текущий размер папки: \t{0} байт", sizeAfter);
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
            // определение папок и файлов, которые старше 30 минут
        {
            List<string> list = new();

            foreach (string path in pathes)
            {
                TimeSpan timeExsits = DateTime.Now - Directory.GetLastAccessTime(path);

                if (timeExsits > TimeSpan.FromMinutes(30))
                {
                    list.Add(path);
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

        static long GetFolderSize(string path)
            // подсчёт размера папки (по сумме размеров вложенных файлов)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                long size = 0;

                if (files.Length != 0)
                {
                    size += GetAllFilesSize(files);
                }

                if (dirs.Length != 0)
                {
                    foreach (string dir in dirs)
                    {
                        size += GetFolderSize(dir);
                    }
                }

                return size;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                long ex = 0;
                return ex;
            }
        }

        static long GetAllFilesSize(string[] files)
            // получение суммы размеров вложенных файлов
        {
            try
            {
                long sizeAllFiles = 0;
                foreach (string file in files)
                {
                    FileInfo f = new(file);
                    sizeAllFiles += f.Length;
                }
                return sizeAllFiles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                long ex = 0;
                return ex;
            }
        }
    }
}
