using ControlzEx.Theming;
using GameLibFinal.Infrastructure;
using GameLibFinal.Model;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace GameLibFinal.ViewModel
{
    class MainViewModel : BasePropertyNotify
    {

        #region Themes

        public List<string> SideColors { get; set; } = new List<string>()
        {
            "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald",
            "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta",
            "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve",
            "Taupe", "Sienna"
        };

        private string sideColorChosen;
        public string SideColorChosen
        {
            get => sideColorChosen;
            set { sideColorChosen = value; Notify(); ChangeTheme(); }
        }

        public List<string> MainColors { get; set; } = new List<string>()
        {
            "Light", "Dark"
        };

        private string mainColorChosen;
        public string MainColorChosen
        {
            get => mainColorChosen;
            set { mainColorChosen = value; Notify(); ChangeTheme(); }
        }

        private void ChangeTheme()
        {
            ThemeManager.Current.ChangeTheme(App.Current, $"{mainColorChosen}.{sideColorChosen}");
        }

        #endregion

        #region SettingMenuButton
        bool isContextMenuEnabled = false;
        public bool IsContextMenuEnabled
        {
            get => isContextMenuEnabled;
            set { isContextMenuEnabled = value; Notify(); }
        }

        public ICommand SettingButtonOpenContextMenu { get; set; }
        #endregion

        #region ThemePicker
        bool isThemePickerOpen = false;
        public bool IsThemePickerOpen
        {
            get => isThemePickerOpen;
            set { isThemePickerOpen = value; Notify(); }
        }

        public ICommand OpenThemePicker { get; set; }
        #endregion

        #region LanguagePicker
        bool isLanguagePickerOpen = false;
        public bool IsLanguagePickerOpen
        {
            get => isLanguagePickerOpen;
            set { isLanguagePickerOpen = value; Notify(); }
        }

        public ICommand OpenLanguagePicker { get; set; }

        public ICommand ChangeLanguageRUS { get; set; }

        public ICommand ChangeLanguageENG { get; set; }
        #endregion

        #region GameListView
        private ObservableCollection<Game> games;
        public ObservableCollection<Game> Games
        {
            get => games;
            set
            {
                games = value;
                Notify();
            }
        }

        private Game currentGame;
        public Game CurrentGame
        {
            get => currentGame;
            set
            {
                currentGame = value;
                Notify();
            }
        }

        private int listViewSize = 320;
        public int ListViewSize
        {
            get => listViewSize;
            set { listViewSize = value; Notify(); ImageViewSize = ListViewSize / 3; }
        }

        private int imageViewSize;
        public int ImageViewSize
        {
            get => imageViewSize;
            set { imageViewSize = value; Notify(); }
        }

        public ICommand RunGame { get; set; }

        public ICommand DeleteGame { get; set; }

        public ICommand EditImage { get; set; }

        #endregion

        #region AddGameView

        private bool isGameAddViewOpen;
        public bool IsGameAddViewOpen
        {
            get => isGameAddViewOpen;
            set { isGameAddViewOpen = value; Notify(); }
        }

        public ICommand OpenGameAddView { get; set; }

        private string gameName;
        public string GameName
        {
            get => gameName;
            set { gameName = value; Notify(); }
        }

        private string gameDescription;
        public string GameDescription
        {
            get => gameDescription;
            set { gameDescription = value; Notify(); }
        }

        private string gameImage = Environment.CurrentDirectory + @"\default.png";
        public string GameImage
        {
            get => gameImage;
            set { gameImage = value; Notify(); }
        }

        private string executableUri = "";
        public string ExecutableUri
        {
            get => executableUri;
            set { executableUri = value; Notify(); if (ExecutableUri != null) { IsFinishReady = ExecutableUri.Length > 0; } }
        }

        private bool isFinishReady;
        public bool IsFinishReady
        {
            get => isFinishReady;

            set { isFinishReady = value; Notify(); }
        }

        public Array PossibleTypes { get; set; } = Enum.GetValues(typeof(GameType));

        private object selectedType;
        public object SelectedType
        {
            get => selectedType;
            set { selectedType = value; Notify(); TypeHelpString = TypeHelpString; }
        }

        private string typeHelpString;
        public string TypeHelpString
        {
            get
            {
                if (Settings.Deserialize().Localization == "en-US")
                {
                    if (selectedType.ToString() == "Flash")
                    {
                        return ".swf file required";
                    }
                    else
                    {
                        return ".exe file required";
                    }
                }
                else
                {
                    if (selectedType.ToString() == "Flash")
                    {
                        return "требуется .swf файл";
                    }
                    else
                    {
                        return "требуется .exe файл";
                    }
                }
            }
            set { typeHelpString = value; Notify(); }
        }

        public ICommand ChooseFile { get; set; }

        public ICommand SelectImage { get; set; }

        public ICommand FinishGameAdd { get; set; }
        #endregion

        #region Sort
        private bool sortFlag = false;
        public bool SortFlag
        {
            get => sortFlag;

            set { sortFlag = value; Notify(); }
        }

        public ICommand Sort { get; set; }
        #endregion

        private IDialogCoordinator dialogCoordinator;

        private GameFIleIO gameFileOperations = new GameFIleIO();

        public MainViewModel(IDialogCoordinator instance)
        {
            this.dialogCoordinator = instance;

            Games = new ObservableCollection<Game>();
            Games = gameFileOperations.GetGames();

            if (Games.Count > 0)
                CurrentGame = Games[0];

            selectedType = PossibleTypes.GetValue(0);
            InitCommands();
        }

        private void InitCommands()
        {
            SettingButtonOpenContextMenu = new Command(x =>
            {
                IsContextMenuEnabled = true;
            });

            OpenThemePicker = new Command(x =>
            {
                IsThemePickerOpen = true;
            });

            OpenLanguagePicker = new Command(x =>
            {
                IsLanguagePickerOpen = true;
            });

            ChangeLanguageRUS = new Command(x =>
            {
                Settings.Serialize(new Settings() { Localization = "ru-RU" });

                App.Current.Shutdown();
                System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
            });

            ChangeLanguageENG = new Command(x =>
            {
                Settings.Serialize(new Settings() { Localization = "en-US" });

                App.Current.Shutdown();
                System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
            });

            OpenGameAddView = new Command(x =>
            {
                IsFinishReady = false;
                IsGameAddViewOpen = true;
            });

            FinishGameAdd = new Command(x =>
            {
                Game tmp = new Game()
                {
                    Name = GameName,
                    ImageUri = GameImage,
                    Type = (GameType)Enum.Parse(typeof(GameType), SelectedType.ToString(), true),
                    Description = GameDescription,
                    ExecutableUri = ExecutableUri
                };
                Games.Add(tmp);

                gameFileOperations.Save(tmp);

                IsGameAddViewOpen = false;
                GameName = null;
                GameImage = Environment.CurrentDirectory + @"\default.png";
                GameDescription = null;
                ExecutableUri = null;
            });

            ChooseFile = new Command(x =>
            {
                OpenFileDialog dlg = new OpenFileDialog();

                if ((GameType)Enum.Parse(typeof(GameType), SelectedType.ToString(), true) == GameType.Flash)
                    dlg.Filter = "Flash Files (*.swf)|*.swf";
                else
                    dlg.Filter = "Exe Files (*.exe)|*.exe";

                if (dlg.ShowDialog() == true)
                {
                    ExecutableUri = dlg.FileName;
                }
            });

            SelectImage = new Command(x =>
            {
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.Filter = "All image files |*.*";

                if (dlg.ShowDialog() == true)
                {
                    GameImage = dlg.FileName;
                }
            });

            EditImage = new Command(x =>
            {
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.Filter = "All image files |*.*";

                if (dlg.ShowDialog() == true)
                {
                    CurrentGame.ImageUri = dlg.FileName;
                }
            });

            RunGame = new Command(x =>
            {

                if (CurrentGame != null)
                {
                    string path = CurrentGame.ExecutableUri;

                    if (CurrentGame.Type == GameType.Flash)
                    {
                        Process.Start(gameFileOperations.GameLibPath + @"\flashplayer.exe", path);
                    }
                    else
                    {
                        Process.Start(path);
                    }
                }
            });

            DeleteGame = new Command(x =>
            {
                gameFileOperations.Delete(CurrentGame);
                Games.Remove(CurrentGame);
            });

            Sort = new Command(x =>
            {

                SortFlag = !SortFlag;

                if (SortFlag)
                {
                    Games = new ObservableCollection<Game>(Games.OrderBy(i => i.Name));
                }
                else
                {
                    Games = new ObservableCollection<Game>(Games.OrderByDescending(i => i.Name));
                }
            });
        }

    }
}
