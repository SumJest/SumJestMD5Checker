using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptlib
{
    class RCC4
    {
        byte[] S = new Byte[256];
        byte[] key;
        int x = 0;
        int y = 0;
        public RCC4(byte[] key)
        {
            if (key.Length < 6)
            {
                throw new tooShortKeyException("The key length must be equal to or greater than six.");
            }
            init(key);

            this.key = key;

        }
        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            S.Swap<byte>(x, y);
            return S[(S[x] + S[y]) % 256];
        }
        public void init(byte[] key)
        {
            int keylenth = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keylenth]) % 256;
                S.Swap<byte>(i, j);
            }
        }

        public byte[] Encode(byte[] dataB)
        {
            byte[] lol = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                lol[i] = key[key.Length - 1 - i];
            }

            byte[] newdata = new byte[dataB.Length + lol.Length];
            for (int i = 0; i < lol.Length; i++)
            {
                newdata[i] = lol[i];
            }
            for (int i = 0; i < dataB.Length; i++)
            {
                newdata[i + 4] = dataB[i];
            }
            byte[] data = newdata.Take(newdata.Length).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(data[i] ^ keyItem());
            }
            return cipher;
        }
        public byte[] Decode(byte[] dataB)
        {
            byte[] data = dataB.Take(dataB.Length).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(data[i] ^ keyItem());
            }
            string scipher = Encoding.ASCII.GetString(cipher);
            byte[] lol = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                lol[i] = key[key.Length - 1 - i];
            }
            Console.WriteLine(scipher);
            if (scipher.StartsWith(Encoding.ASCII.GetString(lol)))
            {
                return Encoding.ASCII.GetBytes(scipher.Remove(0, 4));
            }
            else
            {
                throw new InvalidKeyException("Wrong key for decryption.");
            }

        }

    }
    static class SwapExt
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }


    }
    public class tooShortKeyException : Exception { public tooShortKeyException(string message) : base(message) { } }
    public class InvalidKeyException : Exception { public InvalidKeyException(string message) : base(message) { } }
}
