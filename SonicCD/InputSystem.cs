// Decompiled with JetBrains decompiler
// Type: Retro_Engine.InputSystem
// Assembly: Sonic CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D35AF46A-1892-4F52-B201-E664C9200079
// Assembly location: E:\wamwo\Downloads\Sonic CD Mod Via Rebel\Sonic CD Mod Via Rebel\Sonic CD.dll

using Microsoft.Xna.Framework.Input;

namespace RetroEngine
{
    public static class InputSystem
    {
        public static bool touchControls = false;
        public static InputResult inputPress = new InputResult();
        public static InputResult touchData = new InputResult();
        public const int BUTTON_UP = 1;
        public const int BUTTON_DOWN = 2;
        public const int BUTTON_LEFT = 4;
        public const int BUTTON_RIGHT = 8;
        public const int BUTTON_A = 16;
        public const int BUTTON_B = 32;
        public const int BUTTON_C = 64;
        public const int BUTTON_START = 128;
        public const int ALL_BUTTONS = 255;
        public const int NO_BUTTONS = 0;
        public static int touchWidth;
        public static int touchHeight;

        public static void AddTouch(float touchX, float touchY, int pointerID)
        {
            for (int index = 0; index < 4; ++index)
            {
                if (InputSystem.touchData.touchDown[index] == (byte)0)
                {
                    InputSystem.touchData.touchDown[index] = (byte)1;
                    InputSystem.touchData.touchY[index] = (int)touchY;
                    InputSystem.touchData.touchX[index] = (int)touchX;
                    InputSystem.touchData.touchID[index] = pointerID;
                    index = 4;
                }
            }
        }

        public static void SetTouch(float touchX, float touchY, int pointerID)
        {
            for (int index = 0; index < 4; ++index)
            {
                if (InputSystem.touchData.touchID[index] == pointerID && InputSystem.touchData.touchDown[index] == (byte)1)
                {
                    InputSystem.touchData.touchY[index] = (int)touchY;
                    InputSystem.touchData.touchX[index] = (int)touchX;
                    index = 4;
                }
            }
        }

        public static void RemoveTouch(int pointerID)
        {
            for (int index = 0; index < 4; ++index)
            {
                if (InputSystem.touchData.touchID[index] == pointerID)
                    InputSystem.touchData.touchDown[index] = (byte)0;
            }
        }

        public static void ClearTouchData()
        {
            InputSystem.touchData.touches = 0;
            InputSystem.touchData.touchDown[0] = (byte)0;
            InputSystem.touchData.touchDown[1] = (byte)0;
            InputSystem.touchData.touchDown[2] = (byte)0;
            InputSystem.touchData.touchDown[3] = (byte)0;
        }

        public static void CheckKeyboardInput()
        {
            try
            {
                KeyboardState state = Keyboard.GetState();
                InputSystem.touchData.up = state.IsKeyDown(Keys.Up)/* || state.IsKeyDown(Keys.W) */? (byte)1 : (byte)0;
                InputSystem.touchData.down = state.IsKeyDown(Keys.Down) /*|| state.IsKeyDown(Keys.S)*/ ? (byte)1 : (byte)0;
                InputSystem.touchData.left = state.IsKeyDown(Keys.Left) /*|| state.IsKeyDown(Keys.A)*/ ? (byte)1 : (byte)0;
                InputSystem.touchData.right = state.IsKeyDown(Keys.Right) /*|| state.IsKeyDown(Keys.D)*/ ? (byte)1 : (byte)0;
                InputSystem.touchData.buttonA = state.IsKeyDown(Keys.NumPad1) || state.IsKeyDown(Keys.A) ? (byte)1 : (byte)0;
                InputSystem.touchData.buttonB = state.IsKeyDown(Keys.NumPad2) || state.IsKeyDown(Keys.S) ? (byte)1 : (byte)0;
                InputSystem.touchData.buttonC = state.IsKeyDown(Keys.NumPad3) || state.IsKeyDown(Keys.D) ? (byte)1 : (byte)0;
                InputSystem.touchData.start = state.IsKeyDown(Keys.Enter) || state.IsKeyDown(Keys.V) ? (byte)1 : (byte)0;
                //if (state.IsKeyDown(Keys.Space))
                //{
                //    InputSystem.touchControls = true;
                //}
                //else
                //{
                //    if (InputSystem.touchControls)
                //        ObjectSystem.globalVariables[110] = ObjectSystem.globalVariables[110] + 1 & 1;
                //    InputSystem.touchControls = false;
                //}
            }
            catch
            {
                InputSystem.touchData.up = (byte)0;
                InputSystem.touchData.down = (byte)0;
                InputSystem.touchData.left = (byte)0;
                InputSystem.touchData.right = (byte)0;
            }
        }

        public static void CheckKeyDown(InputResult gameInput, byte keyFlags)
        {
            gameInput.touches = 0;
            for (int index = 0; index < 4; ++index)
            {
                if (InputSystem.touchData.touchDown[index] == (byte)1)
                {
                    gameInput.touchDown[gameInput.touches] = InputSystem.touchData.touchDown[index];
                    gameInput.touchX[gameInput.touches] = InputSystem.touchData.touchX[index] * GlobalAppDefinitions.SCREEN_XSIZE / InputSystem.touchWidth;
                    gameInput.touchY[gameInput.touches] = InputSystem.touchData.touchY[index] * GlobalAppDefinitions.SCREEN_YSIZE / InputSystem.touchHeight;
                    ++gameInput.touches;
                }
            }
            if ((1 & (int)keyFlags) == 1)
                gameInput.up = InputSystem.touchData.up;
            if ((2 & (int)keyFlags) == 2)
                gameInput.down = InputSystem.touchData.down;
            if ((4 & (int)keyFlags) == 4)
                gameInput.left = InputSystem.touchData.left;
            if ((8 & (int)keyFlags) == 8)
                gameInput.right = InputSystem.touchData.right;
            if ((16 & (int)keyFlags) == 16)
                gameInput.buttonA = InputSystem.touchData.buttonA;
            if ((32 & (int)keyFlags) == 32)
                gameInput.buttonB = InputSystem.touchData.buttonB;
            if ((64 & (int)keyFlags) == 64)
                gameInput.buttonC = InputSystem.touchData.buttonC;
            if ((128 & (int)keyFlags) != 128)
                return;
            gameInput.start = InputSystem.touchData.start;
        }

