namespace GenmaWebApp.Models
{
    public class CommentModel
    {
        public int id { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public CommentModel (){}
        public CommentModel(int id, string message, string username){}


        // public Comments()
        // {
        // }
        //
        // public Comments(int id, string message, string username)
        // {
        //     this.id = id;
        //     this.message = message;
        //     this.username = username;
        // }
    }
}