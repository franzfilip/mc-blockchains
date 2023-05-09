using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashCash.Client {
    public class Header {
        public string URL { get; set; }
        public long Timestamp { get; set; }
        public string RandomChars { get; set; }
        public int Counter { get; set; }

        public Header(string url) {
            this.URL = url;
            this.RandomChars = GenerateRandomString();
            this.Timestamp = GetCurrentUnixTimestamp();
        }

        private long GetCurrentUnixTimestamp() {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epoch;
            return (long)timeSpan.TotalSeconds;
        }

        private string GenerateRandomString() {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override string ToString() {
            return $"{URL};{Timestamp};{RandomChars};{Counter}";
        }
    }
}
