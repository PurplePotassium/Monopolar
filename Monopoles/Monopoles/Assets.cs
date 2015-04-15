using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Monopoles{
    public static class Assets{
        public static ContentManager Content;
        static Dictionary<string,Texture2D> textures = new Dictionary<string,Texture2D>();
        static Dictionary<string,SoundEffect> sounds = new Dictionary<string,SoundEffect>();

        public static void Initialize(ContentManager content){
            Content = content;
        }
        
        public static Texture2D GetTex(string image){
            if(!textures.ContainsKey(image))
                textures[image] = Content.Load<Texture2D>(image);
            return textures[image];
        }

        public static SoundEffect GetSound(string sound){
            if(!textures.ContainsKey(sound))
                sounds[sound] = Content.Load<SoundEffect>(sound);
            return sounds[sound];
        }
    }
}
