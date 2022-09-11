using System.Runtime.Serialization.Formatters.Binary;
using FinalTask;


DownloaderBinaryToText.Start();


namespace FinalTask
{
    class DownloaderBinaryToText
    {
        public static void Start()
        {
            Console.WriteLine("Введите полный путь до бинарного файла с базой студентов");

            string path = Console.ReadLine();
            while (!Valid(path))
            {
                Console.WriteLine("\nВведите путь:");
                path = Console.ReadLine();
            }
            Console.WriteLine("Отлично, такой файл есть!");

            Student[] students = ExtractingStudentsFromFile.Extract(path);

            Console.WriteLine("Бинарный файл с массивом студентов прочитан...");

            CreaterFilesWithGroups.Creat(students);

            Console.WriteLine("На рабочем столе, в папке Students, сохранены необходимые файлы.");

            Console.ReadKey();           
        }

        static bool Valid(string path)
            // проверка наличия файла по указанному пути
        {
            if (File.Exists(path))
            return true;

            Console.WriteLine("Такого файла не существует!" +
                "\nПроверьте правильность пути.");
            return false;
        }
    }
}

namespace FinalTask
{
    class ExtractingStudentsFromFile
    {
        public static Student[] Extract(string path)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    Student[] students = (Student[])bf.Deserialize(fs);
                    return students;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }

    class CreaterFilesWithGroups
    {
        public static void Creat (Student[] students)
        {
            string pathFolderToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Students";
            string pathToFile = default(string);

            //сортируем извлечённый массив студентов по группам
            var sorted = students.OrderBy(ob => ob.Group).ToArray();

            string groupName = default(string);

            Directory.CreateDirectory(pathFolderToDesktop);

            for (int i = 0; i < sorted.Length;)
            {
                if (groupName == sorted[i].Group)
                {
                    using (StreamWriter sw = new StreamWriter(pathToFile, false))
                    {
                        for (; i < sorted.Length && groupName == sorted[i].Group; i++)
                        {
                            sw.WriteLine("{0}, {1}", sorted[i].Name, sorted[i].DateOfBirth);
                        }
                    }
                }
                else
                {
                    groupName = sorted[i].Group;
                    pathToFile = pathFolderToDesktop + @"\" + groupName + ".txt";
                }
            }
                
        }
    }


    // в моей голове решения для открытия файла не нашлось, нашлось в slack
    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Student (string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
}