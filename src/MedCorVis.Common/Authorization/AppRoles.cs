namespace MedCorVis.Common.Authorization;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Doctor = "Doctor";
    public const string MedicalSecretary = "MedicalSecretary";
    public const string MedTech = "MedTech";
    public const string Patient = "Patient";
    public const string Nurse = "Nurse";
    public const string Receptionist = "Receptionist";

    public static readonly IReadOnlySet<string> All 
        = new HashSet<string>
        {
            Admin, 
            Doctor, 
            MedicalSecretary, 
            MedTech,
            Patient, 
            Nurse, 
            Receptionist
        };
}