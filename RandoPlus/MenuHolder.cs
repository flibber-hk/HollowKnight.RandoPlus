using MenuChanger;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using MenuChanger.Extensions;
using RandomizerMod.Menu;
using UnityEngine.SceneManagement;

namespace RandoPlus
{
    public class MenuHolder
    {
        internal MenuPage RandoContentEnforcerPage;
        internal MenuElementFactory<GlobalSettings> rceMEF;
        internal VerticalItemPanel rceVIP;

        internal SmallButton JumpToRCEButton;

        private static MenuHolder _instance = null;
        internal static MenuHolder Instance => _instance ?? (_instance = new MenuHolder());

        public static void OnExitMenu(Scene from, Scene to)
        {
            if (from.name == "Menu_Title") _instance = null;
        }

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(Instance.ConstructMenu, Instance.HandleButton);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnExitMenu;
        }

        private bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            JumpToRCEButton = new(landingPage, "Enforce Content");
            JumpToRCEButton.AddHideAndShowEvent(landingPage, RandoContentEnforcerPage);
            button = JumpToRCEButton;
            return true;
        }

        private void ConstructMenu(MenuPage landingPage)
        {
            RandoContentEnforcerPage = new MenuPage("Enforce Content", landingPage);
            rceMEF = new(RandoContentEnforcerPage, RandoPlus.GS);
            rceVIP = new(RandoContentEnforcerPage, new(0, 300), 50f, false, rceMEF.Elements);
        }
    }
}