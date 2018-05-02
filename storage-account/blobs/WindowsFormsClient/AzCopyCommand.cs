using System.IO;
namespace WindowsFormsApplication1
{
    public class AzCopyCommand
    {
        const string azCopyCommand = "AzCopy /Source:$Source$ /Dest:$Dest$ /DestSAS:\"$DestSAS$\" /pattern:\"$FileName$\" /s";
        public string Generate(string source, string destination, string DestinationSAS)
        {
            var sourcelocation = Path.GetDirectoryName(source);
            var fileName = Path.GetFileName(source);
            return azCopyCommand.Replace("$Source$", sourcelocation).Replace("$Dest$", destination).Replace("$DestSAS$", DestinationSAS).Replace("$FileName$", fileName);
        }

        public string GenerateAzCommandWithoutSource(string destination, string DestinationSAS)
        {
            return azCopyCommand.Replace("$Source$", "<Source Location>").Replace("$Dest$", destination).Replace("$DestSAS$", DestinationSAS).Replace("$FileName$", "*.*");
        }
    }
}