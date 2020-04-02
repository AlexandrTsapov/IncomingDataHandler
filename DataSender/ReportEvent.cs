using System;

namespace DataSender
{
    public class ReportEvent
    {
        public DateTime Time { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
