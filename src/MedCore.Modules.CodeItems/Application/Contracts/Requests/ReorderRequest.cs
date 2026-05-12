namespace MedCore.Modules.CodeItems.Application.Contracts.Requests;

using System.ComponentModel.DataAnnotations;

public sealed record ReorderRequest(
    [Required] List<ReorderEntry> Entries);

public sealed record ReorderEntry(
    [Required] [Range(1, int.MaxValue)] long Id, 
    [Required] [Range(1, int.MaxValue)] int SortOrder);