namespace TextRPG
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Xml.Linq;

    public interface ICharacter
    {
        string Name { get; set; }
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
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool IsDead => Health <= 0;
        

        public int Level = 1;
        public int Gold = 999999;

        public int EquipAttack = 0;
        public int EquipDefense = 0;

        public Player(string name, string Select)
        {
            Name = name;

            if (Select == "1" || Select == "전사")
            {
                ClassName = "전사";
                Health = 120;
                Attack = 20;
                Defense = 10;
            }
            else if (Select == "2" || Select == "도적")
            {
                ClassName = "도적";
                Health = 100;
                Attack = 30;
                Defense = 5;
            }
        }

        public void TakeDamage(int damage)
        {
            float playerDef = Defense + EquipDefense; // 플레이어 방어력 = 기본 방어력 + 아이템 방어력
            float ReductionDamage = playerDef / (playerDef + 100); // 감소된 피해량 = 플레이어 방어력 /( 플레이어 방어력 + 100)
            int takeDamage = Convert.ToInt32(damage - (damage * ReductionDamage)); // 받은 피해량 = 몬스터 공격력 - (몬스터 공격력 * 감소된 피해량)
            Health -= takeDamage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {takeDamage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }
    }

    public class Monster : ICharacter
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool IsDead => Health <= 0;

        public int Level {  get; set; }
        public int Gold { get; set; }

        public Monster(string name, int health, int attack, int defense, int level, int gold)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Defense = defense;
            Level = level;
            Gold = gold;
        }

        public void TakeDamage(int damage)
        {
            float ReductionDamage = Defense / (Defense + 100);
            int takeDamage = Convert.ToInt32(damage - (damage * ReductionDamage));
            Health -= takeDamage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {takeDamage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }
    }

    public class Goblin : Monster
    {
        public Goblin(string name) : base(name, 50, 10, 5, 2, 1000) { }
    }
    public class Golom : Monster
    {
        public Golom(string name) : base(name, 50, 10, 11, 2, 1700) { }
    }
    public class Dragon : Monster
    {
        public Dragon(string name) : base(name, 100, 20, 17, 12, 2500) { }
    }

    public class Stage
    {
        private Monster monster;

        public delegate void GameEvent(Monster character);
        public event GameEvent OnCharacterDeath;
        public Stage(Monster monster)
        {
            this.monster = monster;
        }

        public void Start(Player player)
        {
            Random random = new Random();
            bool DungeonFail = false;
            if (player.Defense + player.EquipDefense < monster.Defense)
            {
                int randomfail = random.Next(0, 40);
                if (randomfail > 0)
                {
                    DungeonFail = true;
                }

                else DungeonFail = false;
            }

            Console.Clear();
            Console.WriteLine($"스테이지 시작!");
            Console.WriteLine($"플레이어 정보: 이름({player.Name}), 체력({player.Health}), 공격력({player.Attack + player.EquipAttack}), 방어력({player.Defense + player.EquipDefense})");
            Console.WriteLine($"몬스터 정보: 이름({monster.Name}), 체력({monster.Health}), 공격력({monster.Attack}), 방어력({monster.Defense})");
            Console.WriteLine("----------------------------------------------------");
            int playerHp = player.Health;
            int playerGold = player.Gold;
            int playerLv = player.Level;
            

            while (!player.IsDead && !monster.IsDead && DungeonFail == false) // 플레이어나 몬스터가 죽을 때까지 반복
            {
                // 플레이어의 턴
                Console.WriteLine($"{player.Name}의 턴!");
                //행동 선택
                Console.Clear();
                Console.WriteLine($"플레이어 정보: 이름({player.Name}), 체력({player.Health}), 공격력({player.Attack + player.EquipAttack}), 방어력({player.Defense + player.EquipDefense})");
                Console.WriteLine($"몬스터 정보: 이름({monster.Name}), 체력({monster.Health}), 공격력({monster.Attack}), 방어력({monster.Defense})");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("[행동 선택]\n");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 포션");

                Console.WriteLine("\n원하는 행동을 선택 해주세요");
                Console.Write(">> ");
                string select = Console.ReadLine();

                if (select == "1")
                {
                    monster.TakeDamage(player.Attack + player.EquipAttack);
                }
                else if (select == "2")
                {
                    Console.WriteLine("회복");
                    player.Health += 20;
                    Console.WriteLine("체력 20만큼 회복 하였습니다.");
                }
                
                Console.WriteLine();
                Thread.Sleep(1000);  // 턴 사이에 1초 대기

                if (monster.IsDead)
                {
                    Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                    break;  // 몬스터가 죽었다면 턴 종료
                }
                    

                // 몬스터의 턴
                Console.WriteLine($"{monster.Name}의 턴!");
                player.TakeDamage(monster.Attack);
                Console.WriteLine();
                Thread.Sleep(1000);  // 턴 사이에 1초 대기

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }

            // 플레이어나 몬스터가 죽었을 때 이벤트 호출
            if (DungeonFail)
            {
                DungeonFail = false;
                Console.WriteLine("던전 실패! 패배했습니다...");
                Console.WriteLine("[탐험 결과]\n");

                Console.WriteLine($"체력 {playerHp} -> {player.Health / 2} ");
                Console.WriteLine($"Gold {playerGold} -> {player.Gold} ");
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
            else if (player.IsDead)
            {
                Console.Clear();
                Console.WriteLine("던전 실패! 패배했습니다...");
                Console.WriteLine("[탐험 결과]\n");

                Console.WriteLine($"체력 {playerHp} -> {player.Health} ");
                Console.WriteLine($"Gold {playerGold} -> {player.Gold} ");
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
            else if (monster.IsDead)
            {
                Console.Clear();
                Console.WriteLine("던전 클리어");
                Console.WriteLine($"축하합니다 {monster.Name}를 물리쳤습니다!");
                Console.WriteLine("[탐험 결과]\n");
                player.Level += 1;
                player.Gold += monster.Gold;

                Console.WriteLine($"체력 {playerLv} -> {player.Level} ");
                Console.WriteLine($"체력 {playerHp} -> {player.Health} ");
                Console.WriteLine($"Gold {playerGold} -> {player.Gold} ");
                
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }
    }



    public interface Item
    {
        public string Name { get; }
        public int State { get; }
        public string ItemType { get; }
        public string Description { get; }
        public int Price { get; }
    }
    public class Inventory
    {
        private List<Items> ItemList;

        public Inventory()
        {
            ItemList = new List<Items>();
        }
        public List<Items> GetItems() // 인벤토리 아이템 상점에 아이탬 판매 리스트에 전달
        {
            return ItemList;
        }
        public void RemoveItem(Items item) //인벤토리 아이템 판매시 삭제
        {
            ItemList.Remove(item);
        }

        public void AddItems(Items items) //인벤토리 리스트에 추가 기능
        {
            ItemList.Add(items);
        }

        public void ShowInven() // 인벤토리에 표시 
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].IsEquip == true) Console.Write("[E] ");
                Console.WriteLine($"{ItemList[i].Name} | {ItemList[i].ItemType} + {ItemList[i].State} | {ItemList[i].Description}");
            }
        }
        public void ItemEquipManger(Player player) // 아이템 장착 관리
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
                    if (ItemList[i].IsEquip == true) Console.Write("[E] "); //현재 장착된 아이템은 [E] 표시
                    Console.WriteLine($"{ItemList[i].Name} | {ItemList[i].ItemType} + {ItemList[i].State} | {ItemList[i].Description}");
                }
                

                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int select = int.Parse(Console.ReadLine());

                // 아이템 장착 기능인데 중복된 코드가 있는것 같지만 최적화하다가 
                //작동이 안될까봐 두려워 건들지 못하겠습니다
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
                        ItemList[select - 1].IsEquip = true;
                        if (ItemList[select - 1].ItemType == "공격력") player.EquipAttack += ItemList[select - 1].State;
                        else if (ItemList[select - 1].ItemType == "방어력") player.EquipDefense += ItemList[select - 1].State;

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
                            ItemList[select - 1].IsEquip = true; //선택한 아이템 장착
                            player.EquipAttack += ItemList[select - 1].State;
                        }
                        else if (ItemList[select - 1].ItemType == "방어력")// 선택한 아이템이 공격력이면
                        {
                            //방어력 아이템은 모두 false로 변경
                            for (int i = 0; i < ItemList.Count; i++)
                            {
                                if (ItemList[i].ItemType == "방어력" && ItemList[i].IsEquip == true)
                                {
                                    ItemList[i].IsEquip = false;
                                    player.EquipDefense -= ItemList[i].State;
                                }
                                    
                            }
                            ItemList[select - 1].IsEquip = true; //선택한 아이템 장착
                            player.EquipDefense += ItemList[select - 1].State;
                        }
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
        public int Price { get; set; }
        public bool IsEquip;

        public Items(string name, string itemType, int state, string description, bool isEquip, int price)
        {
            Name= name;
            State = state;
            ItemType = itemType;
            Description = description;
            IsEquip = isEquip;
            Price = price;
        }
    }

    public class ShopItem : Item
    {
        public string Name { get; set; }
        public int State { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
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

        public void SettingShop() // 상점 아이템 기본 세팅
        {
            ShopItemList.Add(new ShopItem("수련자 갑옷", 7, "방어력", "수련자용 갑옷이다.", 500, false));
            ShopItemList.Add(new ShopItem("무쇠갑옷", 10, "방어력", "무쇠로 만들어져 튼튼한 갑옷이다.",750, false));
            ShopItemList.Add(new ShopItem("낡은 검", 7, "공격력", "수련자용 갑옷이다.", 500, false));
            ShopItemList.Add(new ShopItem("청동 도끼", 10, "공격력", "수련자용 갑옷이다.", 750, false));
            ShopItemList.Add(new ShopItem("기사단의 보검 -일곱개의 진실- ", 900, "공격력", "초대기사단장이 사용하던 검. 선택받은자만 사용 가능하다", 8500, false));
            ShopItemList.Add(new ShopItem("황룡의 금빛 플레이트 갑옷", 880, "방어력", "초대기사단장이 사용하던 갑옷. 특별한 능력이 있는 것 같다.", 7500, false));
        }

        public void ViewShopList()
        {
            for (int i = 0; i < ShopItemList.Count; i++)
            {
                Console.WriteLine($"{ShopItemList[i].Name} | {ShopItemList[i].ItemType} + {ShopItemList[i].State} | {ShopItemList[i].Description} | {ShopItemList[i].Price}");
            }
        }
        
        public void ShopManger(Player player, Inventory inventory) // 상점 아이템 구매 기능
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
                            float sellPrice = (ShopItemList[select - 1].Price * 0.85f);
                            Items items = new Items(ShopItemList[select - 1].Name, ShopItemList[select - 1].ItemType,
                                ShopItemList[select - 1].State, ShopItemList[select - 1].Description, false, Convert.ToInt32(sellPrice));
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

        public void ShopSellManager(Player player, Inventory inventory) //플레이어 아이탬 판매 기능
        {
            bool game = true;
            while (game)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("보유한 아이템을 판매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("[판매 가능한 아이템 목록]\n");

                List<Items> sellableItems = inventory.GetItems(); // 인벤토리에서 모든 아이템 가져오기

                for (int i = 0; i < sellableItems.Count; i++)
                {
                    Console.WriteLine($"- {i + 1} {sellableItems[i].Name} | {sellableItems[i].ItemType} +{sellableItems[i].State} | {sellableItems[i].Description} | 판매가: {sellableItems[i].Price} G");
                }

                Console.WriteLine("0. 나가기\n");
                Console.Write("판매할 아이템 번호를 입력하세요: ");

                int select;
                if (int.TryParse(Console.ReadLine(), out select))
                {
                    if (select == 0)
                    {
                        game = false;
                    }
                    else if (select > 0 && select <= sellableItems.Count)
                    {
                        Items selectedItem = sellableItems[select - 1];

                        // 플레이어 골드 증가
                        player.Gold += selectedItem.Price;

                        // 인벤토리에서 아이템 제거
                        inventory.RemoveItem(selectedItem);

                        Console.WriteLine($"{selectedItem.Name}을(를) {selectedItem.Price}G에 판매했습니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                    }
                }
            }
        }

    }

    public class MainStage(Player player, Shop shop, Inventory inventory)
    {
        bool game = true;
        
        public void Start()
        {
            Items items1 = new Items("+99 나무몽둥이", "공격력", 999, "초보자에게 지급되는 기본 몽둥이 하지만 어디선가 본듯하다.", false, 100);
            Items items2 = new Items("허름한 망토", "방어력", 999, "초보자에게 처음 지급되는 허름한 망토 하지만 어디선가 본듯하다", false, 100);
            inventory.AddItems(items1);
            inventory.AddItems(items2);
            MainMenu();
        }
        public void MainMenu()
        {
            while (game) //메인 메뉴
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
                        DungeonUI();
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
        public void StateUI() //플레이어 스텟
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {player.Level}");
                Console.WriteLine($"{player.Name} ( {player.ClassName} )");
                Console.Write($"공격력 : {player.Attack}");
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

        public void InventoryUI()// 인벤토리
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

        public void ShopUI()//상점
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
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (input == "0") MainMenu();
                else if (input == "1") shop.ShopManger(player, inventory);
                else if (input == "2") shop.ShopSellManager(player, inventory);
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
        }
        public void DungeonUI()//던전
        {
            while (game)
            {
                Goblin goblin = new Goblin("Goblin"); // 고블린 생성
                Golom golom = new Golom("Golom");
                Dragon dragon = new Dragon("Dragon"); // 드래곤 생성

                Console.Clear();
                Console.WriteLine("[던전]");
                Console.WriteLine("던전의 난이도를 선택하세요");
                Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전   | 방어력 17 이상 권장");
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

                switch(input)
                {
                    case "0":
                        MainMenu();
                        break;
                    case "1":
                        // 스테이지 1
                        Stage stage1 = new Stage(goblin);
                        stage1.Start(player);
                        break;
                    case "2":
                        Stage stage2 = new Stage(golom);
                        stage2.Start(player);
                        break;
                    case "3":
                        Stage stage3 = new Stage(dragon);
                        stage3.Start(player);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public void Rest()// 휴식하기
        {
            while (game)
            {
                Console.Clear();
                Console.WriteLine("성당");
                Console.WriteLine("500 G를 헌금하시면 채력을 회복할 수 있습니다");
                Console.WriteLine($"현재 채력 : {player.Health}");
                Console.WriteLine($"보유 골드 : {player.Gold}\n");
                Console.WriteLine("1. 헌금하기");
                Console.WriteLine("2. 기도하기 -신체 강화-");
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
                    player.EquipAttack += 100;
                    player.EquipDefense += 100;
                    Console.WriteLine($"체력 30 만큼 대가를 바치고 신체 강화를 하였습니다.");
                    Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
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
            //게임 시작 인트로
            Console.WriteLine("무한의 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정 해주세요.");
            Console.Write("이름 : ");
            string name = Console.ReadLine();
            string select;

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
