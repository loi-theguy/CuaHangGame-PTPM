using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BLDAL
{
    public class DataHelper<T>
    {
        public const int DEPENDED = -1;
        public const int NONEXISTENT = 0;
        public const int SUCCESS = 1;
        protected static CHGameDataClassesDataContext context = new CHGameDataClassesDataContext();
        public virtual string GenerateID() { return null; }
        public virtual List<T> GetData()
        {
            return null;
        }
        public virtual bool Insert(T entity)
        {
            return true;
        }
        public virtual bool Update(T entity)
        {
            return true;
        }
        //Return value meaning:
        // -1: Data is used in another table, thus deletion is not allowed
        // 0: Invalid pID
        // 1: Deletion succeeds
        public virtual int Delete(String pID)
        {
            return 1;
        }
        public string ComputeHash(string input)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] rawOutput = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < rawOutput.Length; i++)
                    builder.Append(rawOutput[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
