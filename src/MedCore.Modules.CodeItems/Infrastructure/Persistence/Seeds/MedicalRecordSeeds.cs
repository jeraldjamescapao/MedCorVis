namespace MedCore.Modules.CodeItems.Infrastructure.Persistence.Seeds;

using MedCore.Common.Localization;
using MedCore.Modules.CodeItems.Infrastructure.Persistence;

internal static class MedicalRecordSeeds
{
    public static List<CategorySeed> All =>
    [
        new CategorySeed(
            Code: "medical_record.type",
            Description: "Types of medical record documents",
            SortOrder: 220,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Record Type",
                [SupportedCultures.French]  = "Type de dossier",
                [SupportedCultures.German]  = "Dokumentart"
            },
            Items:
            [
                new("ConsultationNote",     "Notes from a doctor consultation",         10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Consultation Note",     [SupportedCultures.French] = "Note de consultation",    [SupportedCultures.German] = "Konsultationsnotiz"       }),
                new("Diagnosis",            "Formal diagnosis record",                  20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Diagnosis",             [SupportedCultures.French] = "Diagnostic",              [SupportedCultures.German] = "Diagnose"                 }),
                new("Prescription",         "Medication prescription",                  30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Prescription",          [SupportedCultures.French] = "Ordonnance",              [SupportedCultures.German] = "Rezept"                   }),
                new("LaboratoryResult",     "Result from a laboratory test",            40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Laboratory Result",     [SupportedCultures.French] = "Résultat de laboratoire", [SupportedCultures.German] = "Laborergebnis"            }),
                new("ImagingReport",        "Report from imaging such as MRI or X-ray", 50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Imaging Report",        [SupportedCultures.French] = "Rapport d'imagerie",      [SupportedCultures.German] = "Bildgebungsbericht"       }),
                new("DischargeSummary",     "Summary issued at patient discharge",      60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Discharge Summary",     [SupportedCultures.French] = "Résumé de sortie",        [SupportedCultures.German] = "Entlassungsbericht"       }),
                new("ReferralLetter",       "Letter referring patient to a specialist", 70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Referral Letter",       [SupportedCultures.French] = "Lettre d'adressage",      [SupportedCultures.German] = "Überweisungsschreiben"   }),
                new("VaccinationRecord",    "Record of vaccinations administered",      80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Vaccination Record",    [SupportedCultures.French] = "Carnet de vaccination",   [SupportedCultures.German] = "Impfausweis"              }),
                new("AllergyRecord",        "Documented patient allergies",             90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Allergy Record",        [SupportedCultures.French] = "Dossier d'allergies",     [SupportedCultures.German] = "Allergieakte"             }),
                new("OperationReport",      "Report of a surgical operation",           100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Operation Report",      [SupportedCultures.French] = "Rapport opératoire",      [SupportedCultures.German] = "Operationsbericht"        }),
                new("NursingNote",          "Notes recorded by nursing staff",          110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Nursing Note",          [SupportedCultures.French] = "Note infirmière",         [SupportedCultures.German] = "Pflegenotiz"              }),
                new("ProgressNote",         "Ongoing progress notes by the care team",  120, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Progress Note",         [SupportedCultures.French] = "Note d'évolution",        [SupportedCultures.German] = "Verlaufsnotiz"            })
            ]),

        new CategorySeed(
            Code: "medical_record.status",
            Description: "Status of a medical record document",
            SortOrder: 230,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Record Status",
                [SupportedCultures.French]  = "Statut du dossier",
                [SupportedCultures.German]  = "Dokumentstatus"
            },
            Items:
            [
                new("Draft",     "Document is a draft, not yet finalized", 10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Draft",    [SupportedCultures.French] = "Brouillon",   [SupportedCultures.German] = "Entwurf"      }),
                new("Final",     "Document is finalized and signed",        20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Final",    [SupportedCultures.French] = "Final",       [SupportedCultures.German] = "Abgeschlossen"}),
                new("Amended",   "Document has been amended after signing", 30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Amended",  [SupportedCultures.French] = "Modifié",     [SupportedCultures.German] = "Geändert"     }),
                new("Cancelled", "Document has been cancelled",             40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Cancelled",[SupportedCultures.French] = "Annulé",      [SupportedCultures.German] = "Storniert"    }),
                new("Archived",  "Document has been archived",              50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Archived", [SupportedCultures.French] = "Archivé",     [SupportedCultures.German] = "Archiviert"   })
            ]),

        new CategorySeed(
            Code: "medical_record.visibility",
            Description: "Who can see this medical record",
            SortOrder: 240,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Record Visibility",
                [SupportedCultures.French]  = "Visibilité du dossier",
                [SupportedCultures.German]  = "Dokumentsichtbarkeit"
            },
            Items:
            [
                new("Internal",         "Visible to clinical staff only",           10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Internal",           [SupportedCultures.French] = "Interne",                 [SupportedCultures.German] = "Intern"                   }),
                new("SharedWithPatient","Visible to the patient",                   20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Shared with Patient", [SupportedCultures.French] = "Partagé avec le patient", [SupportedCultures.German] = "Mit Patient geteilt"      }),
                new("Restricted",       "Access restricted to specific staff only", 30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Restricted",          [SupportedCultures.French] = "Restreint",               [SupportedCultures.German] = "Eingeschränkt"            })
            ]),

        new CategorySeed(
            Code: "diagnosis.severity",
            Description: "Severity level of a diagnosis",
            SortOrder: 250,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Diagnosis Severity",
                [SupportedCultures.French]  = "Gravité du diagnostic",
                [SupportedCultures.German]  = "Diagnoseschweregrad"
            },
            Items:
            [
                new("Mild",     "Minor condition",                      10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Mild",     [SupportedCultures.French] = "Légère",   [SupportedCultures.German] = "Leicht"   }),
                new("Moderate", "Moderate condition requiring care",    20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Moderate", [SupportedCultures.French] = "Modérée",  [SupportedCultures.German] = "Mittel"   }),
                new("Severe",   "Serious condition",                    30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Severe",   [SupportedCultures.French] = "Grave",    [SupportedCultures.German] = "Schwer"   }),
                new("Critical", "Life-threatening condition",           40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Critical", [SupportedCultures.French] = "Critique", [SupportedCultures.German] = "Kritisch" }),
                new("Unknown",  "Severity not yet determined",          50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Unknown",  [SupportedCultures.French] = "Inconnu",  [SupportedCultures.German] = "Unbekannt"})
            ]),

        new CategorySeed(
            Code: "medication.route",
            Description: "Route of administration for a medication",
            SortOrder: 260,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Medication Route",
                [SupportedCultures.French]  = "Voie d'administration",
                [SupportedCultures.German]  = "Verabreichungsweg"
            },
            Items:
            [
                new("Oral",             "Taken by mouth",                       10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Oral",           [SupportedCultures.French] = "Oral",            [SupportedCultures.German] = "Oral"             }),
                new("Intravenous",      "Injected into a vein",                 20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Intravenous",    [SupportedCultures.French] = "Intraveineux",    [SupportedCultures.German] = "Intravenös"       }),
                new("Intramuscular",    "Injected into a muscle",               30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Intramuscular",  [SupportedCultures.French] = "Intramusculaire", [SupportedCultures.German] = "Intramuskulär"    }),
                new("Subcutaneous",     "Injected under the skin",              40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Subcutaneous",   [SupportedCultures.French] = "Sous-cutané",     [SupportedCultures.German] = "Subkutan"         }),
                new("Topical",          "Applied to the skin surface",          50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Topical",        [SupportedCultures.French] = "Topique",         [SupportedCultures.German] = "Topisch"          }),
                new("Inhalation",       "Inhaled through the lungs",            60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Inhalation",     [SupportedCultures.French] = "Inhalation",      [SupportedCultures.German] = "Inhalation"       }),
                new("Rectal",           "Administered rectally",                70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Rectal",         [SupportedCultures.French] = "Rectal",          [SupportedCultures.German] = "Rektal"           }),
                new("Sublingual",       "Dissolved under the tongue",           80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Sublingual",     [SupportedCultures.French] = "Sublingual",      [SupportedCultures.German] = "Sublingual"       }),
                new("Ophthalmic",       "Applied to the eye",                   90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Ophthalmic",     [SupportedCultures.French] = "Ophtalmique",     [SupportedCultures.German] = "Ophthalmisch"     }),
                new("Otic",             "Applied to the ear",                   100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Otic",           [SupportedCultures.French] = "Otique",          [SupportedCultures.German] = "Otisch"           }),
                new("Nasal",            "Applied to the nasal passage",         110, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Nasal",          [SupportedCultures.French] = "Nasal",           [SupportedCultures.German] = "Nasal"            }),
                new("Other",            "Other route of administration",        120, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Other",          [SupportedCultures.French] = "Autre",           [SupportedCultures.German] = "Andere"           })
            ]),

        new CategorySeed(
            Code: "medication.frequency",
            Description: "How often a medication is taken",
            SortOrder: 270,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Medication Frequency",
                [SupportedCultures.French]  = "Fréquence de la médication",
                [SupportedCultures.German]  = "Medikamentenhäufigkeit"
            },
            Items:
            [
                new("OnceDaily",        "Once per day",                 10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Once Daily",          [SupportedCultures.French] = "Une fois par jour",       [SupportedCultures.German] = "Einmal täglich"       }),
                new("TwiceDaily",       "Twice per day",                20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Twice Daily",         [SupportedCultures.French] = "Deux fois par jour",      [SupportedCultures.German] = "Zweimal täglich"      }),
                new("ThreeTimesDaily",  "Three times per day",          30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Three Times Daily",   [SupportedCultures.French] = "Trois fois par jour",     [SupportedCultures.German] = "Dreimal täglich"      }),
                new("FourTimesDaily",   "Four times per day",           40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Four Times Daily",    [SupportedCultures.French] = "Quatre fois par jour",    [SupportedCultures.German] = "Viermal täglich"      }),
                new("EveryMorning",     "Every morning",                50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Every Morning",       [SupportedCultures.French] = "Chaque matin",            [SupportedCultures.German] = "Jeden Morgen"         }),
                new("EveryEvening",     "Every evening",                60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Every Evening",       [SupportedCultures.French] = "Chaque soir",             [SupportedCultures.German] = "Jeden Abend"          }),
                new("AsNeeded",         "Only when needed (PRN)",       70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "As Needed",           [SupportedCultures.French] = "Si besoin",               [SupportedCultures.German] = "Bei Bedarf"           }),
                new("Weekly",           "Once per week",                80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Weekly",              [SupportedCultures.French] = "Hebdomadaire",            [SupportedCultures.German] = "Wöchentlich"          }),
                new("Monthly",          "Once per month",               90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Monthly",             [SupportedCultures.French] = "Mensuel",                 [SupportedCultures.German] = "Monatlich"            }),
                new("OneTimeOnly",      "Single dose only",             100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "One Time Only",       [SupportedCultures.French] = "Dose unique",             [SupportedCultures.German] = "Einmalig"             })
            ]),

        new CategorySeed(
            Code: "lab.result_status",
            Description: "Processing status of a laboratory result",
            SortOrder: 280,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Lab Result Status",
                [SupportedCultures.French]  = "Statut du résultat de laboratoire",
                [SupportedCultures.German]  = "Laborergebnisstatus"
            },
            Items:
            [
                new("Ordered",      "Test has been ordered",                    10, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Ordered",    [SupportedCultures.French] = "Commandé",    [SupportedCultures.German] = "Angeordnet"   }),
                new("Collected",    "Sample has been collected",                20, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Collected",  [SupportedCultures.French] = "Collecté",    [SupportedCultures.German] = "Entnommen"    }),
                new("InProgress",   "Sample is being analysed",                 30, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "In Progress",[SupportedCultures.French] = "En cours",    [SupportedCultures.German] = "In Bearbeitung"}),
                new("Completed",    "Result is available",                      40, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Completed",  [SupportedCultures.French] = "Terminé",     [SupportedCultures.German] = "Abgeschlossen"}),
                new("Corrected",    "Result has been corrected after release",  50, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Corrected",  [SupportedCultures.French] = "Corrigé",     [SupportedCultures.German] = "Korrigiert"   }),
                new("Cancelled",    "Test was cancelled",                       60, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Cancelled",  [SupportedCultures.French] = "Annulé",      [SupportedCultures.German] = "Storniert"    })
            ]),

        new CategorySeed(
            Code: "document.type",
            Description: "Type of document uploaded or attached to a patient record",
            SortOrder: 290,
            Labels: new(StringComparer.OrdinalIgnoreCase)
            {
                [SupportedCultures.English] = "Document Type",
                [SupportedCultures.French]  = "Type de document",
                [SupportedCultures.German]  = "Dokumententyp"
            },
            Items:
            [
                new("IdentityDocument",     "Passport, ID card, or residence permit",   10,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Identity Document",      [SupportedCultures.French] = "Document d'identité",     [SupportedCultures.German] = "Ausweisdokument"          }),
                new("InsuranceCard",        "Health insurance card",                    20,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Insurance Card",         [SupportedCultures.French] = "Carte d'assurance",       [SupportedCultures.German] = "Versicherungskarte"       }),
                new("Referral",             "Referral document from another provider",  30,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Referral",               [SupportedCultures.French] = "Orientation médicale",    [SupportedCultures.German] = "Überweisung"              }),
                new("ConsentForm",          "Patient consent form",                     40,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Consent Form",           [SupportedCultures.French] = "Formulaire de consentement",[SupportedCultures.German] = "Einwilligungsformular"  }),
                new("MedicalCertificate",   "Medical certificate issued by a doctor",   50,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Medical Certificate",    [SupportedCultures.French] = "Certificat médical",      [SupportedCultures.German] = "Ärztliches Attest"       }),
                new("LabReport",            "Laboratory report document",               60,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Lab Report",             [SupportedCultures.French] = "Rapport de laboratoire",  [SupportedCultures.German] = "Laborbericht"             }),
                new("ImagingReport",        "Imaging report such as MRI or X-ray",      70,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Imaging Report",         [SupportedCultures.French] = "Rapport d'imagerie",      [SupportedCultures.German] = "Bildgebungsbericht"       }),
                new("DischargeLetter",      "Letter issued at patient discharge",        80,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Discharge Letter",       [SupportedCultures.French] = "Lettre de sortie",        [SupportedCultures.German] = "Entlassungsbrief"         }),
                new("Prescription",         "Medication prescription document",         90,  new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Prescription",           [SupportedCultures.French] = "Ordonnance",              [SupportedCultures.German] = "Rezept"                   }),
                new("AdministrativeDocument","General administrative document",         100, new(StringComparer.OrdinalIgnoreCase) { [SupportedCultures.English] = "Administrative Document",[SupportedCultures.French] = "Document administratif",  [SupportedCultures.German] = "Verwaltungsdokument"      })
            ])
    ];
}