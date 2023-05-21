using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AppointmentScheduler_MarcinJunka.Managers
{
    /// <summary>
    /// Generic FileManager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileManager<T>
    {
        public static async Task<bool> SaveListToJson(List<T> list, string path)
        {
            bool serializeOk = false;
            try
            {
                string jsonString = JsonConvert.SerializeObject(list);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    await sw.WriteLineAsync(jsonString);
                }

                serializeOk = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                serializeOk = false;
            }

            return serializeOk;
        }

        /// <summary>
        /// Returns tuple which is item1: bool deserializeOk and item2: actual list
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<(bool,List<T>)> LoadListFromJson(string path)
        {
            bool deserializeOk = false;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string jsonString = await sr.ReadToEndAsync();
                    List<T> list = JsonConvert.DeserializeObject<List<T>>(jsonString);
                    deserializeOk = true;
                    return (deserializeOk,list);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                deserializeOk = false;
            }

            return (deserializeOk,null);
        }


    }
}
