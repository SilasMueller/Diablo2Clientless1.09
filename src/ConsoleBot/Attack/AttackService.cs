﻿using ConsoleBot.Helpers;
using D2NG.Core;
using D2NG.Core.D2GS;
using D2NG.Core.D2GS.Objects;
using D2NG.Navigation.Extensions;
using D2NG.Navigation.Services.Pathing;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleBot.Attack
{
    public class AttackService : IAttackService
    {
        private readonly IPathingService _pathingService;

        public AttackService(IPathingService pathingService)
        {
            _pathingService = pathingService;
        }

        internal class Line
        {
            public Point StartPoint { get; set; }

            public Point EndPoint { get; set; }
        }

        private static double DotProduct((double X, double Y) a, (double X, double Y) b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        private static double MinimumDistanceToLineSegment(Point p,
            Line line)
        {
            var v = line.StartPoint;
            var w = line.EndPoint;

            double lengthSquared = v.DistanceSquared(w);

            if (lengthSquared == 0.0)
                return p.Distance(v);

            double t = Math.Max(0, Math.Min(1, DotProduct(p.Substract((v.X, v.Y)), w.Substract((v.X, v.Y))) / lengthSquared));

            short dX = (short)(((double)w.X - v.X) * t);
            short dY = (short)(((double)w.Y - v.Y) * t);
            var projection = v;
            projection = projection.Add(dX, dY);

            return p.Distance(projection);
        }

        public async Task<bool> IsInLineOfSight(Client client, Point toLocation)
        {
            return await IsInLineOfSight(client, client.Game.Me.Location, toLocation);
        }

        public async Task<bool> IsInLineOfSight(Client client, Point fromLocation, Point toLocation)
        {
            var directDistance = fromLocation.Distance(toLocation);
            if (directDistance == 0)
            {
                return true;
            }

            var path = await _pathingService.GetPathToLocation(client.Game, toLocation, MovementMode.Walking);
            if (path.Count == 0)
            {
                return true;
            }

            var line = new Line
            {
                StartPoint = fromLocation,
                EndPoint = toLocation
            };

            var pointsOutside = false;
            if (path.Count() > 1)
            {
                pointsOutside = ((double)path.Count(p => MinimumDistanceToLineSegment(p, line) >= 4.1)) / path.Count > 0.15;
            }
            return !pointsOutside;
        }

        public async Task<bool> IsVisitable(Client client, Point point)
        {
            var path = await _pathingService.GetPathToLocation(client.Game, point, MovementMode.Walking);
            return path.Count != 0;
        }

        private List<Point> GetNearbyMonsters(List<Point> enemies, Point location, double distance)
        {
            return enemies.Where(p => p.Distance(location) < distance).ToList();

        }

        private async Task<Point> FindNearbySafeSpot(Client client, List<Point> enemies, Point toLocation, double minDistance = 0, double maxdistance = 30)
        {
            for (int i = 1; i < 5; ++i)
            {
                foreach (var (p1, p2) in new List<(short, short)> {
                    (-5,0), (5, 0), (0, -5), (0, 5), (-5, 5), (-5, -5), (5, -5), (5, 5)})
                {
                    var x = (short)(p1 * i);
                    var y = (short)(p2 * i);

                    var distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                    if (distance < minDistance || distance > maxdistance)
                    {
                        continue;
                    }

                    var tryLocation = toLocation.Add(x, y);
                    if (await IsVisitable(client, tryLocation) && await IsInLineOfSight(client, tryLocation, toLocation) && GetNearbyMonsters(enemies, tryLocation, 5.0).Count() < 2)
                    {
                        return tryLocation;
                    }
                }
            }

            return null;
        }

        public async Task<bool> MoveToNearbySafeSpot(Client client, List<Point> enemies, Point toLocation, MovementMode movementMode, double minDistance = 0, double maxDistance = 30)
        {
            var spot = await FindNearbySafeSpot(client, enemies, toLocation, minDistance, maxDistance);
            if(spot != null)
            {
                if(movementMode == MovementMode.Teleport)
                {
                    if (await GeneralHelpers.TryWithTimeout(async (retryCount) =>
                    {
                        return await client.Game.TeleportToLocationAsync(spot);
                    }, TimeSpan.FromSeconds(4)))
                    {
                        return true;
                    }
                }
                else
                {
                    var path = await _pathingService.GetPathToLocation(client.Game, spot, MovementMode.Walking);
                    if (!await MovementHelpers.TakePathOfLocations(client.Game, path, MovementMode.Walking))
                    {
                        Log.Warning($"Walking to safe spot failed at {client.Game.Me.Location}");
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
