using Task2;

CalculatingFolderSize.Start();

namespace Task2
{
    class CalculatingFolderSize
    {
        public static void Start()
        {
            Console.WriteLine("Введите путь до папки, размер которой необходимо посчитать:");
            string path = Console.ReadLine();

            while (!Valid(path))
            {
                Console.WriteLine("\nВведите путь:");
                path = Console.ReadLine();
            }
            Console.WriteLine("Отлично, такой путь есть!");

            var size = GetFolderSize(path);
            Console.WriteLine("Размер папки: {0} байт", size);

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
