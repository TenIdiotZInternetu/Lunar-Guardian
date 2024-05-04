using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Serialization
{
    /// <summary>
    /// System for saving and loading object data on local machine
    /// </summary>
    public static class SaveSystem
    {
        /* ------------------- Public Methods ------------------- */
        
        /// <summary>
        /// Save data to a file
        /// </summary>
        /// <param name="data"> Object of which data are save </param>
        /// <param name="pathSuffix"> Relative path to resulting save file </param>
        /// <typeparam name="T"> Type of the saved object </typeparam>
        public static void SaveData<T>(T data, string pathSuffix)
        {
            var formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, pathSuffix);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }

        
        /// <summary>
        /// Loads data from a save file
        /// </summary>
        /// <param name="pathSuffix"> Relative path to save file </param>
        /// <typeparam name="T"> Type of object to load </typeparam>
        /// <returns> Object of specified type with values retrieved from the save file </returns>
        /// <exception cref="FileNotFoundException"> Specified file path doesn't exist </exception>
        /// <exception cref="InvalidDataException"> File format on the specified type doesn't match the type T </exception>
        public static T LoadData<T>(string pathSuffix)
        {
            var formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, pathSuffix);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file {path} does not exist");
            }

            using (var stream = new FileStream(path, FileMode.Open))
            {
                var data = formatter.Deserialize(stream);
                
                if (data is T dataOfCorrectType)
                {
                    return dataOfCorrectType;
                }
                
                throw new InvalidDataException($"The file {path} does not contain data of type {typeof(T)}");
            }
        }
    }
}