namespace TextRPG
{
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Xml.Linq;

    public interface ICharacter
    {
        string Name { get; set; }
        string ClassName { get; }
        int Health { get; set; }
        int Attack { get; }
        int Defense { get; set; }
        bool IsDead { get; }
        void TakeDamage(int damage);
    }

    public class Player : ICharacter
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defense { get; set; }
        public bool IsDead => Health <= 0;
        public int Attack => new Random().Next(10, AttackPower); // 공격력은 랜덤

        public int Level = 1;
        public int Gold = 1500;

        public int EquipAttack = 0;
        public int EquipDefense = 0;

        public Player(string name, string Select)
        {
            Name = name;

            if (Select == "1" || Select == "전사")
            {
                ClassName = "전사";
                Health = 120;
                AttackPower = 20;
                Defense = 10;
            }
            else if (Select == "2" || Select == "도적")
            {
                ClassName = "도적";
                Health = 100;
                AttackPower = 30;
                Defense = 5;
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }

    }
    public interface Item
    {
        public string Name { get; }
        public int State { get; }
        public string ItemType { get; }
        public string Description { get; }
    }
    public class Inventory
    {
        private List<Items> ItemList;

        public Inventory()
        {
            ItemList = new List<Items>();
        }

        public void AddItems(Items items)
        {
            ItemList.Add(items);
        }

