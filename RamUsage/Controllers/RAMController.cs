using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Management;
using RamUsage.Models;

namespace RamUsage.Controllers
{
    [RoutePrefix("RamUsage")]
    public class RAMController : ApiController
    {
        [HttpGet]
        [Route("Server_Ram")]
        public dynamic MemoryUsage()
        {
            try
            {
                ramDetails rd = new ramDetails();
                var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

                var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
                {
                    FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                    TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
                }).FirstOrDefault();

                if (memoryValues != null)
                {
                    var percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                    rd.RamUsage = Math.Round(percent, 2).ToString();
                    rd.DateTime = DateTime.Now.ToString();
                }
                return rd;
            }
            catch (Exception exx)
            {
                return exx.Message;
                //exx = null;
            }
           
        }
    }
}