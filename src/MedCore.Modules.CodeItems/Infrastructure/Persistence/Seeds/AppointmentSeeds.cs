namespace MedCore.Modules.CodeItems.Infrastructure.Persistence.Seeds;

using MedCore.Common.Localization;
using MedCore.Modules.CodeItems.Infrastructure.Persistence;

internal static class AppointmentSeeds
{
    public static List<CategorySeed> All =>
    [
        new CategorySeed(
            Code: "appointment.type",
            Description: "Types of appointments offered by the clinic",
            SortOrder: 10,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Appointment Type",
                [SupportedCultures.French]  = "Type de rendez-vous",
                [SupportedCultures.German]  = "Terminart"
            },
            Items:
            [
                new("Consultation",             "Initial visit with a doctor",                  10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Consultation",               [SupportedCultures.French] = "Consultation",                  [SupportedCultures.German] = "Konsultation"                }),
                new("FollowUp",                 "Return visit after initial consultation",      20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Follow-up",                  [SupportedCultures.French] = "Suivi",                         [SupportedCultures.German] = "Nachkontrolle"               }),
                new("Emergency",                "Urgent unplanned appointment",                 30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency",                  [SupportedCultures.French] = "Urgence",                       [SupportedCultures.German] = "Notfall"                     }),
                new("Telemedicine",             "Remote video consultation",                    40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Telemedicine",               [SupportedCultures.French] = "Télémédecine",                  [SupportedCultures.German] = "Telemedizin"                 }),
                new("HomeVisit",                "Doctor visits patient at home",                50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Home Visit",                 [SupportedCultures.French] = "Visite à domicile",             [SupportedCultures.German] = "Hausbesuch"                  }),
                new("PreventiveCheckup",        "Routine preventive health check",             60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Preventive Check-up",        [SupportedCultures.French] = "Bilan de santé préventif",      [SupportedCultures.German] = "Vorsorgeuntersuchung"        }),
                new("Vaccination",              "Vaccination appointment",                      70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Vaccination",                [SupportedCultures.French] = "Vaccination",                   [SupportedCultures.German] = "Impfung"                     }),
                new("LaboratoryTest",           "Blood draw or lab sample collection",          80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Laboratory Test",            [SupportedCultures.French] = "Analyse de laboratoire",        [SupportedCultures.German] = "Laboruntersuchung"           }),
                new("Imaging",                  "X-ray, MRI, CT or ultrasound",                90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Imaging",                    [SupportedCultures.French] = "Imagerie médicale",             [SupportedCultures.German] = "Bildgebung"                  }),
                new("TherapySession",           "Physiotherapy or rehabilitation session",      100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Therapy Session",            [SupportedCultures.French] = "Séance de thérapie",            [SupportedCultures.German] = "Therapiesitzung"             }),
                new("SurgeryConsultation",      "Pre-operative or surgical planning consult",  110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Surgery Consultation",       [SupportedCultures.French] = "Consultation chirurgicale",     [SupportedCultures.German] = "Chirurgische Konsultation"   }),
                new("AdministrativeConsultation","Administrative or non-clinical visit",        120, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Administrative Consultation",[SupportedCultures.French] = "Consultation administrative",  [SupportedCultures.German] = "Administrative Konsultation" })
            ]),

        new CategorySeed(
            Code: "appointment.status",
            Description: "Lifecycle statuses of an appointment",
            SortOrder: 20,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Appointment Status",
                [SupportedCultures.French]  = "Statut du rendez-vous",
                [SupportedCultures.German]  = "Terminstatus"
            },
            Items:
            [
                new("Scheduled",        "Appointment has been booked",              10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Scheduled",         [SupportedCultures.French] = "Planifié",            [SupportedCultures.German] = "Geplant"                }),
                new("Confirmed",        "Appointment confirmed by patient",         20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Confirmed",          [SupportedCultures.French] = "Confirmé",            [SupportedCultures.German] = "Bestätigt"              }),
                new("CheckedIn",        "Patient has arrived at the clinic",        30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Checked In",         [SupportedCultures.French] = "Enregistré",          [SupportedCultures.German] = "Angemeldet"             }),
                new("InProgress",       "Appointment is currently taking place",    40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "In Progress",        [SupportedCultures.French] = "En cours",            [SupportedCultures.German] = "In Bearbeitung"         }),
                new("Completed",        "Appointment has taken place",              50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Completed",          [SupportedCultures.French] = "Terminé",             [SupportedCultures.German] = "Abgeschlossen"          }),
                new("Cancelled",        "Appointment was cancelled",                60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Cancelled",          [SupportedCultures.French] = "Annulé",              [SupportedCultures.German] = "Abgesagt"               }),
                new("NoShow",           "Patient did not attend",                   70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "No-show",            [SupportedCultures.French] = "Absent",              [SupportedCultures.German] = "Nicht erschienen"       }),
                new("Rescheduled",      "Appointment moved to a new time",          80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Rescheduled",        [SupportedCultures.French] = "Reporté",             [SupportedCultures.German] = "Umgeplant"              }),
                new("PendingApproval",  "Awaiting confirmation from the clinic",    90, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Pending Approval",   [SupportedCultures.French] = "En attente",          [SupportedCultures.German] = "Wartet auf Bestätigung" })
            ]),

        new CategorySeed(
            Code: "appointment.cancellation_reason",
            Description: "Reasons an appointment was cancelled",
            SortOrder: 30,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Cancellation Reason",
                [SupportedCultures.French]  = "Motif d'annulation",
                [SupportedCultures.German]  = "Stornierungsgrund"
            },
            Items:
            [
                new("PatientRequest",       "Patient requested cancellation",           10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Patient Request",        [SupportedCultures.French] = "Demande du patient",      [SupportedCultures.German] = "Patientenanfrage"         }),
                new("DoctorUnavailable",    "Doctor unavailable on the day",            20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Doctor Unavailable",     [SupportedCultures.French] = "Médecin indisponible",    [SupportedCultures.German] = "Arzt nicht verfügbar"    }),
                new("SchedulingConflict",   "Conflict with another appointment",        30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Scheduling Conflict",    [SupportedCultures.French] = "Conflit d'horaire",       [SupportedCultures.German] = "Terminkonflikt"           }),
                new("PatientIllness",       "Patient is too ill to attend",             40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Patient Illness",        [SupportedCultures.French] = "Maladie du patient",      [SupportedCultures.German] = "Patientenerkrankung"      }),
                new("EmergencyClosure",     "Clinic closed due to emergency",           50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency Closure",      [SupportedCultures.French] = "Fermeture d'urgence",     [SupportedCultures.German] = "Notfallschliessung"       }),
                new("TransportationIssue",  "Patient unable to travel",                 60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Transportation Issue",   [SupportedCultures.French] = "Problème de transport",   [SupportedCultures.German] = "Transportproblem"         }),
                new("InsuranceIssue",       "Insurance not accepted or expired",        70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Insurance Issue",        [SupportedCultures.French] = "Problème d'assurance",    [SupportedCultures.German] = "Versicherungsproblem"     }),
                new("WeatherConditions",    "Severe weather prevented attendance",      80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Weather Conditions",     [SupportedCultures.French] = "Conditions météo",        [SupportedCultures.German] = "Wetterbedingungen"        }),
                new("DuplicateBooking",     "Appointment was booked twice",             90, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Duplicate Booking",      [SupportedCultures.French] = "Réservation en double",   [SupportedCultures.German] = "Doppelbuchung"            }),
                new("NoReasonGiven",        "No reason was provided",                   100,new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "No Reason Given",        [SupportedCultures.French] = "Aucun motif fourni",      [SupportedCultures.German] = "Kein Grund angegeben"    })
            ]),

        new CategorySeed(
            Code: "appointment.priority",
            Description: "Priority level of an appointment",
            SortOrder: 40,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Appointment Priority",
                [SupportedCultures.French]  = "Priorité du rendez-vous",
                [SupportedCultures.German]  = "Terminpriorität"
            },
            Items:
            [
                new("Routine",   "Standard non-urgent appointment",   10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Routine",   [SupportedCultures.French] = "Routine",   [SupportedCultures.German] = "Routine"    }),
                new("Urgent",    "Needs attention soon",               20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Urgent",    [SupportedCultures.French] = "Urgent",    [SupportedCultures.German] = "Dringend"   }),
                new("Emergency", "Immediate attention required",       30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency", [SupportedCultures.French] = "Urgence",   [SupportedCultures.German] = "Notfall"    })
            ]),

        new CategorySeed(
            Code: "appointment.channel",
            Description: "Channel through which the appointment takes place",
            SortOrder: 50,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Appointment Channel",
                [SupportedCultures.French]  = "Canal de rendez-vous",
                [SupportedCultures.German]  = "Terminkanal"
            },
            Items:
            [
                new("InPerson",  "Patient attends in person",          10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "In Person",  [SupportedCultures.French] = "En personne",      [SupportedCultures.German] = "Persönlich"     }),
                new("Phone",     "Appointment conducted by phone",      20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Phone",      [SupportedCultures.French] = "Téléphone",        [SupportedCultures.German] = "Telefon"        }),
                new("Video",     "Appointment conducted via video",     30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Video",      [SupportedCultures.French] = "Vidéo",            [SupportedCultures.German] = "Video"          }),
                new("HomeVisit", "Doctor visits patient at home",       40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Home Visit", [SupportedCultures.French] = "Visite à domicile",[SupportedCultures.German] = "Hausbesuch"     })
            ]),

        new CategorySeed(
            Code: "appointment.followup_reason",
            Description: "Reasons for scheduling a follow-up appointment",
            SortOrder: 60,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Follow-up Reason",
                [SupportedCultures.French]  = "Motif de suivi",
                [SupportedCultures.German]  = "Nachkontrollgrund"
            },
            Items:
            [
                new("TestResults",              "Review of lab or imaging results",         10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Test Results",               [SupportedCultures.French] = "Résultats d'analyses",        [SupportedCultures.German] = "Testergebnisse"          }),
                new("TreatmentReview",          "Review of ongoing treatment",              20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Treatment Review",           [SupportedCultures.French] = "Révision du traitement",      [SupportedCultures.German] = "Behandlungskontrolle"    }),
                new("MedicationReview",         "Review or renewal of medication",          30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Medication Review",          [SupportedCultures.French] = "Révision des médicaments",    [SupportedCultures.German] = "Medikamentenkontrolle"   }),
                new("PostProcedure",            "Check-up after a procedure or surgery",    40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Post-procedure",             [SupportedCultures.French] = "Après intervention",          [SupportedCultures.German] = "Nachsorge"               }),
                new("ChronicCareMonitoring",    "Monitoring of a chronic condition",        50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Chronic Care Monitoring",    [SupportedCultures.French] = "Suivi maladie chronique",     [SupportedCultures.German] = "Chroniker-Kontrolle"     }),
                new("SymptomRecheck",           "Patient reports recurring symptoms",       60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Symptom Recheck",            [SupportedCultures.French] = "Réévaluation symptômes",      [SupportedCultures.German] = "Symptomkontrolle"        })
            ]),

        new CategorySeed(
            Code: "appointment.visit_reason",
            Description: "Reason the patient is visiting",
            SortOrder: 70,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Visit Reason",
                [SupportedCultures.French]  = "Motif de visite",
                [SupportedCultures.German]  = "Besuchsgrund"
            },
            Items:
            [
                new("NewProblem",           "Patient presents a new health issue",          10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "New Problem",            [SupportedCultures.French] = "Nouveau problème",            [SupportedCultures.German] = "Neues Problem"            }),
                new("ExistingProblem",      "Patient follows up on an existing issue",      20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Existing Problem",       [SupportedCultures.French] = "Problème existant",           [SupportedCultures.German] = "Bestehendes Problem"     }),
                new("PreventiveCheckup",    "Routine preventive health check",             30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Preventive Check-up",    [SupportedCultures.French] = "Bilan préventif",             [SupportedCultures.German] = "Vorsorgeuntersuchung"    }),
                new("Vaccination",          "Patient comes for a vaccination",              40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Vaccination",            [SupportedCultures.French] = "Vaccination",                 [SupportedCultures.German] = "Impfung"                 }),
                new("PrescriptionRenewal",  "Renewal of an existing prescription",         50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Prescription Renewal",   [SupportedCultures.French] = "Renouvellement ordonnance",   [SupportedCultures.German] = "Rezepterneuerung"        }),
                new("MedicalCertificate",   "Patient needs a medical certificate",          60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Medical Certificate",    [SupportedCultures.French] = "Certificat médical",          [SupportedCultures.German] = "Ärztliches Attest"       }),
                new("Referral",             "Patient referred to another specialist",       70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Referral",               [SupportedCultures.French] = "Orientation médicale",        [SupportedCultures.German] = "Überweisung"             }),
                new("SecondOpinion",        "Patient seeks a second medical opinion",       80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Second Opinion",         [SupportedCultures.French] = "Deuxième avis",               [SupportedCultures.German] = "Zweitmeinung"            })
            ])
    ];
}