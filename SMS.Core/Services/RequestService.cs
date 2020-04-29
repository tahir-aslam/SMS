using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.Services
{
    public class RequestService
    {
        public async Task<bool> PingServer(string serverURl = ApplicationConstants.SERVER_URL)
        {            
            try
            {
                HttpClient client = new HttpClient();
                var checkingResponse = await client.GetAsync(serverURl);
                if (!checkingResponse.IsSuccessStatusCode)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {                
                return false;
            }

            return true;
        }
    }
}
