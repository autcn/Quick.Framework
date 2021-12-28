namespace Quick
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntity
    {
        long Id { get; set; }
    }
}
