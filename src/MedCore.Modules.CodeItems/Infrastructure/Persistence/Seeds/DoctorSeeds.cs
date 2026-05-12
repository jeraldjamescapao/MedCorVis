namespace MedCore.Modules.CodeItems.Infrastructure.Persistence.Seeds;

using MedCore.Common.Localization;
using MedCore.Modules.CodeItems.Infrastructure.Persistence;

internal static class DoctorSeeds
{
    public static List<CategorySeed> All =>
    [
        new CategorySeed(
            Code: "doctor.specialty",
            Description: "Medical specialties of doctors",
            SortOrder: 170,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Specialty",
                [SupportedCultures.French]  = "Spécialité",
                [SupportedCultures.German]  = "Fachgebiet"
            },
            Items:
            [
                new("GeneralPractice",              "General / family medicine",                10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "General Practice",               [SupportedCultures.French] = "Médecine générale",               [SupportedCultures.German] = "Allgemeinmedizin"             }),
                new("InternalMedicine",             "Internal medicine",                        20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Internal Medicine",              [SupportedCultures.French] = "Médecine interne",                [SupportedCultures.German] = "Innere Medizin"               }),
                new("Cardiology",                   "Heart and cardiovascular system",          30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Cardiology",                     [SupportedCultures.French] = "Cardiologie",                     [SupportedCultures.German] = "Kardiologie"                  }),
                new("Neurology",                    "Brain and nervous system",                 40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Neurology",                      [SupportedCultures.French] = "Neurologie",                      [SupportedCultures.German] = "Neurologie"                   }),
                new("Orthopedics",                  "Bones, joints, and muscles",               50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Orthopedics",                    [SupportedCultures.French] = "Orthopédie",                      [SupportedCultures.German] = "Orthopädie"                   }),
                new("Pediatrics",                   "Medical care for children",                60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Pediatrics",                     [SupportedCultures.French] = "Pédiatrie",                       [SupportedCultures.German] = "Pädiatrie"                    }),
                new("Psychiatry",                   "Mental health and behavior",               70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Psychiatry",                     [SupportedCultures.French] = "Psychiatrie",                     [SupportedCultures.German] = "Psychiatrie"                  }),
                new("Dermatology",                  "Skin, hair, and nails",                    80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Dermatology",                    [SupportedCultures.French] = "Dermatologie",                    [SupportedCultures.German] = "Dermatologie"                 }),
                new("GynecologyObstetrics",         "Female reproductive system and pregnancy", 90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Gynecology & Obstetrics",        [SupportedCultures.French] = "Gynécologie-Obstétrique",         [SupportedCultures.German] = "Gynäkologie und Geburtshilfe" }),
                new("Ophthalmology",                "Eyes and vision",                          100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Ophthalmology",                  [SupportedCultures.French] = "Ophtalmologie",                   [SupportedCultures.German] = "Augenheilkunde"               }),
                new("ENT",                          "Ear, nose, and throat",                    110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "ENT",                            [SupportedCultures.French] = "ORL",                             [SupportedCultures.German] = "HNO"                          }),
                new("Radiology",                    "Medical imaging and diagnostics",          120, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Radiology",                      [SupportedCultures.French] = "Radiologie",                      [SupportedCultures.German] = "Radiologie"                   }),
                new("Anesthesiology",               "Anesthesia and pain management",           130, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Anesthesiology",                 [SupportedCultures.French] = "Anesthésiologie",                 [SupportedCultures.German] = "Anästhesiologie"              }),
                new("Surgery",                      "Surgical procedures",                      140, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Surgery",                        [SupportedCultures.French] = "Chirurgie",                       [SupportedCultures.German] = "Chirurgie"                    }),
                new("EmergencyMedicine",            "Emergency and acute care",                 150, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency Medicine",             [SupportedCultures.French] = "Médecine d'urgence",              [SupportedCultures.German] = "Notfallmedizin"               }),
                new("Gastroenterology",             "Digestive system",                         160, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Gastroenterology",               [SupportedCultures.French] = "Gastroentérologie",               [SupportedCultures.German] = "Gastroenterologie"            }),
                new("EndocrinologyDiabetology",     "Hormones and metabolism",                  170, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Endocrinology & Diabetology",    [SupportedCultures.French] = "Endocrinologie-Diabétologie",     [SupportedCultures.German] = "Endokrinologie und Diabetologie"}),
                new("Pulmonology",                  "Lungs and respiratory system",             180, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Pulmonology",                    [SupportedCultures.French] = "Pneumologie",                     [SupportedCultures.German] = "Pneumologie"                  }),
                new("Urology",                      "Urinary tract and male reproductive",      190, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Urology",                        [SupportedCultures.French] = "Urologie",                        [SupportedCultures.German] = "Urologie"                     }),
                new("Oncology",                     "Cancer diagnosis and treatment",           200, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Oncology",                       [SupportedCultures.French] = "Oncologie",                       [SupportedCultures.German] = "Onkologie"                    }),
                new("Hematology",                   "Blood and blood disorders",                210, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Hematology",                     [SupportedCultures.French] = "Hématologie",                     [SupportedCultures.German] = "Hämatologie"                  }),
                new("Nephrology",                   "Kidneys and renal function",               220, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Nephrology",                     [SupportedCultures.French] = "Néphrologie",                     [SupportedCultures.German] = "Nephrologie"                  }),
                new("Rheumatology",                 "Joints, muscles, and autoimmune",          230, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Rheumatology",                   [SupportedCultures.French] = "Rhumatologie",                    [SupportedCultures.German] = "Rheumatologie"                }),
                new("InfectiousDiseases",           "Infectious and parasitic diseases",        240, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Infectious Diseases",            [SupportedCultures.French] = "Maladies infectieuses",           [SupportedCultures.German] = "Infektionskrankheiten"        }),
                new("PhysicalMedicineRehabilitation","Physical therapy and rehabilitation",     250, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Physical Medicine & Rehabilitation",[SupportedCultures.French] = "Médecine physique et réadaptation",[SupportedCultures.German] = "Physikalische Medizin und Rehabilitation"})
            ]),

        new CategorySeed(
            Code: "doctor.department",
            Description: "Hospital or clinic department the doctor belongs to",
            SortOrder: 180,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Department",
                [SupportedCultures.French]  = "Département",
                [SupportedCultures.German]  = "Abteilung"
            },
            Items:
            [
                new("GeneralMedicine",      "General medicine department",               10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "General Medicine",       [SupportedCultures.French] = "Médecine générale",       [SupportedCultures.German] = "Allgemeinmedizin"         }),
                new("EmergencyDepartment",  "Emergency and acute care department",       20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency Department",   [SupportedCultures.French] = "Service des urgences",    [SupportedCultures.German] = "Notaufnahme"              }),
                new("InternalMedicine",     "Internal medicine department",              30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Internal Medicine",      [SupportedCultures.French] = "Médecine interne",        [SupportedCultures.German] = "Innere Medizin"           }),
                new("Surgery",              "Surgical department",                       40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Surgery",                [SupportedCultures.French] = "Chirurgie",               [SupportedCultures.German] = "Chirurgie"                }),
                new("Cardiology",           "Cardiology department",                     50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Cardiology",             [SupportedCultures.French] = "Cardiologie",             [SupportedCultures.German] = "Kardiologie"              }),
                new("Neurology",            "Neurology department",                      60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Neurology",              [SupportedCultures.French] = "Neurologie",              [SupportedCultures.German] = "Neurologie"               }),
                new("Orthopedics",          "Orthopedics department",                    70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Orthopedics",            [SupportedCultures.French] = "Orthopédie",              [SupportedCultures.German] = "Orthopädie"               }),
                new("Pediatrics",           "Pediatrics department",                     80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Pediatrics",             [SupportedCultures.French] = "Pédiatrie",               [SupportedCultures.German] = "Pädiatrie"                }),
                new("Psychiatry",           "Psychiatry department",                     90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Psychiatry",             [SupportedCultures.French] = "Psychiatrie",             [SupportedCultures.German] = "Psychiatrie"              }),
                new("GynecologyObstetrics", "Gynecology and obstetrics department",      100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Gynecology & Obstetrics",[SupportedCultures.French] = "Gynécologie-Obstétrique",  [SupportedCultures.German] = "Gynäkologie und Geburtshilfe"}),
                new("Radiology",            "Radiology and imaging department",          110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Radiology",              [SupportedCultures.French] = "Radiologie",              [SupportedCultures.German] = "Radiologie"               }),
                new("Laboratory",           "Clinical laboratory department",            120, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Laboratory",             [SupportedCultures.French] = "Laboratoire",             [SupportedCultures.German] = "Labor"                    }),
                new("IntensiveCare",        "Intensive care unit",                       130, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Intensive Care",         [SupportedCultures.French] = "Soins intensifs",         [SupportedCultures.German] = "Intensivstation"          }),
                new("Rehabilitation",       "Rehabilitation department",                 140, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Rehabilitation",         [SupportedCultures.French] = "Réadaptation",            [SupportedCultures.German] = "Rehabilitation"           }),
                new("OutpatientClinic",     "Outpatient clinic",                         150, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Outpatient Clinic",      [SupportedCultures.French] = "Clinique ambulatoire",    [SupportedCultures.German] = "Ambulatorium"             })
            ]),

        new CategorySeed(
            Code: "doctor.rank",
            Description: "Professional rank or seniority of the doctor",
            SortOrder: 190,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Doctor Rank",
                [SupportedCultures.French]  = "Grade médical",
                [SupportedCultures.German]  = "Ärzterang"
            },
            Items:
            [
                new("AssistantDoctor",      "Junior doctor in training",                    10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Assistant Doctor",      [SupportedCultures.French] = "Médecin assistant",       [SupportedCultures.German] = "Assistenzarzt"            }),
                new("Resident",             "Doctor in a residency program",                20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Resident",              [SupportedCultures.French] = "Résident",                [SupportedCultures.German] = "Facharzt in Ausbildung"  }),
                new("SeniorPhysician",      "Experienced specialist doctor",                30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Senior Physician",      [SupportedCultures.French] = "Médecin senior",          [SupportedCultures.German] = "Oberarzt"                 }),
                new("AttendingPhysician",   "Fully qualified attending doctor",             40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Attending Physician",   [SupportedCultures.French] = "Médecin traitant",        [SupportedCultures.German] = "Leitender Arzt"          }),
                new("ChiefPhysician",       "Head of a department",                         50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Chief Physician",       [SupportedCultures.French] = "Médecin chef",            [SupportedCultures.German] = "Chefarzt"                 }),
                new("Consultant",           "Specialist called for expert opinion",         60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Consultant",            [SupportedCultures.French] = "Consultant",              [SupportedCultures.German] = "Konsiliararzt"            })
            ]),

        new CategorySeed(
            Code: "doctor.employment_type",
            Description: "Employment arrangement of the doctor",
            SortOrder: 200,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Employment Type",
                [SupportedCultures.French]  = "Type d'emploi",
                [SupportedCultures.German]  = "Beschäftigungsart"
            },
            Items:
            [
                new("FullTime",     "Full-time employed doctor",        10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Full-time",  [SupportedCultures.French] = "Temps plein",     [SupportedCultures.German] = "Vollzeit"         }),
                new("PartTime",     "Part-time employed doctor",        20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Part-time",  [SupportedCultures.French] = "Temps partiel",   [SupportedCultures.German] = "Teilzeit"         }),
                new("External",     "External or contracted doctor",    30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "External",   [SupportedCultures.French] = "Externe",         [SupportedCultures.German] = "Extern"           }),
                new("Temporary",    "Temporary contract",               40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Temporary",  [SupportedCultures.French] = "Temporaire",      [SupportedCultures.German] = "Befristet"        }),
                new("Locum",        "Locum or substitute doctor",       50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Locum",      [SupportedCultures.French] = "Remplaçant",      [SupportedCultures.German] = "Vertretungsarzt"  })
            ]),

        new CategorySeed(
            Code: "doctor.availability_status",
            Description: "Current availability status of the doctor",
            SortOrder: 210,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Availability Status",
                [SupportedCultures.French]  = "Statut de disponibilité",
                [SupportedCultures.German]  = "Verfügbarkeitsstatus"
            },
            Items:
            [
                new("Available",        "Doctor is available for appointments",      10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Available",      [SupportedCultures.French] = "Disponible",          [SupportedCultures.German] = "Verfügbar"            }),
                new("Busy",             "Doctor is currently busy",                  20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Busy",           [SupportedCultures.French] = "Occupé",              [SupportedCultures.German] = "Beschäftigt"          }),
                new("InConsultation",   "Doctor is in an active consultation",       30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "In Consultation", [SupportedCultures.French] = "En consultation",     [SupportedCultures.German] = "In der Konsultation"  }),
                new("OnCall",           "Doctor is on call",                         40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "On Call",         [SupportedCultures.French] = "De garde",            [SupportedCultures.German] = "Bereitschaftsdienst"  }),
                new("OnLeave",          "Doctor is on leave",                        50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "On Leave",        [SupportedCultures.French] = "En congé",            [SupportedCultures.German] = "Abwesend"             }),
                new("OffDuty",          "Doctor is off duty",                        60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Off Duty",        [SupportedCultures.French] = "Non disponible",      [SupportedCultures.German] = "Nicht im Dienst"      })
            ])
    ];
}