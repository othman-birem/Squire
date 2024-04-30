using Newtonsoft.Json;
using Squire.UI.Controls;
using System.IO;

namespace Squire.Helpers
{
   class Discussion
   {
      public string Title { get; set; } = "";
      public List<Message> Messages { get; set; } = new();
      public DateTime Date { get; set; } = DateTime.Now;
      public static readonly string PATH_TO_COLLECTION = DirectorySurfer.GetDatabasePath() + "\\Discussions.json";


      public Discussion(QueryPanel panel) 
      {
         Title = panel.Text;
         Messages.Add(new Message { Text = panel.Text, peer = panel.Peer });
      }
      public Discussion() { }

      internal void Save()
      {
         if (!File.Exists(PATH_TO_COLLECTION))
         {
            FileStream stream = File.Create(PATH_TO_COLLECTION);
            stream.Close();
         }
         using StreamReader reader = new(PATH_TO_COLLECTION);
         string? data = reader.ReadToEnd();
         reader.Close();
         List<Discussion>? discussions = JsonConvert.DeserializeObject<List<Discussion>>(data);
         discussions ??= new List<Discussion>();
         discussions.Add(this);
         using StreamWriter writer = new(PATH_TO_COLLECTION);
         data = JsonConvert.SerializeObject(discussions, Formatting.Indented);
         writer.Write(data);
      }
      internal void AddMessage(QueryPanel panel)
      {
         Title = panel.Text;
         Messages.Add(new Message { Text = panel.Text, peer = panel.Peer });
      }
   }

   class Message
   {
      public string Text { get; set; }
      public string peer { get; set; }
   }
}
