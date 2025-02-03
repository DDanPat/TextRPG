namespace TextRPG
{
    using System.Diagnostics;
    using System.Numerics;
    using System.Reflection.Emit;
    using System.Xml.Linq;

    public interface ICharacter
    {
        string Name { get; set; }
        string ClassName { get; }
        int Health { get; set; }
        int Attack { get; }
        int Defence { get; set; }
        bool IsDead { get; }
        void TakeDamage(int damage);
    }

    public class Player : ICharacter
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defence { get; set; }
        public bool IsDead => Health <= 0;
        public int Attack => new Random().Next(10, AttackPower); // 공격력은 랜덤

        public int Level = 1;
        public int Gold = 1500;

        public Player(string name, string Select)
        {
            Name = name;

            if (Select == "1" || Select == "전사")
            {
                ClassName = "전사";
                Health = 120;
                AttackPower = 20;
                Defence = 10;
            }
            else if (Select == "2" || Select == "도적")
            {
                ClassName = "도적";
                Health = 100;
                AttackPower = 30;
                Defence = 5;
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }

    }


    public class MainStage(Player player)
    {
        bool game = true;
        public void Start()
        {
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
                Console.WriteLine("3. 상점\n");
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
                        //ShopUI();
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
                Console.WriteLine($"공격력 : {player.AttackPower}");
                Console.WriteLine($"방어력 : {player.Defence}");
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
            while (game)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");


                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

                if (input == "0") InventoryUI();
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }

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
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("무한의 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정 해주세요.");
            Console.Write("이름 : ");
            string name = Console.ReadLine();

            //저장 시스템 만들기

            //직업 선택창 만들기
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 직업을 골라주세요.");
            Console.WriteLine("1. 전사 ");
            Console.WriteLine("2. 도적 ");
            Console.Write(">> ");
            string select = Console.ReadLine();
            
            Player player = new Player(name, select);
            
            MainStage mainStage = new MainStage(player);
            mainStage.Start();
        }
    }
}
