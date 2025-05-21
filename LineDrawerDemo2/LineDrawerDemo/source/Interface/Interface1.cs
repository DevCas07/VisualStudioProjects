using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineDrawerDemo.source.Interface
{
    internal interface IActor
    {
        void Act();
    }

    public class Monster : IActor
    {
        public Monster() { }
        public void Act()
        {
            // move ...
        }

        public void Healf()
        {

        }
    }
    public class Player : IActor
    {

        public Player() { }

        public void Act()
        {
            // check keyboard
            // if ... flytta
        }
    }

    public class World
    {
        private List<IActor> actors = new List<IActor>();

        public void Test()
        {
            IActor actor1 = new Monster();
            actors.Add(actor1);
            actors.Add(new Monster());
            actors.Add(new Player());
            foreach (var actor in actors)
            {
                actor.Act();
                //actor.Do();
            }
        }
        void Add(IActor actor)
        {
            actors.Add(actor);
        }

       
    }
}
