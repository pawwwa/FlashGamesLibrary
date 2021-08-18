using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibFinal.Model
{
    class GameFIleIO
    {
        public string GameLibPath { get; private set; }

        public void Save(Game game)
        {
            string dir = null;

            if (Directory.GetDirectories(GameLibPath).Any(x => { if (new DirectoryInfo(x).Name == game.ID.ToString()){ dir = x; return true; } return false; }))
            {
                var raw = JsonConvert.DeserializeObject<Game>(File.ReadAllText($@"{dir}\GameInfo.json"));

                if (GetFullImageUri(raw) != game.ImageUri)
                {
                    File.Delete(GetFullImageUri(raw));
                    File.Copy(game.ImageUri, GetFullImageUri(game));

                    game.ImageUri = GetShortImageUri(game);
                }

                game.ExecutableUri = new FileInfo(game.ExecutableUri).Name;
                game.ImageUri = GetShortImageUri(game);
                var data = JsonConvert.SerializeObject(game);
                File.WriteAllText($@"{dir}\GameInfo.json", data);

                game.ImageUri = GetFullImageUri(game);
                game.ExecutableUri = GetFullExecutableUri(game);
            }
            else
            {
                Directory.CreateDirectory($@"{GameLibPath}\{game.ID}");
                File.Copy(game.ImageUri, GetFullImageUri(game));

                if (game.Type == GameType.Flash)
                {
                    File.Copy(game.ExecutableUri, $@"{GameLibPath}\{game.ID}\{new FileInfo(game.ExecutableUri).Name}");

                    game.ExecutableUri = new FileInfo(game.ExecutableUri).Name;
                }

                game.ImageUri = GetShortImageUri(game);
                var data = JsonConvert.SerializeObject(game);

                File.WriteAllText($@"{GameLibPath}\{game.ID}\GameInfo.json", data);

                game.ImageUri = GetFullImageUri(game);
                game.ExecutableUri = GetFullExecutableUri(game);
            }
        }

        public ObservableCollection<Game> GetGames()
        {
            ObservableCollection<Game> data = new ObservableCollection<Game>();


            foreach (var dir in Directory.GetDirectories(GameLibPath))
            {
                var game = JsonConvert.DeserializeObject<Game>(File.ReadAllText($@"{dir}\GameInfo.json"));

                game.ID = Guid.Parse(new DirectoryInfo(dir).Name);

                game.ExecutableUri = $@"{GameLibPath}\{game.ID}\{new FileInfo(game.ExecutableUri).Name}";

                game.ImageUri = GetFullImageUri(game);

                data.Add(game);
            }

            return data;
        }

        public void Delete(Game game)
        {
            try
            {
                foreach (var file in Directory.GetFiles($@"{GameLibPath}\{game.ID}"))
                {
                    File.Delete(file);
                }
                Directory.Delete($@"{GameLibPath}\{game.ID}");

            }
            catch (Exception)
            {

            }
            
        }

        public GameFIleIO()
        {
            GameLibPath = GetDir("GameLibrary") + @"\GameLibrary";
        }

        private string GetImgExt(string img) => new FileInfo(img).Extension;

        private string GetFullImageUri(Game game) => $@"{GameLibPath}\{game.ID}\{game.ID}Img{GetImgExt(game.ImageUri)}";

        private string GetShortImageUri(Game game) => $@"{game.ID}Img{GetImgExt(game.ImageUri)}";

        private string GetFullExecutableUri(Game game) => $@"{GameLibPath}\{game.ID}\{game.ExecutableUri}";

        private string GetShortExecutableUri(Game game) => $@"{game.ExecutableUri}";

        private string GetDir(string name)
        {
            DirectoryInfo tmp = new DirectoryInfo(Environment.CurrentDirectory);

            while (!tmp.GetDirectories().Any(x => { return x.Name == name; }))
            {
                tmp = tmp.Parent;
            }
            return tmp.FullName;
        }
    }
}
