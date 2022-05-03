﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Diagnostics;
using OpenTK.Platform.Windows;
using Un4seen.Bass;
using System.IO;
using System.Drawing.Imaging;

namespace GHtest1 {
    class Records {
        public int ver = 1;
        public int offset;
        public int[] accuracy;
        public int[] p50;
        public int[] p100;
        public int[] p200;
        public int[] p300;
        public int[] pMax;
        public int[] fail;
        public bool[] easy;
        public bool[] nofail;
        public int[] speed;
        public int[] mode;
        public int[] hidden;
        public bool[] hard;
        public int[] score;
        public int[] rank;
        public int[] streak;
        public int players;
        public string time;
        public string[] name;
        public string[] diff;
        public string path;
        public int totalScore;
        public bool failsong;
        public Records() { }
    }
    class MainMenu {
        public static Key volumeUpKey = Key.O;
        public static Key volumeDownKey = Key.L;
        public static Key songPauseResumeKey = Key.U;
        public static Key songNextKey = Key.I;
        public static Key songPrevKey = Key.Y;

        public static double volumePopUpTime = 10000;
        public static double songPopUpTime = 0;
        public static List<int> songIdsList = new List<int>();
        public static int songListIndex = 0;

        public static bool drawMenuBackgroundFx = false;
        public static bool isDebugOn = false;
        public static double menuFadeOut = 0f;
        public static List<Records> records = new List<Records>();
        public static game gameObj;
        public static bool[] playerOnOptions = new bool[4] { false, false, false, false };
        public static bool[] playerProfileReady = new bool[4] { false, false, false, false };
        public static int[] playerProfileSelect = new int[4] { 0, 0, 0, 0 };
        public static int[] playerProfileSelect2 = new int[4] { 0, 0, 0, 0 };
        public static bool[] playerOn2Menu = new bool[4] { false, false, false, false };
        public static PlayerInfo[] playerInfos;
        public static int playerAmount = 1;
        public static string[] profilesPath = new string[0];
        public static string[] profilesName = new string[0];
        public static Font font = new Font(FontFamily.GenericSansSerif, 20);
        public static float input1 = 1;
        public static float input2 = 0;
        public static float input3 = 0;
        public static float input4 = 0;
        public static bool Menu = true;
        public static bool Editor = false;
        public static bool Game = false;
        public static double songChangeFade = 0;
        public static double[] menuTextFadeTime = new double[4];
        public static float[] menuTextFadeStart = new float[4];
        public static float[] menuTextFadeEnd = new float[4] { 1, 0, 0, 0 };
        public static float[] menuTextFadeNow = new float[4];
        public static bool animationOnToGame = false;
        public static Texture2D oldBG = new Texture2D(0, 0, 0);
        public static Stopwatch animationOnToGameTimer = new Stopwatch();
        public static Audio.StreamArray song = new Audio.StreamArray();
        public static Texture2D album = new Texture2D(0, 0, 0);
        static bool typingQuery = false;
        static string searchQuery = "";
        public static void MenuInput(GuitarButtons gg, int gtype, int player) {
            MenuIn(gg, gtype, player);
        }
        static public void MenuInputRawGamepad(int button) {
            if ((menuWindow == 6) && subOptionSelect > 1 && onSubOptionItem) {
                if (subOptionSelect < 26) {
                    if (button >= 500)
                        return;
                    Console.WriteLine("Key Enter");
                    int player = controllerBindPlayer - 1;
                    if (subOptionSelect == 14) playerInfos[player].ggreen = Input.lastGamePadButton;
                    if (subOptionSelect == 15) playerInfos[player].gred = Input.lastGamePadButton;
                    if (subOptionSelect == 16) playerInfos[player].gyellow = Input.lastGamePadButton;
                    if (subOptionSelect == 17) playerInfos[player].gblue = Input.lastGamePadButton;
                    if (subOptionSelect == 18) playerInfos[player].gorange = Input.lastGamePadButton;
                    //
                    if (subOptionSelect == 19) playerInfos[player].gopen = Input.lastGamePadButton;
                    if (subOptionSelect == 20) playerInfos[player].gsix = Input.lastGamePadButton;
                    if (subOptionSelect == 21) playerInfos[player].gstart = Input.lastGamePadButton;
                    if (subOptionSelect == 22) playerInfos[player].gselect = Input.lastGamePadButton;
                    if (subOptionSelect == 23) playerInfos[player].gup = Input.lastGamePadButton;
                    if (subOptionSelect == 24) playerInfos[player].gdown = Input.lastGamePadButton;
                    if (subOptionSelect == 25) playerInfos[player].gwhammy = Input.lastGamePadButton;
                    onSubOptionItem = false;
                    waitInput = true;
                    return;
                } else {
                    if (button >= 500) {
                        Console.WriteLine("Axis Enter");
                        int player = controllerBindPlayer - 1;
                        if (subOptionSelect == 26) playerInfos[player].gWhammyAxis = Input.lastGamePadButton;
                        onSubOptionItem = false;
                        waitInput = true;
                    }
                }
            }
        }
        static public void MenuInputRaw(Key key) {
#if DEBUG
            Console.WriteLine("KeyCode: " + (int)key + ", Name: " + key);
            if (key == Key.Menu) {
                isDebugOn = !isDebugOn;
                MainGame.useMatrix = false;
            }
#endif
            if (isDebugOn) {
                if (key == Key.Home) {
                    if (songLoad.IsAlive) {
                        songLoad.Abort();
                        Console.WriteLine("Stop song from load");
                    }
                }
                if (key == Key.F1) {
                    MainGame.showSyncBar = !MainGame.showSyncBar;
                }
                if (key == Key.F2) {
                    MainGame.showNotesPositions = !MainGame.showNotesPositions;
                }
                if (key == Key.F3) {
                    Difficulty.DiffCalcDev = true;
                    if (playerInfos[0].difficulty < Song.songList[songselected].dificulties.Length)
                        Difficulty.CalcDifficulty(0, 10, Song.loadSongthread(true, 0, Song.songList[songselected], Song.songList[songselected].dificulties[playerInfos[0].difficulty]));
                    Difficulty.DiffCalcDev = false;
                }
                if (key == Key.F4) {
                    if (!Menu)
                        EndGame();
                    else
                        game.Closewindow();
                    return;
                }
                if (key == Key.F5) {
                    Textures.load();
                }
                if (key == Key.F6) {
                    song.setPos(song.getTime() - (song.length * 1000) / 20);
                    Song.notes[0] = Song.notesCopy.ToList();
                    Song.beatMarkers = Song.beatMarkersCopy.ToList();
                    MainGame.CleanNotes();
                    return;
                }
                if (key == Key.F7) {
                    song.Pause();
                    return;
                }
                if (key == Key.F8) {
                    song.play();
                    return;
                }
                if (key == Key.F9) {
                    song.setPos(song.getTime() + (song.length * 1000) / 20);
                    return;
                }
                if (key == Key.F10) {
                    bool k = Audio.keepPitch;
                    Audio.keepPitch = false;
                    song.setVelocity(false, 0.5f);
                    Audio.keepPitch = k;
                    game.timeSpeed = 0.5f;
                    return;
                }
                if (key == Key.F11) {
                    bool k = Audio.keepPitch;
                    Audio.keepPitch = false;
                    song.setVelocity(false, 0.1f);
                    Audio.keepPitch = k;
                    game.timeSpeed = 0.1f;
                    return;
                }
                if (key == Key.F12) {
                    bool k = Audio.keepPitch;
                    Audio.keepPitch = false;
                    song.setVelocity(false, 1f);
                    Audio.keepPitch = k;
                    game.timeSpeed = 1f;
                    return;
                }
                if (key == Key.Pause) {
                    MainGame.useMatrix = !MainGame.useMatrix;
                }
                Console.WriteLine(key);
            }
            if (typingQuery) {
                if ((int)key >= (int)Key.A && (int)key <= (int)Key.Z) {
                    searchQuery += key;
                } else if ((int)key >= (int)Key.Number0 && (int)key <= (int)Key.Number9) {
                    searchQuery += (char)((int)'0' + ((int)key - (int)Key.Number0));
                } else if (key == Key.Space) {
                    searchQuery += " ";
                } else if (key == Key.BackSpace) {
                    if (searchQuery.Length > 0)
                        searchQuery = searchQuery.Substring(0, searchQuery.Length - 1);
                } else if (key == Key.Enter) {
                    typingQuery = false;
                    //searchQuery.ToUpper();
                    int q = SongScan.SearchSong(songselected, searchQuery);
                    if (q >= 0) {
                        songselected = q;
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange(true);
                    }
                }
                //searchQuery = searchQuery.ToLower();
                return;
            }
            if (!Game) {
                if (key == volumeDownKey) {
                    Audio.masterVolume -= 0.05f;
                    if (Audio.masterVolume < 0f)
                        Audio.masterVolume = 0f;
                    volumePopUpTime = 0.0;
                    SaveChanges();
                    song.setVolume();
                    Sound.setVolume();
                } else if (key == volumeUpKey) {
                    Audio.masterVolume += 0.05f;
                    if (Audio.masterVolume > 1f)
                        Audio.masterVolume = 1f;
                    volumePopUpTime = 0.0;
                    SaveChanges();
                    song.setVolume();
                    Sound.setVolume();
                } else if (key == songPauseResumeKey) {
                    pauseSong();
                } else if (key == songNextKey) {
                    nextSong();
                } else if (key == songPrevKey) {
                    prevSong();
                }
            }
            if (creatingNewProfile) {
                if ((int)key >= (int)Key.A && (int)key <= (int)Key.Z) {
                    newProfileName += key;
                } else if ((int)key >= (int)Key.Number0 && (int)key <= (int)Key.Number9) {
                    newProfileName += (char)((int)'0' + ((int)key - (int)Key.Number0));
                } else if (key == Key.Space) {
                    newProfileName += " ";
                } else if (key == Key.BackSpace) {
                    if (newProfileName.Length > 0)
                        newProfileName = newProfileName.Substring(0, newProfileName.Length - 1);
                } else if (key == Key.Enter) {
                    creatingNewProfile = false;
                    CreateProfile();
                    game.LoadProfiles();
                    newProfileName = "";
                } else if (key == Key.Escape) {
                    creatingNewProfile = false;
                    newProfileName = "";
                }
                newProfileName = newProfileName.ToLower();
                return;
            }
            if (menuWindow == 6 && subOptionSelect > 1 && onSubOptionItem) {
                Console.WriteLine("Key Enter");
                if (Input.lastKey == Key.Escape) {
                    onSubOptionItem = false;
                    waitInput = true;
                    return;
                }
                int player = controllerBindPlayer - 1;
                if (subOptionSelect == 2) playerInfos[player].green = Input.lastKey;
                if (subOptionSelect == 3) playerInfos[player].red = Input.lastKey;
                if (subOptionSelect == 4) playerInfos[player].yellow = Input.lastKey;
                if (subOptionSelect == 5) playerInfos[player].blue = Input.lastKey;
                if (subOptionSelect == 6) playerInfos[player].orange = Input.lastKey;
                //
                if (subOptionSelect == 7) playerInfos[player].open = Input.lastKey;
                if (subOptionSelect == 8) playerInfos[player].six = Input.lastKey;
                if (subOptionSelect == 9) playerInfos[player].start = Input.lastKey;
                if (subOptionSelect == 10) playerInfos[player].select = Input.lastKey;
                if (subOptionSelect == 11) playerInfos[player].up = Input.lastKey;
                if (subOptionSelect == 12) playerInfos[player].down = Input.lastKey;
                if (subOptionSelect == 13) playerInfos[player].whammy = Input.lastKey;
                onSubOptionItem = false;
                waitInput = true;
                return;
            }
            if (menuWindow == 3 && onSubOptionItem && optionsSelect == 2) {
                if (Input.lastKey == Key.Escape) {
                    onSubOptionItem = false;
                    return;
                }

                if (subOptionSelect == 0) volumeUpKey = Input.lastKey;
                else if (subOptionSelect == 1) volumeDownKey = Input.lastKey;
                else if (subOptionSelect == 2) songPrevKey = Input.lastKey;
                else if (subOptionSelect == 3) songPauseResumeKey = Input.lastKey;
                else if (subOptionSelect == 4) songNextKey = Input.lastKey;

                onSubOptionItem = false;
                return;
            }
        }
        static void nextSong() {
            if (SongScan.songsScanned == 0)
                return;
            if (songIdsList.Count - 1 > songListIndex) {
                songListIndex++;
                songselected = songIdsList[songListIndex];
            } else {
                songselected = new Random().Next(0, Song.songList.Count);
                songIdsList.Add(songselected);
                songListIndex = songIdsList.Count - 1;
            }
            if (songChangeFadeUp != 0)
                songChangeFadeDown = 0;
            songChangeFadeUp = 0;
            songChangeFadeWait = 0;
            songChange(false);
            showSongList();
            songPopUpTime = 0;
        }
        static void prevSong() {
            if (SongScan.songsScanned == 0)
                return;
            if (songListIndex == 0) {
                songselected = new Random().Next(0, Song.songList.Count);
                songIdsList.Insert(0, songselected);
            } else {
                songListIndex--;
                songselected = songIdsList[songListIndex];
            }
            if (songChangeFadeUp != 0)
                songChangeFadeDown = 0;
            songChangeFadeUp = 0;
            songChangeFadeWait = 0;
            songChange(false);
            showSongList();
            songPopUpTime = 0;
        }
        static void pauseSong() {
            if (song.isPaused)
                song.Resume();
            else
                song.Pause();
            songPopUpTime = 0;
        }
        static void showSongList() {
            if (SongScan.songsScanned != 0) {
                Console.WriteLine("----Song List-----");
                for (int i = 0; i < songIdsList.Count; i++) {
                    if (i == songListIndex)
                        Console.Write("> ");
                    if (songIdsList[i] < Song.songList.Count)
                        Console.WriteLine("\t " + songIdsList[i] + ", " + Song.songList[songIdsList[i]].Name);
                }
                Console.WriteLine("------------------");
            }
        }
        static bool waitInput = false;
        static bool[] goingDown = new bool[4] { false, false, false, false };
        static bool[] goingUp = new bool[4] { false, false, false, false };
        static public bool mouseClicked = false;
        static public bool creatingNewProfile = false;
        static public string newProfileName = "";
        static public void MouseClick() {
            mouseClicked = true;
            if (Editor)
                return;
            if (menuWindow == 0) {
                mouseClicked = false;
                if (mainMenuSelect == 0)
                    menuWindow = 8;
                else if (mainMenuSelect == 1) {
                    Editor = true;
                    Menu = false;
                    EditorScreen.Start();
                    int x = 1;
                } else if (mainMenuSelect == 2) {
                    menuWindow = 2;
                    setOptionsValues();
                } else if (mainMenuSelect == 3)
                    game.Closewindow();
            }
        }
        public static void MenuIn(GuitarButtons g, int type, int player) {
            if (typingQuery || creatingNewProfile)
                return;
            player--;
            if (Game)
                return;
            if (optionsSelect == 2 && onSubOptionItem) {
                return;
            }
            if (waitInput) {
                waitInput = false;
                return;
            }
            menuFadeOut = 0f;
            if (g == GuitarButtons.start) {
                if (type == 0) {
                    playerOnOptions[player] = !playerOnOptions[player];
                    if (playerOnOptions[player]) {
                        playerProfileSelect[player] = 0;
                        playerProfileSelect2[player] = 0;
                        playerOn2Menu[player] = false;
                    } else {
                        if (!playerProfileReady[player]) {
                            Input.ignore = Input.controllerIndex[player];
                            Input.controllerIndex[player] = -1;
                        }
                    }
                }
            }
            if (playerInfos[player].leftyMode && type != 2) {
                if (g == GuitarButtons.up)
                    g = GuitarButtons.down;
                else if (g == GuitarButtons.down)
                    g = GuitarButtons.up;
            }
            if (playerOnOptions[player]) {
                if (type == 0) {
                    if (g == GuitarButtons.blue)
                        if (playerProfileReady[player])
                            playerOn2Menu[player] = !playerOn2Menu[player];
                    if (g == GuitarButtons.up) {
                        if (!playerOn2Menu[player]) {
                            playerProfileSelect[player]--;
                            if (playerProfileSelect[player] < 0)
                                playerProfileSelect[player] = 0;
                        } else {
                            playerProfileSelect2[player]--;
                            if (playerProfileSelect2[player] < 0)
                                playerProfileSelect2[player] = 0;
                        }
                    }
                    if (g == GuitarButtons.down) {
                        if (!playerProfileReady[player]) {
                            playerProfileSelect[player]++;
                            if (playerProfileSelect[player] >= profilesPath.Length + 1)
                                playerProfileSelect[player] = profilesPath.Length;
                        } else {
                            if (!playerOn2Menu[player]) {
                                playerProfileSelect[player]++;
                                if (playerProfileSelect[player] > 11)
                                    playerProfileSelect[player] = 11;
                            } else {
                                playerProfileSelect2[player]++;
                                if (playerProfileSelect2[player] > 0)
                                    playerProfileSelect2[player] = 0;
                            }
                        }
                    }
                    if (g == GuitarButtons.blue) {
                        if (!playerProfileReady[player]) {
                            game.LoadProfiles();
                        }
                    }
                    if (g == GuitarButtons.red) {
                        playerOnOptions[player] = false;
                        if (!playerProfileReady[player])
                            Input.ignore = Input.controllerIndex[player];
                    }
                    if (g == GuitarButtons.yellow) {
                        if (!playerProfileReady[player]) {
                            if (playerProfileSelect[player] == 0) {
                            } else {
                                string path = profilesPath[playerProfileSelect[player] - 1];
                                Console.WriteLine("delete: " + path);
                                if (!playerProfileReady[0]) playerProfileSelect[0] = 0;
                                if (!playerProfileReady[1]) playerProfileSelect[1] = 0;
                                if (!playerProfileReady[2]) playerProfileSelect[2] = 0;
                                if (!playerProfileReady[3]) playerProfileSelect[3] = 0;
                                if (File.Exists(path)) {
                                    File.Delete(path);
                                }
                                while (File.Exists(path)) ;
                                game.LoadProfiles();
                            }
                        }
                    }
                    if (g == GuitarButtons.green) {
                        if (!playerProfileReady[player]) {
                            if (playerProfileSelect[player] == 0) {
                                //playerOnOptions[player] = false;
                                newProfileName = "";
                                creatingNewProfile = true;
                            } else {
                                playerInfos[player] = new PlayerInfo(player + 1, profilesPath[playerProfileSelect[player] - 1]);
                                Console.WriteLine("path: " + profilesPath[playerProfileSelect[player] - 1]);
                                playerProfileReady[player] = true;
                                playerOnOptions[player] = false;
                            }
                        } else {
                            if (!playerOn2Menu[player]) {
                                if (playerProfileSelect[player] == 0) {
                                    playerInfos[player].HardRock = !playerInfos[player].HardRock;
                                    if (playerInfos[player].HardRock)
                                        playerInfos[player].Easy = false;
                                } else if (playerProfileSelect[player] == 1) {
                                    if (playerInfos[player].Hidden == 0)
                                        playerInfos[player].Hidden = 1;
                                    else if (playerInfos[player].Hidden == 1)
                                        playerInfos[player].Hidden = 0;
                                } else if (playerProfileSelect[player] == 2) {
                                    playerInfos[player].autoPlay = !playerInfos[player].autoPlay;
                                } else if (playerProfileSelect[player] == 3) {
                                    playerInfos[player].Easy = !playerInfos[player].Easy;
                                    if (playerInfos[player].Easy)
                                        playerInfos[player].HardRock = false;
                                } else if (playerProfileSelect[player] == 4) {
                                    playerInfos[0].gameplaySpeed += 0.10f;
                                    if (playerInfos[0].gameplaySpeed > 2.05f)
                                        playerInfos[0].gameplaySpeed = 0.5f;
                                } else if (playerProfileSelect[player] == 5) {
                                    playerInfos[player].noteModifier += 1;
                                    if (playerInfos[player].noteModifier > 3)
                                        playerInfos[player].noteModifier = 0;
                                } else if (playerProfileSelect[player] == 6) {
                                    playerInfos[player].noFail = !playerInfos[player].noFail;
                                } else if (playerProfileSelect[player] == 7) {
                                    MainGame.performanceMode = !MainGame.performanceMode;
                                } else if (playerProfileSelect[player] == 8) {
                                    playerInfos[player].transform = !playerInfos[player].transform;
                                } else if (playerProfileSelect[player] == 9) {
                                    playerInfos[player].autoSP = !playerInfos[player].autoSP;
                                } else if (playerProfileSelect[player] == 10) {
                                    playerInfos[player].inputModifier += 1;
                                    if (playerInfos[player].inputModifier > 4)
                                        playerInfos[player].inputModifier = 0;
                                } else if (playerProfileSelect[player] == 11) {
                                    playerProfileReady[player] = false;
                                    playerOnOptions[player] = false;
                                    playerInfos[player] = new PlayerInfo(player + 1);
                                    if (player == 0) {
                                        if (Input.controllerIndex[player] != 2)
                                            Input.ignore = Input.controllerIndex[player];
                                        Input.controllerIndex[player] = -1;
                                    }
                                }
                                playerInfos[player].modMult = CalcModMult(player);
                            } else {
                                if (playerProfileSelect2[player] == 0) {
                                    if (Gameplay.pGameInfo[player].gameMode == GameModes.Normal)
                                        Gameplay.pGameInfo[player].gameMode = GameModes.Mania;
                                    else if (Gameplay.pGameInfo[player].gameMode == GameModes.Mania)
                                        Gameplay.pGameInfo[player].gameMode = GameModes.New;
                                    else if (Gameplay.pGameInfo[player].gameMode == GameModes.New)
                                        Gameplay.pGameInfo[player].gameMode = GameModes.Normal;
                                } else if (playerProfileSelect2[player] == 1) {
                                    if (playerInfos[player].instrument == Instrument.Fret5)
                                        playerInfos[player].instrument = Instrument.Drums;
                                    else if (playerInfos[player].instrument == Instrument.Drums)
                                        playerInfos[player].instrument = Instrument.Fret5;
                                }
                            }
                        }
                    }
                    if (g == GuitarButtons.red) {
                        if (!playerProfileReady[player]) {
                            Input.controllerIndex[player] = -1;
                            playerOnOptions[player] = false;
                        }
                    }
                }
                return;
            }
            int prevWindow = menuWindow;
            if (g == GuitarButtons.up) {
                if (type == 0 || type == 2) {
                    if (type == 0)
                        up[player].Start();
                    if (menuWindow == 1) {
                        do {
                            songselected--;
                            if (songselected < 0) {
                                songselected = 0;
                                break;
                            }
                            if (songselected > Song.songListShow.Count)
                                songselected = Song.songListShow.Count - 1;
                        } while (!Song.songListShow[songselected]);
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange();
                    } else if (menuWindow == 0) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect--;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        if (mainMenuSelect < 0)
                            mainMenuSelect = 0;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
                    } else if (menuWindow == 8) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect--;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        mainMenuSelect = 0;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
                    } else if (menuWindow == 2) {
                        optionsSelect--;
                        if (optionsSelect < 0)
                            optionsSelect = 0;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 2) {
                                    subOptionItemFrameRate += 5;
                                    if (subOptionItemFrameRate >= 1000)
                                        subOptionItemFrameRate = 1000;
                                } else if (subOptionSelect == 3) {
                                    subOptionItemResolutionSelect--;
                                    if (subOptionItemResolutionSelect < 0)
                                        subOptionItemResolutionSelect = 0;
                                }
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0) {
                                    Audio.masterVolume += 0.05f;
                                }
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset += 5;
                                if (subOptionSelect == 2) {
                                    Sound.fxVolume += 0.05f;
                                }
                                if (subOptionSelect == 3) {
                                    Sound.maniaVolume += 0.05f;
                                }
                                if (subOptionSelect == 4) {
                                    Audio.musicVolume += 0.05f;
                                }
                                Sound.setVolume();
                                song.setVolume();
                            } else if (optionsSelect == 4) {
                                if (subOptionSelect == 1) {
                                    subOptionItemSkinSelect--;
                                    if (subOptionItemSkinSelect < 0)
                                        subOptionItemSkinSelect = 0;
                                } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                    subOptionItemHwSelect--;
                                    if (subOptionItemHwSelect < 0)
                                        subOptionItemHwSelect = 0;
                                }
                            }
                        } else {
                            subOptionSelect--;
                            if (subOptionSelect < 0)
                                subOptionSelect = 0;
                        }
                    } else if (menuWindow == 4) {
                        playerInfos[player].difficulty--;
                        if (playerInfos[player].difficulty < 0)
                            playerInfos[player].difficulty = 0;
                    } else if (menuWindow == 5) {
                        recordSelect--;
                        if (recordSelect < 0)
                            recordSelect = 0;
                    }
                } else {
                    up[player].Stop();
                    up[player].Reset();
                    goingUp[player] = false;
                }
            }
            if (g == GuitarButtons.down) {
                if (type == 0 || type == 2) {
                    if (type == 0)
                        down[player].Start();
                    if (menuWindow == 1) {
                        do {
                            songselected++;
                            if (songselected >= Song.songList.Count) {
                                songselected = Song.songList.Count - 1;
                                break;
                            }
                        } while (!Song.songListShow[songselected]);
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange();
                    } else if (menuWindow == 0) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect++;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        if (mainMenuSelect >= mainMenuText.Length)
                            mainMenuSelect = mainMenuText.Length - 1;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
                    } else if (menuWindow == 8) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        mainMenuSelect++;
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        mainMenuSelect = 1;
                        menuTextFadeEnd[mainMenuSelect] = 1f;
                    } else if (menuWindow == 2) {
                        optionsSelect++;
                        if (optionsSelect >= optionsText.Length)
                            optionsSelect = optionsText.Length - 1;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            /*if (subOptionsItem[optionsSelect][subOptionSelect].type == 1) {
                                subOptionsItem[optionsSelect][subOptionSelect].select++;
                                if (subOptionsItem[optionsSelect][subOptionSelect].select >= subOptionsItem[optionsSelect][subOptionSelect].options.Length)
                                    subOptionsItem[optionsSelect][subOptionSelect].select = subOptionsItem[optionsSelect][subOptionSelect].options.Length - 1;
                            }*/
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 2) {
                                    subOptionItemFrameRate -= 5;
                                    if (subOptionItemFrameRate < 0)
                                        subOptionItemFrameRate = 0;
                                } else if (subOptionSelect == 3) {
                                    subOptionItemResolutionSelect++;
                                    if (subOptionItemResolutionSelect >= subOptionItemResolution.Length)
                                        subOptionItemResolutionSelect = subOptionItemResolution.Length - 1;
                                }
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0) {
                                    Audio.masterVolume -= 0.05f;
                                }
                                if (subOptionSelect == 1)
                                    MainGame.AudioOffset -= 5;
                                if (subOptionSelect == 2) {
                                    Sound.fxVolume -= 0.05f;
                                }
                                if (subOptionSelect == 3) {
                                    Sound.maniaVolume -= 0.05f;
                                }
                                if (subOptionSelect == 4) {
                                    Audio.musicVolume -= 0.05f;
                                }
                                Sound.setVolume();
                                song.setVolume();
                            } else if (optionsSelect == 4) {
                                if (subOptionSelect == 1) {
                                    subOptionItemSkinSelect++;
                                    if (subOptionItemSkinSelect >= subOptionItemSkin.Length)
                                        subOptionItemSkinSelect = subOptionItemSkin.Length - 1;
                                } else if (subOptionSelect >= 2 && subOptionSelect <= 5) {
                                    subOptionItemHwSelect++;
                                    if (subOptionItemHwSelect >= subOptionItemHw.Length)
                                        subOptionItemHwSelect = subOptionItemHw.Length - 1;
                                }
                            }
                        } else {
                            subOptionSelect++;
                            if (subOptionSelect >= subOptionslength[optionsSelect])
                                subOptionSelect = subOptionslength[optionsSelect] - 1;
                        }
                    } else if (menuWindow == 4) {
                        playerInfos[player].difficulty++;
                        if (playerInfos[player].difficulty >= Song.songInfo.dificulties.Length)
                            playerInfos[player].difficulty = Song.songInfo.dificulties.Length - 1;
                    } else if (menuWindow == 5) {
                        recordSelect++;
                        if (recordSelect >= recordMenuMax)
                            recordSelect = recordMenuMax;
                    }
                } else {
                    down[player].Stop();
                    down[player].Reset();
                    goingDown[player] = false;
                }
            }
            if (type == 0) {
                if (g == GuitarButtons.green) {
                    if (menuWindow == 0) {
                        if (mainMenuSelect == 0) {
                            menuWindow = 8;
                            mainMenuSelect = 0;
                        } else if (mainMenuSelect == 2) {
                            LoadOptions();
                            menuWindow = 2;
                            setOptionsValues();
                        } else if (mainMenuSelect == 3)
                            game.Closewindow();
                    } else if (menuWindow == 8) {
                        if (mainMenuSelect == 0)
                            menuWindow = 1;
                    } else if (menuWindow == 1) {
                        if (Song.songList.Count > 0) {
                            playerInfos[0].difficulty = 0;
                            playerInfos[1].difficulty = 0;
                            playerInfos[2].difficulty = 0;
                            playerInfos[3].difficulty = 0;
                            menuWindow = 4;
                            loadRecords();
                        }
                    } else if (menuWindow == 2) {
                        menuWindow = 3;
                        subOptionSelect = 0;
                        onSubOptionItem = false;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            onSubOptionItem = false;
                            saveOptionsValues();
                        } else {
                            if (optionsSelect == 0) {
                                if (subOptionSelect == 0) {
                                    fullScreen = !fullScreen;
                                    saveOptionsValues();
                                    if (!fullScreen)
                                        DisplayDevice.Default.RestoreResolution();
                                } else if (subOptionSelect == 1)
                                    vSync = !vSync;
                                else if (subOptionSelect == 4)
                                    Draw.showFps = !Draw.showFps;
                                else if (subOptionSelect == 5)
                                    MainGame.MyPCisShit = !MainGame.MyPCisShit;
                                else if (subOptionSelect == 6) {
                                    if (Draw.tailSizeMult == 1)
                                        Draw.tailSizeMult = 2;
                                    else if (Draw.tailSizeMult == 2)
                                        Draw.tailSizeMult = 4;
                                    else if (Draw.tailSizeMult == 4)
                                        Draw.tailSizeMult = 1;
                                } else if (subOptionSelect == 7)
                                    game.isSingleThreaded = !game.isSingleThreaded;
                                else if (subOptionSelect == 8)
                                    drawMenuBackgroundFx = !drawMenuBackgroundFx;
                                else if (subOptionSelect == 2 || subOptionSelect == 3)
                                    onSubOptionItem = true;
                            } else if (optionsSelect == 1) {
                                if (subOptionSelect == 0 || subOptionSelect == 1 || subOptionSelect == 2 || subOptionSelect == 3 || subOptionSelect == 4)
                                    onSubOptionItem = true;
                                if (subOptionSelect == 5)
                                    Audio.keepPitch = !Audio.keepPitch;
                                if (subOptionSelect == 6)
                                    Audio.onFailPitch = !Audio.onFailPitch;
                                if (subOptionSelect == 7)
                                    Sound.ChangeEngine();
                            } else if (optionsSelect == 2) {
                                //subOptionSelect = i + 2;
                                onSubOptionItem = true;
                            } else if (optionsSelect == 3) {
                                if (subOptionSelect == 0)
                                    Draw.tailWave = !Draw.tailWave;
                                else if (subOptionSelect == 1)
                                    MainGame.drawSparks = !MainGame.drawSparks;
                                else if (subOptionSelect == 2)
                                    SongScan.ScanSongsThread(false);
                                else if (subOptionSelect == 3)
                                    MainGame.failanimation = !MainGame.failanimation;
                                else if (subOptionSelect == 4)
                                    MainGame.songfailanimation = !MainGame.songfailanimation;
                                else if (subOptionSelect == 5) {
                                    if (Language.language.Equals("en"))
                                        Language.language = "es";
                                    else if (Language.language.Equals("es"))
                                        Language.language = "en";
                                    else
                                        Language.language = "en";
                                    Language.LoadLanguage();
                                } else if (subOptionSelect == 6)
                                    MainGame.useGHhw = !MainGame.useGHhw;

                            } else if (optionsSelect == 4) {
                                if (subOptionSelect > 0)
                                    onSubOptionItem = true;
                            }
                        }
                        /*if (subOptionsItem[optionsSelect][subOptionSelect].type > 0) {
                            onSubOptionItem = true;
                        }*/
                    } else if (menuWindow == 4) {
                        if (Song.songInfo.dificulties.Length != 0) {
                            playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
                            playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
                            playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
                            playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];
                            StartGame();
                        }
                    } else if (menuWindow == 5) {
                        loadRecordGameplay();
                    } else if (menuWindow == 7)
                        menuWindow = 1;
                }
                if (g == GuitarButtons.red) {
                    if (menuWindow == 1) {
                        menuMainPos = 1f;
                        menuMainGeneralPos = 1f;
                        menuMainPlayPos = 0f;
                        menuWindow = 0;
                        if (songIdsList[songListIndex] != songselected)
                            songIdsList.Insert(++songListIndex, songselected);
                    } else if (menuWindow == 8) {
                        mainMenuSelect = 0;
                        menuWindow = 0;
                    } else if (menuWindow == 2)
                        menuWindow = 0;
                    else if (menuWindow == 6) {
                        LoadOptions();
                        menuWindow = 2;
                    } else if (menuWindow == 3) {
                        if (onSubOptionItem) {
                            onSubOptionItem = false;
                            saveOptionsValues();
                        } else {
                            LoadOptions();
                            SaveChanges();
                            menuWindow = 2;
                        }
                    } else if (menuWindow == 4) {
                        menuWindow = 1;
                    } else if (menuWindow == 5) {
                        menuWindow = 4;
                    }
                }
                if (g == GuitarButtons.blue) {
                    if (menuWindow == 1 || menuWindow == 0) {
                        /*songselected = new Random().Next(0, Song.songList.Count);
                        if (songChangeFadeUp != 0)
                            songChangeFadeDown = 0;
                        songChangeFadeUp = 0;
                        songChangeFadeWait = 0;
                        songChange(true);*/
                        nextSong();
                    } else if (menuWindow == 4) {
                        menuWindow = 5;
                        recordSelect = 0;
                    } else if (menuWindow == 5) {
                        menuWindow = 4;
                    }
                }
                if (g == GuitarButtons.yellow) {
                    if (menuWindow == 1) {
                        typingQuery = true;
                        searchQuery = "";
                    }
                    if (menuWindow == 7)
                        ResetGame();
                }
                if (g == GuitarButtons.select) {
                    if (menuWindow == 1) {
                        SongScan.sortType++;
                        if (SongScan.sortType > 8)
                            SongScan.sortType = 0;
                        SongScan.SortSongs();
                        songChange();
                    }
                }
                if (g == GuitarButtons.orange) {
                    if (menuWindow == 1) {
                        SongScan.useInstrument = !SongScan.useInstrument;
                        SongScan.SortSongs();
                        SongScan.SearchSong(songselected, searchQuery);
                        songChange();
                    }
                }
            }
            if (prevWindow != menuWindow)
                Sound.playSound(Sound.clickMenu);
        }
        static void WriteLine(FileStream fs, string text) {
            Byte[] Text = new UTF8Encoding(true).GetBytes(text + '\n');
            fs.Write(Text, 0, Text.Length);
        }
        public static void CreateProfile() {
            string path;
            path = "Content/Profiles/" + newProfileName + ".txt";
            if (File.Exists(path)) {
                int tries = 1;
                while (File.Exists("Content/Profiles/" + newProfileName + tries + ".txt")) {
                    tries++;
                    Console.WriteLine("Content/Profiles/" + newProfileName + tries + ".txt");
                }
                path = "Content/Profiles/" + newProfileName + tries + ".txt";
            }
            using (FileStream fs = File.Create(path)) {
                WriteLine(fs, newProfileName);
                WriteLine(fs, "gamepad=0\ninstrument = 0\nlefty = 0\nhw = GHWoR.png\ngreen = Number1\nred = Number2\nyellow = Number3\n"
                    + "blue = Number4\norange = Number5\nopen = Space\nsix = Number6\nwhammy = Unknown\nstart = Enter\nselect = BackSpace\nup = Up\n"
                    + "down = Down\ngreen2 = Unknown\nred2 = Unknown\nyellow2 = Unknown\nblue2 = Unknown\norange2 = Unknown\nopen2 = Unknown\n"
                    + "six2 = Unknown\nwhammy2 = Unknown\nstart2 = Unknown\nselect2 = Unknown\nup2 = Unknown\ndown2 = Unknown\n"
                    + "Xgreen = 0\nXred = 1\nXyellow = 1000\nXblue = 1000\nXorange = 1000\nXopen = 1000\nXsix = 1000\nXwhammy = 1000\n"
                    + "Xstart = 1000\nXselect = 1000\nXup = 3\nXdown = 2\nXaxis = 1000\nXdeadzone = 0");
            }
        }
        public static void LoadOptions() {
            List<int[]> reslist = new List<int[]>();
            foreach (var d in DisplayDevice.Default.AvailableResolutions) {
                bool nofound = true;
                for (int i = 0; i < reslist.Count; i++) {
                    if (d.Width == reslist[i][0] && d.Height == reslist[i][1]) {
                        nofound = false;
                        break;
                    }
                }
                if (nofound) {
                    reslist.Add(new int[] { d.Width, d.Height });
                }
            }
            if (reslist.Count == 0) {
                reslist.Add(new int[] { 800, 600 });
            }
            subOptionItemResolution = reslist.ToArray();
            subOptionItemResolutionSelect = 0;
            for (int i = 0; i < reslist.Count; i++) {
                if (reslist[i][0] == game.width && reslist[i][1] == game.height) {
                    subOptionItemResolutionSelect = i;
                    break;
                }
            }
            subOptionItemFrameRate = (int)(game.Fps);
        }
        public static void SaveChanges() {
            if (File.Exists("config.txt")) {
                File.Delete("config.txt");
            }
            while (File.Exists("config.txt")) ;
            using (FileStream fs = File.Create("config.txt")) {
                WriteLine(fs, ";Video");
                WriteLine(fs, "fullScreen=" + (fullScreen ? 1 : 0));
                WriteLine(fs, "width=" + game.width);
                WriteLine(fs, "height=" + game.height);
                WriteLine(fs, "vsync=" + (vSync ? 1 : 0));
                WriteLine(fs, "frameRate=" + game.Fps);
                WriteLine(fs, "updateMultiplier=" + game.UpdateMultiplier);
                WriteLine(fs, "notesInfo=" + (Draw.drawNotesInfo ? 1 : 0));
                WriteLine(fs, "showFps=" + (Draw.showFps ? 1 : 0));
                WriteLine(fs, "myPCisShit=" + (MainGame.MyPCisShit ? 1 : 0));
                WriteLine(fs, "singleThread=" + (game.isSingleThreaded ? 1 : 0));
                WriteLine(fs, "menuFx=" + (drawMenuBackgroundFx ? 1 : 0));
                WriteLine(fs, "tailQuality=" + Draw.tailSizeMult);
                WriteLine(fs, "");
                WriteLine(fs, ";Keys");
                WriteLine(fs, "volUp=" + (int)volumeUpKey);
                WriteLine(fs, "volDn=" + (int)volumeDownKey);
                WriteLine(fs, "nextS=" + (int)songNextKey);
                WriteLine(fs, "prevS=" + (int)songPrevKey);
                WriteLine(fs, "pauseS=" + (int)songPauseResumeKey);
                WriteLine(fs, "");
                WriteLine(fs, ";Audio");
                WriteLine(fs, "master=" + Math.Round(Audio.masterVolume * 100));
                WriteLine(fs, "offset=" + MainGame.AudioOffset);
                WriteLine(fs, "maniaHit=0");
                WriteLine(fs, "maniaVolume=" + Math.Round(Sound.maniaVolume * 100));
                WriteLine(fs, "fxVolume=" + Math.Round(Sound.fxVolume * 100));
                WriteLine(fs, "musicVolume=" + Math.Round(Audio.musicVolume * 100));
                WriteLine(fs, "keeppitch=" + (Audio.keepPitch ? 1 : 0));
                WriteLine(fs, "failpitch=" + (Audio.onFailPitch ? 1 : 0));
                WriteLine(fs, "useal=" + (Sound.OpenAlMode ? 1 : 0));
                WriteLine(fs, "bendPitch=" + (MainGame.bendPitch ? 1 : 0));
                WriteLine(fs, "");
                WriteLine(fs, ";Gameplay");
                WriteLine(fs, "tailwave=" + (Draw.tailWave ? 1 : 0));
                WriteLine(fs, "failanimation=" + (MainGame.failanimation ? 1 : 0));
                WriteLine(fs, "failsonganim=" + (MainGame.songfailanimation ? 1 : 0));
                WriteLine(fs, "useghhw=" + (MainGame.useGHhw ? 1 : 0));
                WriteLine(fs, "drawsparks=" + (MainGame.drawSparks ? 1 : 0));
                WriteLine(fs, "lang=" + Language.language);
                WriteLine(fs, "");
                WriteLine(fs, ";Skin");
                WriteLine(fs, "skin=" + Textures.skin);
            }
            for (int i = 0; i < playerInfos.Length; i++) {
                PlayerInfo PI = playerInfos[i];
                if (PI.profilePath.Equals("__Guest__"))
                    continue;
                if (File.Exists(PI.profilePath)) {
                    File.Delete(PI.profilePath);
                }
                while (File.Exists(PI.profilePath)) ;
                using (FileStream fs = File.Create(PI.profilePath)) {
                    WriteLine(fs, PI.playerName);
                    WriteLine(fs, "gamepad=" + (PI.gamepadMode ? 1 : 0));
                    WriteLine(fs, "instrument=" + (int)PI.instrument);
                    WriteLine(fs, "lefty=" + (PI.leftyMode ? 1 : 0));
                    WriteLine(fs, "hw=" + PI.hw);
                    WriteLine(fs, "green=" + PI.green);
                    WriteLine(fs, "red=" + PI.red);
                    WriteLine(fs, "yellow=" + PI.yellow);
                    WriteLine(fs, "blue=" + PI.blue);
                    WriteLine(fs, "orange=" + PI.orange);
                    WriteLine(fs, "open=" + PI.open);
                    WriteLine(fs, "six=" + PI.six);
                    WriteLine(fs, "whammy=" + PI.whammy);
                    WriteLine(fs, "start=" + PI.start);
                    WriteLine(fs, "select=" + PI.select);
                    WriteLine(fs, "up=" + PI.up);
                    WriteLine(fs, "down=" + PI.down);
                    //
                    WriteLine(fs, "green2=" + PI.green2);
                    WriteLine(fs, "red2=" + PI.red2);
                    WriteLine(fs, "yellow2=" + PI.yellow2);
                    WriteLine(fs, "blue2=" + PI.blue2);
                    WriteLine(fs, "orange2=" + PI.orange2);
                    WriteLine(fs, "open2=" + PI.open2);
                    WriteLine(fs, "six2=" + PI.six2);
                    WriteLine(fs, "whammy2=" + PI.whammy2);
                    WriteLine(fs, "start2=" + PI.start2);
                    WriteLine(fs, "select2=" + PI.select2);
                    WriteLine(fs, "up2=" + PI.up2);
                    WriteLine(fs, "down2=" + PI.down2);
                    //
                    WriteLine(fs, "Xgreen=" + PI.ggreen);
                    WriteLine(fs, "Xred=" + PI.gred);
                    WriteLine(fs, "Xyellow=" + PI.gyellow);
                    WriteLine(fs, "Xblue=" + PI.gblue);
                    WriteLine(fs, "Xorange=" + PI.gorange);
                    WriteLine(fs, "Xopen=" + PI.gopen);
                    WriteLine(fs, "Xsix=" + PI.gsix);
                    WriteLine(fs, "Xwhammy=" + PI.gwhammy);
                    WriteLine(fs, "Xstart=" + PI.gstart);
                    WriteLine(fs, "Xselect=" + PI.gselect);
                    WriteLine(fs, "Xup=" + PI.gup);
                    WriteLine(fs, "Xdown=" + PI.gdown);
                    WriteLine(fs, "Xaxis=" + PI.gWhammyAxis);
                    WriteLine(fs, "Xdeadzone=" + (PI.gAxisDeadZone > 0.1 ? 1 : 0));
                }
            }
        }
        static bool loadSkin = false;
        static bool loadHw = false;
        public static void saveOptionsValues() {
            game.Fps = subOptionItemFrameRate;
            if (subOptionItemFrameRate == 0)
                game.Fps = 9999;
            game.vSync = true;
            if (!subOptionItemSkin[subOptionItemSkinSelect].Equals(Textures.skin)) {
                Textures.skin = subOptionItemSkin[subOptionItemSkinSelect];
                //Textures.load();
                loadSkin = true;
            }
            if (subOptionSelect == 2)
                playerInfos[0].hw = subOptionItemHw[subOptionItemHwSelect];
            if (subOptionSelect == 3)
                playerInfos[1].hw = subOptionItemHw[subOptionItemHwSelect];
            if (subOptionSelect == 4)
                playerInfos[2].hw = subOptionItemHw[subOptionItemHwSelect];
            if (subOptionSelect == 5)
                playerInfos[3].hw = subOptionItemHw[subOptionItemHwSelect];
            //Textures.loadHighway();
            DisplayDevice di = DisplayDevice.Default;
            int w = subOptionItemResolution[subOptionItemResolutionSelect][0];
            int h = subOptionItemResolution[subOptionItemResolutionSelect][1];
            if (fullScreen) {
                di.ChangeResolution(di.SelectResolution(w, h, di.BitsPerPixel, di.RefreshRate));
                gameObj.Width = w;
                gameObj.Height = h;
            }
            Textures.swpath1 = playerInfos[0].hw;
            Textures.swpath2 = playerInfos[1].hw;
            Textures.swpath3 = playerInfos[2].hw;
            Textures.swpath4 = playerInfos[3].hw;
            loadHw = true;
        }
        public static void ScanSkin() {
            string folder = "";
            folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Skins";
            string[] dirInfos;
            try {
                dirInfos = Directory.GetDirectories(folder, "*.*", System.IO.SearchOption.TopDirectoryOnly);
            } catch { return; }
            for (int i = 0; i < dirInfos.Length; i++) {
                dirInfos[i] = dirInfos[i].Replace(folder + "\\", "");
            }
            subOptionItemSkin = dirInfos;

            folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Content\Highways";
            try {
                dirInfos = Directory.GetFiles(folder, "*.*");
            } catch { return; }
            for (int i = 0; i < dirInfos.Length; i++) {
                dirInfos[i] = Path.GetFileName(dirInfos[i]);
            }
            subOptionItemHw = dirInfos;
        }
        public static void loadRecords() {
            recordsLoaded = false;
            ThreadStart loadStart = new ThreadStart(recordsThread);
            Thread load = new Thread(loadStart);
            load.Start();
        }
        public static bool recordsLoaded = false;
        public static void recordsThread() {
            records.Clear();
            string[] chart;
            try {
                chart = Directory.GetFiles(Song.songList[songselected].Path, "*.txt", System.IO.SearchOption.AllDirectories);
            } catch {
                try {
                    chart = Directory.GetFiles(Song.songList[songselected].Path, "*.txt", System.IO.SearchOption.AllDirectories);
                } catch {
                    return;
                }
            }
            Console.WriteLine(chart.Length);
            foreach (string dir in chart) {
                if (!dir.Contains("Record"))
                    continue;
                string[] lines = File.ReadAllLines(dir, Encoding.UTF8);
                int players = 1;
                string time = "0";
                bool songfail = false;
                string[] diff = new string[4];
                int[] p50 = new int[4];
                int[] p100 = new int[4];
                int[] p200 = new int[4];
                int[] p300 = new int[4];
                int[] pMax = new int[4];
                int[] fail = new int[4];
                int[] mode = new int[4];
                int[] speed = new int[4];
                bool[] easy = new bool[4];
                bool[] nofail = new bool[4];
                int[] hidden = new int[4];
                int[] acc = new int[4];
                bool[] hard = new bool[4];
                int[] score = new int[4];
                int[] rank = new int[4];
                int totalScore = 0;
                int offset = 0;
                int[] streak = new int[4];
                string[] name = new string[4];
                Records record = new Records();
                record.path = dir;
                int ver = 1;
                foreach (var s in lines) {
                    if (s.Equals("v2")) {
                        ver = 2;
                        continue;
                    } else if (s.Equals("v3")) {
                        ver = 3;
                        continue;
                    }
                    if (ver == 1) {
                        record.ver = ver;
                        records.Add(record);
                        break;
                    } else if (ver == 2 || ver == 3) {
                        string[] split = s.Split('=');
                        if (s[0] == 'p') {
                            int player = 0;
                            if (s[1] == '1') player = 0;
                            else if (s[1] == '2') player = 1;
                            else if (s[1] == '3') player = 2;
                            else if (s[1] == '4') player = 3;
                            if (split[0].Equals("p" + (player + 1) + "50")) p50[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "100")) p100[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "200")) p200[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "300")) p300[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Max")) pMax[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "Miss")) fail[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "streak")) streak[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "rank")) rank[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "name")) name[player] = split[1];
                            if (split[0].Equals("p" + (player + 1) + "score")) score[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hidden")) hidden[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "hard")) hard[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "mode")) mode[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "easy")) easy[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "nofail")) nofail[player] = bool.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "speed")) speed[player] = int.Parse(split[1]);
                            if (split[0].Equals("p" + (player + 1) + "diff")) diff[player] = split[1];
                            if (split[0].Equals("p" + (player + 1) + "acc")) acc[player] = int.Parse(split[1]);
                        }
                        if (split[0].Equals("time")) time = split[1];
                        if (split[0].Equals("players")) players = int.Parse(split[1]);
                        if (split[0].Equals("failed")) songfail = bool.Parse(split[1]);
                        if (split[0].Equals("offset")) offset = int.Parse(split[1]);
                        if (s.Equals(" ")) {
                            record.p100 = p100;
                            record.p50 = p50;
                            record.p200 = p200;
                            record.p300 = p300;
                            record.fail = fail;
                            record.easy = easy;
                            record.nofail = nofail;
                            record.speed = speed;
                            record.streak = streak;
                            record.name = name;
                            record.score = score;
                            record.hidden = hidden;
                            record.hard = hard;
                            record.mode = mode;
                            record.time = time;
                            record.players = players;
                            record.diff = diff;
                            record.failsong = songfail;
                            record.ver = ver;
                            record.offset = offset;
                            record.accuracy = acc;
                            for (int p = 0; p < record.players; p++)
                                record.totalScore += record.score[p];
                            records.Add(record);
                            break;
                        }
                    }
                }
            }
            foreach (var record in records) {
                Console.WriteLine(">>>Record");
                Console.WriteLine("Ver = " + record.ver);
                if (record.ver == 1)
                    continue; ;
                for (int i = 0; i < 4; i++) {
                    Console.WriteLine(record.p100[i] + ", p100");
                    Console.WriteLine(record.p50[i] + ", p50");
                    Console.WriteLine(record.p200[i] + ", p200");
                    Console.WriteLine(record.p300[i] + ", p300");
                    Console.WriteLine(record.fail[i] + ", fail");
                    Console.WriteLine(record.streak[i] + ", streak");
                    Console.WriteLine(record.name[i] + ", name");
                    Console.WriteLine(record.score[i] + ", score");
                    Console.WriteLine(record.hidden[i] + ", hidden");
                    Console.WriteLine(record.hard[i] + ", hard");
                    Console.WriteLine(record.mode[i] + ", mode");
                    Console.WriteLine(record.diff[i] + ", diff");
                    Console.WriteLine(record.accuracy[i] + ", acc");
                }
                Console.WriteLine(record.time + ", time");
                Console.WriteLine(record.players + ", players");
            }
            records = records.OrderBy(Record => Record.totalScore).Reverse().ToList();
            recordsLoaded = true;
        }
        public static int recordIndex = 0;
        public static float recordSpeed = 1;
        public static void loadRecordGameplay() {
            recordSpeed = 1;
            int RecordCount = 0;
            for (int i = 0; i < records.Count; i++) {
                if (records[i].diff == null) continue;
                if (records[i].diff[0] == null) continue;
                if (records[i].diff[1] == null) continue;
                if (records[i].diff[2] == null) continue;
                if (records[i].diff[3] == null) continue;
                bool match = false;
                for (int d = 0; d < Song.songInfo.dificulties.Length; d++) {
                    string diffString = Song.songInfo.dificulties[d];
                    for (int p = 0; p < 4; p++) {
                        if (records[i].diff[p].Equals(diffString)) {
                            match = true;
                        }
                    }
                }
                if (!match)
                    continue;
                //Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                if (recordSelect == RecordCount) {
                    Song.recordPath = records[i].path;
                    recordSpeed = records[i].speed[0] / 100;
                    recordIndex = i;
                }
                RecordCount++;
            }
            if (File.Exists(Song.recordPath))
                Gameplay.recordLines = File.ReadAllLines(Song.recordPath, Encoding.UTF8);
            else {
                Gameplay.record = false;
                return;
            }
            playerInfos[0].difficultySelected = Song.songInfo.dificulties[playerInfos[0].difficulty];
            playerInfos[1].difficultySelected = Song.songInfo.dificulties[playerInfos[1].difficulty];
            playerInfos[2].difficultySelected = Song.songInfo.dificulties[playerInfos[2].difficulty];
            playerInfos[3].difficultySelected = Song.songInfo.dificulties[playerInfos[3].difficulty];
            string ver = Gameplay.recordLines[0];
            if (ver.Equals("v2"))
                Gameplay.recordVer = 2;
            else if (ver.Equals("v3"))
                Gameplay.recordVer = 3;
            if (Gameplay.recordVer <= 3) {
                for (int i = 0; i < Gameplay.recordLines.Length; i++) {
                    if (Gameplay.recordLines[i].Equals(" ")) {
                        MainGame.recordIndex = i + 1;
                        break;
                    }
                }
            }
            StartGame(true);
        }
        public static void AlwaysUpdate() {
            Input.UpdateControllers();
            if (Input.KeyDown(Key.Q))
                input1 += 0.001f;
            if (Input.KeyDown(Key.W))
                input1 -= 0.001f;
            if (Input.KeyDown(Key.A))
                input2 += 0.001f;
            if (Input.KeyDown(Key.S))
                input2 -= 0.001f;
            if (Input.KeyDown(Key.E))
                input3 += 0.2f;
            if (Input.KeyDown(Key.R))
                input3 -= 0.2f;
            if (Input.KeyDown(Key.D))
                input4 += 0.05f;
            if (Input.KeyDown(Key.F))
                input4 -= 0.05f;
            if (MainGame.useMatrix && isDebugOn) {
                if (Input.KeyDown(Key.Number1))
                    MainGame.Matrix2X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number2))
                    MainGame.Matrix2X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Q))
                    MainGame.Matrix2Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.W))
                    MainGame.Matrix2Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.A))
                    MainGame.Matrix2Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.S))
                    MainGame.Matrix2Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Z))
                    MainGame.Matrix2W -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.X))
                    MainGame.Matrix2W += (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number3))
                    MainGame.Matrix1X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number4))
                    MainGame.Matrix1X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.E))
                    MainGame.Matrix1Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.R))
                    MainGame.Matrix1Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.D))
                    MainGame.Matrix1Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.F))
                    MainGame.Matrix1Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.C))
                    MainGame.Matrix1W += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.V))
                    MainGame.Matrix1W -= (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number5))
                    MainGame.Matrix0X += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Number6))
                    MainGame.Matrix0X -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.T))
                    MainGame.Matrix0Y += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.Y))
                    MainGame.Matrix0Y -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.G))
                    MainGame.Matrix0Z += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.H))
                    MainGame.Matrix0Z -= (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.B))
                    MainGame.Matrix0W += (float)(game.timeEllapsed / 2000);
                if (Input.KeyDown(Key.N))
                    MainGame.Matrix0W -= (float)(game.timeEllapsed / 2000);

                if (Input.KeyDown(Key.Number7))
                    MainGame.TranslateX += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number8))
                    MainGame.TranslateX -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.U))
                    MainGame.TranslateY += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.I))
                    MainGame.TranslateY -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.J))
                    MainGame.TranslateZ += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.K))
                    MainGame.TranslateZ -= (float)(game.timeEllapsed / 20);

                if (Input.KeyDown(Key.Number9))
                    MainGame.RotateX += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Number0))
                    MainGame.RotateX -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.O))
                    MainGame.RotateY += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.P))
                    MainGame.RotateY -= (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.L))
                    MainGame.RotateZ += (float)(game.timeEllapsed / 20);
                if (Input.KeyDown(Key.Semicolon))
                    MainGame.RotateZ -= (float)(game.timeEllapsed / 20);
            }
            //Console.Write(string.Format("\r" + input1 + " - " + input2 + " - " + input3 + " - " + input4));
            if (Menu)
                UpdateMenu();
            if (Game)
                MainGame.update();
            if (Editor)
                EditorScreen.Update();

        }
        static public void AlwaysRender() {
            if (Menu)
                RenderMenu();
            if (Game)
                MainGame.render();
            if (Editor)
                EditorScreen.Render();
        }
        static Stopwatch[] up = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        static Stopwatch[] down = new Stopwatch[4] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        public static int songselected = 0;
        public static bool fullScreen = false;
        public static bool vSync = false;
        static int mainMenuSelect = 0;
        static int optionsSelect = 0;
        static int subOptionSelect = 0;
        static int controllerBindPlayer = 1;
        static int menuWindow = 0;
        static bool onSubOptionItem = false;
        static int recordSelect = 0;
        static int recordMenuMax = 0;
        //public static int dificultySelect = 0;
        public static int[] dificultySelect = new int[4] { 0, 0, 0, 0 };
        static string[] mainMenuText = new string[] {
            "Play", //Play
            "Editor (WIP)", //Editor
            "Options",
            "Exit"
        };
        static string[] optionsText = new string[] {
            "Video",
            "Audio",
            "Keys",
            "Gameplay",
            "Skin"
        };
        public static void changeText() {
            mainMenuText[0] = Language.menuPlay;
            mainMenuText[1] = Language.menuEditor;
            mainMenuText[2] = Language.menuOption;
            mainMenuText[3] = Language.menuExit;
            optionsText[0] = Language.optionVideo;
            optionsText[1] = Language.optionAudio;
            optionsText[2] = Language.optionKeys;
            optionsText[3] = Language.optionGameplay;
            optionsText[4] = Language.optionSkin;
        }
        static int[] subOptionslength = new int[] { 9, 8, 99, 7, 7 };
        public static int subOptionItemFrameRate = 0;
        public static string[] subOptionItemSkin = new string[] { };
        public static int subOptionItemSkinSelect = 0;
        public static string[] subOptionItemHw = new string[] { };
        public static int subOptionItemHwSelect = 0;
        public static int[][] subOptionItemResolution = new int[][] { new int[] { 800, 600 } };
        public static int subOptionItemResolutionSelect = 0;
        public static void setOptionsValues() {
            if (game.Fps == 9999)
                subOptionItemFrameRate = 0;
        }
        public static void SwapProfiles(int destiny, int origin) {
            int originVal = -1;
            originVal = Input.controllerIndex[origin];
            Input.controllerIndex[origin] = Input.controllerIndex[destiny];
            Input.controllerIndex[destiny] = originVal;
            PlayerInfo destinyClone = playerInfos[destiny].Clone();
            playerInfos[destiny] = playerInfos[origin].Clone();
            playerInfos[origin] = destinyClone;
        }
        public static void SortPlayers() {
            int playerSize = 0;
            /*if (Input.controllerIndex_1 == -1) {
                if (Input.controllerIndex_2 != -1)
                    SwapProfiles(1, 2);
                else if (Input.controllerIndex_3 != -1)
                    SwapProfiles(1, 3);
                else if (Input.controllerIndex_4 != -1)
                    SwapProfiles(1, 4);
            }
            if (Input.controllerIndex_2 == -1) {
                if (Input.controllerIndex_3 != -1)
                    SwapProfiles(2, 3);
                else if (Input.controllerIndex_4 != -1)
                    SwapProfiles(2, 4);
            }
            if (Input.controllerIndex_3 == -1) {
                if (Input.controllerIndex_4 != -1)
                    SwapProfiles(3, 4);
            }*/
            if (Input.controllerIndex[3] != -1 && Input.controllerIndex[2] == -1)
                SwapProfiles(2, 3);
            if (Input.controllerIndex[2] != -1 && Input.controllerIndex[1] == -1)
                SwapProfiles(1, 2);
            if (Input.controllerIndex[1] != -1 && Input.controllerIndex[0] == -1)
                SwapProfiles(0, 1);
            /*if (playerInfos[0].playerName.Equals("__Guest__"))
                Input.controllerIndex_1*/
            for (int i = 0; i < 4; i++) {
                Console.WriteLine(Input.controllerIndex[i]);
                if (Input.controllerIndex[i] == -1) {
                    playerProfileReady[i] = false;
                } else {
                    playerSize++;
                    playerProfileReady[i] = true;
                }
            }
            playerAmount = playerSize;
            if (subOptionItemHw.Length != 0) {
                if (playerInfos[0].hw == String.Empty)
                    playerInfos[0].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[1].hw == String.Empty)
                    playerInfos[1].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[2].hw == String.Empty)
                    playerInfos[2].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
                if (playerInfos[3].hw == String.Empty)
                    playerInfos[3].hw = subOptionItemHw[Draw.rnd.Next(subOptionItemHw.Length)];
            }
            Textures.swpath1 = playerInfos[0].hw;
            Textures.swpath2 = playerInfos[1].hw;
            Textures.swpath3 = playerInfos[2].hw;
            Textures.swpath4 = playerInfos[3].hw;
            loadHw = true;
        }
        public static float CalcModMult(int player) {
            float ret = 1;
            if (playerInfos[player].Hidden == 1)
                ret += 0.1f;
            if (playerInfos[player].HardRock)
                ret += 0.1f;
            if (playerInfos[player].Easy)
                ret -= 0.4f;
            if (playerInfos[player].noteModifier == 1)
                ret += 0.1f;
            if (playerInfos[player].noteModifier > 1)
                ret -= 99999f;
            if (playerInfos[player].inputModifier > 0)
                ret -= 99999f;
            if (playerInfos[player].gameplaySpeed < 1f)
                ret -= 1 - playerInfos[player].gameplaySpeed;
            else if (playerInfos[player].gameplaySpeed > 1f)
                ret -= (1 - playerInfos[player].gameplaySpeed) * 0.45f;
            if (playerInfos[player].noFail)
                ret -= 0.3f;
            if (playerInfos[player].performance)
                ret += 0.2f;
            if (playerInfos[player].transform)
                ret -= 99999f;
            if (playerInfos[player].autoSP)
                ret -= 0.1f;
            if (ret < 0)
                ret = 0;
            return ret;
        }
        public static void StartGame(bool record = false) {
            //Ordenar Controles
            if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Priority = ThreadPriority.Lowest;
            MainGame.player1Scgmd = false;
            SortPlayers();
            MainGame.drawBackground = true;
            MainGame.onPause = false;
            MainGame.onFailMenu = false;
            MainGame.rewindTime = 0;
            MainGame.lastTime = -6000;
            Gameplay.lastHitTime = -2500;
            MainMenu.song.negativeTime = false;
            Gameplay.record = record;
            Gameplay.SetPlayers();
            if (animationOnToGame)
                return;
            Draw.LoadFreth();
            song.stop();
            song.free();
            Gameplay.reset();
            List<string> paths = new List<string>();
            foreach (var e in Song.songList[songselected].audioPaths)
                paths.Add(e);
            song.loadSong(paths.ToArray());
            Song.unloadSong();
            animationOnToGame = true;
            Song.songInfo = Song.songList[songselected];
            Gameplay.saveInput = true;
            Gameplay.keyBuffer.Clear();
            Gameplay.keyIndex = 0;
            MainGame.recordIndex = 0;
            Console.WriteLine(Song.songInfo.Path);
            Song.loadSong();
            Draw.ClearSustain();
            MainGame.songfailDir = 0;
            for (int p = 0; p < 4; p++) {
                Draw.uniquePlayer[p].deadNotes.Clear();
                Draw.uniquePlayer[p].SpLightings.Clear();
                Draw.uniquePlayer[p].SpSparks.Clear();
                Draw.uniquePlayer[p].sparks.Clear();
                Draw.uniquePlayer[p].pointsList.Clear();
                Draw.uniquePlayer[p].noteGhosts.Clear();
                Gameplay.gameInputs[p].keyHolded = 0;
                Gameplay.gameInputs[p].onHopo = false;
                Gameplay.pGameInfo[p].lifeMeter = 0.5f;
                Gameplay.pGameInfo[p].lastNoteTime = 0;
                Gameplay.pGameInfo[p].notePerSecond = 0;
            }
            MainGame.beatTime = 0;
            MainGame.currentBeat = 0;
            Game = true;
            Menu = true;//this is true, but for test i leave it false
            animationOnToGameTimer.Reset();
            animationOnToGameTimer.Start();
            game.vSync = vSync;
            Audio.musicSpeed = playerInfos[0].gameplaySpeed;
            song.negTimeCount = -2500.0;
            //song.negativeTime = true;
            MainGame.songFailAnimation = 0;
            MainGame.onFailSong = false;
            MainGame.onFailMenu = false;
            Gameplay.gameInputs[0].keyHolded = 0;
            Gameplay.gameInputs[1].keyHolded = 0;
            Gameplay.gameInputs[2].keyHolded = 0;
            Gameplay.gameInputs[3].keyHolded = 0;
            if (record)
                Audio.musicSpeed = recordSpeed;
            gameObj.Title = "GH / Playing: " + Song.songInfo.Artist + " - " + Song.songInfo.Name + " [" + MainMenu.playerInfos[0].difficultySelected + "] // " + Song.songInfo.Charter;
            if (Song.songInfo.warning) {
                Draw.popUps.Add(new PopUp() { isWarning = true, advice = Language.popupEpilepsy, life = 0 });
            }
            //MainMenu.song.play();
        }
        public static void EndGame(bool score = false) {
            Song.unloadSong();
            MainGame.player1Scgmd = false;
            //score = false;
            if (!score) {
                animationOnToGame = false;
                animationOnToGameTimer.Stop();
                animationOnToGameTimer.Reset();
                menuWindow = 1;
            } else {
                menuWindow = 7;
            }
            Game = false;
            Menu = true;
            game.vSync = true;
            Storyboard.FreeBoard();
            song.free();
            if (Difficulty.DifficultyThread.IsAlive)
                Difficulty.DifficultyThread.Priority = ThreadPriority.Normal;
        }
        public static void ResetGame() {
            Song.unloadSong();
            Storyboard.FreeBoard();
            StartGame();
            animationOnToGame = false;
        }
        static int timesMoved = 0;
        public static void UpdateMenu() {
            if (!SongScan.firstScan) {
                firstLoad = true;
                SongScan.firstScan = true;
                SongScan.ScanSongsThread();
            }
            for (int i = 0; i < 4; i++) {
                menuTextFadeTime[i] += game.timeEllapsed;
                menuTextFadeNow[i] = Ease.Out(menuTextFadeStart[i], menuTextFadeEnd[i], (Ease.OutElastic(Ease.In((float)menuTextFadeTime[i], 400))));
            }
            songChangeFade += game.timeEllapsed;
            songChangeFadeDown += game.timeEllapsed;
            songChangeFadeWait += game.timeEllapsed;
            if (songChangeFadeWait > 500)
                songChangeFadeUp += game.timeEllapsed;
            if (songChangeFadeUp == 0) {
                float volume = (float)(1 - songChangeFadeDown / 500);
                song.setVolume(volume > 1 ? 1 : volume);
            } else {
                //float volume = (float)(songChangeFadeUp / 500);
                //song.setVolume(volume > 1 ? 1 : volume);
            }
            //Ease.Out(SongSelectedprev, SongSelected, Ease.OutQuad(Ease.In((float)SongListEaseTime, SonsEaseLimit))) * textHeight;
            //Console.WriteLine(SongListEaseTime + ", " + SongSelectedprev + ", " + SongSelected + ", " + SonsEaseLimit);
            for (int p = 0; p < 4; p++) {
                if (!goingDown[p]) {
                    if (down[p].ElapsedMilliseconds > 400) {
                        goingDown[p] = true;
                        down[p].Restart();
                        timesMoved = 0;
                    }
                } else {
                    if (down[p].ElapsedMilliseconds > (timesMoved > 50 ? 20 : 100)) {
                        MenuIn(GuitarButtons.down, 2, p + 1);
                        down[p].Restart();
                        timesMoved++;
                    }
                }
                if (!goingUp[p]) {
                    if (up[p].ElapsedMilliseconds > 400) {
                        goingUp[p] = true;
                        up[p].Restart();
                        timesMoved = 0;
                    }
                } else {
                    if (up[p].ElapsedMilliseconds > (timesMoved > 50 ? 20 : 100)) {
                        MenuIn(GuitarButtons.up, 2, p + 1);
                        up[p].Restart();
                        timesMoved++;
                    }
                }
            }
            if (game.fileDropped) {
                foreach (var d in game.files) {
                    Song.songList.Add(SongScan.ScanSingle(d));
                    Song.songListShow.Add(true);
                    Console.WriteLine(d);
                }
                game.fileDropped = false;
                game.files.Clear();
                songselected = Song.songList.Count - 1;
                songChange();
                while (songLoad.IsAlive) ;
                SongScan.SortSongs();
                //StartGame();
            }
            menuFadeOut += game.timeEllapsed;
            songPopUpTime += game.timeEllapsed;
            volumePopUpTime += game.timeEllapsed;
        }
        static ThreadStart start = new ThreadStart(songChangeThread);
        static Thread songLoad = new Thread(start);
        public static bool firstLoad = true;
        public static void songChange(bool prev = true) {
            Console.WriteLine(song.finishLoadingFirst + ", " + Song.songList.Count);
            if (Song.songList.Count == 0)
                return;
            if (songselected >= Song.songList.Count)
                songselected = Song.songList.Count - 1;
            if (songselected < 0)
                songselected = 0;
            Song.songInfo = Song.songList[songselected];
            SongListTarget = songselected;
            int sum = 0;
            for (int i = 0; i < songselected; i++) {
                if (i >= Song.songListShow.Count)
                    break;
                if (!Song.songListShow[i])
                    sum++;
            }
            SongListTarget -= sum;
            if (!songLoad.IsAlive && (song.finishLoadingFirst || song.firstLoad) && (SongScan.songsScanned != 0)) {
                Console.WriteLine("loading song");
                isPrewiewOn = prev;
                songLoad = new Thread(start);
                songLoad.Start();
            }
        }
        static bool isPrewiewOn = false;
        static void songChangeThread() {
            Console.WriteLine(SongScan.songsScanned + ", " + Song.songList.Count);
            song.firstLoad = false;
            if (Song.songList.Count == 0 || SongScan.songsScanned == 0) {
                return;
            }
            bool prev = isPrewiewOn;
            //ContentPipe.UnLoadTexture(album.ID);
            while (songChangeFadeWait < 500) ;
            int songi = songselected;
            Song.songInfo = Song.songList[songi];
            song.free();
            if (Song.songInfo.previewSong.Length > 0) {
                song.loadSong(new string[] { Song.songInfo.previewSong });
            } else {
                List<string> paths = new List<string>();
                foreach (var e in Song.songList[songi].audioPaths)
                    paths.Add(e);
                song.loadSong(paths.ToArray());
            }
            int preview = prev ? Song.songList[songi].Preview : 0;
            song.setPos(preview);
            song.play();
            //
            Song.unloadSong();
            Song.beatMarkers = Song.loadJustBeats(Song.songInfo);
            needBGChange = true;
            gameObj.Title = "GH / Listening: " + Song.songInfo.Artist + " - " + Song.songInfo.Name;
        }
        static double songChangeFadeDown = 0;
        static double songChangeFadeWait = 0;
        static double songChangeFadeUp = 0;
        static bool needBGChange = false;
        static bool BGChanging = false;
        static bool currentBGisCustom = false;
        static void changeBG() {
            needBGChange = false;
            BGChanging = true;
            ContentPipe.UnLoadTexture(album.ID);
            album = new Texture2D(ContentPipe.LoadTexture(Song.songList[songselected].albumPath).ID, 500, 500);
            /*album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.png").ID, 500, 500);
            if (album.ID == 0)
                album = new Texture2D(ContentPipe.LoadTexture("Content/Songs/" + Song.songList[songselected].Path + "/album.jpg").ID, 500, 500);*/
            songChangeFade = 0;
            if (menuWindow == 0 || !Song.songList[songselected].backgroundPath.Equals("") || currentBGisCustom) {
                if (oldBG.ID != 0)
                    ContentPipe.UnLoadTexture(oldBG.ID);
                oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
                if (!Song.songList[songselected].backgroundPath.Equals("")) {
                    Textures.loadSongBG(Song.songList[songselected].backgroundPath);
                    currentBGisCustom = true;
                } else {
                    currentBGisCustom = false;
                    Textures.loadDefaultBG();
                }
            } else {
                oldBG = new Texture2D(Textures.background.ID, Textures.background.Width, Textures.background.Height);
            }
            BGChanging = false;
        }
        static Stopwatch beatPunch = new Stopwatch();
        static Stopwatch beatPunchSoft = new Stopwatch();
        static double SongListPos = 0;
        static float SongListTarget = 0;
        static float SonsEaseLimit = 250;
        static float SonsEaseBGLimit = 250;
        static float SongVolume = 0f;

        static float[] playerMenuPos = new float[] { 0, 0, 0, 0 };
        static float menuOptionPos = 0f;
        static float menuMainPos = 1f;
        static float menuMainGeneralPos = 1f;
        static float menuMainPlayPos = 0f;
        static float pmouseX = 0;
        static float pmouseY = 0;
        public static float volumeValueSmooth = 0f;
        public static void drawVolume() {
            float menuFadeOutTr = 0f;
            if (volumePopUpTime < 5000) {
                if (volumePopUpTime > 4000) {
                    float map = (float)(volumePopUpTime - 4000f) / 1000.0f;
                    menuFadeOutTr = 1 - map;
                    if (menuFadeOutTr < 0) {
                        volumeValueSmooth = 0f;
                        menuFadeOutTr = 0f;
                    }
                } else
                    menuFadeOutTr = 1f;
            }
            //menuFadeOutTr = 1f;
            float startX = getXCanvas(-40, 2);
            float endX = getXCanvas(-5, 2);
            float startY = getYCanvas(menuFadeOut > 40000 ? 35 : 20);
            float endY = getYCanvas(menuFadeOut > 40000 ? 45 : 30);
            float margin = getYCanvas(1.25f);
            int menuFadeOutTr8 = (int)(menuFadeOutTr * 255);
            Color colWhite = Color.FromArgb(menuFadeOutTr8, 255, 255, 255);
            Graphics.drawRect(startX, startY, endX, endY, 0, 0, 0, 0.5f * menuFadeOutTr);
            startX -= margin;
            startY += margin;
            float height = startY - endY;
            height /= 500;
            Vector2 size = new Vector2(height, height);
            size *= 2.5f;
            Draw.DrawString(Language.menuVolume, startX, -startY, size, colWhite, new Vector2(1, 1));
            endY -= margin;
            endX += margin;
            string percent = Math.Round(Audio.masterVolume * 100) + "%";
            float PercentWidth = Draw.GetWidthString(percent, size);
            Draw.DrawString(percent, endX - PercentWidth, -startY, size, colWhite, new Vector2(-1, 1));
            volumeValueSmooth += (Audio.masterVolume - volumeValueSmooth) * 0.3f;
            float volumeValue = Draw.Lerp(startX, endX, volumeValueSmooth);
            Graphics.drawRect(startX, endY, volumeValue, endY - margin * 2, 1f, 1f, 1f, 0.5f * menuFadeOutTr);
        }
        public static void drawPlayer() {
            float positionX = Draw.Lerp(getXCanvas(65, 3), getXCanvas(0), menuMainGeneralPos * menuMainPos);
            float fadeTr = menuMainGeneralPos * menuMainPos;
            if (fadeTr > 1) {
                fadeTr = -fadeTr;
                fadeTr += 2;
            }
            float menuFadeOutTr = 1f;
            if (menuFadeOut > 30000) {
                float map = (float)(menuFadeOut - 30000) / 10000.0f;
                menuFadeOutTr = 1 - map;
                if (menuFadeOutTr < 0) {
                    menuFadeOutTr = 0f;
                }
                if (songPopUpTime < 5000) {
                    if (songPopUpTime > 4000) {
                        map = (float)(songPopUpTime - 4000f) / 1000.0f;
                        menuFadeOutTr = 1 - map;
                        if (menuFadeOutTr < 0) {
                            menuFadeOutTr = 0f;
                        }
                    } else
                        menuFadeOutTr = 1f;
                }
            }
            int menuFadeOutTr8 = (int)(menuFadeOutTr * fadeTr * 255);
            Color colWhite = Color.FromArgb(menuFadeOutTr8, 255, 255, 255);
            float startX = getXCanvas(5, 0) + positionX;
            float endX = getXCanvas(65, 0) + positionX;
            float startY = getYCanvas(menuFadeOut > 40000 ? 30 : 15);
            float endY = getYCanvas(menuFadeOut > 40000 ? 45 : 30);
            float margin = getYCanvas(1.25f);
            Graphics.drawRect(startX, startY, endX, endY, 0, 0, 0, 0.5f * menuFadeOutTr * fadeTr);
            float height = startY - endY;
            height /= 500;
            Vector2 size = new Vector2(height, height);
            float textHeight = Draw.font.Height * size.Y * 1.5f;
            float posY = -startY;
            Vector2 scale = new Vector2(height, height);
            scale *= 2;
            if (SongScan.songsScanned != 1) {
                startX -= margin;
                posY -= textHeight * 2;
                Vector2 align = new Vector2(1, -1);
                if (SongScan.songsScanned == 0)
                    Draw.DrawString(Language.menuScanning + ": " + (Song.songList.Count + SongScan.badSongs) + "/" + SongScan.totalFolders, startX, posY, scale, colWhite, align);
                else if (SongScan.songsScanned == 2)
                    Draw.DrawString(Language.menuCalcDiffs, startX, posY, scale, colWhite, align);
                else if (SongScan.songsScanned == 3)
                    Draw.DrawString(Language.menuCaching, startX, posY, scale, colWhite, align);
                posY -= textHeight;
                scale *= 0.6f;
                if (SongScan.songsScanned == 0) {
                    int count = Song.songList.Count;
                    for (int i = count - 1; i > count - 6; i--) {
                        if (i < 0)
                            break;
                        Draw.DrawString(Song.songList[i].Name, startX, posY, scale, colWhite, align);
                        posY -= textHeight * 0.6f;
                    }
                }
                startX += margin;
            }
            Draw.DrawString(string.Format(Language.menuPlayerHelp, "[" + volumeUpKey + "]", "[" + volumeDownKey + "]", "[" + songPrevKey + "]", "[" + songPauseResumeKey + "]", "[" + songNextKey + "]"), startX, -startY - textHeight, size * 1.25f, colWhite, new Vector2(1, 1), 0);
            Graphics.Draw(album, new Vector2(startX, -startY), size, colWhite, new Vector2(1, 1));
            startX += startY - endY;
            startX -= margin;
            startY += margin;
            endY -= margin;
            endX += margin;
            Draw.DrawString(Song.songInfo.Name, startX, -startY, size * 2f, colWhite, new Vector2(1, 1), 0, endX);
            startY += margin * 3;
            Draw.DrawString("   " + Song.songInfo.Artist, startX, -startY, size * 1.25f, colWhite, new Vector2(1, 1), 0, endX);
            float d = (float)(MainMenu.song.getTime() / (MainMenu.song.length * 1000));
            if (d < 0)
                d = 0;
            float timeRemaining = Draw.Lerp(startX, endX, d);
            Graphics.drawRect(startX, endY, timeRemaining, endY - margin * 2, 1f, 1f, 1f, 0.7f * menuFadeOutTr);
            int length = Song.songInfo.Length / 1000;
            int length2 = (int)MainMenu.song.getTime() / 1000;
            Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00") + " / " + (length2 / 60) + ":" + (length2 % 60).ToString("00"), startX, -(endY - margin * 2), size * 1.25f, colWhite, new Vector2(1, -1));
        }
        public static void RenderMenu() {
            #region decorative
            if (needBGChange)
                if (!BGChanging)
                    changeBG();
            if (loadHw) {
                Textures.loadHighway();
                loadHw = false;
            }
            if (loadSkin) {
                Textures.load();
                loadSkin = false;
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreateOrthographic(game.width, game.height, -1f, 1f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            Graphics.drawRect(0, 0, 1f, 1f, 1f, 1f, 1f);
            double t = song.getTime();
            if (firstLoad) {
                if (!songLoad.IsAlive && (song.finishLoadingFirst || song.firstLoad) && SongScan.songsScanned != 0) {
                    firstLoad = false;
                    //songselected = new Random().Next(0, Song.songList.Count);
                    nextSong();
                    SongListTarget = songselected;
                    //songChange(false);
                }
            }
            if (!song.firstLoad) {
                if (t >= song.length * 1000 - 50 && menuWindow != 7) {
                    if (menuWindow == 1 || menuWindow == 4 || menuWindow == 5) {
                        if (!songLoad.IsAlive) {
                            songChange(true);
                        }
                    } else {
                        if (!songLoad.IsAlive) {
                            nextSong();
                        }
                    }
                }
                if (!Game)
                    if (MainMenu.song.stream.Length == 0 && menuWindow != 7) {
                        Console.WriteLine("Song doesnt have Length!");
                        if (song.finishLoadingFirst && !songLoad.IsAlive) {
                            Console.WriteLine("> Skipping");
                            if (menuWindow == 1 || menuWindow == 4 || menuWindow == 5) {
                                songChange();
                            } else {
                                nextSong();
                            }
                        }
                    }
                //
            }
            float bgScalew = (float)game.width / oldBG.Width;
            float bgScaleh = (float)game.height / oldBG.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            //Graphics.Draw(oldBG, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.White, Vector2.Zero);
            float BGtr = Ease.OutQuad(Ease.In((float)songChangeFade, SonsEaseBGLimit));
            if (BGtr < 1)
                Graphics.Draw(oldBG, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.White, Vector2.Zero);
            bgScalew = (float)game.width / Textures.background.Width;
            bgScaleh = (float)game.height / Textures.background.Height;
            if (bgScaleh > bgScalew) {
                bgScalew = bgScaleh;
            }
            /*if (bgScale < 1)
                bgScale = 1;*/
            //Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(0.655f * bgScale, 0.655f * bgScale), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            Graphics.Draw(Textures.background, Vector2.Zero, new Vector2(bgScalew, bgScalew), Color.FromArgb((int)(BGtr * 255), 255, 255, 255), Vector2.Zero);
            //TimeSpan t = song.getTime();
            int punch = 1000;
            float Punchscale = 0;
            if (drawMenuBackgroundFx) {
                if (Song.songLoaded) {
                    for (int i = 0; i < Song.beatMarkers.Count; i++) {
                        beatMarker n;
                        try {
                            n = Song.beatMarkers[i];
                        } catch {
                            break;
                        }
                        double delta = (n.time) - t;
                        if (delta >= 0)
                            break;
                        if (i < 0)
                            continue;
                        try {
                            if (delta <= -punch) {
                                if (Song.songLoaded)
                                    Song.beatMarkers.RemoveAt(i--);
                                continue;
                            }
                            if (delta <= 0) {
                                if (n.type == 1) {
                                    beatPunch.Restart();
                                    beatPunchSoft.Restart();
                                } else if (n.type == 0) {
                                    beatPunchSoft.Restart();
                                }
                                if (Song.songLoaded)
                                    Song.beatMarkers.RemoveAt(i--);
                            }
                        } catch { break; }
                    }
                }
                if (beatPunch.ElapsedMilliseconds != 0) {
                    double tr = beatPunch.Elapsed.TotalMilliseconds;
                    tr /= punch;
                    tr *= -1;
                    tr += 1;
                    tr *= 0.08f;
                    if (tr > 1) tr = 1.0;
                    if (tr < 0) tr = 0.0;
                    Graphics.EnableAdditiveBlend();
                    Graphics.drawRect(-game.width / 2, -game.height / 2, game.width / 2, game.height / 2, 1f, 1f, 1f, (float)tr);
                    Graphics.EnableAlphaBlend();
                    if (beatPunch.ElapsedMilliseconds > punch)
                        beatPunch.Reset();
                }
                punch = 400;
                if (beatPunchSoft.ElapsedMilliseconds != 0) {
                    Punchscale = (float)beatPunchSoft.Elapsed.TotalMilliseconds;
                    Punchscale = Ease.Out(1, 0, Ease.OutQuad(Ease.In(Punchscale, punch)));
                    if (Punchscale < 0)
                        Punchscale = 0;
                    if (beatPunchSoft.ElapsedMilliseconds > punch)
                        beatPunchSoft.Reset();
                }

                float[] fft = song.GetFFT(0, 100);
                Graphics.EnableAdditiveBlend();
                GL.Color4(1f, 1f, 1f, 0.15f);
                float start = getXCanvas(0, 0);
                float end = getXCanvas(0, 2);
                float step = (end - start) / (fft.Length - 3);
                float lvlH = getYCanvas(25);
                /*GL.Disable(EnableCap.Texture2D);
                GL.Begin(PrimitiveType.Quads);
                for (int i = 0; i < fft.Length - 3; i++) {
                    float x = i * step - end;
                    float xEnd = (i + 0.7f) * step - end;
                    float y = fft[i] * lvlH;
                    GL.Vertex2(x, y);
                    GL.Vertex2(xEnd, y);
                    GL.Vertex2(xEnd, -y);
                    GL.Vertex2(x, -y);
                }
                GL.End();
                GL.Enable(EnableCap.Texture2D);*/
                GL.BindTexture(TextureTarget.Texture2D, Textures.menuBar.ID);
                GL.Color4(Color.White);
                GL.Begin(PrimitiveType.Quads);
                for (int i = 0; i < fft.Length - 3; i++) {
                    float x = i * step - end;
                    float xEnd = (i + 1) * step - end;
                    float y = fft[i] * lvlH;
                    GL.TexCoord2(new Vector2(0, 0));
                    GL.Vertex2(x, y);
                    GL.TexCoord2(new Vector2(1, 0));
                    GL.Vertex2(xEnd, y);
                    GL.TexCoord2(new Vector2(1, 1));
                    GL.Vertex2(xEnd, -y);
                    GL.TexCoord2(new Vector2(0, 1));
                    GL.Vertex2(x, -y);
                }
                GL.End();
                Graphics.EnableAlphaBlend();
            }
            #endregion
            bool movedMouse = false;
            float mouseX = Input.mousePosition.X - (float)gameObj.Width / 2;
            float mouseY = -Input.mousePosition.Y + (float)gameObj.Height / 2;
            if (mouseX != pmouseX || mouseY != pmouseY) {
                movedMouse = true;
                menuFadeOut = 0;
            }
            pmouseX = mouseX;
            pmouseY = mouseY;
            float menuFadeOutTr = 1f;
            if (drawMenuBackgroundFx) {
                if (menuFadeOut > 30000) {
                    float map = (float)(menuFadeOut - 30000) / 10000.0f;
                    menuFadeOutTr = 1 - map;
                    if (menuFadeOutTr < 0) {
                        menuFadeOutTr = 0f;
                    }
                }
            } else
                menuFadeOut = 0;
            int menuFadeOutTr8 = (int)(menuFadeOutTr * 255);
            Color colWhite = Color.FromArgb(menuFadeOutTr8, 255, 255, 255);
            Color colYellow = Color.FromArgb(menuFadeOutTr8, 255, 255, 0);
            Color colGrey = Color.FromArgb(menuFadeOutTr8, 127, 127, 127);
            Color colGreyYellow = Color.FromArgb(menuFadeOutTr8, 127, 127, 0);
            float scalef = (float)game.height / 1366f;
            if (game.width < game.height) {
                scalef *= (float)game.width / game.height;
            }
            if (menuWindow == 0 || menuWindow == 8) {
                menuMainPos += (1 - menuMainPos) * 0.2f;
                menuOptionPos += (0 - menuOptionPos) * 0.2f;
                if (menuWindow == 8) {
                    menuMainGeneralPos += (2 - menuMainGeneralPos) * 0.2f;
                    menuMainPlayPos += (1 - menuMainPlayPos) * 0.2f;
                } else {
                    menuMainGeneralPos += (1 - menuMainGeneralPos) * 0.2f;
                    menuMainPlayPos += (0 - menuMainPlayPos) * 0.2f;
                }

            } else if (menuWindow == 2 || menuWindow == 3) {
                menuMainPos += (2 - menuMainPos) * 0.2f;
                menuOptionPos += (1 - menuOptionPos) * 0.2f;
            }
            bool click = mouseClicked;
            Vector2 scale = new Vector2(scalef, scalef);
            float textHeight = Draw.font.Height * scalef;
            PointF position = PointF.Empty;
            if (menuWindow == 1 || ((menuWindow == 4 || menuWindow == 5) && playerAmount == 1)) {
                menuFadeOut = 0;
                position.X = getXCanvas(8, 0);
                position.Y = getYCanvas(35);
                position.Y += 4 * textHeight;
                SongListPos += (SongListTarget - SongListPos) * 0.2;
                //Console.WriteLine(SongListTarget + ", " + SongListPos);
                position.Y -= (float)(SongListPos * textHeight);
                if (Song.songList.Count != 0) {
                    float level = (float)(SongListPos / Song.songList.Count);
                    float levelPercent = Draw.Lerp(getYCanvas(-40), getYCanvas(25), level);
                    float barSize = getYCanvas(5);
                    Graphics.drawRect(getXCanvas(4, 0), getYCanvas(-40) - barSize, getXCanvas(6, 0), getYCanvas(25) + barSize, 1, 1, 1, 0.2f);
                    Graphics.drawRect(getXCanvas(4, 0), levelPercent + barSize, getXCanvas(6, 0), levelPercent - barSize, 1, 1, 1, 0.7f);
                    if (SongScan.currentQuery.Equals("")) {
                        if (mouseClicked) {
                            if (mouseX > getXCanvas(4, 0) && mouseX < getXCanvas(6, 0) && mouseY < getYCanvas(-40) - barSize && mouseY > getYCanvas(25) + barSize) {
                                levelPercent = mouseY;
                                levelPercent += getYCanvas(40);
                                levelPercent /= getYCanvas(32.5f) * 2;
                                songselected = (int)Draw.Lerp(0, Song.songList.Count - 1, levelPercent);
                                songChange();
                            }
                        }
                    }
                    Graphics.drawRect(getXCanvas(7, 0), getYCanvas(-47.5f), getXCanvas(2.5f), getYCanvas(30), 0, 0, 0, 0.35f);
                    Graphics.drawRect(getXCanvas(5), getYCanvas(-47.5f), getXCanvas(50), getYCanvas(30), 0, 0, 0, 0.35f);
                    Vector2 songListScale = scale * 0.85f;
                    for (int i = 0; i < Song.songList.Count; i++) {
                        if (i >= Song.songListShow.Count)
                            break;
                        if (!Song.songListShow[i])
                            continue;
                        if (position.Y >= getYCanvas(47.5f) && position.Y < getYCanvas(-25)) {
                            if (songselected == i) {
                                Graphics.drawRect(
                                    position.X,
                                    -position.Y - textHeight,
                                    getXCanvas(0),
                                   -position.Y, 1f, 1f, 1f, 0.5f);
                            }
                            float lengthScale = 1f;
                            if (Draw.GetWidthString(Song.songList[i].Name, songListScale) - (-position.X) > getXCanvas(0))
                                lengthScale = 0.8f;
                            bool rechedLimit = Draw.DrawString(Song.songList[i].Name, position.X, position.Y, new Vector2(songListScale.X * lengthScale, songListScale.Y), songselected == i ? colYellow : colWhite, new Vector2(1, 1), 0, getXCanvas(0) - 20);
                            if (rechedLimit) {
                                Draw.DrawString("...", getXCanvas(-2), position.Y, songListScale, songselected == i ? colYellow : colWhite, new Vector2(1, 1));
                            }
                        }
                        position.Y += textHeight;
                    }
                    if (!(menuWindow == 4 || menuWindow == 5)) {
                        position.Y = getYCanvas(-25f);
                        Draw.DrawString(Language.songSortbyInstrument + (SongScan.useInstrument ? Language.songSortinsOn : Language.songSortinsOff), getXCanvas(5), position.Y, scale / 1.3f, colWhite, new Vector2(1, 1));
                        position.Y = getYCanvas(45);
                        string sortType = "";
                        switch (SongScan.sortType) {
                            case (int)SortType.Album: sortType = Language.songSortAlbum; break;
                            case (int)SortType.Artist: sortType = Language.songSortArtist; break;
                            case (int)SortType.Charter: sortType = Language.songSortCharter; break;
                            case (int)SortType.Genre: sortType = Language.songSortGenre; break;
                            case (int)SortType.Length: sortType = Language.songSortLength; break;
                            case (int)SortType.Name: sortType = Language.songSortName; break;
                            case (int)SortType.Path: sortType = Language.songSortPath; break;
                            case (int)SortType.Year: sortType = Language.songSortYear; break;
                            case (int)SortType.MaxDiff: sortType = Language.songSortDiff; break;
                            default: sortType = "{default}"; break;
                        }
                        Draw.DrawString(Language.songSortBy + sortType, getXCanvas(5), position.Y, scale / 1.2f, colWhite, new Vector2(1, 1));
                        if (!(menuWindow == 4 || menuWindow == 5))
                            Draw.DrawString(Language.songCount + Song.songList.Count, getXCanvas(45), position.Y, scale / 1.2f, colWhite, new Vector2(1, 1));
                    }
                }
                if (menuWindow == 4 || menuWindow == 5) { //solo quiero mantener ordenado
                    if (playerAmount == 1) {
                        position.X = getXCanvas(8);
                        position.Y = getYCanvas(35);
                        //position.Y += textHeight;
                        Draw.DrawString(Language.songDiffList, position.X, position.Y, scale, colWhite, Vector2.Zero);
                        position.Y += textHeight;
                        position.X = getXCanvas(12);
                        int skip = 0;
                        if (Song.songInfo.dificulties.Length > 7) {
                            if (playerInfos[0].difficulty > 4) {
                                skip = playerInfos[0].difficulty - 4;
                            }
                        }
                        if (Song.songInfo.diffs != null)
                            if (Song.songInfo.dificulties.Length != Song.songInfo.diffs.Length) {
                                Song.songInfo.diffs = null;
                            }
                        for (int i = skip; i < Song.songInfo.dificulties.Length; i++) {
                            string diffString = GetDifficulty(Song.songInfo.dificulties[i], Song.songInfo.ArchiveType);
                            if (i - skip >= 7) {
                                Draw.DrawString("...", position.X, position.Y, scale, colWhite, Vector2.Zero);
                                break;
                            }
                            Draw.DrawString(diffString, position.X, position.Y, scale, playerInfos[0].difficulty == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight;
                            if (Song.songInfo.diffs != null && Song.songInfo.diffs.Length != 0) {
                                Draw.DrawString(Song.songInfo.diffs[i].ToString("0.00") + "* ", position.X + getXCanvas(5), position.Y, scale / 1.5f, playerInfos[0].difficulty == i ? colYellow : colWhite, new Vector2(1, -1));
                            } else
                                Draw.DrawString("...", position.X + getXCanvas(5), position.Y, scale / 1.5f, playerInfos[0].difficulty == i ? colYellow : colWhite, new Vector2(1, -1));
                            position.Y += textHeight / 2f;
                        }
                        position.X = (getXCanvas(0, 2) + getXCanvas(0)) / 2;
                        position.Y = getYCanvas(50) + textHeight;
                        Draw.DrawString(Language.recordsList, position.X, position.Y, scale, menuWindow == 5 ? colYellow : colWhite, Vector2.Zero);
                        int RecordCount = 0;
                        int recordStart = 0;
                        if (records.Count > 4) {
                            if (recordSelect > 2)
                                recordStart = recordSelect - 2;
                        }
                        RecordCount = -recordStart;
                        int RecordMax = 0;
                        //Console.WriteLine(recordsLoaded + "," + records.Count);
                        if (recordsLoaded) {
                            if (records.Count == 0) {
                                position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                position.Y = getYCanvas(0);
                                Draw.DrawString(Language.recordsNorec, position.X, position.Y, scale, menuWindow == 5 ? colYellow : colWhite, Vector2.Zero);
                            } else {
                                for (int i = 0; i < records.Count; i++) {
                                    if (records[i].diff == null) continue;
                                    if (records[i].diff[0] == null) continue;
                                    if (records[i].diff[1] == null) continue;
                                    if (records[i].diff[2] == null) continue;
                                    if (records[i].diff[3] == null) continue;
                                    bool match = false;
                                    string diffString = Song.songInfo.dificulties[playerInfos[0].difficulty];
                                    for (int p = 0; p < records[i].players; p++) {
                                        if (records[i].diff[p].Equals(diffString))
                                            match = true;
                                    }
                                    if (!match)
                                        continue;
                                    float y1 = getYCanvas((-40 + (15 * RecordCount)) + 1);
                                    float y2 = getYCanvas(-40 + (15 * (RecordCount + 1)));
                                    Graphics.drawRect((getXCanvas(0, 2) + getXCanvas(0)) / 2, y1, getXCanvas(0, 2), y2, 1f, 1f, 1f, recordSelect - recordStart == RecordCount && menuWindow == 5 ? 0.7f : 0.4f);
                                    RecordCount++;
                                    float acc = 0;
                                    for (int p = 0; p < records[i].players; p++)
                                        acc += (float)(records[i].accuracy[p] / 100f);
                                    acc /= records[i].players;
                                    string accStr = acc.ToString("0.00") + "%";
                                    float accPos = getXCanvas(-3, 2) - Draw.GetWidthString(accStr, scale * 0.7f);
                                    string modStr = "";
                                    if (records[i].hard[0])
                                        modStr += "HR,";
                                    if (records[i].hidden[0] == 1)
                                        modStr += "HD,";
                                    if (records[i].easy[0])
                                        modStr += "EZ,";
                                    if (records[i].nofail[0])
                                        modStr += "NF,";
                                    if (records[i].speed[0] != 100)
                                        modStr += "S" + records[i].speed[0] + ",";
                                    if (records[i].mode[0] != 1)
                                        modStr += "MD" + records[i].mode[0] + ",";
                                    if (modStr.Length > 0)
                                        modStr = modStr.TrimEnd(',');
                                    float modPos = getXCanvas(-3, 2) - Draw.GetWidthString(modStr, scale * 0.7f);
                                    position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                    position.Y = getYCanvas((38 - (15 * (RecordCount - 1))) + 1) + textHeight;
                                    string name = records[i].name[0];
                                    for (int p = 1; p < records[i].players; p++)
                                        name += ", " + records[i].name[p];
                                    Draw.DrawString(name, position.X, position.Y, scale, colWhite, Vector2.Zero);
                                    Draw.DrawString(modStr, modPos, position.Y, scale * 0.7f, colWhite, Vector2.Zero);
                                    int totalScore = records[i].totalScore;
                                    position.Y += textHeight;
                                    Draw.DrawString((records[i].failsong ? "F: " : "") + totalScore + "", position.X, position.Y, scale, colWhite, Vector2.Zero);
                                    Draw.DrawString(accStr, accPos, position.Y, scale * 0.7f, colWhite, Vector2.Zero);
                                    RecordMax++;
                                }
                                recordMenuMax = RecordMax - 1;
                                if (RecordMax == 0) {
                                    position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                                    position.Y = getYCanvas(0);
                                    Draw.DrawString(Language.recordsNorec, position.X, position.Y, scale, menuWindow == 5 ? colYellow : colWhite, Vector2.Zero);
                                    Draw.DrawString(Language.recordsSong, position.X, position.Y + textHeight, scale / 1.2f, menuWindow == 5 ? colYellow : colWhite, Vector2.Zero);
                                }
                            }
                        } else {
                            position.X = (getXCanvas(0, 2) + getXCanvas(10)) / 2;
                            position.Y = getYCanvas(0);
                            Draw.DrawString(Language.recordsLoading, position.X, position.Y, scale, colWhite, Vector2.Zero);
                        }
                    }
                } else {
                    position.X = getXCanvas(10);
                    position.Y = getYCanvas(10);
                    Vector2 infoScale = scale * 0.8f;
                    float infoTextHeight = textHeight * 0.8f;
                    if (album.ID != 0)
                        Graphics.Draw(album, new Vector2(position.X, position.Y), infoScale * 0.8f, colWhite, new Vector2(1, -1));
                    if (Song.songInfo.Artist != null) {
                        Draw.DrawString(Song.songInfo.Artist, position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        Draw.DrawString(Song.songInfo.Album, position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        Draw.DrawString(Song.songInfo.Charter, position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        Draw.DrawString(Song.songInfo.Year, position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        Draw.DrawString(Song.songInfo.Genre, position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        Draw.DrawString(Song.songInfo.maxDiff.ToString("0.00") + "*", position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        position.Y += infoTextHeight;
                        int length = Song.songInfo.Length / 1000;
                        if (length > 0)
                            Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00"), position.X, position.Y, infoScale, colWhite, new Vector2(1, 1));
                        else {
                            length = (int)(song.length);
                            if (song.length != 0)
                                Draw.DrawString((length / 60) + ":" + (length % 60).ToString("00") + ",", position.X, position.Y, scale, colWhite, new Vector2(1, 1));
                            else {
                                Draw.DrawString("Null: " + song.length, position.X, position.Y, scale, colWhite, new Vector2(1, 1));
                            }
                        }
                    }
                    //position.Y += textHeight;
                }
                if (menuWindow == 5) {

                }
                if (typingQuery) {
                    float queryWidth = Draw.GetWidthString(searchQuery, scale) + 50;
                    float rectwidth = getXCanvas(15);
                    if (queryWidth / 2 > getXCanvas(15))
                        rectwidth = queryWidth / 2;
                    Graphics.drawRect(-rectwidth, -getYCanvas(-10), rectwidth, getYCanvas(-10), 0f, 0f, 0f, 0.7f);
                    Draw.DrawString("Search:", -Draw.GetWidthString("Search:", scale) / 2, getYCanvas(10), scale * 0.8f, colWhite, Vector2.Zero);
                    Draw.DrawString(searchQuery + "_", getXCanvas(0) - (rectwidth - getXCanvas(5)), 0, scale, colWhite, Vector2.Zero);
                }
            }
            if ((menuWindow == 4 || menuWindow == 5) && playerAmount > 1) {
                /*
                for (int player = 0; player < playerAmount; player++) {
                    textRenderer.renderer.Clear(Color.Transparent);
                    position.X = getX(-40 + (20 * player));
                    position.Y = getY(-50);
                    position.Y += font.Height;
                    textRenderer.renderer.DrawString("  " + playerInfos[player].playerName, font, ItemNotSelected, position);
                    position.Y += font.Height * 1.5f;
                    for (int i = 0; i < Song.songInfo.dificulties.Length; i++) {
                        string diffString = Song.songInfo.dificulties[i];
                        if (diffString.Equals("ExpertSingle"))
                            diffString = "Expert";
                        if (diffString.Equals("HardSingle"))
                            diffString = "Hard";
                        if (diffString.Equals("MediumSingle"))
                            diffString = "Medium";
                        if (diffString.Equals("EasySingle"))
                            diffString = "Easy";
                        textRenderer.renderer.DrawString(diffString, font, playerInfos[player].difficulty == i ? ItemSelected : ItemNotSelected, position);
                        position.Y += font.Height;
                    }
                    Graphics.Draw(textRenderer.renderer.texture, new Vector2(2, 2), Vector2.One, Color.Black, Vector2.Zero);
                    Graphics.Draw(textRenderer.renderer.texture, Vector2.Zero, Vector2.One, colWhite, Vector2.Zero);
                }
                */
            }
            if (menuMainPos > 0.1f && menuMainPos < 1.9f && (menuWindow == 0 || menuWindow == 8 || menuWindow == 2 || menuWindow == 3)) {
                if (menuMainGeneralPos * menuMainPos > 0.1f && menuMainGeneralPos * menuMainPos < 1.9f) {
                    position.X = Draw.Lerp(getXCanvas(60, 3), getXCanvas(5), menuMainGeneralPos * menuMainPos);
                    position.Y = getYCanvas(25);
                    float fadeTr = menuMainGeneralPos * menuMainPos;
                    if (fadeTr > 1) {
                        fadeTr = -fadeTr;
                        fadeTr += 2;
                    }
                    Color selected = Color.FromArgb((int)((fadeTr * menuFadeOutTr) * 255), 255, 255, 50);
                    Color notSelected = Color.FromArgb((int)((fadeTr * menuFadeOutTr) * 255), 255, 255, 255);
                    Color selectedOpaque = Color.FromArgb((int)((fadeTr * menuFadeOutTr) * 255), 127, 127, 25);
                    Color notSelectedOpaque = Color.FromArgb((int)((fadeTr * menuFadeOutTr) * 255), 127, 127, 127);
                    int tr = (int)(Punchscale * 255 * fadeTr) - 70;
                    if (drawMenuBackgroundFx) {
                        float[] level = song.GetLevel(0);
                        if (level != null && level.Length > 1) {
                            float target = (level[0] + level[1]) / 2;
                            if (target > SongVolume)
                                SongVolume += (target - SongVolume) * 0.7f;
                            else
                                SongVolume += (target - SongVolume) * 0.2f;
                        }
                    } else
                        SongVolume = 0;
                    if (tr < 0) tr = 0;
                    int prevMenuSelect = mainMenuSelect;
                    float halfx = Draw.GetWidthString("a", scale * 2) / 2;
                    float halfy = textHeight;
                    if (mouseX > position.X - halfx && mouseX < position.X + Draw.GetWidthString(mainMenuText[0], scale * 2) - halfx)
                        if (mouseY > -position.Y - halfy && mouseY < -position.Y + textHeight * 2 - halfy && movedMouse)
                            mainMenuSelect = 0;
                    float volumePunch = (SongVolume * SongVolume) * 2f;
                    Draw.DrawString(mainMenuText[0], position.X, position.Y, scale * 2 * ((-Punchscale + 2) / 3 + 1) * (0.1f * -menuTextFadeNow[1] + 1.25f), mainMenuSelect == 0 ? Color.FromArgb((int)(tr * menuFadeOutTr), 255, 255, 0) : Color.FromArgb((int)(tr * menuFadeOutTr), 255, 255, 255), Vector2.Zero);
                    Draw.DrawString(mainMenuText[0], position.X, position.Y, scale * 2 * ((Punchscale + volumePunch) / 6 + 1) * (0.1f * menuTextFadeNow[0] + 1), mainMenuSelect == 0 ? selected : notSelected, Vector2.Zero);
                    position.Y += textHeight * 2;
                    if (mouseX > position.X - halfx && mouseX < position.X + Draw.GetWidthString(mainMenuText[1], scale * 2) - halfx)
                        if (mouseY > -position.Y - halfy && mouseY < -position.Y + textHeight * 2 - halfy && movedMouse)
                            mainMenuSelect = 1;
                    Draw.DrawString(mainMenuText[1], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[1] + 1), mainMenuSelect == 1 ? selectedOpaque : notSelectedOpaque, Vector2.Zero);
                    position.Y += textHeight * 2;
                    if (mouseX > position.X - halfx && mouseX < position.X + Draw.GetWidthString(mainMenuText[2], scale * 2) - halfx)
                        if (mouseY > -position.Y - halfy && mouseY < -position.Y + textHeight * 2 - halfy && movedMouse)
                            mainMenuSelect = 2;
                    Draw.DrawString(mainMenuText[2], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[2] + 1), mainMenuSelect == 2 ? selected : notSelected, Vector2.Zero);
                    position.Y += textHeight * 2;
                    if (mouseX > position.X - halfx && mouseX < position.X + Draw.GetWidthString(mainMenuText[3], scale * 2) - halfx)
                        if (mouseY > -position.Y - halfy && mouseY < -position.Y + textHeight * 2 - halfy && movedMouse)
                            mainMenuSelect = 3;
                    Draw.DrawString(mainMenuText[3], position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[3] + 1), mainMenuSelect == 3 ? selected : notSelected, Vector2.Zero);
                    if (prevMenuSelect != mainMenuSelect) {
                        menuTextFadeTime = new double[4] { 0, 0, 0, 0 };
                        menuTextFadeNow.CopyTo(menuTextFadeStart, 0);
                        menuTextFadeEnd = new float[4] { 0, 0, 0, 0 };
                        menuTextFadeEnd[mainMenuSelect] = 1f;
                    }
                }
                if (menuMainPlayPos * menuMainPos > 0.1f && menuMainPlayPos * menuMainPos < 1.9f) {
                    position.X = Draw.Lerp(getXCanvas(60, 3), getXCanvas(5), menuMainPlayPos * menuMainPos);
                    float tr = menuMainPlayPos * menuMainPos;
                    if (tr > 1) {
                        tr = -tr;
                        tr += 2;
                    }
                    Color selected = Color.FromArgb((int)((tr * menuFadeOutTr) * 255), 255, 255, 50);
                    Color notSelected = Color.FromArgb((int)((tr * menuFadeOutTr) * 255), 255, 255, 255);
                    Color selectedOpaque = Color.FromArgb((int)((tr * menuFadeOutTr) * 255), 127, 127, 25);
                    Color notSelectedOpaque = Color.FromArgb((int)((tr * menuFadeOutTr) * 255), 127, 127, 127);
                    position.Y = getYCanvas(25);
                    Draw.DrawString(Language.menuLclPlay, position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[0] + 1), mainMenuSelect == 0 ? selected : notSelected, Vector2.Zero);
                    position.Y += textHeight * 2;
                    Draw.DrawString(Language.menuOnlPlay, position.X, position.Y, scale * 2 * (0.1f * menuTextFadeNow[1] + 1), mainMenuSelect == 1 ? selectedOpaque : notSelectedOpaque, Vector2.Zero);
                }
                drawPlayer();
                position.X = getXCanvas(-45);
            }
            if (menuOptionPos > 0.1f && menuOptionPos < 1.9f && (menuWindow == 0 || menuWindow == 8 || menuWindow == 2 || menuWindow == 3)) {
                menuFadeOut = 0;
                position.X = Draw.Lerp(getXCanvas(60, 3), getXCanvas(5), menuOptionPos);
                float otr = menuOptionPos;
                if (otr > 1) {
                    otr = -otr;
                    otr += 2;
                }
                Color itemSelected = Color.FromArgb((int)(otr * 255), 255, 255, 50);
                Color itemNotSelected = Color.FromArgb((int)(otr * 255), 255, 255, 255);
                Color selectedOpaque = Color.FromArgb((int)(otr * 255), 127, 127, 25);
                Color notSelectedOpaque = Color.FromArgb((int)(otr * 255), 127, 127, 127);
                //position.X = getXCanvas(-35);
                position.X = Draw.Lerp(getXCanvas(30, 3), getXCanvas(-35), menuOptionPos);
                position.Y = getYCanvas(25);
                for (int i = 0; i < optionsText.Length; i++) {
                    Draw.DrawString(optionsText[i], position.X, position.Y, scale, optionsSelect == i ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                }
                //float defaultX = getXCanvas(5);
                float defaultX = Draw.Lerp(getXCanvas(60, 3), getXCanvas(5), menuOptionPos);
                position.X = defaultX;
                position.Y = getYCanvas(25);

                float textWidth = Draw.GetWidthString(Language.optionController, scale * 1.1f);
                float tr = 0.4f;
                float Y = getYCanvas(-20);
                float X = defaultX + getXCanvas(-45);
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        controllerBindPlayer = 1;
                        menuWindow = 6;
                        onSubOptionItem = false;
                        click = false;
                        mouseClicked = false;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr * tr);
                Draw.DrawString(Language.optionController, X, Y, scale, tr > 0.5f ? itemSelected : itemNotSelected, new Vector2(1, 1));

                if (menuWindow != 3) {
                    itemSelected = notSelectedOpaque;
                    itemNotSelected = notSelectedOpaque;
                }
                if (optionsSelect == 0) {
                    Draw.DrawString((fullScreen ? (char)(7) : (char)(8)) + Language.optionVideoFullscreen, position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((vSync ? (char)(7) : (char)(8)) + Language.optionVideoVSync, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 2) {
                        Draw.DrawString(Language.optionVideoFPS +
                            " < " + (subOptionItemFrameRate == 0 ? Language.optionVideoUnlimited : subOptionItemFrameRate.ToString()) + " > ", position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    } else
                        Draw.DrawString(Language.optionVideoFPS + (game.Fps == 9999 ? Language.optionVideoUnlimited : "" + game.Fps), position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    if (game.Fps == 9999) {
                        position.Y += textHeight * 0.7f;
                        Draw.DrawString(Language.optionVideoThreadWarning, position.X, position.Y, scale * 0.5f, itemNotSelected, Vector2.Zero);
                    }
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 3) {
                        Draw.DrawString(Language.optionVideoResolution +
                            (subOptionItemResolutionSelect > 0 && subOptionItemResolution.Length > 1 ? " < " : "") + subOptionItemResolution[subOptionItemResolutionSelect][0] + "x" + subOptionItemResolution[subOptionItemResolutionSelect][1] +
                            (subOptionItemResolutionSelect < subOptionItemResolution.Length - 1 ? " > " : "")
                            , position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    } else
                        Draw.DrawString(Language.optionVideoResolution + game.width + "x" + game.height, position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((Draw.showFps ? (char)(7) : (char)(8)) + Language.optionVideoShowFPS, position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.MyPCisShit ? (char)(7) : (char)(8)) + Language.optionVideoExtreme, position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(string.Format(Language.optionVideoTailQuality, (Draw.tailSizeMult == 1 ? "0.5x" : Draw.tailSizeMult == 2 ? "1x" : "2x")), position.X, position.Y, scale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight * 0.7f;
                    Draw.DrawString(Language.optionRestart, position.X, position.Y, scale * 0.5f, itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;

                    Draw.DrawString((game.isSingleThreaded ? (char)(7) : (char)(8)) + Language.optionVideoSingleThread, position.X, position.Y, scale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight * 0.7f;
                    Draw.DrawString(Language.optionRestart, position.X, position.Y, scale * 0.5f, itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((drawMenuBackgroundFx ? (char)(7) : (char)(8)) + Language.optionVideoMenuFx, position.X, position.Y, scale, subOptionSelect == 8 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                } else if (optionsSelect == 1) {
                    if (onSubOptionItem && subOptionSelect == 0)
                        Draw.DrawString(Language.optionAudioMaster + "< " + Math.Round(Audio.masterVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString(Language.optionAudioMaster + Math.Round(Audio.masterVolume * 100), position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 1)
                        Draw.DrawString(Language.optionAudioOffset + "< " + MainGame.AudioOffset + ">", position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString(Language.optionAudioOffset + MainGame.AudioOffset, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 2)
                        Draw.DrawString(Language.optionAudioFx + "< " + Math.Round(Sound.fxVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString(Language.optionAudioFx + Math.Round(Sound.fxVolume * 100), position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 3)
                        Draw.DrawString(Language.optionAudioMania + "< " + Math.Round(Sound.maniaVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString(Language.optionAudioMania + Math.Round(Sound.maniaVolume * 100), position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 4)
                        Draw.DrawString(Language.optionAudioMusic + "< " + Math.Round(Audio.musicVolume * 100) + ">", position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    else
                        Draw.DrawString(Language.optionAudioMusic + Math.Round(Audio.musicVolume * 100), position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((Audio.keepPitch ? (char)(7) : (char)(8)) + Language.optionAudioPitch, position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((Audio.onFailPitch ? (char)(7) : (char)(8)) + Language.optionAudioFail, position.X, position.Y, scale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionAudioEngine + (Sound.OpenAlMode ? Language.optionAudioLagfree : Language.optionAudioInstant), position.X, position.Y, scale, subOptionSelect == 7 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else if (optionsSelect == 2) {
                    Draw.DrawString(Language.optionKeysIncrease + volumeUpKey, position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionKeysDecrease + volumeDownKey, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionKeysPrev + songPrevKey, position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionKeysPause + songPauseResumeKey, position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionKeysNext + songNextKey, position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else if (optionsSelect == 3) {
                    Draw.DrawString((Draw.tailWave ? (char)(7) : (char)(8)) + Language.optionGameplayTailwave, position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.drawSparks ? (char)(7) : (char)(8)) + Language.optionGameplayDrawspark, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionGameplayScan, position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.failanimation ? (char)(7) : (char)(8)) + Language.optionGameplayFailanim, position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.songfailanimation ? (char)(7) : (char)(8)) + Language.optionGameplayFailanim, position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionGameplayLanguage + (Language.language == "en" ? "English" : Language.language == "es" ? "Español (Spanish)" : Language.language == "jp" ? "日本語 (Japanese)" : "???"), position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString((MainGame.useGHhw ? (char)(7) : (char)(8)) + Language.optionGameplayHighway, position.X, position.Y, scale, subOptionSelect == 6 ? itemSelected : itemNotSelected, Vector2.Zero);
                } else if (optionsSelect == 4) {
                    Draw.DrawString(Language.optionSkinCustomscan, position.X, position.Y, scale, subOptionSelect == 0 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    Draw.DrawString(Language.optionSkinSkin, position.X, position.Y, scale, subOptionSelect == 1 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    float plus = getXCanvas(5);
                    if (onSubOptionItem && subOptionSelect == 1) {
                        for (int i = 0; i < subOptionItemSkin.Length; i++) {
                            Draw.DrawString(subOptionItemSkin[i], position.X + plus, position.Y, scale * 0.8f, subOptionItemSkinSelect == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight * 0.8f;
                        }
                    }
                    Draw.DrawString(string.Format(Language.optionSkinHighway, 1), position.X, position.Y, scale, subOptionSelect == 2 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 2) {
                        for (int i = 0; i < subOptionItemHw.Length; i++) {
                            Draw.DrawString(Path.GetFileNameWithoutExtension(subOptionItemHw[i]), position.X + plus, position.Y, scale * 0.8f, subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight * 0.8f;
                        }
                    }
                    Draw.DrawString(string.Format(Language.optionSkinHighway, 2), position.X, position.Y, scale, subOptionSelect == 3 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 3) {
                        for (int i = 0; i < subOptionItemHw.Length; i++) {
                            Draw.DrawString(Path.GetFileNameWithoutExtension(subOptionItemHw[i]), position.X + plus, position.Y, scale * 0.8f, subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight * 0.8f;
                        }
                    }
                    Draw.DrawString(string.Format(Language.optionSkinHighway, 3), position.X, position.Y, scale, subOptionSelect == 4 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 4) {
                        for (int i = 0; i < subOptionItemHw.Length; i++) {
                            Draw.DrawString(Path.GetFileNameWithoutExtension(subOptionItemHw[i]), position.X + plus, position.Y, scale * 0.8f, subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight * 0.8f;
                        }
                    }
                    Draw.DrawString(string.Format(Language.optionSkinHighway, 4), position.X, position.Y, scale, subOptionSelect == 5 ? itemSelected : itemNotSelected, Vector2.Zero);
                    position.Y += textHeight;
                    if (onSubOptionItem && subOptionSelect == 5) {
                        for (int i = 0; i < subOptionItemHw.Length; i++) {
                            Draw.DrawString(Path.GetFileNameWithoutExtension(subOptionItemHw[i]), position.X + plus, position.Y, scale * 0.8f, subOptionItemHwSelect == i ? colYellow : colWhite, Vector2.Zero);
                            position.Y += textHeight * 0.8f;
                        }
                    }
                }
            }
            if (menuWindow == 6) {
                menuFadeOut = 0;
                float X = getXCanvas(-44);
                float Y = getXCanvas(-45);
                Vector2 topleft = new Vector2(1, 1);
                float textWidth = Draw.GetWidthString(string.Format(Language.optionButtonPlayer, 1), scale * 1.1f);
                float tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click)
                        controllerBindPlayer = 1;
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(string.Format(Language.optionButtonPlayer, 1), X, Y, scale, controllerBindPlayer == 1 ? colYellow : colWhite, topleft);
                X = getXCanvas(-20);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click)
                        controllerBindPlayer = 2;
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(string.Format(Language.optionButtonPlayer, 2), X, Y, scale, controllerBindPlayer == 2 ? colYellow : colWhite, topleft);
                X = getXCanvas(4);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click)
                        controllerBindPlayer = 3;
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(string.Format(Language.optionButtonPlayer, 3), X, Y, scale, controllerBindPlayer == 3 ? colYellow : colWhite, topleft);
                X = getXCanvas(28);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click)
                        controllerBindPlayer = 4;
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(string.Format(Language.optionButtonPlayer, 4), X, Y, scale, controllerBindPlayer == 4 ? colYellow : colWhite, topleft);

                X = getXCanvas(-51);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + Draw.GetWidthString("<", scale * 1.4f) && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        menuWindow = 2;
                        LoadOptions();
                        SaveChanges();
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + Draw.GetWidthString("<", scale * 1.4f), -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString("<", X, Y, scale, colWhite, topleft);
                X = getXCanvas(-59);
                Y += textHeight * 1.5f;
                Draw.DrawString(Language.optionButtonInstrument, X, Y, scale, colWhite, topleft);
                X += Draw.GetWidthString(Language.optionButtonInstrument, scale);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        playerInfos[controllerBindPlayer - 1].gamepadMode = false;
                        playerInfos[controllerBindPlayer - 1].instrument = Instrument.Fret5;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButton5Fret, X, Y, scale,
                    (playerInfos[controllerBindPlayer - 1].instrument == Instrument.Fret5
                     && !playerInfos[controllerBindPlayer - 1].gamepadMode) ? colYellow : colWhite, topleft);
                X += textWidth * 1.05f;
                tr = 0.4f;
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButton6Fret, X, Y, scale, Color.Gray, topleft);
                X += textWidth * 1.05f;
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        playerInfos[controllerBindPlayer - 1].gamepadMode = true;
                        playerInfos[controllerBindPlayer - 1].instrument = Instrument.Fret5;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButtonGamepad, X, Y, scale, playerInfos[controllerBindPlayer - 1].gamepadMode ? colYellow : colWhite, topleft);
                X += textWidth * 1.05f;
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        playerInfos[controllerBindPlayer - 1].gamepadMode = false;
                        playerInfos[controllerBindPlayer - 1].instrument = Instrument.Drums;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButtonDrums, X, Y, scale, playerInfos[controllerBindPlayer - 1].instrument == Instrument.Drums ? colYellow : colWhite, topleft);
                //
                X = getXCanvas(-50);
                Y = getXCanvas(-30);
                Draw.DrawString(Language.optionButtonKeyboard, X, Y, scale, colWhite, topleft);
                X = getXCanvas(-60);
                Y = getXCanvas(-22);
                Draw.DrawString(Language.optionButtonGreen, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonRed, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonYellow, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonBlue, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonOrange, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonOpen, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonSix, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonStart, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonSp, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonUp, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonDown, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonWhammy, X, Y, scale, colWhite, topleft);
                X = getXCanvas(-32);
                Y = getXCanvas(-22);
                for (int i = 0; i < 12; i++) {
                    tr = 0.4f;
                    if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                        if (click) {
                            subOptionSelect = i + 2;
                            onSubOptionItem = true;
                        }
                        tr = 0.6f;
                    }
                    Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 0.95f, 1, 1, 1, tr);
                    string text = "";
                    if (i == 0) text = playerInfos[controllerBindPlayer - 1].green + "";
                    if (i == 1) text = playerInfos[controllerBindPlayer - 1].red + "";
                    if (i == 2) text = playerInfos[controllerBindPlayer - 1].yellow + "";
                    if (i == 3) text = playerInfos[controllerBindPlayer - 1].blue + "";
                    if (i == 4) text = playerInfos[controllerBindPlayer - 1].orange + "";
                    if (i == 5) text = playerInfos[controllerBindPlayer - 1].open + "";
                    if (i == 6) text = playerInfos[controllerBindPlayer - 1].six + "";
                    if (i == 7) text = playerInfos[controllerBindPlayer - 1].start + "";
                    if (i == 8) text = playerInfos[controllerBindPlayer - 1].select + "";
                    if (i == 9) text = playerInfos[controllerBindPlayer - 1].up + "";
                    if (i == 10) text = playerInfos[controllerBindPlayer - 1].down + "";
                    if (i == 11) text = playerInfos[controllerBindPlayer - 1].whammy + "";
                    if (subOptionSelect == i + 2 && onSubOptionItem) text = "...";
                    Draw.DrawString(text, X, Y, scale, subOptionSelect == i + 2 && onSubOptionItem ? colYellow : colWhite, topleft);
                    Y += textHeight;
                }
                X = getXCanvas(-60);
                tr = 0.4f;
                if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click)
                        playerInfos[controllerBindPlayer - 1].leftyMode = !playerInfos[controllerBindPlayer - 1].leftyMode;
                    tr = 0.6f;
                }
                X += textWidth / 2;
                Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButtonLefty, X, Y, scale, playerInfos[controllerBindPlayer - 1].leftyMode ? colYellow : colWhite, topleft);
                tr = 0.4f;
                //GamePad
                X = getXCanvas(10);
                Y = getXCanvas(-30);
                Draw.DrawString(Language.optionButtonGamepad, X, Y, scale, colWhite, topleft);
                X = getXCanvas(0);
                Y = getXCanvas(-22);
                Draw.DrawString(Language.optionButtonGreen, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonRed, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonYellow, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonBlue, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonOrange, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonOpen, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonSix, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonStart, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonSp, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonUp, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonDown, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonWhammy, X, Y, scale, colWhite, topleft);
                Y += textHeight;
                Draw.DrawString(Language.optionButtonAxis, X, Y, scale, colWhite, topleft);
                X = getXCanvas(28);
                Y = getXCanvas(-22);
                for (int i = 0; i < 13; i++) {
                    tr = 0.4f;
                    if (mouseX > X && mouseX < X + textWidth && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                        if (click) {
                            subOptionSelect = i + 14;
                            onSubOptionItem = true;
                        }
                        tr = 0.6f;
                    }
                    Graphics.drawRect(X, -Y, X + textWidth, -Y - textHeight * 1.1f, 1, 1, 1, tr);
                    if (i == 12) {
                        float length = Draw.Lerp(X, X + textWidth, (float)playerInfos[controllerBindPlayer - 1].LastAxis / 200 + 0.5f);
                        Graphics.drawRect(X, -Y, length, -Y - textHeight * 1.1f, 1, 0, 0, 0.2f);
                    }
                    string text = "";
                    if (i == 0) text = playerInfos[controllerBindPlayer - 1].ggreen + "";
                    if (i == 1) text = playerInfos[controllerBindPlayer - 1].gred + "";
                    if (i == 2) text = playerInfos[controllerBindPlayer - 1].gyellow + "";
                    if (i == 3) text = playerInfos[controllerBindPlayer - 1].gblue + "";
                    if (i == 4) text = playerInfos[controllerBindPlayer - 1].gorange + "";
                    if (i == 5) text = playerInfos[controllerBindPlayer - 1].gopen + "";
                    if (i == 6) text = playerInfos[controllerBindPlayer - 1].gsix + "";
                    if (i == 7) text = playerInfos[controllerBindPlayer - 1].gstart + "";
                    if (i == 8) text = playerInfos[controllerBindPlayer - 1].gselect + "";
                    if (i == 9) text = playerInfos[controllerBindPlayer - 1].gup + "";
                    if (i == 10) text = playerInfos[controllerBindPlayer - 1].gdown + "";
                    if (i == 11) text = playerInfos[controllerBindPlayer - 1].gwhammy + "";
                    if (i == 12) text = playerInfos[controllerBindPlayer - 1].gWhammyAxis + "";
                    int o;
                    if (subOptionSelect == i + 14 && onSubOptionItem) text = "...";
                    else {
                        int.TryParse(text, out o);
                        if (o >= 0)
                            text = "Button " + o;
                        if (o < 0)
                            text = "Axis " + Math.Abs(o);
                        if (o > 100)
                            text = "Pad " + (o - 100);
                        if (o > 500)
                            text = "Axis " + (o - 500);
                        if (o == -500)
                            text = "Unknown";
                    }
                    Draw.DrawString(text, X, Y, scale, subOptionSelect == i + 14 && onSubOptionItem ? colYellow : colWhite, topleft);
                    Y += textHeight;
                }
                Y -= textHeight;
                X += textWidth + 10;
                tr = 0.4f;
                if (mouseX > X && mouseX < X + Draw.GetWidthString(Language.optionButtonDz, scale * 1.4f) && mouseY < -Y && mouseY > -Y - textHeight * 1.1f) {
                    if (click) {
                        if (playerInfos[controllerBindPlayer - 1].gAxisDeadZone > 0.1)
                            playerInfos[controllerBindPlayer - 1].gAxisDeadZone = 0;
                        else
                            playerInfos[controllerBindPlayer - 1].gAxisDeadZone = 0.2f;
                    }
                    tr = 0.6f;
                }
                Graphics.drawRect(X, -Y, X + Draw.GetWidthString(Language.optionButtonDz, scale * 1.4f), -Y - textHeight * 1.1f, 1, 1, 1, tr);
                Draw.DrawString(Language.optionButtonDz, X, Y, scale, playerInfos[controllerBindPlayer - 1].gAxisDeadZone > 0.1f ? colYellow : colWhite, topleft);
            } else if (menuWindow == 7) {
                menuFadeOut = 0;
                float x = getXCanvas(10, 0);
                float y = getYCanvas(45);
                Vector2 topleft = new Vector2(1, 1);
                Draw.DrawString(Song.songInfo.Name, x, y, scale, colWhite, topleft);
                y += textHeight;
                Draw.DrawString(Song.songInfo.Artist + " // " + Song.songInfo.Charter, x, y, scale * 0.6f, colWhite, topleft);
                float scalewidth = ((float)game.width / (float)game.height);
                if (scalewidth < 1.2f)
                    scale = new Vector2(scalef / 1.5f, scalef);
                for (int p = 0; p < playerAmount; p++) {
                    x = getXCanvas((-48 + 25 * p) * scalewidth);
                    if (playerAmount == 1)
                        x = getXCanvas((-20) * scalewidth);
                    y = getYCanvas(30);
                    Draw.DrawString(playerInfos[p].playerName, x, y, scale, colWhite, topleft);
                    y += textHeight;
                    Draw.DrawString((int)Gameplay.pGameInfo[p].score + "", x, y, scale, colWhite, topleft);
                    y += textHeight;
                    string modStr = "";
                    if (playerInfos[p].autoPlay)
                        modStr += "Auto,";
                    if (playerInfos[p].HardRock)
                        modStr += "HR,";
                    if (playerInfos[p].Hidden == 1)
                        modStr += "HD,";
                    if (playerInfos[p].Easy)
                        modStr += "EZ,";
                    if (playerInfos[p].noFail)
                        modStr += "NF,";
                    if (playerInfos[p].gameplaySpeed != 100)
                        modStr += "S" + (int)Math.Round(playerInfos[p].gameplaySpeed * 100) + ",";
                    if (playerInfos[p].noteModifier != 0)
                        modStr += "MD" + (playerInfos[p].noteModifier + 1) + ",";
                    if (playerInfos[p].inputModifier != 0)
                        modStr += "IM" + (playerInfos[p].inputModifier + 1) + ",";
                    if (modStr.Length > 0)
                        modStr = modStr.TrimEnd(',');
                    Draw.DrawString("Difficulty: " + GetDifficulty(playerInfos[p].difficultySelected, Song.songInfo.ArchiveType), x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                    Draw.DrawString("Mods: " + modStr, x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                    Draw.DrawString("Acc: " + Gameplay.pGameInfo[p].percent + "%  " + (Gameplay.pGameInfo[p].FullCombo ? "FC" : ""), x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                    Draw.DrawString("Notes: " + Gameplay.pGameInfo[p].totalNotes + "/" + (Gameplay.pGameInfo[p].totalNotes + Gameplay.pGameInfo[p].failCount), x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                    Draw.DrawString("Streak: " + Gameplay.pGameInfo[p].maxStreak, x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                    Draw.DrawString("Mode: " + Gameplay.pGameInfo[p].gameMode, x, y, scale * 0.7f, colWhite, topleft);
                    y += textHeight * 0.7f;
                }
                x = getXCanvas(-20);
                y = getYCanvas(-15);
                Draw.DrawString("(Green) Continue", x, y, scale, colWhite, topleft);
                y += textHeight;
                Draw.DrawString("(Yellow) Restart", x, y, scale, colWhite, topleft);
                y += textHeight;
                scale = new Vector2(scalef, scalef);
            }
            float xMax = getX(0, 0);
            float yMax = getY(-50);
            for (int p = 0; p < 4; p++) {
                if (playerOnOptions[p]) {
                    menuFadeOut = 0;
                    playerMenuPos[p] += (0.0f - playerMenuPos[p]) * 0.3f;
                } else {
                    playerMenuPos[p] += (1.0f - playerMenuPos[p]) * 0.3f;
                }
                float startPosX = getXCanvas(5, 0);
                float startPosY = getYCanvas(50) + +textHeight;
                float endPosX = getXCanvas(60, 0);
                float endPosY = getYCanvas(15);
                float posOff = playerMenuPos[p] * getYCanvas(40);
                float transparency = (float)(Math.Min(1.0, (1.0 - playerMenuPos[p]) * 20));
                Color colorTrasparent = Color.FromArgb((int)(transparency * 255), 255, 255, 255);
                float screenRatio = (float)game.height / Textures.background.Height;
                Vector2 textureScale = new Vector2(screenRatio, screenRatio);
                if (p == 0) {
                    startPosY -= -posOff;
                    endPosY -= -posOff;
                    //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(-50) - posOff, getXCanvas(-15, 3), getYCanvas(-10) - posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                    Graphics.Draw(Textures.menuOption, new Vector2(getXCanvas(0, 0), getYCanvas(50) + posOff), Textures.menuOptioni.Xy * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
                } else if (p == 1) {
                    startPosY -= -posOff;
                    startPosX = getXCanvas(-60, 2);
                    endPosX = getXCanvas(0, 2);
                    endPosY -= -posOff;
                    //Graphics.drawRect(getXCanvas(15, 3), getYCanvas(-50) - posOff, endPosX, getYCanvas(-10) - posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                    Graphics.Draw(Textures.menuOption, new Vector2(getXCanvas(0, 2), getYCanvas(50) + posOff), Textures.menuOptioni.Xy * new Vector2(-1, 1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
                } else if (p == 2) {
                    startPosY = getYCanvas(-15);
                    endPosY = getYCanvas(-50);
                    startPosY += -posOff;
                    endPosY += -posOff;
                    //Graphics.drawRect(getXCanvas(0, 0), getYCanvas(50) + posOff, getXCanvas(-15, 3), getYCanvas(10) + posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                    Graphics.Draw(Textures.menuOption, new Vector2(getXCanvas(0, 0), getYCanvas(-50) - posOff), Textures.menuOptioni.Xy * new Vector2(1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
                } else if (p == 3) {
                    startPosX = getXCanvas(-60, 2);
                    endPosX = getXCanvas(0, 2);
                    startPosY = getYCanvas(-15);
                    endPosY = getYCanvas(-50);
                    startPosY += -posOff;
                    endPosY += -posOff;
                    //Graphics.drawRect(getXCanvas(15, 3), getYCanvas(50) + posOff, endPosX, getYCanvas(10) + posOff, 0, 0, 0, 0.75f * menuFadeOutTr);
                    Graphics.Draw(Textures.menuOption, new Vector2(getXCanvas(0, 2), getYCanvas(-50) - posOff), Textures.menuOptioni.Xy * new Vector2(-1, -1) * textureScale, colorTrasparent, Textures.menuOptioni.Zw, 0);
                }
                float tr = playerMenuPos[p] / 1f;
                if (tr > 0.05f && (menuWindow == 0 || menuWindow == 8 || menuWindow == 2 || menuWindow == 3)) {
                    int controllerindex = 0;
                    controllerindex = Input.controllerIndex[p];
                    if (p > 1 && controllerindex == -1)
                        continue;
                    string controller = "";
                    if (controllerindex == -2)
                        controller = Language.menuKeyboard;
                    else if (controllerindex == -1)
                        controller = Language.menuPressBtn;
                    else if (controllerindex > 0)
                        controller = Language.menuController;
                    Color col = Color.FromArgb((int)((tr * menuFadeOutTr) * 255), 255, 255, 255);
                    Color black = Color.FromArgb((int)((tr * menuFadeOutTr) * 200), 0, 0, 0);
                    Color transparent = Color.FromArgb(0, 0, 0, 0);
                    if (p == 0) {
                        Graphics.drawPoly(getXCanvas(0, 0), getYCanvas(-50), getXCanvas(0, 0), getYCanvas(-20), getXCanvas(50, 0), getYCanvas(-20), getXCanvas(50, 0), getYCanvas(-50), black, transparent, transparent, transparent);
                        Draw.DrawString(controller, getXCanvas(5, 0), getYCanvas(45), scale, col, Vector2.Zero);
                        if (playerProfileReady[p]) {
                            Draw.DrawString(playerInfos[p].playerName, getXCanvas(5, 0), getYCanvas(45) + textHeight, scale, col, Vector2.Zero);
                        }
                    } else if (p == 1) {
                        Graphics.drawPoly(getXCanvas(0, 2), getYCanvas(-50), getXCanvas(0, 2), getYCanvas(-20), getXCanvas(-50, 2), getYCanvas(-20), getXCanvas(-50, 2), getYCanvas(-50), black, transparent, transparent, transparent);
                        float stringWidth = Draw.GetWidthString(controller, scale);
                        Draw.DrawString(controller, getXCanvas(-5, 2) - stringWidth, getYCanvas(45), scale, col, Vector2.Zero);
                        if (playerProfileReady[p]) {
                            stringWidth = Draw.GetWidthString(playerInfos[p].playerName, scale);
                            Draw.DrawString(playerInfos[p].playerName, getXCanvas(-5, 2) - stringWidth, getYCanvas(45) + textHeight, scale, col, Vector2.Zero);
                        }
                    } else if (p == 2) {
                        Graphics.drawPoly(getXCanvas(0, 0), getYCanvas(50), getXCanvas(0, 0), getYCanvas(20), getXCanvas(50, 0), getYCanvas(20), getXCanvas(50, 0), getYCanvas(50), black, transparent, transparent, transparent);
                        Draw.DrawString(controller, getXCanvas(5, 0), getYCanvas(-45), scale, col, Vector2.Zero);
                        if (playerProfileReady[p]) {
                            Draw.DrawString(playerInfos[p].playerName, getXCanvas(5, 0), getYCanvas(-45) - textHeight, scale, col, Vector2.Zero);
                        }
                    } else if (p == 3) {
                        Graphics.drawPoly(getXCanvas(0, 2), getYCanvas(50), getXCanvas(0, 2), getYCanvas(20), getXCanvas(-50, 2), getYCanvas(20), getXCanvas(-50, 2), getYCanvas(50), black, transparent, transparent, transparent);
                        float stringWidth = Draw.GetWidthString(controller, scale);
                        Draw.DrawString(controller, getXCanvas(-5, 2) - stringWidth, getYCanvas(-45), scale, col, Vector2.Zero);
                        if (playerProfileReady[p]) {
                            stringWidth = Draw.GetWidthString(playerInfos[p].playerName, scale);
                            Draw.DrawString(playerInfos[p].playerName, getXCanvas(-5, 2) - stringWidth, getYCanvas(-45) - textHeight, scale, col, Vector2.Zero);
                        }
                    }
                }
                if (playerMenuPos[p] < 0.95f) {
                    Vector2 menuScale = scale * 0.8f;
                    float menuTextHeight = textHeight * 0.8f;
                    position.X = startPosX + 30;
                    position.Y = endPosY - menuTextHeight * 1.25f;
                    string playerStr = String.Format(Language.menuModPlayer, p + 1);
                    string playerName = playerInfos[p].playerName;
                    playerName = playerName.Equals("__Guest__") ? playerStr : playerName;
                    float nameLength = Draw.GetWidthString(playerName, menuScale * 2.5f);
                    float namePos = endPosX - nameLength - 30;
                    if (namePos < startPosX + 30)
                        namePos = startPosX + 30;
                    Draw.DrawString(playerName, namePos, position.Y, menuScale * 2.5f, Color.FromArgb(50, 255, 255, 255), Vector2.Zero, 0, endPosX);
                    position.X = startPosX;
                    if (creatingNewProfile) {
                        position.Y = startPosY;
                        Draw.DrawString(Language.menuProfileCreateIn, position.X, position.Y, menuScale, Color.LightGray, Vector2.Zero, 0, endPosX);
                        position.Y += menuTextHeight * 1.2f;
                        Draw.DrawString(newProfileName, position.X, position.Y, menuScale, colWhite, Vector2.Zero, 0, endPosX);
                        position.Y += menuTextHeight * 1.2f;
                        Draw.DrawString(Language.menuProfileAccept, position.X, position.Y, menuScale, Color.Gray, Vector2.Zero, 0, endPosX);
                        position.Y += menuTextHeight;
                        Draw.DrawString(Language.menuProfileCancel, position.X, position.Y, menuScale, Color.Gray, Vector2.Zero, 0, endPosX);
                    } else if (!playerProfileReady[p]) {
                        position.Y = startPosY;
                        Draw.DrawString(Language.menuProfileCreate, position.X, position.Y, menuScale, playerProfileSelect[p] == 0 ? Color.LightGreen : Color.DarkGreen, Vector2.Zero, 0, endPosX);
                        for (int i = 1; i <= profilesName.Length; i++) {
                            position.Y = startPosY + menuTextHeight * i;
                            Draw.DrawString(profilesName[i - 1], position.X, position.Y, menuScale, playerProfileSelect[p] == i ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        }
                        int ci = Input.controllerIndex[p];
                        if (ci > 0) {
                            position.Y += menuTextHeight * 1.2f;
                            Draw.DrawString("Btn 0: Green, Btn 1: Red", position.X, position.Y, menuScale * 0.7f, Color.Gray, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight * 0.7f;
                            Draw.DrawString("Btn 2: Down, Btn 3: Up", position.X, position.Y, menuScale * 0.7f, Color.Gray, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight * 0.7f;
                            Draw.DrawString("Btn Pressed: " + Input.lastGamePadButton, position.X, position.Y, menuScale * 0.7f, Color.Gray, Vector2.Zero, 0, endPosX);
                        } else {
                            position.Y += menuTextHeight * 1.2f;
                            Draw.DrawString("Number1: Accept", position.X, position.Y, menuScale * 0.7f, Color.Gray, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight * 0.7f;
                            Draw.DrawString("Number3: Delete", position.X, position.Y, menuScale * 0.7f, Color.DarkRed, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight * 0.7f;
                            Draw.DrawString("Number4: Reload", position.X, position.Y, menuScale * 0.7f, Color.Gray, Vector2.Zero, 0, endPosX);
                        }
                    } else {
                        position.Y = startPosY;
                        Draw.DrawString(Language.menuModMods, position.X, position.Y, menuScale, !playerOn2Menu[p] ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        position.X = (startPosX + endPosX) / 2;
                        Draw.DrawString(Language.menuModOptions, position.X, position.Y, menuScale, playerOn2Menu[p] ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                        position.X = startPosX;
                        position.Y = startPosY + menuTextHeight * 1.5f;
                        int offset = playerProfileSelect[p] - 3;
                        if (offset < 0)
                            offset = 0;
                        if (offset > 5)
                            offset = 5;
                        if (!playerOn2Menu[p]) {
                            position.X = endPosX + (startPosX - endPosX) / 5;
                            Draw.DrawString("x" + playerInfos[p].modMult.ToString("0.0"), position.X, position.Y, menuScale * 1.2f, playerInfos[p].modMult == 1f ? colWhite : playerInfos[p].modMult > 1f ? Color.PaleGreen : Color.Orange, Vector2.Zero, 0, endPosX);
                            position.X = startPosX;
                            position.Y -= menuTextHeight * offset;
                            if (offset <= 0) Draw.DrawString((playerProfileSelect[p] == 0 ? ">" : " ") + Language.menuModHard, position.X, position.Y, menuScale, playerInfos[p].HardRock ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset <= 1) Draw.DrawString((playerProfileSelect[p] == 1 ? ">" : " ") + Language.menuModHidden, position.X, position.Y, menuScale, playerInfos[p].Hidden == 1 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset <= 2) Draw.DrawString((playerProfileSelect[p] == 2 ? ">" : " ") + Language.menuModAuto, position.X, position.Y, menuScale, playerInfos[p].autoPlay ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset <= 3) Draw.DrawString((playerProfileSelect[p] == 3 ? ">" : " ") + Language.menuModEasy, position.X, position.Y, menuScale, playerInfos[p].Easy ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset <= 4) Draw.DrawString((playerProfileSelect[p] == 4 ? ">" : " ") + Language.menuModSpeed + ": " + Math.Round(playerInfos[p].gameplaySpeed * 100) + "%", position.X, position.Y, menuScale, Math.Round(playerInfos[p].gameplaySpeed * 100) != 100 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            Draw.DrawString((playerProfileSelect[p] == 5 ? ">" : " ") + String.Format(Language.menuModNotes, playerInfos[p].noteModifier == 0 ? Language.menuModNormal : playerInfos[p].noteModifier == 1 ? Language.menuModFlip : playerInfos[p].noteModifier == 2 ? Language.menuModShuffle : playerInfos[p].noteModifier == 3 ? Language.menuModRandom : "???"), position.X, position.Y, menuScale, playerInfos[p].noteModifier != 0 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            Draw.DrawString((playerProfileSelect[p] == 6 ? ">" : " ") + Language.menuModNofail, position.X, position.Y, menuScale, playerInfos[p].noFail ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset >= 1) Draw.DrawString((playerProfileSelect[p] == 7 ? ">" : " ") + Language.menuModPerformance, position.X, position.Y, menuScale, MainGame.performanceMode ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset >= 2) Draw.DrawString((playerProfileSelect[p] == 8 ? ">" : " ") + Language.menuModTransform, position.X, position.Y, menuScale, playerInfos[p].transform ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset >= 3) Draw.DrawString((playerProfileSelect[p] == 9 ? ">" : " ") + Language.menuModAutoSP, position.X, position.Y, menuScale, playerInfos[p].autoSP ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset >= 4) Draw.DrawString((playerProfileSelect[p] == 10 ? ">" : " ") + String.Format(Language.menuModInput, playerInfos[p].inputModifier == 0 ? Language.menuModInputNormal : playerInfos[p].inputModifier == 1 ? Language.menuModInputAllStrum : playerInfos[p].inputModifier == 2 ? Language.menuModInputAllTap : playerInfos[p].inputModifier == 3 ? Language.menuModInputStrum : playerInfos[p].inputModifier == 4 ? Language.menuModInputFretLess : "???"), position.X, position.Y, menuScale, playerInfos[p].inputModifier != 0 ? colYellow : colWhite, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                            if (offset >= 5) Draw.DrawString((playerProfileSelect[p] == 11 ? ">" : " ") + Language.menuModQuit, position.X, position.Y, menuScale, Color.Orange, Vector2.Zero, 0, endPosX);
                            position.Y += menuTextHeight;
                        } else {
                            position.Y += menuTextHeight;
                            Draw.DrawString((playerProfileSelect2[p] == 0 ? ">" : " ") + string.Format(Language.menuOptionMode, Gameplay.pGameInfo[p].gameMode), position.X, position.Y, menuScale, colWhite, Vector2.Zero, 0, endPosX);
                        }
                    }
                }
            }
            if (click)
                mouseClicked = false;
            if (menuWindow != 6)
                Graphics.drawRect(getXCanvas(0, 0), getYCanvas(35), getXCanvas(0, 2), getYCanvas(50), 0, 0, 0, 0.7f * menuFadeOutTr);
            string Btnstr = "";
            if (menuWindow == 0 || menuWindow == 8)
                Btnstr = string.Format(Language.menuBtnsMain, (char)(0), (char)(3));
            else if (menuWindow == 1)
                Btnstr = string.Format(Language.menuBtnsSong, (char)(0), (char)(1), (char)(2), (char)(3), (char)(6));
            else if (menuWindow == 2 || menuWindow == 3)
                Btnstr = string.Format(Language.menuBtnsOptions, (char)(0), (char)(1));
            else if (menuWindow == 4)
                Btnstr = string.Format(Language.menuBtnsDiff, (char)(0), (char)(1), (char)(3));
            else if (menuWindow == 5)
                Btnstr = string.Format(Language.menuBtnsRecords, (char)(0), (char)(1), (char)(3));
            Vector2 btnScale = scale;
            float Btnwidth = Draw.GetWidthString(Btnstr, Vector2.One * btnScale * 1.1f);
            float screenWidth = Math.Abs(getXCanvas(0, 0) - getXCanvas(0, 2));
            if (Btnwidth > screenWidth) {
                btnScale *= screenWidth / Btnwidth;
                Btnwidth = Draw.GetWidthString(Btnstr, Vector2.One * btnScale * 1.1f);
            }
            Draw.DrawString(Btnstr, -Btnwidth / 2, getYCanvas(-40f), Vector2.One * btnScale * 1.1f, colWhite, new Vector2(0, 0.75f));
            drawVolume();
        }
        static float getAspect() {
            float ret = (float)game.height / game.width;
            ret *= 1.166f;
            return ret;
        }
        public static float getXCanvas(float x, int side = 1) {
            float pos = getX(x, side);
            return pos - ((float)game.width / 2);
        }
        public static float getYCanvas(float y, int side = 1) {
            float pos = getY(-y, side);
            return pos - ((float)game.height / 2);
        }
        public static float getX(float x, int side = 1) {
            float cent = (float)game.height / 100;
            if (side == 3)
                cent = (float)game.width / 100;
            float halfx = (float)game.width / 2;
            if (side == 0)
                return cent * x;
            else if (side == 2)
                return (float)game.width + cent * x;
            return halfx + cent * x;
        }
        static float getXCenter(float x) {
            x /= 100;
            x *= game.height;
            x += game.width / 2;
            return x;
        }
        public static float getY(float y, int side = 1, bool graphic = false) {
            if (graphic) y += 50;
            float half = (float)game.height / 2;
            float cent = (float)game.height / 100;
            return half + cent * y;
        }
        public static bool IsDifficulty(string diffString, SongInstruments i, int mode = 1) {
            if (mode == 1) {
                if ((diffString.Equals("ExpertSingle") ||
                    diffString.Equals("HardSingle") ||
                    diffString.Equals("MediumSingle") ||
                    diffString.Equals("EasySingle")) && i == SongInstruments.guitar)
                    return true;
                else if (diffString.Contains("GHLBass") && i == SongInstruments.ghl_bass)
                    return true;
                else if (diffString.Contains("GHLGuitar") && i == SongInstruments.ghl_guitar)
                    return true;
                else if (diffString.Contains("Bass") && i == SongInstruments.bass)
                    return true;
                else if (diffString.Contains("Guitar") && i == SongInstruments.guitar)
                    return true;
                else if (diffString.Contains("Rhythm") && i == SongInstruments.rhythm)
                    return true;
                else if (diffString.Contains("Drums") && i == SongInstruments.drums)
                    return true;
                else if (diffString.Contains("Keyboard") && i == SongInstruments.keys)
                    return true;
                else if (diffString.Contains("SCGMD") && i == SongInstruments.scgmd)
                    return true;
            } else if (mode == 2) {
                string[] parts = diffString.Split('$');
                if (parts.Length == 1)
                    return false;
                string instrument = parts[1].TrimStart(new char[] { 'P', 'A', 'R', 'T', ' ' });
                if (instrument.Equals("GUITAR") && i == SongInstruments.guitar)
                    return true;
                else if (instrument.Equals("BASS") && i == SongInstruments.bass)
                    return true;
                else if (instrument.Equals("DRUMS") && i == SongInstruments.drums)
                    return true;
                else if (instrument.Equals("VOCALS") && i == SongInstruments.vocals)
                    return true;
                else if ((instrument.Equals("RHYTHM") || instrument.Equals("HYTHM")) && i == SongInstruments.rhythm)
                    return true;
                else if (instrument.Equals("KEYS") && i == SongInstruments.keys)
                    return true;
                else if (instrument.Equals("GUITAR GHL") && i == SongInstruments.ghl_guitar)
                    return true;
                else if (instrument.Equals("BASS GHL") && i == SongInstruments.ghl_bass)
                    return true;
            }
            return false;
        }
        public static string GetDifficulty(string diffString, int mode = 1) {
            if (mode == 1) {
                if (diffString.Equals("ExpertSingle"))
                    diffString = Language.songDiffExpert;
                else if (diffString.Equals("HardSingle"))
                    diffString = Language.songDiffHard;
                else if (diffString.Equals("MediumSingle"))
                    diffString = Language.songDiffMedium;
                else if (diffString.Equals("EasySingle"))
                    diffString = Language.songDiffEasy;
                else if (diffString.Equals("ExpertSingleBass"))
                    diffString = Language.instrumentBass + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardSingleBass"))
                    diffString = Language.instrumentBass + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumSingleBass"))
                    diffString = Language.instrumentBass + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasySingleBass"))
                    diffString = Language.instrumentBass + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertDoubleGuitar"))
                    diffString = Language.instrument2Guitar + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardDoubleGuitar"))
                    diffString = Language.instrument2Guitar + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumDoubleGuitar"))
                    diffString = Language.instrument2Guitar + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyDoubleGuitar"))
                    diffString = Language.instrument2Guitar + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertDoubleBass"))
                    diffString = Language.instrument2Bass + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardDoubleBass"))
                    diffString = Language.instrument2Bass + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumDoubleBass"))
                    diffString = Language.instrument2Bass + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyDoubleBass"))
                    diffString = Language.instrument2Bass + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertSingleRhythm"))
                    diffString = Language.instrumentRhythm + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardSingleRhythm"))
                    diffString = Language.instrumentRhythm + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumSingleRhythm"))
                    diffString = Language.instrumentRhythm + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasySingleRhythm"))
                    diffString = Language.instrumentRhythm + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertDoubleRhythm"))
                    diffString = Language.instrument2Rhythm + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardDoubleRhythm"))
                    diffString = Language.instrument2Rhythm + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumDoubleRhythm"))
                    diffString = Language.instrument2Rhythm + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyDoubleRhythm"))
                    diffString = Language.instrument2Rhythm + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertKeyboard"))
                    diffString = Language.instrumentKeys + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardKeyboard"))
                    diffString = Language.instrumentKeys + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumKeyboard"))
                    diffString = Language.instrumentKeys + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyKeyboard"))
                    diffString = Language.instrumentKeys + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertDrums"))
                    diffString = Language.instrumentDrums + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardDrums"))
                    diffString = Language.instrumentDrums + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumDrums"))
                    diffString = Language.instrumentDrums + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyDrums"))
                    diffString = Language.instrumentDrums + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertGHLBass"))
                    diffString = Language.instrumentBassGHL + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardGHLBass"))
                    diffString = Language.instrumentBassGHL + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumGHLBass"))
                    diffString = Language.instrumentBassGHL + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyGHLBass"))
                    diffString = Language.instrumentBassGHL + " - " + Language.songDiffEasy;
                else if (diffString.Equals("ExpertGHLGuitar"))
                    diffString = Language.instrumentGuitarGHL + " - " + Language.songDiffExpert;
                else if (diffString.Equals("HardGHLGuitar"))
                    diffString = Language.instrumentGuitarGHL + " - " + Language.songDiffHard;
                else if (diffString.Equals("MediumGHLGuitar"))
                    diffString = Language.instrumentGuitarGHL + " - " + Language.songDiffMedium;
                else if (diffString.Equals("EasyGHLGuitar"))
                    diffString = Language.instrumentGuitarGHL + " - " + Language.songDiffEasy;
            } else if (mode == 2) {
                string[] parts = diffString.Split('$');
                string instrument = parts[1].TrimStart(new char[] { 'P', 'A', 'R', 'T', ' ' });
                string difficulty = parts[0];
                if (instrument.Equals("GUITAR")) {
                    //instrument = Language.instrumentGuitar;
                    return difficulty;
                } else if (instrument.Equals("BASS"))
                    instrument = Language.instrumentBass;
                else if (instrument.Equals("DRUMS"))
                    instrument = Language.instrumentDrums;
                else if (instrument.Equals("VOCALS"))
                    instrument = Language.instrumentVocals;
                else if (instrument.Equals("RHYTHM") || instrument.Equals("HYTHM"))
                    instrument = Language.instrumentRhythm;
                else if (instrument.Equals("KEYS"))
                    instrument = Language.instrumentKeys;
                else if (instrument.Equals("GUITAR GHL"))
                    instrument = Language.instrumentGuitarGHL;
                else if (instrument.Equals("BASS GHL"))
                    instrument = Language.instrumentBassGHL;
                if (difficulty.Equals("Expert"))
                    difficulty = Language.songDiffExpert;
                else if (difficulty.Equals("Hard"))
                    difficulty = Language.songDiffHard;
                else if (difficulty.Equals("Medium"))
                    difficulty = Language.songDiffMedium;
                else if (difficulty.Equals("Easy"))
                    difficulty = Language.songDiffEasy;
                diffString = instrument + " - " + difficulty;
            }
            return diffString;
        }
    }
    enum SongInstruments {
        guitar, bass, drums, vocals, rhythm, keys, mania, ghl_guitar, ghl_bass, scgmd
    }
}
