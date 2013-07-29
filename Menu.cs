using System;
using System.Collections.Generic;

namespace MenuEngine
{
    public class Menu 
    {
        private static List<MenuComponent> _comp = new List<MenuComponent>();
        private static MenuComponent selected;
        private static int selecint;
        private static ConsoleKeyInfo cki;
        private static string title = "";
        private static string caption = "";

        public static void Render() 
        {
            goto prepPhase;

        prepPhase:
    
            selecint = 0;
            goto renPhase;

        renPhase:
       
            Console.Clear();

            for (int i = 0; i < _comp.Count; i++) 
            {
                if (i == selecint)
                    selected = _comp[i];
            }
            
            for(int i = 0; i < _comp.Count; i++) 
            {
                if (_comp[i] == selected)
                    Console.WriteLine("  >  " + _comp[i].Name);
                
                else
                    Console.WriteLine("     " + _comp[i].Name);
            }

            cki = Console.ReadKey();

            switch (cki.Key) 
            {
                case ConsoleKey.UpArrow:
                    if (selecint > 0)
                        selecint--;
                    break;
                case ConsoleKey.DownArrow:
                    if (selecint < _comp.Count)
                        selecint++;
                    break;
                case ConsoleKey.Enter:
                    _comp[selecint].Fire();

                    if (_comp[selecint].willQuitMenu)
                        return;

                    break;
                default:
                    goto renPhase;
            }

            goto renPhase;
        }

        public static void SetTitle(string title) 
        {
            Menu.title = title;
        }

        public static void SetCaption(string caption)
        {
            Menu.caption = caption;
        }

        public static void Add(MenuComponent _comp)
        {
            if (_comp != null) 
            {
                Menu._comp.Add(_comp);
                return;
            }

            Menu._comp.Add(GetComponentByID("ThisParameterWillNotMatchAnyIDInThe_compList:D"));
        }

        private static int NullPos = -1;

        public static MenuComponent GetComponentByID(string IDName) 
        {
            MenuComponent ret;
            foreach (MenuComponent mc in _comp) 
            {
                if (mc.IDName == IDName)
                {
                    ret = mc;
                    return ret;
                }
            }

            NullPos++;
            return new MenuComponent(NullPos.ToString() + "_nullComp", "Null" + NullPos.ToString(), FiredNull);
        }

        public static void Remove(MenuComponent _comp) 
        {
            Menu._comp.Remove(_comp);
        }

        public static void RemoveAll() 
        {
            foreach (MenuComponent mc in _comp)
                _comp.Remove(mc);
        }

        static void FiredNull() 
        {
            Console.WriteLine("Fired a null Component");
            Console.ReadKey();
        }
    }
    
    public class MenuComponent
    {
        public string IDName { get; set; }
        public string Name { get; set; }
        public int Length { get { return Name.Length; } set { Length = value; } }
        public bool willQuitMenu { get; set; }

        public delegate void OnMenuComponentFiredEvent();
        public event OnMenuComponentFiredEvent MenuFiredEvent;

        public MenuComponent(string IDName, string Name, OnMenuComponentFiredEvent FiredEventHandler, bool s = false) 
        {
            this.IDName = IDName;
            this.Name = Name;
            MenuFiredEvent += FiredEventHandler;

            if (s)
                willQuitMenu = true;
        }

        internal void Fire() 
        {
            if (MenuFiredEvent != null) 
            {
                MenuFiredEvent.Invoke();
            }

            else 
            {
                Console.WriteLine("\nFired a null Event");
                Console.ReadKey();
            }
        }
    }
}
