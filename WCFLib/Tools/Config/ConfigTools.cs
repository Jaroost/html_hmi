
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace WCFLib
{
    public class ConfigTools
    {
        public static List<Uri> GetWCFBaseAddress(string wcfExeFilename)
        {
            var config = ConfigurationManager.OpenExeConfiguration(wcfExeFilename);

            // Get the collection of the section groups.
            var sectionGroups = config.SectionGroups;

            // Get the serviceModel section
            var serviceModelSection = sectionGroups.OfType<ServiceModelSectionGroup>().SingleOrDefault();

            // Check if serviceModel section is configured
            if (serviceModelSection == null)
                return new List<Uri>();

            // Get base addresses
            return (from ServiceElement service in serviceModelSection.Services.Services
                    from BaseAddressElement baseAddress in service.Host.BaseAddresses
                    select new Uri(baseAddress.BaseAddress)).ToList();
        }

        public static List<Uri> GetWCFBaseAddress()
        {
            return GetWCFBaseAddress(ApplicationTools.ExecutablePath);
        }

        public static Uri GetFirstBaseAddress(string wcfExeFilename)
        {
            List<Uri> list = ConfigTools.GetWCFBaseAddress(wcfExeFilename);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public static Uri GetFirstBaseAddress()
        {
            return GetFirstBaseAddress(ApplicationTools.ExecutablePath);
        }        

        public static int GetFirstBaseAddressPort()
        {
            Uri baseAdress = GetFirstBaseAddress();
            if (baseAdress!=null)
            {
                return baseAdress.Port;
            }
            return 8080;
        }
    }
}
