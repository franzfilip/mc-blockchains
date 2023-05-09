using HashCash.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;

namespace HashCash.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class HashCashController : Controller {
        [HttpGet("greet")]
        public IActionResult Greet() {
            var value = Request.Headers.FirstOrDefault(h => h.Key.Equals("HashREST"));
            if(value.Equals(default(KeyValuePair<string, StringValues>))) {
                return BadRequest("HashREST not found");
            }

            var isValid = HashRestValid(value.Value, 1);

            if (!isValid) {
                return BadRequest("HashREST has not been valid");
            }

            return Ok("HashREST has been valid");
        }

        [HttpGet("greet2")]
        public IActionResult Greet2() {
            var value = Request.Headers.FirstOrDefault(h => h.Key.Equals("HashREST"));
            if (value.Equals(default(KeyValuePair<string, StringValues>))) {
                return BadRequest("HashREST not found");
            }

            var isValid = HashRestValid(value.Value, 2);

            if (!isValid) {
                return BadRequest("HashREST has not been valid");
            }

            return Ok("HashREST has been valid");
        }

        [HttpGet("greet3")]
        public IActionResult Greet3() {
            var value = Request.Headers.FirstOrDefault(h => h.Key.Equals("HashREST"));
            if (value.Equals(default(KeyValuePair<string, StringValues>))) {
                return BadRequest("HashREST not found");
            }

            var isValid = HashRestValid(value.Value, 3);

            if (!isValid) {
                return BadRequest("HashREST has not been valid");
            }

            return Ok("HashREST has been valid");
        }

        private bool HashRestValid(string data, int difficulty) {
            byte[] hash = null;
            using (SHA256 sha256 = SHA256.Create()) {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            }

            return HasLeadingZeros(hash, difficulty);
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