        public static void MenuKeyDown(InputResult gameInput, byte keyFlags)
        {
            gameInput.touches = 0;
            for (int index = 0; index < 4; ++index)
            {
                if (InputSystem.touchData.touchDown[index] == (byte)1)
                {
                    gameInput.touchDown[gameInput.touches] = InputSystem.touchData.touchDown[index];
                    gameInput.touchX[gameInput.touches] = InputSystem.touchData.touchX[index] * GlobalAppDefinitions.SCREEN_XSIZE / InputSystem.touchWidth;
                    gameInput.touchY[gameInput.touches] = InputSystem.touchData.touchY[index] * 240 / InputSystem.touchHeight;
                    ++gameInput.touches;
                }
            }
        }

        public static void CheckKeyPress(InputResult gameInput, byte keyFlags)
        {
            if ((1 & (int)keyFlags) == 1)
            {
                if (InputSystem.touchData.up == (byte)1)
                {
                    if (InputSystem.inputPress.up == (byte)0)
                    {
                        InputSystem.inputPress.up = (byte)1;
                        gameInput.up = (byte)1;
                    }
                    else
                        gameInput.up = (byte)0;
                }
                else
                {
                    gameInput.up = (byte)0;
                    InputSystem.inputPress.up = (byte)0;
                }
            }
            if ((2 & (int)keyFlags) == 2)
            {
                if (InputSystem.touchData.down == (byte)1)
                {
                    if (InputSystem.inputPress.down == (byte)0)
                    {
                        InputSystem.inputPress.down = (byte)1;
                        gameInput.down = (byte)1;
                    }
                    else
                        gameInput.down = (byte)0;
                }
                else
                {
                    gameInput.down = (byte)0;
                    InputSystem.inputPress.down = (byte)0;
                }
            }
            if ((4 & (int)keyFlags) == 4)
            {
                if (InputSystem.touchData.left == (byte)1)
                {
                    if (InputSystem.inputPress.left == (byte)0)
                    {
                        InputSystem.inputPress.left = (byte)1;
                        gameInput.left = (byte)1;
                    }
                    else
                        gameInput.left = (byte)0;
                }
                else
                {
                    gameInput.left = (byte)0;
                    InputSystem.inputPress.left = (byte)0;
                }
            }
            if ((8 & (int)keyFlags) == 8)
            {
                if (InputSystem.touchData.right == (byte)1)
                {
                    if (InputSystem.inputPress.right == (byte)0)
                    {
                        InputSystem.inputPress.right = (byte)1;
                        gameInput.right = (byte)1;
                    }
                    else
                        gameInput.right = (byte)0;
                }
                else
                {
                    gameInput.right = (byte)0;
                    InputSystem.inputPress.right = (byte)0;
                }
            }
            if ((16 & (int)keyFlags) == 16)
            {
                if (InputSystem.touchData.buttonA == (byte)1)
                {
                    if (InputSystem.inputPress.buttonA == (byte)0)
                    {
                        InputSystem.inputPress.buttonA = (byte)1;
                        gameInput.buttonA = (byte)1;
                    }
                    else
                        gameInput.buttonA = (byte)0;
                }
                else
                {
                    gameInput.buttonA = (byte)0;
                    InputSystem.inputPress.buttonA = (byte)0;
                }
            }
            if ((32 & (int)keyFlags) == 32)
            {
                if (InputSystem.touchData.buttonB == (byte)1)
                {
                    if (InputSystem.inputPress.buttonB == (byte)0)
                    {
                        InputSystem.inputPress.buttonB = (byte)1;
                        gameInput.buttonB = (byte)1;
                    }
                    else
                        gameInput.buttonB = (byte)0;
                }
                else
                {
                    gameInput.buttonB = (byte)0;
                    InputSystem.inputPress.buttonB = (byte)0;
                }
            }
            if ((64 & (int)keyFlags) == 64)
            {
                if (InputSystem.touchData.buttonC == (byte)1)
                {
                    if (InputSystem.inputPress.buttonC == (byte)0)
                    {
                        InputSystem.inputPress.buttonC = (byte)1;
                        gameInput.buttonC = (byte)1;
                    }
                    else
                        gameInput.buttonC = (byte)0;
                }
                else
                {
                    gameInput.buttonC = (byte)0;
                    InputSystem.inputPress.buttonC = (byte)0;
                }
            }
            if ((128 & (int)keyFlags) != 128)
                return;
            if (InputSystem.touchData.start == (byte)1)
            {
                if (InputSystem.inputPress.start == (byte)0)
                {
                    InputSystem.inputPress.start = (byte)1;
                    gameInput.start = (byte)1;
                }
                else
                    gameInput.start = (byte)0;
            }
            else
            {
                gameInput.start = (byte)0;
                InputSystem.inputPress.start = (byte)0;
            }
        }
    }
}
