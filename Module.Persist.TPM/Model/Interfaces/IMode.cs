namespace Module.Persist.TPM.Model.Interfaces
{
    public interface IMode
    {
        TPMmode TPMmode { get; set; }
        RSstatus RSstatus { get; set; }
    }
    public enum TPMmode
    {
        Current,
        RS
    }
    public enum RSstatus
    {
        NotChanged,
        Changed
    }
}
