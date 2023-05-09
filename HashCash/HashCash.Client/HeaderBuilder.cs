using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashCash.Client {
    public class HeaderBuilder {
        public static string GenerateMessage(Header header) {
            return $"{header.URL};{header.Timestamp};{header.RandomChars};{header.Counter}";
        }

        public static Header CreateHashWithLeadingZeros(Header header, int difficulty) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] hash;
                header.Counter = 0;

                do {
                    header.Counter++;
                    hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(GenerateMessage(header)));
                    //Console.WriteLine(GenerateMessage(header));

                }
                while (!HasLeadingZeros(hash, difficulty));

                //string hashString = Convert.ToHexString(hash);
                return header;
            }
        }

        private static bool HasLeadingZeros(byte[] bytes, int difficulty) {
            for (int i = 0; i < difficulty; i++) {
                if (bytes[i] != 0x00) {
                    return false;
                }
            }

            return true;
        }
    }
}
