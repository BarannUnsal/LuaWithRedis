namespace RedisLua.Context.Entity
{
    public class SortedSetWithLua
    {
        public int Id { get; set; }

        public required string Score { get; set; }

        public required string Member { get; set; }
    }
}
