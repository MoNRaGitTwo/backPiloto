namespace DemoPilotoV1.Clases
{
    public class AudioFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
