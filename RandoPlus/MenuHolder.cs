using MenuChanger;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using MenuChanger.Extensions;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace RandoPlus
{
    public class MenuHolder
    {
        internal MenuPage RandoPlusMenuPage;
        internal MenuElementFactory<GlobalSettings> rpMEF;
        internal VerticalItemPanel rpVIP;

        internal SmallButton JumpToRPButton;

        private static MenuHolder _instance = null;
        internal static MenuHolder Instance => _instance ?? (_instance = new MenuHolder());

        public static void OnExitMenu()
        {
            _instance = null;
        }

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(Instance.ConstructMenu, Instance.HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            JumpToRPButton = new(landingPage, Localize("RandoPlus"));
            JumpToRPButton.AddHideAndShowEvent(landingPage, RandoPlusMenuPage);
            SetTopLevelButtonColor();

            button = JumpToRPButton;
            return true;
        }

        private void SetTopLevelButtonColor()
        {
            if (JumpToRPButton != null)
            {
                JumpToRPButton.Text.color = RandoPlus.GS.Any ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private void ConstructMenu(MenuPage landingPage)
        {
            RandoPlusMenuPage = new MenuPage(Localize("RandoPlus"), landingPage);
            rpMEF = new(RandoPlusMenuPage, RandoPlus.GS);
            foreach (IValueElement e in rpMEF.Elements)
            {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            rpVIP = new(RandoPlusMenuPage, new(0, 300), 50f, true, rpMEF.Elements);
            Localize(rpMEF);
        }
    }
}