        public void ShowInven()
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].IsEquip == true) Console.Write("[E] ");
                Console.WriteLine($"{ItemList[i].Name} | {ItemList[i].ItemType} + {ItemList[i].State} | {ItemList[i].Description}");
            }
        }
        public void ItemEquipManger(Player player)
        {
            bool game = true;
            while (game)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");
                for (int i = 0; i < ItemList.Count; i++)
                {
                    Console.Write($"- {i + 1} ");
                    if (ItemList[i].IsEquip == true) Console.Write("[E] ");
                    Console.WriteLine($"{ItemList[i].Name} | {ItemList[i].ItemType} + {ItemList[i].State} | {ItemList[i].Description}");
                }
                

                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int select = int.Parse(Console.ReadLine());

                if (select == 0) game = false;
                else if (select > 0 && select <= ItemList.Count && ItemList[select - 1] != null )
                {
                    if (ItemList[select - 1].IsEquip == true)//장비 해제
                    {
                        ItemList[select - 1].IsEquip = false;
                        if (ItemList[select - 1].ItemType == "공격력") player.EquipAttack -= ItemList[select - 1].State;
                        else if (ItemList[select - 1].ItemType == "방어력") player.EquipDefense -= ItemList[select - 1].State;
                    }
                    else //장비 장착
                    {
<<<<<<< Updated upstream
                        ItemList[select - 1].IsEquip = true;
                        if (ItemList[select - 1].ItemType == "공격력") player.EquipAttack += ItemList[select - 1].State;
                        else if (ItemList[select - 1].ItemType == "방어력") player.EquipDefense += ItemList[select - 1].State;
=======
                        if (ItemList[select - 1].ItemType == "공격력")// 선택한 아이템이 공격력이면
                        {
                            //공격력 아이템은 모두 false로 변경
                            for (int i = 0; i < ItemList.Count; i++)
                            {
                                if (ItemList[i].ItemType == "공격력" && ItemList[i].IsEquip == true)
                                {
                                    ItemList[i].IsEquip = false;
                                    player.EquipAttack -= ItemList[i].State;
                                }
                            }
                            ItemList[select - 1].IsEquip = true;
                            player.EquipAttack += ItemList[select - 1].State;
                        }
                        else if (ItemList[select - 1].ItemType == "방어력")// 선택한 아이템이 공격력이면
                        {
                            //공격력 아이템은 모두 false로 변경
                            for (int i = 0; i < ItemList.Count; i++)
                            {
                                if (ItemList[i].ItemType == "방어력" && ItemList[i].IsEquip == true)
                                {
                                    ItemList[i].IsEquip = false;
                                    player.EquipDefense -= ItemList[i].State;
                                }
                                    
                            }
                            ItemList[select - 1].IsEquip = true;
                            player.EquipDefense += ItemList[select - 1].State;
                        }
>>>>>>> Stashed changes
                    }
                }
                else 
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
        }
    }
    public class Items
    {
        public string Name { get; set; }
        public int State { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public bool IsEquip;

        public Items(string name, string itemType, int state, string description, bool isEquip)
        {
            Name= name;
            State = state;
            ItemType = itemType;
            Description = description;
            IsEquip = isEquip;
        }
    }

    public class ShopItem : Item
    {
        public string Name { get; set; }
        public int State { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }

        public int Price;
        public bool IsBuy = false;

        public ShopItem(string name, int state, string itemType, string description, int price, bool isBey)
        {
            Name = name;
            State = state;
            ItemType = itemType;
            Description = description;
            Price = price;
            IsBuy = isBey;
        }
    }

    public class Shop
    {
        private List<ShopItem> ShopItemList;

        public Shop()
        {
            ShopItemList = new List<ShopItem>();
        }

        public void SettingShop()
        {
            ShopItemList.Add(new ShopItem("수련자 갑옷", 7, "방어력", "수련자용 갑옷이다.", 500, false));
            ShopItemList.Add(new ShopItem("무쇠갑옷", 10, "방어력", "무쇠로 만들어져 튼튼한 갑옷이다.", 500, false));
            ShopItemList.Add(new ShopItem("낡은 검", 7, "공격력", "수련자용 갑옷이다.", 500, false));
            ShopItemList.Add(new ShopItem("청동 도끼", 10, "공격력", "수련자용 갑옷이다.", 500, false));
        }

        public void ViewShopList()
        {
            for (int i = 0; i < ShopItemList.Count; i++)
            {
                Console.WriteLine($"{ShopItemList[i].Name} | {ShopItemList[i].ItemType} + {ShopItemList[i].State} | {ShopItemList[i].Description} | {ShopItemList[i].Price}");
            }
        }
        
        public void ShopManger(Player player, Inventory inventory)
        {
            bool game = true;
            while (game)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < ShopItemList.Count; i++)
                {
                    Console.Write($"- {i + 1} ");
                    if (ShopItemList[i].IsBuy != true) Console.WriteLine($"{ShopItemList[i].Name} | {ShopItemList[i].ItemType} + {ShopItemList[i].State} | {ShopItemList[i].Description} | {ShopItemList[i].Price}");
                    else Console.WriteLine($"{ShopItemList[i].Name} | {ShopItemList[i].ItemType} + {ShopItemList[i].State} | {ShopItemList[i].Description} | 구매완료");
                }

                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int select = int.Parse(Console.ReadLine());
                if (select == 0) game = false;
                else if (select > 0 && select <= ShopItemList.Count && ShopItemList[select - 1] != null)
                {
                    if (ShopItemList[select - 1].IsBuy == false)
                    {
                        if (player.Gold >= ShopItemList[select - 1].Price)
                        {
                            ShopItemList[select - 1].IsBuy = true;
                            player.Gold -= ShopItemList[select - 1].Price;
                            Items items = new Items(ShopItemList[select - 1].Name, ShopItemList[select - 1].ItemType,
                                ShopItemList[select - 1].State, ShopItemList[select - 1].Description, false);
                            inventory.AddItems(items);
                        }
                        else if (player.Gold < ShopItemList[select - 1].Price)
                        {
                            Console.WriteLine("돈이 부족합니다. 계속하려면 아무 키나 누르세요...");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("이미 구매한 아이템입니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }

                    
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
        }
    }



    public class MainStage(Player player, Shop shop, Inventory inventory)
    {
        bool game = true;
        
        public void Start()
        {
            Items items1 = new Items("초보자 나무 목검", "공격력", 5, "초보자도 쉽게 다룰수 있는 나무 목검", false);
            Items items2 = new Items("초보자 천갑옷", "방어력", 5, "초보자에게 처음 지급되는 천갑옷", false);
            inventory.AddItems(items1);
            inventory.AddItems(items2);
            MainMenu();
        }
        public void MainMenu()
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("에데온 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 성당");
                Console.WriteLine("0. 게임 종료\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StateUI();
                        break;
                    case "2":
                        InventoryUI();
                        break;
                    case "3":
                        ShopUI();
                        break;
                    case "4":
                        //던전 입장
                        break;
                    case "5":
                        // 휴식 하기
                        Rest();
                        break;
                    case "0":
                        game = false;
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public void StateUI()
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {player.Level}");
                Console.WriteLine($"{player.Name} ( {player.ClassName} )");
                Console.Write($"공격력 : {player.AttackPower}");
                if (player.EquipAttack > 0) Console.WriteLine($"({player.EquipAttack})");
                else Console.WriteLine("");
                Console.Write($"방어력 : {player.Defense}");
                if (player.EquipDefense > 0) Console.WriteLine($"({player.EquipDefense})");
                else Console.WriteLine("");
                Console.WriteLine($"체 력 : {player.Health}");
                Console.WriteLine($"Gold : {player.Gold} G");
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");


                string input = Console.ReadLine();
                if (input == "0") MainMenu();
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }

        }

        public void InventoryUI()
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");
                inventory.ShowInven();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (input == "1") ItemManagerUI();
                else if (input == "0") MainMenu();
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }

        }
        public void ItemManagerUI()
        {
            inventory.ItemEquipManger(player);
        }

        public void ShopUI()
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("[아이템 목록]");

                shop.ViewShopList();
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (input == "0") MainMenu();
                else if (input == "1") shop.ShopManger(player, inventory);
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }

        }
        public void Dungen()
        {

        }
        public void Rest()
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("성당");
                Console.WriteLine("500 G를 헌금하시면 채력을 회복할 수 있습니다");
                Console.WriteLine($"현재 채력 : {player.Health}");
                Console.WriteLine($"보유 골드 : {player.Gold}\n");
                Console.WriteLine("1. 헌금하기");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (input == "0") MainMenu();
                else if (input == "1")
                {
                    if (player.Health < 100 && player.Gold >= 500)
                    {
                        int healing = 100 - player.Health;
                        player.Health += healing;
                        player.Gold -= 500;
                        Console.WriteLine($"{healing}만큼 회복이 완료 되었습니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }
                    else if (player.Health >= 100)
                    {
                        Console.WriteLine("체력이 너무 많아 회복이 불가능 합니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }
                    else if (player.Gold < 500)
                    {
                        Console.WriteLine("돈이 부족하여 회복이 불가능 합니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }
                    
                }
                else if (input == "2")
                {
                    player.Health -= 30;
                    Console.WriteLine(player.Health);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("무한의 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정 해주세요.");
            Console.Write("이름 : ");
            string name = Console.ReadLine();
            string select;
            //저장 시스템 만들기

            //직업 선택창 만들기
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 직업을 골라주세요.");
                Console.WriteLine("1. 전사 ");
                Console.WriteLine("2. 도적 ");
                Console.Write(">> ");
                select = Console.ReadLine();

                if (select == "1" || select == "2" || select == "전사" || select == "도적")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
            
            

            Player player = new Player(name, select);
            Shop shop = new Shop();
            shop.SettingShop();
            Inventory inventory = new Inventory();
           
            MainStage mainStage = new MainStage(player, shop, inventory);
            mainStage.Start();
        }
    }
}
