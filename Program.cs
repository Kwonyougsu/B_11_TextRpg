using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

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
            player = new Player("Chad", "전사", 1, 10, 5, 100, 15000,100);

            inventory = new List<Item>();
            storeInventory = new List<Item>();
            storeInventory.Add(new Item("무쇠갑옷", "튼튼한 갑옷", ItemType.ARMOR, 0, 5, 0, 500));
            storeInventory.Add(new Item("낡은 검", "낡은 검", ItemType.WEAPON, 2, 0, 0, 1000));
            storeInventory.Add(new Item("골든 헬름", "희귀한 투구", ItemType.ARMOR, 0, 9, 0, 2000));
            storeInventory.Add(new Item("튼튼한 가시갑옷", "튼튼하고 날카로운 갑옷", ItemType.ARMOR, 5, 5, 10, 3000));


        }

        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintGameHeader();
            CreateMenu();
        }

        private void CreateMenu()
        {
            Console.Clear();

            Console.Write("원하시는 이름을 설정해주세요. : "); //이름 설정
            player.Name = Console.ReadLine();

            Console.WriteLine("직업을 선택해주세요.");  // 직업 설정 직업은 일단 2가지로 탱커와 딜러로 나눔
            Console.WriteLine("1.탱커 : 높은 체력, 낮은 공격력");
            Console.WriteLine("2.딜러 : 낮은 체력, 높은 공격력");
            Console.WriteLine("");

            int choice = ConsoleUtility.PromptMenuChoice(1, 2);
            switch (choice)
            {
                case 1:
                    player = new Player(player.Name, "탱커", 1, 0, 0, 0, 15000,0,0);
                    break;
                case 2:
                    player = new Player(player.Name, "딜러", 1, 0, 0, 0, 15000,0,0);
                    break;
            }

            int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
            int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
            int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();
            if (player.Job == "탱커")
            {
                player.MaxHp = (150 + bonusHp) + (player.Level * 20);
                player.Atk = (10 + bonusAtk) + (player.Level * 0.5);
                player.Def = (10 + bonusDef) + (player.Level * 1);
            }
            else
            {
                player.MaxHp = (100 + bonusHp) + (player.Level * 20);
                player.Atk = (20 + bonusAtk) + (player.Level * 0.5);
                player.Def = (5 + bonusDef) + (player.Level * 1);
            }
            player.Current_Hp = player.MaxHp;



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
            Console.WriteLine("5. 회복");
            Console.WriteLine("");

            // 2. 선택한 결과를 검증함
            int choice = ConsoleUtility.PromptMenuChoice(1, 5);

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
                case 5:
                    Rest();
                    break;
            }
            MainMenu();
        }

        private void Rest(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            } 

            Console.Clear();

            ConsoleUtility.ShowTitle("■ 회복 ■");
            Console.WriteLine("포션을 사용하면 체력을 30 회복 할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 사용하기");
            Console.WriteLine("0. 나가기");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:// 현재 체력이 최대 체력보다 낮을때 포션 사용 가능 
                    if (player.Current_Hp <= (player.MaxHp - 31))
                    {
                        player.Current_Hp += 30;
                        Rest("체력이 30 회복되었습니다.");
                    }
                    else if (player.Current_Hp >= (player.MaxHp -30) && player.Current_Hp <= (player.MaxHp - 1))
                    {
                        player.Current_Hp = player.MaxHp;
                        Rest("체력이 모두 찼습니다.");
                    }
                    else
                    {
                        Rest("체력이 이미 가득차있습니다.");
                    }                    
                  
                    break;
            }
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
            if (player.Job == "탱커")
            {
                player.MaxHp = (150 + bonusHp) + (player.Level * 20);
                player.Atk = (10 + bonusAtk) + (player.Level * 2);
                player.Def = (10 + bonusDef) + (player.Level * 3);
            }
            else
            {
                player.MaxHp = (100 + bonusHp) + (player.Level * 20);
                player.Atk = (20 + bonusAtk) + (player.Level * 2);
                player.Def = (5 + bonusDef) + (player.Level * 3);
            }

            ConsoleUtility.PrintTextHighlights("공격력 : ", player.Atk.ToString(), bonusAtk > 0 ? $" (+{bonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", player.Def.ToString(), bonusDef > 0 ? $" (+{bonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("현재 체력 :", player.Current_Hp.ToString()+" / "+player.MaxHp.ToString(), bonusHp > 0 ? $" (+{bonusHp})" : "");


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
            // 저장되어있는 랜덤몬스터 클리어 
            RandomMonster.randmonsters.Clear();
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
            Console.WriteLine($"HP :{player.Current_Hp}/{player.MaxHp}");

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
            //battle에서 저장한 랜덤몬스터 생성 
            for (int i = 0; i < RandomMonster.randmonsters.Count; i++)
            {
                RandomMonster.randmonsters[i].MonsterBattle(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP :{player.Current_Hp}/{player.MaxHp}");
            Console.WriteLine("");
            Console.WriteLine("0. 취소");
            Console.WriteLine("");
            int SelectMonster = ConsoleUtility.PromptMenuChoice(0, RandomMonster.randmonsters.Count);
            //랜덤 공격력
            Random randatk = new Random();
            double attackPower = player.Atk * (1 - 0.1 * randatk.NextDouble());
            attackPower = Math.Ceiling(attackPower);

            // 치명타 발생 여부 결정
            bool isCritical = new Random().Next(100) < 15;

            // 치명타 발생 시 공격력 증가
            if (isCritical)
            {
                attackPower *= 1.6;
            }

            switch (SelectMonster)
            {
                case 0:
                    Battle();
                    break;
                default:
                    if (RandomMonster.randmonsters[SelectMonster - 1].Hp >= 0 && RandomMonster.randmonsters[SelectMonster - 1].IsAlive)
                    {
                        Console.Clear();
                        ConsoleUtility.ShowTitle("Battle!!");
                        Console.WriteLine("");
                        Console.WriteLine($"{player.Name}의 공격!");
                        if(isCritical)
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}을(를) 공격했습니다. [데미지] : {attackPower} - 치명타 공격!!");
                        }
                        else
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}을(를) 공격했습니다. [데미지] : {attackPower}");
                        }
                        Console.WriteLine("");

                        RandomMonster selectedMonster = RandomMonster.randmonsters[SelectMonster - 1];

                        int monsterhp = selectedMonster.Hp - (int)attackPower;
                        if (monsterhp <= 0)
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}");
                            Console.WriteLine($"Hp : {RandomMonster.randmonsters[SelectMonster - 1].Hp} -> Dead");
                            selectedMonster.Hp = selectedMonster.Hp - (int)attackPower;
                            Console.WriteLine("");
                            Console.WriteLine("0. 다음");
                            Console.WriteLine("");
                            selectedMonster.IsAliveToggle();
                            int choice = ConsoleUtility.PromptMenuChoice(0, 0);

                            switch (choice)
                            {
                                case 0:
                                    EnemyPhase();
                                    break;
                                default:
                                    Console.WriteLine("다시 입력해주세요");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}");
                            Console.WriteLine($"Hp : {RandomMonster.randmonsters[SelectMonster - 1].Hp} -> Hp : {monsterhp}");
                            selectedMonster.Hp = selectedMonster.Hp - (int)attackPower;
                            Console.WriteLine("");
                            Console.WriteLine("0. 다음");
                            int choice = ConsoleUtility.PromptMenuChoice(0, 0);
                            switch (choice)
                            {
                                case 0:
                                    EnemyPhase();
                                    break;
                                default:
                                    Console.WriteLine("다시 입력해주세요");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("이미 죽은 몬스터를 공격할 수 없습니다.다시 입력해주세요");
                        Thread.Sleep(2000);
                        BattleAttack();
                        break;
                    }
                    EnemyPhase();
                    break;
            }
        }

        private void EnemyPhase(string? prompt = null)
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
            bool MonsterAlive = false;

            for (int i = 0; i < RandomMonster.randmonsters.Count; i++)
            {
              //랜덤 공격력
              Random randatk = new Random();
              double MattackPower = RandomMonster.randmonsters[i].Atk * (1 - 0.1 * randatk.NextDouble());
              MattackPower = Math.Ceiling(MattackPower);
              
              if (RandomMonster.randmonsters[i].IsAlive)
              {
                  Console.Clear();
                  ConsoleUtility.ShowTitle("Battle!!");
                  Console.WriteLine("");
                  Console.WriteLine($"{RandomMonster.randmonsters[i].Name}의 공격!");
                  Console.WriteLine($"{player.Name}을(를) 공격했습니다. [데미지] : {RandomMonster.randmonsters[i].Atk}");
                  Console.WriteLine("");
                  Console.WriteLine("[내 정 보]");
                  Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
                  player.Current_Hp = Math.Max(player.Current_Hp - (int)MattackPower,0);
                  Console.WriteLine($"HP :{player.Current_Hp}/{player.MaxHp}");
                  Console.WriteLine("");
                    if (player.Current_Hp <= 0)
                    {
                        losePhase();
                    }
                    Console.WriteLine("0. 다음");
                  int choice = ConsoleUtility.PromptMenuChoice(0, 0);
                  switch (choice)
                  {
                      case 0:
                          break;
                      default:
                          Console.WriteLine("다시 입력해주세요");
                          break;
                  }
              }
              if (RandomMonster.randmonsters[i].IsAlive)
              {
                  MonsterAlive = true; // 현재 conunt i에 몬스터가 살아있으면 살림
              }
            }
            if (!MonsterAlive) // 몬스터 죽으면 넘어감
            {
                EndPhase();
            }
            else 
            {
                BattleAttack();
            }
        }

        private void losePhase()
        {
            Console.Clear();
            int hp = player.Current_Hp;
            hp = 0;
            ConsoleUtility.ShowTitle("Battle - Result");
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("You Lose");
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP :{player.Current_Hp} -> {hp}");
            Console.WriteLine("");
            Console.WriteLine("0. 메인으로");
            int choice = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("다시 입력해주세요");
                    break;
            }
        }

        private void EndPhase()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle - Result");
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("Victory");
            Console.WriteLine("");
            Console.WriteLine($"던전에서 몬스터 {RandomMonster.randmonsters.Count}마리를 잡았습니다");
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP :{player.MaxHp} -> {player.Current_Hp}");
            Console.WriteLine("");
            Console.WriteLine("0. 다음");
            int choice = ConsoleUtility.PromptMenuChoice(0, 0);
            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("다시 입력해주세요");
                    break;
            }
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

