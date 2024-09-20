using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Путь для сохранения изображений
        string savePath = @"C:\Users\Infinitive\Downloads\DownloadedImages";

        // Условный путь к вашему файлу с данными
        string filePath = @"D:\parsed_info.txt";

        // Проверка наличия папки, если нет — создание
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // Чтение строк из файла
        string[] lines = File.ReadAllLines(filePath);

        string currentName = string.Empty;

        foreach (string line in lines)
        {
            // Убираем пробелы по краям для строки
            string trimmedLine = line.Trim();

            // Если строка начинается с "Name:", значит, это имя файла
            if (trimmedLine.StartsWith("Name:"))
            {
                // Извлекаем имя, удаляя "Name:" и пробелы, заменяем пробелы на подчеркивания
                currentName = trimmedLine.Replace("Name:", "").Trim().Replace(" ", "_");
            }
            // Если строка начинается с "Image URL:", значит, это ссылка на изображение
            else if (trimmedLine.StartsWith("Image URL:"))
            {
                // Извлекаем URL, удаляя "Image URL:" и пробелы
                string imageUrl = trimmedLine.Replace("Image URL:", "").Trim();

                // Указываем полное имя файла с расширением
                string fileName = Path.Combine(savePath, currentName + ".jpg");

                try
                {
                    // Скачивание изображения
                    using (WebClient client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(new Uri(imageUrl), fileName);
                        Console.WriteLine($"Изображение '{currentName}' успешно скачано.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при скачивании изображения '{currentName}': {ex.Message}");
                }
            }
        }
    }
}
