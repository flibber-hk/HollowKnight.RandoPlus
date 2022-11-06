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

        internal static MenuHolder Instance { get; private set; }

        public static void OnExitMenu()
        {
            Instance = null;
        }

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            button = Instance.JumpToRPButton;
            return true;
        }

        private void SetTopLevelButtonColor()
        {
            if (JumpToRPButton != null)
            {
                JumpToRPButton.Text.color = RandoPlus.GS.Any ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private static void ConstructMenu(MenuPage landingPage) => Instance = new(landingPage);

        private MenuHolder(MenuPage landingPage)
        {
            RandoPlusMenuPage = new MenuPage(Localize("RandoPlus"), landingPage);
            rpMEF = new(RandoPlusMenuPage, RandoPlus.GS);
            foreach (IValueElement e in rpMEF.Elements)
            {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            rpVIP = new(RandoPlusMenuPage, new(0, 300), 50f, true, rpMEF.Elements);
            Localize(rpMEF);

            JumpToRPButton = new(landingPage, Localize("RandoPlus"));
            JumpToRPButton.AddHideAndShowEvent(landingPage, RandoPlusMenuPage);
            SetTopLevelButtonColor();
        }

        internal void ResetMenu()
        {
            rpMEF.SetMenuValues(RandoPlus.GS);
            SetTopLevelButtonColor();
        }
    }
}