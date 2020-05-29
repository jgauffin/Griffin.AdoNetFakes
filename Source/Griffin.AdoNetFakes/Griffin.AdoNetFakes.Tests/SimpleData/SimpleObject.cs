using System;

namespace Griffin.AdoNetFakes.Tests.SimpleData
{
    public class SimpleObject
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        internal int ApproxUnixAge => DateOfBirth.Year - UnixEpoch.Year;
    }
}
