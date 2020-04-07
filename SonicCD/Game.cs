// Decompiled with JetBrains decompiler
// Type: Sonic_CD.Game
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

//using Microsoft.Devices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using RetroEngine;

namespace SonicCD
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private object achievementsLockObject = new object();
        public string[] achievementName = new string[12];
        public string[] achievementDesc = new string[12];
        public int[] achievementEarned = new int[12];
        public int[] achievementGamerScore = new int[12];
        public int[] achievementID = new int[12];
        private const int LeaderboardPageSize = 100;
        private GraphicsDeviceManager graphics;
        private AchievementCollection achievements;
        public int earnedGamerScore;
        public int maxGamerScore;
        private LeaderboardReader leaderboardReader;
        protected Game.SigninStatus signinStatus;
        public bool displayTitleUpdateMessage;
        private bool resize;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager((Microsoft.Xna.Framework.Game)this);
            this.graphics.HardwareModeSwitch = false;
            //this.graphics.IsFullScreen = true;
            
            this.resize = true;
            //this.graphics.PreferredBackBufferWidth = 1280;
            //this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            // this.graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr565;
            this.graphics.PreferredDepthStencilFormat = DepthFormat.None;
            this.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.graphics_PreparingDeviceSettings);
            this.Content.RootDirectory = "Content";
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
            this.InactiveSleepTime = TimeSpan.FromSeconds(1.0);
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += OnClientSizeChange;
        }

        private void OnClientSizeChange(object sender, EventArgs e)
        {
            resize = true;
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
        }

        protected override void Initialize()
        {
            SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(this.GamerSignedInCallback);
            base.Initialize();
        }

        protected void GamerSignedInCallback(object sender, SignedInEventArgs args)
        {
            //SignedInGamer gamer = args.Gamer;
            //if (gamer != null)
            //{
            //    GlobalAppDefinitions.gameOnlineActive = (byte)2;
            //    if (GlobalAppDefinitions.gameOnlineActive != (byte)2)
            //        return;
            //    GlobalAppDefinitions.gameOnlineActive = (byte)3;
            //    gamer.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), (object)gamer);
            //}
            //else
            //    GlobalAppDefinitions.gameOnlineActive = (byte)0;
        }

        protected void GetAchievementsCallback(IAsyncResult result)
        {
            if (!(result.AsyncState is SignedInGamer asyncState))
            {
                GlobalAppDefinitions.gameOnlineActive = (byte)0;
            }
            else
            {
                GlobalAppDefinitions.gameOnlineActive = (byte)1;
                lock (this.achievementsLockObject)
                {
                    this.maxGamerScore = 0;
                    this.earnedGamerScore = 0;
                    this.achievements = asyncState.EndGetAchievements(result);
                    for (int index = 0; index < this.achievements.Count; ++index)
                    {
                        Achievement achievement = this.achievements[index];
                        this.achievementName[index] = achievement.Name;
                        this.achievementDesc[index] = achievement.Description;
                        this.achievementGamerScore[index] = achievement.GamerScore;
                        switch (achievement.Key)
                        {
                            case "88 Miles Per Hour":
                                this.achievementID[index] = 0;
                                break;
                            case "Just One Hug is Enough":
                                this.achievementID[index] = 1;
                                break;
                            case "Paradise Found":
                                this.achievementID[index] = 2;
                                break;
                            case "Take the High Road":
                                this.achievementID[index] = 3;
                                break;
                            case "King of the Rings":
                                this.achievementID[index] = 4;
                                break;
                            case "Statue Saviour":
                                this.achievementID[index] = 5;
                                break;
                            case "Heavy Metal":
                                this.achievementID[index] = 6;
                                break;
                            case "All Stages Clear":
                                this.achievementID[index] = 7;
                                break;
                            case "Treasure Hunter":
                                this.achievementID[index] = 8;
                                break;
                            case "Dr Eggman Got Served":
                                this.achievementID[index] = 9;
                                break;
                            case "Just In Time":
                                this.achievementID[index] = 10;
                                break;
                            case "Saviour of the Planet":
                                this.achievementID[index] = 11;
                                break;
                        }
                        this.maxGamerScore += achievement.GamerScore;
                        if (achievement.IsEarned)
                        {
                            this.earnedGamerScore += achievement.GamerScore;
                            this.achievementEarned[index] = 1;
                        }
                        else
                            this.achievementEarned[index] = 0;
                    }
                }
            }
        }

        public void AwardAchievement(string achievementKey)
        {
            SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
            if (signedInGamer == null)
                return;
            lock (this.achievementsLockObject)
            {
                if (this.achievements == null)
                    return;
                foreach (Achievement achievement in this.achievements)
                {
                    if (achievement.Key == achievementKey)
                    {
                        if (achievement.IsEarned)
                            break;
                        signedInGamer.BeginAwardAchievement(achievementKey, new AsyncCallback(this.AwardAchievementCallback), (object)signedInGamer);
                        break;
                    }
                }
            }
        }

        protected void AwardAchievementCallback(IAsyncResult result)
        {
            if (!(result.AsyncState is SignedInGamer asyncState))
                return;
            asyncState.EndAwardAchievement(result);
            asyncState.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), (object)asyncState);
        }

        public void LoadLeaderboardEntries()
        {
            SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
            int globalVariable = ObjectSystem.globalVariables[114];
            try
            {
                //LeaderboardIdentity leaderboardId = globalVariable != 0 ? LeaderboardIdentity.Create(LeaderboardKey.BestTimeLifeTime, globalVariable + 1) : LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, globalVariable);
                GlobalAppDefinitions.gameMode = (byte)7;
                //LeaderboardReader.BeginRead(leaderboardId, (Gamer)signedInGamer, 100, new AsyncCallback(this.LeaderboardReadCallback), (object)signedInGamer);
                this.ReadNullLeaderboardEntries();
            }
            catch (GameUpdateRequiredException ex)
            {
                this.ReadNullLeaderboardEntries();
                this.HandleGameUpdateRequired(ex);
            }
            catch (Exception ex)
            {
                this.ReadNullLeaderboardEntries();
                //Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], new string[1] { "OK" }, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), (object)null);
            }
        }

        protected void LeaderboardReadCallback(IAsyncResult result)
        {
            //object asyncState = result.AsyncState;
            try
            {
                //this.leaderboardReader = LeaderboardReader.EndRead(result);
                //this.ReadLeaderboardEntries();
                ReadNullLeaderboardEntries();
            }
            catch (GameUpdateRequiredException ex)
            {
                this.ReadNullLeaderboardEntries();
                this.HandleGameUpdateRequired(ex);
            }
            catch (Exception ex)
            {
                this.ReadNullLeaderboardEntries();
                //Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], new string[1] { "OK" }, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), (object)null);
            }
            GlobalAppDefinitions.gameMode = (byte)1;
        }

        protected void ReadLeaderboardEntries()
        {
            int globalVariable = ObjectSystem.globalVariables[114];
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            LeaderboardReader leaderboardReader = this.leaderboardReader;
            if (globalVariable == 0)
            {
                for (int index = 0; index < this.leaderboardReader.Entries.Count(); ++index)
                {
                    LeaderboardEntry entry = leaderboardReader.Entries.ElementAt(index);
                    string str = string.Format("{0,4}{1,-15}{2,1}{3,8}", (object)((index + 1).ToString() + "."), (object)entry.Gamer.Gamertag, (object)" ", (object)entry.Columns.GetValueInt32("BestScore").ToString());
                    TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
                }
            }
            else
            {
                for (int index = 0; index < this.leaderboardReader.Entries.Count(); ++index)
                {
                    LeaderboardEntry entry = leaderboardReader.Entries.ElementAt(index);
                    int valueInt32 = entry.Columns.GetValueInt32("BestTime");
                    int num1 = valueInt32 / 6000;
                    int num2 = valueInt32 / 100 % 60;
                    valueInt32 %= 100;
                    string str = string.Format("{0,4}{1,-15}{2,2}{3,1}{4,1}{5,2}{6,1}{7,2}", (object)((index + 1).ToString() + "."), (object)entry.Gamer.Gamertag, (object)"  ", (object)num1.ToString(), (object)"'", (object)num2.ToString(), (object)"\"", (object)valueInt32.ToString());
                    TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
                }
            }
            for (int count = this.leaderboardReader.Entries.Count(); count < 100; ++count)
            {
                string str = string.Format("{0,4}{1,-15}", (object)((count + 1).ToString() + "."), (object)"---------------");
                TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
            }
        }

        protected void ReadNullLeaderboardEntries()
        {
            int globalVariable = ObjectSystem.globalVariables[114];
            TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
            for (int index = 0; index < 100; ++index)
            {
                string str = string.Format("{0,4}{1,-15}", (object)((index + 1).ToString() + "."), (object)"---------------");
                TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], str.ToCharArray());
            }
        }

        public void SetLeaderboard(int leaderboardID, int result)
        {
            //LeaderboardOutcome leaderboardOutcome = LeaderboardOutcome.Win;
            SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
            try
            {
                if (leaderboardID == 0)
                {
                    LeaderboardIdentity leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, leaderboardID);
                    //LeaderboardEntry leaderboard = signedInGamer.LeaderboardWriter.GetLeaderboard(leaderboardId);
                    //leaderboard.Rating = (long)result;
                    //leaderboard.Columns.SetValue("Outcome", leaderboardOutcome);
                    //leaderboard.Columns.SetValue("TimeStamp", DateTime.Now);
                }
                else
                {
                    LeaderboardIdentity leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestTimeLifeTime, leaderboardID);
                    //LeaderboardEntry leaderboard = signedInGamer.LeaderboardWriter.GetLeaderboard(leaderboardId);
                    //leaderboard.Rating = (long)result;
                    //leaderboard.Columns.SetValue("Outcome", leaderboardOutcome);
                    //leaderboard.Columns.SetValue("TimeStamp", DateTime.Now);
                }
            }
            catch (GameUpdateRequiredException ex)
            {
                this.HandleGameUpdateRequired(ex);
            }
            catch (Exception ex)
            {
                this.ReadNullLeaderboardEntries();
                //Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], (IEnumerable<string>)new string[1] { "OK" }, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), (object)null);
            }
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            if (StageSystem.stageMode == (byte)2)
            {
                if (GlobalAppDefinitions.gameMode == (byte)7)
                {
                    GlobalAppDefinitions.gameMode = (byte)1;
                    GlobalAppDefinitions.gameMessage = 4;
                }
            }
            else
            {
                if (GlobalAppDefinitions.gameMode == (byte)7)
                    GlobalAppDefinitions.gameMode = (byte)1;
                GlobalAppDefinitions.gameMessage = 2;
                AudioPlayback.ResumeSound();
            }
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            GlobalAppDefinitions.gameMessage = 2;
            if (StageSystem.stageMode != (byte)2 && GlobalAppDefinitions.gameMode != (byte)7)
                AudioPlayback.PauseSound();
            GlobalAppDefinitions.gameMode = (byte)7;
            base.OnDeactivated(sender, args);
        }

        protected override void LoadContent()
        {
            GlobalAppDefinitions.CalculateTrigAngles();
            RenderDevice.InitRenderDevice(this.GraphicsDevice);
            RenderDevice.SetScreenDimensions(1280, 720);
            EngineCallbacks.StartupRetroEngine();
            EngineCallbacks.gameRef = this;
            AudioPlayback.gameRef = this;
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
                case "fr":
                    GlobalAppDefinitions.gameLanguage = (byte)1;
                    break;
                case "it":
                    GlobalAppDefinitions.gameLanguage = (byte)2;
                    break;
                case "de":
                    GlobalAppDefinitions.gameLanguage = (byte)3;
                    break;
                case "es":
                    GlobalAppDefinitions.gameLanguage = (byte)4;
                    break;
                case "ja":
                    GlobalAppDefinitions.gameLanguage = (byte)5;
                    break;
                default:
                    GlobalAppDefinitions.gameLanguage = (byte)0;
                    break;
            }
            //if (!Guide.IsTrialMode)
            //    return;
            //GlobalAppDefinitions.gameTrialMode = (byte)1;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (resize)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();

                RenderDevice.SetScreenDimensions(Window.ClientBounds.Width, Window.ClientBounds.Height);

                resize = false;
            }

            int pointerID = 0;
            InputSystem.CheckKeyboardInput();
            TouchCollection state = TouchPanel.GetState();
            InputSystem.ClearTouchData();
            foreach (TouchLocation touchLocation in state)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Pressed:
                        InputSystem.AddTouch(touchLocation.Position.X, touchLocation.Position.Y, pointerID);
                        break;
                    case TouchLocationState.Moved:
                        InputSystem.AddTouch(touchLocation.Position.X, touchLocation.Position.Y, pointerID);
                        break;
                }
                ++pointerID;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (FileIO.activeStageList == (byte)0)
                {
                    switch (StageSystem.stageListPosition)
                    {
                        case 4:
                        case 5:
                            InputSystem.touchData.start = (byte)1;
                            break;
                        default:
                            InputSystem.touchData.buttonB = (byte)1;
                            break;
                    }
                }
                else if (StageSystem.stageMode == (byte)2)
                {
                    if (ObjectSystem.objectEntityList[9].state == (byte)3 && GlobalAppDefinitions.gameMode == (byte)1)
                    {
                        ObjectSystem.objectEntityList[9].state = (byte)4;
                        ObjectSystem.objectEntityList[9].value[0] = 0;
                        ObjectSystem.objectEntityList[9].value[1] = 0;
                        ObjectSystem.objectEntityList[9].alpha = (byte)248;
                        AudioPlayback.PlaySfx(27, (byte)0);
                    }
                }
                else
                    GlobalAppDefinitions.gameMessage = 2;
            }
            if (StageSystem.stageMode != (byte)2)
                EngineCallbacks.ProcessMainLoop();
            try
            {
                base.Update(gameTime);
            }
            catch (GameUpdateRequiredException ex)
            {
                this.HandleGameUpdateRequired(ex);
            }
        }

        private void HandleGameUpdateRequired(GameUpdateRequiredException e)
        {
            //this.gamerServiceInstance.Enabled = false;
            this.displayTitleUpdateMessage = true;
            this.signinStatus = Game.SigninStatus.UpdateNeeded;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (this.displayTitleUpdateMessage)
                EngineCallbacks.ShowLiveUpdateMessage();
            if (StageSystem.stageMode == (byte)2)
                EngineCallbacks.ProcessMainLoop();
            if (RenderDevice.highResMode == 0)
                RenderDevice.FlipScreen();
            else
                RenderDevice.FlipScreenHRes();
            base.Draw(gameTime);
        }

        protected enum SigninStatus
        {
            None,
            SigningIn,
            Local,
            LIVE,
            Error,
            UpdateNeeded,
        }
    }
}
