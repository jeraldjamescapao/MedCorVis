namespace MedCore.Modules.CodeItems.Infrastructure.Persistence.Seeds;

using MedCore.Common.Localization;
using MedCore.Modules.CodeItems.Infrastructure.Persistence;

internal static class PatientSeeds
{
    public static List<CategorySeed> All =>
    [
        new CategorySeed(
            Code: "patient.type",
            Description: "Classification of patients by care level",
            SortOrder: 80,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Patient Type",
                [SupportedCultures.French]  = "Type de patient",
                [SupportedCultures.German]  = "Patientenart"
            },
            Items:
            [
                new("Outpatient",       "Patient visits without admission",             10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Outpatient",        [SupportedCultures.French] = "Ambulatoire",              [SupportedCultures.German] = "Ambulant"                 }),
                new("Inpatient",        "Patient admitted to the facility",             20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Inpatient",         [SupportedCultures.French] = "Hospitalisé",              [SupportedCultures.German] = "Stationär"                }),
                new("Emergency",        "Walk-in emergency patient",                    30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Emergency",         [SupportedCultures.French] = "Urgence",                  [SupportedCultures.German] = "Notfall"                  }),
                new("DayPatient",       "Patient admitted for less than one day",       40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Day Patient",       [SupportedCultures.French] = "Patient de jour",          [SupportedCultures.German] = "Tagesklinikpatient"       }),
                new("WalkIn",           "Patient without a prior appointment",          50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Walk-in",           [SupportedCultures.French] = "Sans rendez-vous",         [SupportedCultures.German] = "Ohne Termin"              }),
                new("ReferredPatient",  "Patient referred by another provider",         60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Referred Patient",  [SupportedCultures.French] = "Patient référé",           [SupportedCultures.German] = "Überwiesener Patient"    }),
                new("PrivatePatient",   "Patient paying privately",                     70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Private Patient",   [SupportedCultures.French] = "Patient privé",            [SupportedCultures.German] = "Privatpatient"            }),
                new("PriorityCare",     "Patient requiring priority attention",         80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Priority Care",     [SupportedCultures.French] = "Soins prioritaires",       [SupportedCultures.German] = "Vorrangige Versorgung"   })
            ]),

        new CategorySeed(
            Code: "patient.status",
            Description: "Current status of a patient record",
            SortOrder: 90,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Patient Status",
                [SupportedCultures.French]  = "Statut du patient",
                [SupportedCultures.German]  = "Patientenstatus"
            },
            Items:
            [
                new("Active",               "Patient is currently under care",                  10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Active",                [SupportedCultures.French] = "Actif",                   [SupportedCultures.German] = "Aktiv"                    }),
                new("InTreatment",          "Patient is receiving active treatment",            20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "In Treatment",          [SupportedCultures.French] = "En traitement",           [SupportedCultures.German] = "In Behandlung"            }),
                new("WaitingAdmission",     "Patient is waiting to be admitted",                30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Waiting Admission",     [SupportedCultures.French] = "En attente d'admission",  [SupportedCultures.German] = "Wartet auf Aufnahme"     }),
                new("Discharged",           "Patient has been discharged",                      40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Discharged",            [SupportedCultures.French] = "Sorti",                   [SupportedCultures.German] = "Entlassen"                }),
                new("Transferred",          "Patient transferred to another facility",          50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Transferred",           [SupportedCultures.French] = "Transféré",               [SupportedCultures.German] = "Verlegt"                  }),
                new("Deceased",             "Patient is deceased",                              60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Deceased",              [SupportedCultures.French] = "Décédé",                  [SupportedCultures.German] = "Verstorben"               }),
                new("Archived",             "Patient record archived, no longer active",        70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Archived",              [SupportedCultures.French] = "Archivé",                 [SupportedCultures.German] = "Archiviert"               }),
                new("PendingVerification",  "Patient identity or data awaiting verification",   80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Pending Verification", [SupportedCultures.French] = "En attente vérification", [SupportedCultures.German] = "Ausstehende Prüfung"     })
            ]),

        new CategorySeed(
            Code: "patient.blood_type",
            Description: "ABO and Rh blood group system",
            SortOrder: 100,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Blood Type",
                [SupportedCultures.French]  = "Groupe sanguin",
                [SupportedCultures.German]  = "Blutgruppe"
            },
            Items:
            [
                new("APlus",   "A+ blood type",   10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "A+",      [SupportedCultures.French] = "A+",      [SupportedCultures.German] = "A+"      }),
                new("AMinus",  "A- blood type",   20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "A-",      [SupportedCultures.French] = "A-",      [SupportedCultures.German] = "A-"      }),
                new("BPlus",   "B+ blood type",   30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "B+",      [SupportedCultures.French] = "B+",      [SupportedCultures.German] = "B+"      }),
                new("BMinus",  "B- blood type",   40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "B-",      [SupportedCultures.French] = "B-",      [SupportedCultures.German] = "B-"      }),
                new("OPlus",   "O+ blood type",   50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "O+",      [SupportedCultures.French] = "O+",      [SupportedCultures.German] = "O+"      }),
                new("OMinus",  "O- blood type",   60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "O-",      [SupportedCultures.French] = "O-",      [SupportedCultures.German] = "O-"      }),
                new("ABPlus",  "AB+ blood type",  70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "AB+",     [SupportedCultures.French] = "AB+",     [SupportedCultures.German] = "AB+"     }),
                new("ABMinus", "AB- blood type",  80, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "AB-",     [SupportedCultures.French] = "AB-",     [SupportedCultures.German] = "AB-"     }),
                new("Unknown", "Blood type unknown or not recorded", 90, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown", [SupportedCultures.French] = "Inconnu", [SupportedCultures.German] = "Unbekannt" })
            ]),

        new CategorySeed(
            Code: "patient.gender",
            Description: "Gender of the patient",
            SortOrder: 110,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Gender",
                [SupportedCultures.French]  = "Genre",
                [SupportedCultures.German]  = "Geschlecht"
            },
            Items:
            [
                new("Male",    "Male",                          10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Male",    [SupportedCultures.French] = "Homme",           [SupportedCultures.German] = "Männlich"         }),
                new("Female",  "Female",                        20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Female",  [SupportedCultures.French] = "Femme",           [SupportedCultures.German] = "Weiblich"         }),
                new("Other",   "Other gender identity",         30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Other",   [SupportedCultures.French] = "Autre",           [SupportedCultures.German] = "Divers"           }),
                new("Unknown", "Not specified or not recorded", 40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown", [SupportedCultures.French] = "Non spécifié",    [SupportedCultures.German] = "Nicht angegeben"  })
            ]),

        new CategorySeed(
            Code: "patient.marital_status",
            Description: "Marital or civil status of the patient",
            SortOrder: 120,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Marital Status",
                [SupportedCultures.French]  = "État civil",
                [SupportedCultures.German]  = "Zivilstand"
            },
            Items:
            [
                new("Single",                   "Not married",                                  10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Single",                    [SupportedCultures.French] = "Célibataire",             [SupportedCultures.German] = "Ledig"                    }),
                new("Married",                  "Legally married",                              20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Married",                   [SupportedCultures.French] = "Marié(e)",                [SupportedCultures.German] = "Verheiratet"              }),
                new("RegisteredPartnership",    "Registered civil partnership",                 30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Registered Partnership",    [SupportedCultures.French] = "Partenariat enregistré",  [SupportedCultures.German] = "Eingetragene Partnerschaft"}),
                new("Divorced",                 "Legally divorced",                             40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Divorced",                  [SupportedCultures.French] = "Divorcé(e)",              [SupportedCultures.German] = "Geschieden"               }),
                new("Widowed",                  "Spouse is deceased",                           50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Widowed",                   [SupportedCultures.French] = "Veuf/Veuve",              [SupportedCultures.German] = "Verwitwet"                }),
                new("Separated",                "Separated but not divorced",                   60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Separated",                 [SupportedCultures.French] = "Séparé(e)",               [SupportedCultures.German] = "Getrennt lebend"          }),
                new("Unknown",                  "Not specified or not recorded",                70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown",                   [SupportedCultures.French] = "Non spécifié",            [SupportedCultures.German] = "Nicht angegeben"          })
            ]),

        new CategorySeed(
            Code: "patient.language",
            Description: "Preferred spoken language of the patient",
            SortOrder: 130,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Language",
                [SupportedCultures.French]  = "Langue",
                [SupportedCultures.German]  = "Sprache"
            },
            Items:
            [
                new("German",                   "German",                   10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "German",                     [SupportedCultures.French] = "Allemand",    [SupportedCultures.German] = "Deutsch"      }),
                new("French",                   "French",                   20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "French",                     [SupportedCultures.French] = "Français",    [SupportedCultures.German] = "Französisch"  }),
                new("Italian",                  "Italian",                  30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Italian",                    [SupportedCultures.French] = "Italien",     [SupportedCultures.German] = "Italienisch"  }),
                new("Romansh",                  "Romansh",                  40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Romansh",                    [SupportedCultures.French] = "Romanche",    [SupportedCultures.German] = "Rätoromanisch"}),
                new("English",                  "English",                  50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "English",                    [SupportedCultures.French] = "Anglais",     [SupportedCultures.German] = "Englisch"     }),
                new("Portuguese",               "Portuguese",               60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Portuguese",                 [SupportedCultures.French] = "Portugais",   [SupportedCultures.German] = "Portugiesisch"}),
                new("Spanish",                  "Spanish",                  70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Spanish",                    [SupportedCultures.French] = "Espagnol",    [SupportedCultures.German] = "Spanisch"     }),
                new("Albanian",                 "Albanian",                 80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Albanian",                   [SupportedCultures.French] = "Albanais",    [SupportedCultures.German] = "Albanisch"    }),
                new("SerbianCroatianBosnian",   "Serbian/Croatian/Bosnian", 90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Serbian/Croatian/Bosnian",   [SupportedCultures.French] = "Serbo-croate",[SupportedCultures.German] = "Serbisch/Kroatisch/Bosnisch"}),
                new("Turkish",                  "Turkish",                  100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Turkish",                    [SupportedCultures.French] = "Turc",        [SupportedCultures.German] = "Türkisch"     }),
                new("Other",                    "Other language",           110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Other",                      [SupportedCultures.French] = "Autre",       [SupportedCultures.German] = "Andere"       })
            ]),

        new CategorySeed(
            Code: "patient.nationality",
            Description: "Nationality of the patient. ISO country table planned for future.",
            SortOrder: 140,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Nationality",
                [SupportedCultures.French]  = "Nationalité",
                [SupportedCultures.German]  = "Nationalität"
            },
            Items:
            [
                new("Swiss",            "Swiss national",                       10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Swiss",           [SupportedCultures.French] = "Suisse",          [SupportedCultures.German] = "Schweizer/in"     }),
                new("ForeignNational",  "Non-Swiss national",                   20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Foreign National", [SupportedCultures.French] = "Ressortissant étranger", [SupportedCultures.German] = "Ausländische Person" }),
                new("Unknown",          "Nationality not recorded",             30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown",          [SupportedCultures.French] = "Inconnu",         [SupportedCultures.German] = "Unbekannt"        })
            ]),

        new CategorySeed(
            Code: "patient.insurance_type",
            Description: "Type of health insurance held by the patient",
            SortOrder: 150,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Insurance Type",
                [SupportedCultures.French]  = "Type d'assurance",
                [SupportedCultures.German]  = "Versicherungsart"
            },
            Items:
            [
                new("BasicInsurance",           "Mandatory basic health insurance (LAMal)", 10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Basic Insurance",           [SupportedCultures.French] = "Assurance de base",           [SupportedCultures.German] = "Grundversicherung"        }),
                new("SupplementaryInsurance",   "Optional additional coverage",             20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Supplementary Insurance",   [SupportedCultures.French] = "Assurance complémentaire",    [SupportedCultures.German] = "Zusatzversicherung"       }),
                new("AccidentInsurance",        "Coverage for accidents (UVG/LAA)",         30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Accident Insurance",        [SupportedCultures.French] = "Assurance accidents",         [SupportedCultures.German] = "Unfallversicherung"       }),
                new("MilitaryInsurance",        "Federal military insurance (MV/AM)",       40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Military Insurance",        [SupportedCultures.French] = "Assurance militaire",         [SupportedCultures.German] = "Militärversicherung"      }),
                new("InternationalInsurance",   "Cross-border or travel insurance",         50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "International Insurance",   [SupportedCultures.French] = "Assurance internationale",    [SupportedCultures.German] = "Internationale Versicherung"}),
                new("SelfPay",                  "Patient pays out of pocket",               60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Self-pay",                  [SupportedCultures.French] = "Paiement personnel",          [SupportedCultures.German] = "Selbstzahler"             }),
                new("Unknown",                  "Insurance type not recorded",              70, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown",                   [SupportedCultures.French] = "Inconnu",                     [SupportedCultures.German] = "Unbekannt"                })
            ]),

        new CategorySeed(
            Code: "patient.allergy_severity",
            Description: "Severity level of a patient allergy",
            SortOrder: 160,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Allergy Severity",
                [SupportedCultures.French]  = "Gravité de l'allergie",
                [SupportedCultures.German]  = "Allergieschweregrad"
            },
            Items:
            [
                new("Mild",             "Minor reaction, no treatment needed",          10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Mild",             [SupportedCultures.French] = "Légère",          [SupportedCultures.German] = "Leicht"           }),
                new("Moderate",         "Noticeable reaction, may require treatment",   20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Moderate",         [SupportedCultures.French] = "Modérée",         [SupportedCultures.German] = "Mittel"           }),
                new("Severe",           "Serious reaction requiring treatment",         30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Severe",           [SupportedCultures.French] = "Grave",           [SupportedCultures.German] = "Schwer"           }),
                new("LifeThreatening",  "Anaphylaxis or life-threatening reaction",     40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Life-threatening", [SupportedCultures.French] = "Potentiellement mortel", [SupportedCultures.German] = "Lebensbedrohlich" }),
                new("Unknown",          "Severity not recorded",                        50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown",          [SupportedCultures.French] = "Inconnu",         [SupportedCultures.German] = "Unbekannt"        })
            ])
    ];
}