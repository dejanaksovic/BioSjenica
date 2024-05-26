using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Payloads
{
    public class RegionPayload
    {
        public string Name { get; set; }
        public float Area { get; set; }
        public string Villages { get; set; }
        public string ProtectionType { get; set; }
    }
}
