namespace Quick
{
    public interface IValidatableObject
    {
        string ValidateProperty(string propertyName);
        string Validate();
    }
}
