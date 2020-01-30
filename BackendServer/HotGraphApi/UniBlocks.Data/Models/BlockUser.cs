namespace HotGraphApi.UniBlocks.Data.Models
{
    public class BlockUser
    {
        public int BlockUserId { get; set; }
        public int BlockId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Block Block { get; set; }
    }
}