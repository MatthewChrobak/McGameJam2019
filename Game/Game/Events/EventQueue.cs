﻿using Game.Patterns.Singleton;
using Game.UserInterface;
using Game.UserInterface.Scenes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Events
{
    public class EventQueue : Singleton
    {
        private List<GameEvent>[] queue = new List<GameEvent>[(int)PriorityTypes.END + 1];

        public EventQueue() {
            for (int i = (int)PriorityTypes.START; i <= (int)PriorityTypes.END; i++) {
                queue[i] = new List<GameEvent>();
            }
        }

        public void AddEvent(PriorityTypes type, GameEvent e) {
            queue[(int)type].Add(e);
        }

        public void AddEvent(PriorityTypes type, Func<EVENT_RETURN> e, int interval_ms, int delay_ms, string identifier = "null") {
            AddEvent(type, new GameEvent(e, interval_ms, delay_ms, identifier));
        }

        public void Run() {
            int tick;
            int lastTick = Environment.TickCount;
            var ui = Singleton.Get<UIManager>();

            int statsInterval = 7000;
            int statsTick = Environment.TickCount + statsInterval;

            while (ui._currentSceneID != typeof(Closing)) {
                tick = Environment.TickCount;
                int dif = tick - lastTick;
                lastTick = tick;

                if (dif == 0) {
                    Thread.Yield();
                    continue;
                }

                for (int i = (int)PriorityTypes.START; i <= (int)PriorityTypes.END; i++) {
                    for (int ii = 0; ii < queue[i].Count; ii++) {
                        var res = queue[i][ii].Probe(dif);

                        if (res == EVENT_RETURN.REMOVE_FROM_QUEUE) {
                            queue[i].RemoveAt(ii);
                            ii--;
                        }
                    }
                }

                // Uncomment for stats
                //if (statsTick <= tick) {
                //    Console.WriteLine();
                //    Console.WriteLine("-------------------------------------------------");
                //    for (int i = (int)PriorityTypes.START; i <= (int)PriorityTypes.END; i++) {
                //        for (var ii = 0; ii < queue[i].Count; ii++) {
                //            queue[i][ii].DisplayStatistics(statsInterval);
                //        }
                //    }
                //    Console.WriteLine("-------------------------------------------------");
                //    statsTick = tick + statsInterval;
                //}
                Thread.Yield();
            }
        }
    }
}
