using System.ComponentModel;

namespace Common;

public enum AdminType
{
    AppAdmin = 0,
    ClinicAdmin = 1
}

public enum Gender
{
    Male = 0,
    Female = 1,
    [Description("Rather not disclose")]
    RatherNotDisclose = 2
}

public enum UserType
{
    Admin = 0,
    Doctor = 1,
    MedicalStaff = 2,
    Patient = 3
}

public enum MediWebFeature
{
    AccountManagement = 0,
    Administration = 1,
    AppointmentMaking = 2,
    AppointmentManagement = 3,
    ClinicManagement = 4,
    DoctorManagement = 5, 
    MedicalStaffManagement = 6,
    PatientManagement = 7,
    SpecializationManagement = 8,
    CRUD = 9
}
