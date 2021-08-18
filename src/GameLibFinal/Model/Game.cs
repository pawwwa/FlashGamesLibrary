using GameLibFinal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibFinal.Model
{
    enum GameType
    {
        Flash,
        Exe
    }

    class Game : BasePropertyNotify
    {
        public Game()
        {
            ID = Guid.NewGuid();
        }

        private GameType type;
        public GameType Type
        {
            get => type;
            set { type = value; Notify(); }
        }

        private Guid id;
        public Guid ID
        {
            get => id;
            set { id = value; Notify(); }
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; Notify(); }
        }

        private string description;
        public string Description
        {
            get => description;
            set { description = value; Notify(); }
        }

        private string imageUri;
        public string ImageUri
        {
            get => imageUri;
            set { imageUri = value; Notify(); }
        }

        private string executableUri;
        public string ExecutableUri
        {
            get => executableUri;
            set { executableUri = value; Notify(); }
        }
    }
}
