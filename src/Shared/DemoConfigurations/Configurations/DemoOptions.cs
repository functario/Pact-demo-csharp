using System.ComponentModel.DataAnnotations;
using PactNet;

namespace DemoConfigurations.Configurations;

public class DemoOptions
{
    public const string Section = "DemoOptions";

    [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(PactFolder)} is required")]
    public string PactFolder { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(AuthenticationKey)} is required")]
    public string AuthenticationKey { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(DemoCase)} is required")]
    public DemoCases DemoCase { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(PactLogLevel)} is required")]
    public PactLogLevel PactLogLevel { get; set; }
}
