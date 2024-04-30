using System.IO;

namespace Squire.Helpers
{
   static class DirectorySurfer
   {
      internal static string GetDatabasePath()
      {
         string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SQuire";
         if(!Directory.Exists(path)) { Directory.CreateDirectory(path); }
         return path;
      }
   }
}
