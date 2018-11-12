using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class SnapshotId
    {
        public short Id { get; set; }
        public SentoniServiceReference.SnapshotType SnapshotType { get; set; }
        public DateTime? TimeStamp { get; set; }

        public override string ToString()
        {
            if (!TimeStamp.HasValue)
            {
                return SnapshotType.ToString();
            }
            else
            {
                return String.Format(@"{0:yyyy-MM-dd hh\:mm} - {1}", TimeStamp, SnapshotType.ToString());
            }
        }
    }
}

