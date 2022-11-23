using E_HealthCare_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compare = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace E_HealthCare_Web.ViewModels
{

    public class PatientHomeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Name { get; set; }


        [Display(Name = "Gender")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Gender { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a valid Email Address")]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
        public string Email { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date, ErrorMessage = "Please Enter a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}", NullDisplayText = "Not Filled")]
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "Address")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Address { get; set; }

        [Display(Name = "Contact")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Blood Group")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string BloodGroup { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }

        
    }
    public class EditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Required")]
        public string Gender { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Enter Valid EmailAddress")]
        [Display(Name = "Email Address")]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "Not Filled")]
        [Display(Name = "Date Of Birth")]
        [Required]
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Blood Group")]
        [DataType(DataType.Text)]
        public string BloodGroup { get; set; }
    }

    public class PatientProfileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Name { get; set; }

        [Display(Name = "Gender")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Gender { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a valid Email Address")]
        [Remote("IsEmailAlreadyExist", "Account", ErrorMessage = "Email Address is already in use")]
        public string Email { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date, ErrorMessage = "Please Enter a valid Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}", NullDisplayText = "Not Filled")]
        public Nullable<System.DateTime> DOB { get; set; }

        [Display(Name = "Address")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string Address { get; set; }

        [Display(Name = "Contact")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Display(Name = "Blood Group")]
        [DisplayFormat(NullDisplayText = "Not Filled")]
        public string BloodGroup { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string ProfileImagePath { get; set; }

    }

    public class PatientAccountViewModel
    {
        public int id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public bool IsEmailVerified { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

    public class PatientPasswordChangeViewModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "field is required")]
        [DataType(DataType.Password)]
        [Remote("IsCorrectPassword", "Patient", ErrorMessage = "Password is incorrect", AdditionalFields = "id")]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "field is required")]
        [StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "field is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password Does Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }
    }

    public class AppointmentListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Doctor Name")]
        [DisplayFormat(NullDisplayText = "Not Available")]
        public string doctorName { get; set; }

        [Display(Name = "Appointment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime appointmentDate { get; set; }

        [Display(Name = "Appointment Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime appointmentTime { get; set; }

        [Display(Name = "Department")]
        public string departmentName { get; set; }

    }
    
    public class BookAppointmentViewModel
    {

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "please Enter appointment date")]
        [Display(Name = "Appointment Date")]
        [Required(ErrorMessage = "appointment date is mandatory")]
        [Remote("IsDateValid", "Patient", ErrorMessage = "please enter date starting today between monday and friday")]
        public DateTime AppointmentDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time, ErrorMessage = "please Enter appointment Time")]
        [Display(Name = "Time")]
        [Required(ErrorMessage = "select valid time")]
        [Remote("IsTimeValid", "Patient", ErrorMessage = "please choose time between 9 AM to 5 PM only")]
        public DateTime AppointmentTime { get; set; }

        [Display(Name = "Describe Your Problem")]
        [Required(ErrorMessage = "please describe your problem")]
        public string ProblemDescription { get; set; }

        public IEnumerable<Doctor> doctors { get; set; }

        [Display(Name = "Doctor")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select doctor")]
        public int DoctorSelectedId { get; set; }

        [Required(ErrorMessage = "select department")]
        [Display(Name = "Department")]
        public int DepartmentSelectedId { get; set; }

        public SelectList selectDepartmentList { get; set; }

        [Display(Name = "Department")]
        public IEnumerable<Department> departments { get; set; }

    }

    public class UploadReportViewModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "please select report type")]
        [Display(Name = "Report Type")]
        public ReportType? ReportTypes { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "please give report name")]
        public string ReportName { get; set; }
    }
    
    public enum ReportType
    {
        [Display(Name = "Others")]
        Others,

        [Display(Name = "Medical Certificate")]
        MedicalCertificate,

        [Display(Name = "Comorbidity Decleration COVID19")]
        ComorbidityDeclerationCOVID19,

        [Display(Name = "Diet Charts")]
        DietCharts,

        [Display(Name = "UltraSound Documents")]
        UltrasoundDocuments,

        [Display(Name = "Cardio Documents ECG-ECHO-TMT")]
        CardioDocumentsECGECHOTMT,

        [Display(Name = "Lab Documents")]
        LabDocuments,

        [Display(Name = "Clinical Images")]
        ClinicalImages,

        [Display(Name = "X-Ray Documents")]
        XRayDocuments,


        [Display(Name = "Perscription")]
        Prescription,

        [Display(Name = "ePerscription")]
        ePrescriptions,

        [Display(Name = "Vaccination Records")]
        VaccinationRecord,

        [Display(Name = "RadioLogy Reports")]
        RadiologyReport,

        [Display(Name = "Pathology Reports")]
        PathologyReport,

        [Display(Name = "Discharge Summary")]
        DischargeSummary

    }
    
    public class ReportListViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Report Name")]
        public string reportName { get; set; }

        [Display(Name = "Report")]
        public string fileLink { get; set; }

        [Display(Name = "Report Type")]
        public string reportType { get; set; }

        public string ReprotTypeFromDB
        {
            get
            {
                return GetReportType();
            }
        }

        public string GetReportType()
        {
            if (reportType.Equals(ReportType.CardioDocumentsECGECHOTMT.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.CardioDocumentsECGECHOTMT);
            }
            else if (reportType.Equals(ReportType.ClinicalImages.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.ClinicalImages);
            }
            else if (reportType.Equals(ReportType.ComorbidityDeclerationCOVID19.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.ComorbidityDeclerationCOVID19);
            }
            else if (reportType.Equals(ReportType.DietCharts.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.DietCharts);
            }
            else if (reportType.Equals(ReportType.DischargeSummary.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.DischargeSummary);
            }
            else if (reportType.Equals(ReportType.ePrescriptions.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.ePrescriptions);
            }
            else if (reportType.Equals(ReportType.LabDocuments.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.LabDocuments);
            }
            else if (reportType.Equals(ReportType.MedicalCertificate.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.MedicalCertificate);
            }
            else if (reportType.Equals(ReportType.Others.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.Others);
            }
            else if (reportType.Equals(ReportType.PathologyReport.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.PathologyReport);
            }
            else if (reportType.Equals(ReportType.Prescription.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.Prescription);
            }
            else if (reportType.Equals(ReportType.RadiologyReport.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.RadiologyReport);
            }
            else if (reportType.Equals(ReportType.UltrasoundDocuments.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.UltrasoundDocuments);
            }
            else if (reportType.Equals(ReportType.VaccinationRecord.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.VaccinationRecord);
            }
            else if (reportType.Equals(ReportType.XRayDocuments.ToString()))
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.XRayDocuments);
            }
            else
            {
                return EnumHelperForDisplay.GetDisplayName(ReportType.Others); ;
            }
        }

    }

    public class FindDoctorViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Department")]
        public IEnumerable<Department> departmentName { get; set; }

        [Display(Name = "Doctor Name")]
        public IEnumerable<Doctor> doctorsName { get; set; }


        [Display(Name = "Department")]
        public int? DepartmentSelectedId { get; set; }

        public SelectList selectedDepartment { get; set; }

    }

    public class ConsultationViewModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int id { get; set; }

        [Display(Name = "Doctors")]
        public IEnumerable<Doctor> DoctorsList { get; set; }

        public Patient CurrentPatient { get; set; }
        
    }

    public class ChatBoxViewModel
    {   
        public string MessageText { get; set; }
        [DataType(DataType.Date)]
        public DateTime MessageDate { get; set; }                
        [DataType(DataType.Time)]
        public TimeSpan MessageTime { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string ConnectionID { get; set; }

    }

    public class ChatLoadViewModel
    {
        public Patient patient { get; set; }
        public Doctor doctor { get; set; }
    }


    

}

