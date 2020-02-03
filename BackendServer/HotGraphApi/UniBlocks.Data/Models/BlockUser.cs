namespace HotGraphApi.UniBlocks.Data.Models
{
    public class BlockUser
    {
        public int BlockId { get; set; }
        public int UserId { get; set; }

        public AspNetUser AspNetUser { get; set; }
        public Block Block { get; set; }
    }
}