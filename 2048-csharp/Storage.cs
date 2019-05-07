using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Game2048
{
    public class Storage
    {
        public Storage()
        {
            _Storage = IsolatedStorageFile.GetStore(
                IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly,
                null,
                null
            );
        }

        /// <summary>
        /// Получает лучший счет из стора приложения.
        /// </summary>
        /// <returns>
        /// Возвращает лучший счет из стора приложения. Если счет получить из стора не удалось, то возвращает 0.
        /// </returns>
        public int ReadBestScore()
        {
            int score;
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(new IsolatedStorageFileStream("Data\\bestScore.txt", FileMode.Open, _Storage));
                score = Convert.ToInt32(sr.ReadLine());
                sr.Close();
            }
            catch
            {
                score = 0;
            }

            return score;
        }

        /// <summary>
        /// Записывает значение лучшего счета в стор.
        /// </summary>
        /// <param name="score">Значение лучшего счета.</param>
        public void WriteBestScore(int score)
        {
            _Storage.CreateDirectory("Data");
            StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream(
                "Data\\bestScore.txt",
                FileMode.Create,
                _Storage
            ));
            sw.WriteLine(score);
            sw.Close();
        }

        private readonly IsolatedStorageFile _Storage;
    }
}