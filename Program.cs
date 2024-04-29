using System;
using System.ComponentModel.Design;
using System.Threading;

namespace Team_B_11_RPG
{
    public class GameManager
    {
        private Player player;
        private List<Item> inventory;

        private List<Item> storeInventory;


        enum PlayerChoice
        {
            MainMenu,
            BattleAttack
        }

        public GameManager()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new Player("Chad", "전사", 1, 10, 5, 100, 15000);

            inventory = new List<Item>();
            storeInventory = new List<Item>();
            storeInventory.Add(new Item("무쇠갑옷", "튼튼한 갑옷", ItemType.ARMOR, 0, 5, 0, 500));
            storeInventory.Add(new Item("낡은 검", "낡은 검", ItemType.WEAPON, 2, 0, 0, 1000));
            storeInventory.Add(new Item("골든 헬름", "희귀한 투구", ItemType.ARMOR, 0, 9, 0, 2000));
        }

        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintGameHeader();
            MainMenu();
        }

        private void MainMenu()
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();

            // 1. 선택 멘트를 줌
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("");

            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 전투");
            Console.WriteLine("");

            // 2. 선택한 결과를 검증함
            int choice = ConsoleUtility.PromptMenuChoice(1, 4);

            // 3. 선택한 결과에 따라 보내줌
            switch (choice)
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    StoreMenu();
                    break;
                case 4:
                    Battle();
                    break;
            }
            MainMenu();
        }

        private void StatusMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상태보기 ■");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine($"{player.Name} ( {player.Job} )");

            // TODO : 능력치 강화분을 표현하도록 변경

            int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
            int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
            int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();

            ConsoleUtility.PrintTextHighlights("공격력 : ", (player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? $" (+{bonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", (player.Def + bonusDef).ToString(), bonusDef > 0 ? $" (+{bonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("체 력 : ", (player.Hp + bonusHp).ToString(), bonusHp > 0 ? $" (+{bonusHp})" : "");

            ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
            Console.WriteLine("");

            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 0))
            {
                case 0:
                    MainMenu();
                    break;
            }
        }

        private void InventoryMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription();
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }

        private void EquipMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 - 장착 관리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription(true, i + 1); // 나가기가 0번 고정, 나머지가 1번부터 배정
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int KeyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count);

            switch (KeyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    inventory[KeyInput - 1].ToggleEquipStatus();
                    EquipMenu();
                    break;
            }
        }

        private void StoreMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription();
            }
            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    PurchaseMenu();
                    break;
            }
        }

        private void PurchaseMenu(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, storeInventory.Count);

            switch (keyInput)
            {
                case 0:
                    StoreMenu();
                    break;
                default:
                    // 1 : 이미 구매한 경우
                    if (storeInventory[keyInput - 1].IsPurchased) // index 맞추기
                    {
                        PurchaseMenu("이미 구매한 아이템입니다.");
                    }
                    // 2 : 돈이 충분해서 살 수 있는 경우
                    else if (player.Gold >= storeInventory[keyInput - 1].Price)
                    {
                        player.Gold -= storeInventory[keyInput - 1].Price;
                        storeInventory[keyInput - 1].Purchase();
                        inventory.Add(storeInventory[keyInput - 1]);
                        PurchaseMenu();
                    }
                    // 3 : 돈이 모자라는 경우
                    else
                    {
                        PurchaseMenu("Gold가 부족합니다.");
                    }
                    break;
            }
        }

        private void Battle(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();
            ConsoleUtility.ShowTitle("Battle ! ");

            // 랜덤 몬스터 생성
            Random randnumber = new Random();
            // 몬스터 출력
            for (int i = 0; i < randnumber.Next(1, 5); i++)
            {
                int j = randnumber.Next(0, 3);
                Monster.MakeMonster();
                Console.WriteLine($"Lv.{Monster.monsters[j].Level} {Monster.monsters[j].Name} HP {Monster.monsters[j].Hp} " +
                $" || 공격력 : {Monster.monsters[j].Hp} 방어력 : {Monster.monsters[j].Def}");
                Monster randomMonster = Monster.monsters[j];
                RandomMonster.randmonsters.Add(new RandomMonster(randomMonster.Name, randomMonster.Level, randomMonster.Atk, randomMonster.Def, randomMonster.Hp, randomMonster.Type, randomMonster.IsAlive));
            }

            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP :{player.Hp}/{player.Hp}");

            Console.WriteLine("");
            Console.WriteLine("1. 공격");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");

            int choice = ConsoleUtility.PromptMenuChoice(0, 1);

            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    BattleAttack();
                    break;
                default:
                    Console.WriteLine("다시 입력해주세요");
                    Battle();
                    break;
            }
        }
        private void BattleAttack(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle!!");

            for (int i = 0; i < RandomMonster.randmonsters.Count; i++)
            {
                RandomMonster.randmonsters[i].MonsterBattle(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP :{player.Hp}/{player.Hp}");
            Console.WriteLine("");
            Console.WriteLine("0. 취소");
            Console.WriteLine("");
            int Choice = ConsoleUtility.PromptMenuChoice(0, RandomMonster.randmonsters.Count);
            //랜덤 공격력
            Random randatk = new Random();
            double attackPower = player.Atk * (1 - 0.1 * randatk.NextDouble());
            attackPower = Math.Ceiling(attackPower);

            switch (Choice)
            {
                case 0:
                    Battle();
                    break;
                default:
                    if (RandomMonster.randmonsters[Choice - 1].Hp >= 0 && RandomMonster.randmonsters[Choice - 1].IsAlive)
                    {
                        Console.Clear();
                        ConsoleUtility.ShowTitle("Battle!!");
                        Console.WriteLine("");
                        Console.WriteLine($"{player.Name}의 공격!");
                        Console.WriteLine($"{RandomMonster.randmonsters[Choice - 1].Name}을(를) 공격했습니다. [데미지] : {player.Atk}");
                        Console.WriteLine("");

                        RandomMonster selectedMonster = RandomMonster.randmonsters[Choice - 1];

                        int monsterhp = selectedMonster.Hp - (int)attackPower;
                        if (monsterhp <= 0)
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[Choice - 1].Name}");
                            Console.WriteLine($"Hp : {RandomMonster.randmonsters[Choice - 1].Hp} -> Dead");
                            selectedMonster.Hp = selectedMonster.Hp - (int)attackPower;
                            Console.WriteLine("");
                            Console.WriteLine("0. 다음");
                            Console.WriteLine("");
                            selectedMonster.IsAliveToggle();
                            int choice = int.Parse(Console.ReadLine());
                            switch (choice)
                            {
                                case 0:
                                    BattleAttack();
                                    break;
                                default:
                                    Console.WriteLine("다시 입력해주세요");
                                    BattleAttack();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[Choice - 1].Name}");
                            Console.WriteLine($"Hp : {RandomMonster.randmonsters[Choice - 1].Hp} -> Hp : {monsterhp}");
                            selectedMonster.Hp = selectedMonster.Hp - (int)attackPower;
                            Console.WriteLine("");
                            Console.WriteLine("0. 다음");
                            int choice = int.Parse(Console.ReadLine());
                            switch (choice)
                            {
                                case 0:
                                    BattleAttack();
                                    break;
                                default:
                                    Console.WriteLine("다시 입력해주세요");
                                    BattleAttack();
                                    break;
                            }
                        }
                    }
                    else
                    {
                        RandomMonster.randmonsters[Choice - 1].IsAliveToggle();
                        BattleAttack();
                    }
                    //if (RandomMonster.randmonsters[Choice - 1].IsAlive)
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Gray;
                    //    Console.WriteLine($"Lv.{RandomMonster.randmonsters[Choice - 1].Level} {RandomMonster.randmonsters[Choice - 1].Name} Dead");
                    //    Console.ResetColor();
                    //    BattleAttack();
                    //}
                    BattleAttack();
                    break;
            }
        }

        private void EnemyPhase()
        {

        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
        }
    }
}
