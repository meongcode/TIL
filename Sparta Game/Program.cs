using System;
using System.Diagnostics;

namespace Sparta_Game
{
    // 캐릭터 클래스: 게임 플레이어의 캐릭터 정보를 저장합니다.
    public class Character
    {
        public string Name { get; private set; }
        public string Job { get; private set; }
        public int Level { get; private set; }

        // 기본 능력치
        private int baseAtk;
        private int baseDef;

        // 현재 능력치 (아이템 능력치 포함)
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int Gold { get; private set; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            baseAtk = atk;
            baseDef = def;
            Hp = hp;
            Gold = gold;
            UpdateCurrentStats(); // 현재 능력치를 기본 능력치로 초기화
        }

        // 현재 능력치 업데이트 메소드
        public void UpdateCurrentStats(int additionalAtk = 0, int additionalDef = 0)
        {
            Atk = baseAtk + additionalAtk;
            Def = baseDef + additionalDef;
        }

        // 캐릭터 정보 표시
        public void DisplayInfo()
        {
            Console.WriteLine($"Lv.{Level}");
            Console.WriteLine($"{Name}({Job})");
            Console.WriteLine($"공격력: {baseAtk} {(Atk > baseAtk ? $"(+{Atk - baseAtk})" : "")}");
            Console.WriteLine($"방어력: {baseDef} {(Def > baseDef ? $"(+{Def - baseDef})" : "")}");
            Console.WriteLine($"체력: {Hp}");
            Console.WriteLine($"Gold: {Gold} G");
        }

    }

    // 아이템 클래스: 게임 내 아이템 정보를 저장합니다.
    public class Item
    {
        public string ItemName { get; private set; }
        public ItemType Type { get; private set; }
        public int ItemAttack { get; private set; }
        public int ItemDefense { get; private set; }
        public int Price { get; private set; }
        public string ItemInfo { get; private set; }
        public bool IsEquipped { get; set; }

        // 아이템 생성자: 새로운 아이템을 생성할 때 사용됩니다.
        public Item(string itemName, ItemType type, int itemAttack, int itemDefense, int price, string itemInfo)
        {
            ItemName = itemName;
            Type = type;
            ItemAttack = itemAttack;
            ItemDefense = itemDefense;
            Price = price;
            ItemInfo = itemInfo;
        }

        // 아이템 정보를 출력합니다.
        public void Display()
        {
            string equippedStatus = IsEquipped ? "[E]" : "   "; // 장착 상태 표시
            Console.WriteLine($"{equippedStatus} {ItemName}");
            Console.WriteLine($"- 종류: {Type}");
            Console.WriteLine($"- 공격력: {ItemAttack}");
            Console.WriteLine($"- 방어력: {ItemDefense}");
            Console.WriteLine($"- 가격: {Price}원");
            Console.WriteLine($"- 설명: {ItemInfo}");
            Console.WriteLine();
        }
    }

    // 아이템 유형을 정의하는 열거형
    public enum ItemType
    {
        Weapon,
        Armor,
    }

    internal class Program
    {
        private static Character player;
        private static List<Item> items = new List<Item>();

        static void Main(string[] args)
        {
            InitializeGameData();
            DisplayGameIntro();
        }

        // 게임 데이터를 초기화합니다.
        static void InitializeGameData()
        {
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
            items.Add(new Item("낡은 검", ItemType.Weapon, 2, 0, 200, "쉽게 볼 수 있는 낡은 검 입니다."));
            items.Add(new Item("무쇠갑옷", ItemType.Armor, 0, 5, 300, "무쇠로 만들어져 튼튼한 갑옷입니다."));
            items.Add(new Item("날렵한 단검", ItemType.Weapon, 3, 0, 500, "가볍고 강하게 만들어진 단검입니다."));
            items.Add(new Item("엘프 가죽 옷", ItemType.Armor, 0, 5, 800, "엘프가 자주 입는 가벼운 가죽 옷입니다."));
        }

        // 게임의 시작 화면을 표시합니다.
        static void DisplayGameIntro()
        {
            Console.Clear();
            Console.Title = "스파르타 던전";
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();
                    break;
                case 2:
                    DisplayInventory();
                    break;
            }
        }

        // 플레이어의 정보를 표시합니다.
        static void DisplayMyInfo()
        {
            Console.Clear();
            Console.WriteLine("상태보기");
            player.DisplayInfo();
            Console.WriteLine("0. 나가기");

            if (CheckValidInput(0, 0) == 0)
            {
                DisplayGameIntro();
            }
        }

        // 인벤토리를 표시합니다.
        static void DisplayInventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("[아이템 목록]");
            int itemNumber = 1; // 아이템 번호 초기화
            foreach (var item in items)
            {
                string equippedStatus = item.IsEquipped ? "[E] " : "    ";
                Console.WriteLine($"- {itemNumber}. {equippedStatus}{item.ItemName} | {ItemStat(item)} | {item.ItemInfo}");
                itemNumber++;
            }

            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 1);
            switch (input)
            {
                case 1:
                    ManageEquipment();
                    break;
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        // 장착 관리 창
        static void ManageEquipment()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("장착 관리");
                Console.WriteLine("[아이템 목록]");

                int itemNumber = 1; // 아이템 번호 초기화
                foreach (var item in items)
                {
                    string equippedStatus = item.IsEquipped ? "[E] " : "    ";
                    Console.WriteLine($"- {itemNumber}. {equippedStatus}{item.ItemName} | {ItemStat(item)} | {item.ItemInfo}");
                    itemNumber++;
                }

                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                int input = CheckValidInput(0, items.Count); // 아이템 개수를 기준으로 입력값 검증

                if (input == 0)
                {
                    break; // '0. 나가기' 선택 시 while 루프를 종료합니다.
                }
                else if (input >= 1 && input <= items.Count)
                {
                    ChangeEquipStatus(input); // 아이템 번호 선택 시 장착 상태 변경
                                              // 장착 상태 변경 후에는 장착 관리 창을 유지합니다.
                    
                }
            }
            DisplayInventory(); // 장착 관리를 빠져나오면 인벤토리를 다시 표시
        }




        static string ItemStat(Item item)
        {
            if (item.Type == ItemType.Weapon)
            {
                return $"공격력 +{item.ItemAttack}";
            }
            else if (item.Type == ItemType.Armor)
            {
                return $"방어력 +{item.ItemDefense}";
            }
            return "";
        }


        // 아이템의 장착 상태를 변경합니다.
        static void ChangeEquipStatus(int itemNumber)
        {
            if (itemNumber >= 1 && itemNumber <= items.Count)
            {
                Item selected = items[itemNumber - 1];
                selected.IsEquipped = !selected.IsEquipped;

                int additionalAtk = 0, additionalDef = 0;
                foreach (var item in items.Where(i => i.IsEquipped))
                {
                    additionalAtk += item.ItemAttack;
                    additionalDef += item.ItemDefense;
                }
                player.UpdateCurrentStats(additionalAtk, additionalDef);

                Console.WriteLine($"{selected.ItemName} {(selected.IsEquipped ? "을(를) 장착했습니다." : "을(를) 해제했습니다.")}");
                Console.WriteLine("[확인 = Anykey]");
            }
            else
            {
                Console.WriteLine("잘못된 아이템 번호입니다.");
            }
            Console.ReadLine();
        }


        // 사용자의 입력을 확인하고 검증합니다.
        static int CheckValidInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out var ret) && ret >= min && ret <= max)
                {
                    return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}
