using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.NetworkInformation;
using System.Drawing;
using static QuestClear;
using System.Collections.Generic;

namespace Team_B_11_RPG
{
    internal class GameManager
    {
        private Player player;
        private Item item;

        public List<Item> inventory = new List<Item>();
        private List<Item> storeInventory;

        private List<QuestList> quests = new List<QuestList>(QuestListOrder.questOrder);
        private QuestReward questsReward = new QuestReward();
        private QuestClear questClear = new QuestClear();

        public int questAcceptCheck = 0;
        public int questClearCheck = 0;
        public int monsterCount = 0;
        public int monster2Count = 0;

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
            player = new Player(this, "Chad", "전사", 1, 10, 5, 100, 15000, 100, 0, 3, 50, 50);
            storeInventory = new List<Item>
            {
                new Item("무쇠갑옷", "튼튼한 갑옷", ItemType.ARMOR, 0, 3, 0, 500),
                new Item("낡은 검", "낡은 검", ItemType.WEAPON, 2, 0, 0, 1000),
                new Item("골든 헬름", "희귀한 투구", ItemType.ARMOR, 0, 5, 0, 2000),
                new Item("튼튼한 가시갑옷", "튼튼하고 날카로운 갑옷", ItemType.ARMOR, 3, 3, 10, 3000),
                new Item("대검","큰검이다",ItemType.WEAPON,20,0,50,5000)

            }; // 아이템 리스트 단순화


        }

        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintGameHeader();
            if (File.Exists("playerfile.json"))
            {
                player = DataMamager<Player>.Load("playerfile.json");
                inventory = DataMamager<List<Item>>.Load("itemfile.json");
                MainMenu();
            }
            else
            {
                CreateMenu();
            }
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
                    player = new Player(this, player.Name, "탱커", 1, 0, 0, 0, 15000, 0, 0, 3, 50, 50);
                    break;
                case 2:
                    player = new Player(this, player.Name, "딜러", 1, 0, 0, 0, 15000, 0, 0, 3, 50, 50);
                    break;
            }

            int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
            int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
            int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();
            int bonusMp = inventory.Select(item => item.IsEquipped ? item.Mp : 0).Sum();

            if (player.Job == "탱커")
            {
                player.MaxHp = (150 + bonusHp) + (player.Level * 20);
                player.MaxMp = (50 + bonusMp) + (player.Level * 20);
                player.Atk = (10 + bonusAtk) + (player.Level * 0.5);
                player.Def = (5 + bonusDef) + (player.Level * 2);
            }
            else
            {
                player.MaxHp = (100 + bonusHp) + (player.Level * 20);
                player.MaxMp = (50 + bonusMp) + (player.Level * 20);
                player.Atk = (20 + bonusAtk) + (player.Level * 0.5);
                player.Def = (2 + bonusDef) + (player.Level * 1);
            }
            player.Current_Hp = player.MaxHp;
            player.Current_Mp = player.MaxMp;
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
            Console.Write("4. 전투");
            Console.WriteLine($"  ({player.floor})층");
            Console.WriteLine("5. 회복");
            Console.WriteLine("6. 퀘스트");
            Console.WriteLine("7. 저장하기");
            Console.WriteLine("");
            Console.WriteLine("0. 이름 변경하기");
            Console.WriteLine("");

            // 2. 선택한 결과를 검증함
            int choice = ConsoleUtility.PromptMenuChoice(0, 7);

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
                case 6:
                    Quest();
                    break;
                case 7:
                    SavePlayerData();
                    break;
                case 0:
                    ReName();
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
            Console.WriteLine("포션을 사용하면 체력을 30 회복 할 수 있습니다. 남은 포션 : " + player.Postion + "개");
            Console.WriteLine("");
            Console.WriteLine("1. 사용하기");
            Console.WriteLine("0. 나가기");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:// 현재 체력이 최대 체력보다 낮을때 포션 사용 가능 
                    for (int i = 0; i < 3; i++)
                    {
                        if (i >= quests.Count) break;
                        if (questAcceptCheck == 1 && quests[i].Type == RewardType.GOLD3 && quests[i].IsAccept == true)
                        {
                            questClear.QuestClearCheck = 1;
                        }
                    }
                    if (player.Current_Hp <= (player.MaxHp - 31) && player.Postion >= 1)
                    {
                        player.Current_Hp += 30;
                        player.Postion -= 1;
                        Rest("체력이 30 회복되었습니다.");
                    }
                    else if (player.Current_Hp >= (player.MaxHp - 30) && player.Current_Hp <= (player.MaxHp - 1) && player.Postion >= 1)
                    {
                        player.Current_Hp = player.MaxHp;
                        player.Postion -= 1;
                        Rest("체력이 모두 찼습니다.");
                    }
                    else if (player.Postion == 0)
                    {
                        Rest("포션이 부족합니다.");
                    }
                    else
                    {
                        Rest("체력이 이미 가득차있습니다.");
                    }

                    break;
            }
        }
        private void BattlePostion()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 회복 ■");
            Console.WriteLine("포션을 사용하면 체력을 30 회복 할 수 있습니다. 남은 포션 : " + player.Postion + "개");
            Console.WriteLine("");
            Console.WriteLine("1. 사용하기");
            Console.WriteLine("0. 나가기");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    BattleAttack();
                    break;
                case 1:
                    if (player.Current_Hp <= (player.MaxHp - 31) && player.Postion >= 1)
                    {
                        player.Current_Hp += 30;
                        player.Postion -= 1;
                        Console.WriteLine("체력이 30 회복되었습니다.");
                        Thread.Sleep(1000);
                        EnemyPhase();
                    }
                    else if (player.Current_Hp >= (player.MaxHp - 30) && player.Current_Hp <= (player.MaxHp - 1) && player.Postion >= 1)
                    {
                        player.Current_Hp = player.MaxHp;
                        player.Postion -= 1;
                        Console.WriteLine("체력이 모두 찼습니다.");
                        Thread.Sleep(1000);
                        EnemyPhase();
                    }
                    else if (player.Postion == 0)
                    {
                        Console.WriteLine("포션이 부족합니다.");
                        Thread.Sleep(1000);
                        EnemyPhase();
                    }
                    else
                    {
                        Console.WriteLine("체력이 이미 가득차있습니다.");
                        Thread.Sleep(1000);
                        EnemyPhase();
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
            int bonusMp = inventory.Select(item => item.IsEquipped ? item.Mp : 0).Sum();
            if (player.Job == "탱커")
            {
                player.MaxHp = (150 + bonusHp) + (player.Level * 20);
                player.MaxMp = (50 + bonusMp) + (player.Level * 20);
                player.Atk = (10 + bonusAtk) + (player.Level * 2);
                player.Def = (10 + bonusDef) + (player.Level * 3);
            }
            else
            {
                player.MaxHp = (100 + bonusHp) + (player.Level * 20);
                player.MaxMp = (50 + bonusMp) + (player.Level * 20);
                player.Atk = (20 + bonusAtk) + (player.Level * 2);
                player.Def = (5 + bonusDef) + (player.Level * 3);
            }

            ConsoleUtility.PrintTextHighlights("공격력 : ", player.Atk.ToString(), bonusAtk > 0 ? $" (+{bonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", player.Def.ToString(), bonusDef > 0 ? $" (+{bonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("현재 체력 :", player.Current_Hp.ToString() + " / " + player.MaxHp.ToString(), bonusHp > 0 ? $" (+{bonusHp})" : "");
            ConsoleUtility.PrintTextHighlights("현재 MP :", player.Current_Mp.ToString() + " / " + player.MaxMp.ToString(), bonusMp > 0 ? $" (+{bonusMp})" : "");


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

                    if (inventory[KeyInput - 1].Type == ItemType.WEAPON)
                    {
                        for (int j = 0; j < inventory.Count; j++)
                        {
                            if (inventory[j].Type == ItemType.WEAPON)
                            {
                                inventory[j].IsEquipped = false;
                            }
                        }
                        inventory[KeyInput - 1].ToggleEquipStatus();

                    }
                    else if (inventory[KeyInput - 1].Type == ItemType.ARMOR)
                    {
                        for (int j = 0; j < inventory.Count; j++)
                        {
                            if (inventory[j].Type == ItemType.ARMOR)
                            {
                                inventory[j].IsEquipped = false;
                            }
                        }
                        inventory[KeyInput - 1].ToggleEquipStatus();

                    }

                    if (inventory[KeyInput - 1].ToggleEquipStatus != null && quests[0].IsAccept == true && quests[0].Type == RewardType.GOLD1)
                    {
                        questClear.QuestClearCheck = 1;
                    }
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

            player.DungeonFloor();
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine("3. 아이템");
            Console.WriteLine("");
            int choice = ConsoleUtility.PromptMenuChoice(0, 3);

            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    BattleAttack();
                    break;
                case 2:
                    ShowSkills();
                    break;
                case 3:
                    BattlePostion();
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
            Console.WriteLine($"MP :{player.Current_Mp}/{player.MaxMp}");
            Console.WriteLine("");
            Console.WriteLine("0. 도망가기");
            Console.WriteLine($"{RandomMonster.randmonsters.Count + 1} . 스킬사용");
            Console.WriteLine($"{RandomMonster.randmonsters.Count + 2} . 아이템 사용");
            Console.WriteLine("");
            int SelectMonster = ConsoleUtility.PromptMenuChoice(0, RandomMonster.randmonsters.Count + 2);
            //랜덤 공격력
            Random randatk = new Random();
            double attackPower = player.Atk * (1 - 0.1 * randatk.NextDouble());
            attackPower = Math.Ceiling(attackPower);
            int Run = randatk.Next(0, 10);
            // 치명타 발생 여부 결정
            bool isCritical = new Random().Next(100) < 15;

            // 회피 발생 여부 결정
            bool isDodge = new Random().Next(100) < 10;

            // 치명타 발생 시 공격력 증가
            if (isCritical)
            {
                attackPower *= 1.6;
                attackPower = (int)Math.Ceiling(attackPower); // 정수로 변환
            }
            else
            {
                attackPower = (int)Math.Ceiling(attackPower); // 정수로 변환
            }

            switch (SelectMonster)
            {
                case 0:
                    if (Run > 2)
                    {
                        Console.WriteLine("도주에 성공 했습니다.");
                        Console.WriteLine("마을로 돌아갑니다.");
                        Thread.Sleep(2000);
                        MainMenu();
                    }
                    else
                    {
                        Console.WriteLine("도주에 실패 했습니다.");
                        Console.WriteLine("선공권을 빼앗겼습니다");
                        Thread.Sleep(2000);
                        EnemyPhase();
                    }
                    break;

                default:
                    if (SelectMonster == RandomMonster.randmonsters.Count + 1)
                    {
                        ShowSkills();
                    }
                    if (SelectMonster == RandomMonster.randmonsters.Count + 2)
                    {
                        BattlePostion();
                    }
                    if (RandomMonster.randmonsters[SelectMonster - 1].Hp >= 0 && RandomMonster.randmonsters[SelectMonster - 1].IsAlive)
                    {
                        Console.Clear();
                        ConsoleUtility.ShowTitle("Battle!!");
                        Console.WriteLine("");
                        Console.WriteLine($"{player.Name}의 공격!");
                        if (isDodge)
                        {
                            Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");
                        }
                        else
                        {
                            if (isCritical)
                            {
                                Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}을(를) 공격했습니다. [데미지] : {attackPower} - 치명타 공격!!");
                            }
                            else
                            {
                                Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}을(를) 공격했습니다. [데미지] : {attackPower}");
                            }
                        }
                        Console.WriteLine("");
                        // 회피가 발생하지 않은 경우에만 공격 처리
                        if (!isDodge)
                        {
                            // 공격 결과 계산 및 출력
                            RandomMonster selectedMonster = RandomMonster.randmonsters[SelectMonster - 1];
                            int monsterhp = selectedMonster.Hp - (int)attackPower;
                            if (monsterhp <= 0)
                            {
                                // 몬스터 사망 처리
                                Console.WriteLine($"{RandomMonster.randmonsters[SelectMonster - 1].Name}");
                                Console.WriteLine($"Hp : {RandomMonster.randmonsters[SelectMonster - 1].Hp} -> Dead");
                                selectedMonster.Hp = selectedMonster.Hp - (int)attackPower;
                                Console.WriteLine("");
                                Console.WriteLine("0. 다음");
                                Console.WriteLine("");
                                selectedMonster.IsAliveToggle();
                                for (int i = 0; i < 3; i++)
                                {
                                    if (i >= quests.Count) break;
                                    if (questAcceptCheck == 1 && quests[i].Type == RewardType.GOLD2 && quests[i].IsAccept == true)
                                    {
                                        monsterCount++;
                                        questClear.QuestCount(monsterCount, ref questClearCheck);
                                    }
                                    else if (questAcceptCheck == 1 && quests[i].Type == RewardType.ITEM4 && quests[i].IsAccept == true && player.floor >= 4)
                                    {
                                        monsterCount++;
                                        questClear.QuestCount(monsterCount, ref questClearCheck);
                                    }
                                    else if (questAcceptCheck == 1 && quests[i].Type == RewardType.ITEM5 && quests[i].IsAccept == true && player.floor >= 4 && RandomMonster.randmonsters[SelectMonster - 1].Name == "장로드래곤")
                                    {
                                        monsterCount++;
                                        questClear.QuestCount(monsterCount, ref questClearCheck);
                                    }
                                }
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
                                // 몬스터가 살아있는 경우에는 Hp 갱신 후 다음 단계로 진행
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
                            // 회피가 발생한 경우에는 다음 단계로 진행
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

        private void ShowSkills()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("스킬 선택");

            // 스킬 목록 출력
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"H :{player.Current_Hp}/{player.MaxHp} ");
            Console.WriteLine($"MP :{player.Current_Mp}/{player.MaxMp}");


            Console.WriteLine("");
            Console.WriteLine("1. 알파 스트라이크 - MP 10");
            Console.WriteLine("2. 더블 스트라이크 - MP 15");
            Console.WriteLine("0. 취소");
            Console.WriteLine("");
            int skillChoice = ConsoleUtility.PromptMenuChoice(0, 2);

            switch (skillChoice)
            {
                case 1:
                    if (player.Current_Mp >= 10)
                    {
                        player.Current_Mp -= 10;
                        SelectMonsterForSkill(1); // 알파 스트라이크 선택
                    }
                    else
                    {
                        Console.WriteLine("MP가 부족합니다.");
                        Thread.Sleep(2000);
                        ShowSkills();
                    }
                    break;
                case 2:
                    if (player.Current_Mp >= 15)
                    {
                        player.Current_Mp -= 15;
                        SelectMonsterForSkill(2); // 더블 스트라이크 선택
                    }
                    else
                    {
                        Console.WriteLine("MP가 부족합니다.");
                        Thread.Sleep(2000);
                        ShowSkills();
                    }
                    break;
                case 0:
                    Battle();
                    break;
            }
        }
        private void SelectMonsterForSkill(int skillChoice)
        {
            Console.WriteLine("스킬을 사용할 몬스터를 선택하세요: ");
            Console.WriteLine(" ");

            for (int i = 0; i < RandomMonster.randmonsters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {RandomMonster.randmonsters[i].Name} - HP: {RandomMonster.randmonsters[i].Hp}");
            }

            Console.WriteLine("0. 돌아가기");
            Console.WriteLine(" ");

            int monsterChoice = ConsoleUtility.PromptMenuChoice(0, RandomMonster.randmonsters.Count);

            if (monsterChoice == 0)
            {
                Battle(); // 돌아가기 선택 시 전투 메뉴로 돌아감
            }
            else
            {
                // 선택한 몬스터에 스킬 적용
                ApplySkillToMonster(monsterChoice, skillChoice);
            }
        }

        private void ApplySkillToMonster(int monsterIndex, int skillChoice)
        {
            // 선택한 몬스터에 스킬을 적용하고 결과 출력
            double attackPower = 0;

            if (skillChoice == 1)
            {
                attackPower = player.Atk * 2; // 알파 스트라이크의 공격력은 플레이어의 공격력의 두 배
                Console.WriteLine($"{player.Name}의 알파 스트라이크!");
            }
            else if (skillChoice == 2)
            {
                attackPower = player.Atk * 1.5; // 더블 스트라이크의 공격력은 플레이어의 공격력의 1.5배
                Console.WriteLine($"{player.Name}의 더블 스트라이크!");
            }

            // 선택한 몬스터에게 공격력 적용
            RandomMonster selectedMonster = RandomMonster.randmonsters[monsterIndex - 1];
            int monsterhp = selectedMonster.Hp - (int)attackPower;

            // 공격 결과 출력
            if (monsterhp <= 0)
            {
                // 몬스터 사망 처리
                Console.WriteLine($"{selectedMonster.Name}을(를) 처치했습니다!");
                Console.WriteLine($"HP : {selectedMonster.Hp} -> Dead");
                selectedMonster.Hp = 0;
                selectedMonster.IsAliveToggle();

                Console.WriteLine(" ");
                Console.WriteLine("0. 다음");
                Console.WriteLine(" ");
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
                // 몬스터가 살아있는 경우에만 Hp 갱신 후 다음 단계로 진행
                Console.WriteLine($"{selectedMonster.Name}");
                Console.WriteLine($"HP : {selectedMonster.Hp} -> HP : {monsterhp}");
                selectedMonster.Hp = monsterhp;
                Console.WriteLine(" ");
                Console.WriteLine("0. 다음");
                int choice = ConsoleUtility.PromptMenuChoice(0, 0);
                switch (choice)
                {
                    case 0:
                        EnemyPhase();
                        break;
                    default:
                        Console.WriteLine("다시 입력해주세요.");
                        break;
                }
            }

            Thread.Sleep(2000);
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
                    Console.WriteLine($"{player.Name}을(를) 공격했습니다. [데미지] : {RandomMonster.randmonsters[i].Atk}" + $" , {(player.Def * 0.5)} 만큼 방어!");
                    Console.WriteLine($"[최종 데미지] : {RandomMonster.randmonsters[i].Atk - (player.Def * 0.5)}");
                    if ((int)MattackPower - (player.Def * 0.5) <= 0)
                    {
                        Console.WriteLine("공격을 완벽하게 방어했습니다!");
                    }
                    else
                    {
                        player.Current_Hp = Math.Max(player.Current_Hp - ((int)MattackPower - (player.Def / 2)), 0);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("[내 정 보]");
                    Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
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
            RandomMonster.MonsterDropTable();
            ConsoleUtility.ShowTitle("Battle - Result");
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("Victory");
            Console.WriteLine("");
            Console.WriteLine($"던전에서 몬스터 {RandomMonster.randmonsters.Count}마리를 잡았습니다");
            Console.WriteLine("");
            for (int i = 0; i < RandomMonster.randmonsters.Count; i++)
            {
                if (player.Exp >= 0)
                {
                    player.GetExp(RandomMonster.randmonsters[i].Level);
                }
            }
            player.PlayerLevelUp();
            player.DungeonResult();
            player.PlayerLevelUp();
            player.DungeonResult();
            Console.WriteLine("");
            Console.WriteLine($"0. 다음층으로 올라가지 않는다 {player.floor}층 -> {player.floor}층");
            Console.WriteLine($"1. 다음층으로 올라간다 {player.floor}층 -> {player.floor + 1}층");
            Console.WriteLine("");

            int choice = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    player.floor = player.floor + 1;
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("다시 입력해주세요");
                    break;
            }
        }

        private void Quest(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }
            Console.Clear();
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("퀘스트!");
            Console.WriteLine("");
            for (int i = 0; i < 3; i++)
            {
                if (i >= quests.Count)
                {
                    break;
                }
                Console.WriteLine($"{i + 1} . {quests[i].QuestName}");
            }
            Console.WriteLine("");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine("");
            int choice = ConsoleUtility.PromptMenuChoice(0, quests.Count);
            if (choice == 0)
            {
                MainMenu();
            }
            Console.Clear();
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("퀘스트목록");
            Console.WriteLine("");
            Console.WriteLine($"{quests[choice - 1].QuestName}");
            Console.WriteLine("");
            Console.WriteLine($"{quests[choice - 1].QuestDetail}");
            Console.WriteLine("");
            questClear.QuestClearRequest(quests[choice - 1].Type);
            Console.WriteLine("");
            questsReward.QuestRewardType(quests[choice - 1].Type);
            Console.WriteLine("");
            if (quests[choice - 1].IsAccept == false)
            {
                Console.WriteLine("0. 돌아가기");
                Console.WriteLine("1. 수락");
            }
            else if (quests[choice - 1].IsAccept == true)
            {
                Console.WriteLine("0. 돌아가기");
                Console.WriteLine("1. 보상 받기");
            }
            Console.WriteLine("");
            for (int i = 0; i < 3; i++)
            {
                if (i >= quests.Count) break;
                if (questAcceptCheck == 1 && quests[i].IsAccept == true && quests[i].Type == RewardType.GOLD4)
                {
                    if (player.floor > 1)
                    {
                        questClear.QuestClearCheck = 1;
                    }
                }
                else if (questAcceptCheck == 1 && quests[i].IsAccept == true && quests[i].Type == RewardType.ITEM1)
                {
                    if (player.Def >= 20)
                    {
                        questClear.QuestClearCheck = 1;
                    }
                }
                else if (questAcceptCheck == 1 && quests[i].IsAccept == true && quests[i].Type == RewardType.ITEM2)
                {
                    if (player.Atk >= 30)
                    {
                        questClear.QuestClearCheck = 1;
                    }
                }
                else if (questAcceptCheck == 1 && quests[i].IsAccept == true && quests[i].Type == RewardType.GOLD3)
                {
                    if (player.floor >= 4)
                    {
                        questClear.QuestClearCheck = 1;
                    }
                }
            }
            questClear.QuestCount(monsterCount, ref questClearCheck);
            int accept = ConsoleUtility.PromptMenuChoice(0, 1);
            switch (accept)
            {
                case 0:
                    Quest();
                    break;
                case 1:
                    if (quests[choice - 1].IsAccept == false)
                    {
                        if (questAcceptCheck == 0)
                        {
                            Console.WriteLine("퀘스트 수락 완료!");
                            quests[choice - 1].IsAccept = true;
                            questAcceptCheck = 1;
                            Thread.Sleep(1000);
                        }
                        else if (questAcceptCheck == 1)
                        {
                            Console.WriteLine("퀘스트를 이미 받으신 상태입니다!");
                            Thread.Sleep(1000);
                        }
                        Quest();
                    }

                    else if (quests[choice - 1].IsAccept == true)
                    {
                        if (questClearCheck == 1)
                        {
                            Console.WriteLine("보상을 받으셨습니다.");
                            if (quests[choice - 1].Type == RewardType.GOLD1) { questClear.QuestClearRewardGold(quests[choice - 1].Type, player, 1000); }
                            else if (quests[choice - 1].Type == RewardType.GOLD2) { questClear.QuestClearRewardGold(quests[choice - 1].Type, player, 2000); }
                            else if (quests[choice - 1].Type == RewardType.GOLD3) { questClear.QuestClearRewardGold(quests[choice - 1].Type, player, 3000); }
                            else if (quests[choice - 1].Type == RewardType.GOLD4) { questClear.QuestClearRewardGold(quests[choice - 1].Type, player, 4000); }
                            else if (quests[choice - 1].Type == RewardType.ITEM1) { inventory.Add(new Item("파수꾼의 갑옷", "견고 합니다", ItemType.ARMOR, 0, 20, 0, 5000)); }
                            else if (quests[choice - 1].Type == RewardType.ITEM2) { inventory.Add(new Item("톱날 단검", "날이 톱으로 되어있는 단검입니다", ItemType.WEAPON, 15, 0, 0, 5000)); }
                            else if (quests[choice - 1].Type == RewardType.ITEM3) { inventory.Add(new Item("암흑의 인장", "적을 처치하고 자신의 영광을 증명하세요", ItemType.ARMOR, 20, 20, 50, 50000)); }
                            else if (quests[choice - 1].Type == RewardType.ITEM4) { inventory.Add(new Item("망자의 갑옷", "이갑옷은 당신이 어떤 전장을 해쳐왔는지 증명합니다", ItemType.ARMOR, 0, 80, 100, 500000)); }
                            else if (quests[choice - 1].Type == RewardType.ITEM5) { inventory.Add(new Item("무한의 대검", "당신을 막을수는없다는걸 말대신 이칼로 증명합니다", ItemType.WEAPON, 80, 0, 0, 500000)); }
                            Thread.Sleep(1000);
                            quests.RemoveAt(choice - 1);
                            monsterCount = 0;
                            monster2Count = 0;
                            questAcceptCheck = 0;
                            questClearCheck = 0;
                            questClear.QuestClearCheck = 0;
                        }
                        Quest();
                    }
                    Thread.Sleep(1000);
                    break;
            }

        }

        public void SavePlayerData()
        {
            Console.Clear();
            DataMamager<Player>.Save(player, "playerfile.json");
            DataMamager<List<Item>>.Save(inventory, "itemfile.json");
            Console.WriteLine("플레이어 정보가 저장되었습니다.");
            Thread.Sleep(2000);
            MainMenu();
        }
        public void ReName()
        {
            Console.Clear();
            Console.Write("이름을 설정해주세요. : ");
            player.Name = Console.ReadLine();
            Console.WriteLine($"플레이어 이름이{player.Name}으로 변경되었습니다.");
            Thread.Sleep(2000);
            MainMenu();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
            //gameManager.SavePlayerData();
        }
    }
}

