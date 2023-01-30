namespace Meets.WebApi.MeetupFolder
{
    internal class MeetupEntity
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public string Place { get; set; }
        public int Duration { get; set; }
    }
}
