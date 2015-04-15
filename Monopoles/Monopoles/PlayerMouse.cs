using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Monopoles {
    public static class PlayerMouse{
        public static bool KeyDown(Keys key){
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
