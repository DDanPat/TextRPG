namespace TextRPG
{
    using System.Diagnostics;
    using System.Numerics;

    public interface ICharacter
    {
        string Name { get; }
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

        public string ClassName { get; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defence { get; set; }
        public bool IsDead => Health <= 0;
        public int Attack => new Random().Next(10, AttackPower); // 공격력은 랜덤

        public int Level = 1;
        public int Gold = 1500;

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }
    }



    public class MainStage : Player
    {
        Player player = new Player();
        public void Start()
        {
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정 해주세요.");
            Console.Write("이름 : ");
            player.Name = Console.ReadLine();

            //저장 시스템 만들기

            //직업 선택창 만들기

            Menu();
        }
        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    State();
                    break;
                case "2":
                    Inventory();
                    break;
                case "3":
                    Shop();
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }

        public void State()
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
            if (input == "0") Menu();

        }
        public void Inventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();
            if (input == "0") Menu();
        }

        public void Shop()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine("800 G");
            Console.WriteLine("[아이템 목록]");

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            string input = Console.ReadLine();
            if (input == "0") Menu();
        }
        // 메인 메서드

    }
    class Program
    {
        static void Main(string[] args)
        {
            MainStage mainStage = new MainStage();
            mainStage.Start();
        }
    }
}
