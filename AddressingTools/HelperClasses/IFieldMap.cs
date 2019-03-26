
namespace AddressingTools
{
    public enum FieldMapType
    {
        AddressPoint,
        Centerline,
        MSAG,
    }
    public interface IFieldMap
    {
        FieldMapType getType();
    }
}
