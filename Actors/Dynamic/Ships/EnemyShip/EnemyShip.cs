using GalagaFramework.Actors.Dynamic.Bullets;
using GalagaFramework.Actors.Dynamic.Ships.Galaga;
using GalagaFramework.Actors.Kinemactic.Squad;
using GalagaFramework.Directors;
using GalagaFramework.Directors.Gameplays;
using GalagaFramework.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace GalagaFramework.Actors.Dynamic.Ships.EnemyShip
{
    public class EnemyShip : Dynamic
    {
        internal EnemyShipState State { get; private set; }

        public int FleetIndex { get; private set; }
        Vector2 FleetPosition;
        Vector2 FleetRelativePosition;

        public SquadController SquadCtr { get; private set; }

        internal const float MAX_VELOCITY = 150;//60
        const float MARGIN = 25;

        protected bool bottomReached;
        protected bool fleetExited;

        protected const float DIVE_VELOCITY = 100;

        public BezierCurve BezierCurve;

        GameSprite EndPosition;

        bool shotAlready;
        bool returningFromAttack;
        int Point;
        protected bool AnimationTime;

        internal float HitTime { get; private set; }

        public EnemyShip(Texture2D texture, BezierCurve bezierCurve, int index, SquadController squadCtr = null)
            : base(texture)
        {
            BezierCurve = bezierCurve;
            FleetIndex = index;
            Position = bezierCurve.GetControlPoints()[0];
            Point = 1;

            SquadCtr = squadCtr;
            State = EnemyShipState.EnterScene;

            Image.Frame = new Rectangle(0, 0, 14, 14);
            Image.RotationPoint = new Vector2(14, 14) / 2;
            Image.SetPosition(Position);

            EndPosition = new GameSprite(TextureManager.Empty);
            EndPosition.CenterInPosition(FleetPosition);
            EndPosition.Scale = 0.25f;
            EndPosition.Color = Microsoft.Xna.Framework.Color.Pink;
        }

        public EnemyShip(Vector2 position, int index)
            : base(TextureManager.Honet)
        {
            Position = position;
            FleetIndex = index;
            State = EnemyShipState.InFleet;

            Image.SetPosition(Position);
            //Image.Scale = 0.5f;

            EndPosition = new GameSprite(TextureManager.Empty);
            EndPosition.CenterInPosition(FleetPosition);
            EndPosition.Scale = 0.25f;
            EndPosition.Color = Microsoft.Xna.Framework.Color.Pink;
        }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            UpdateState();
        }

        internal void SetFleetPosition(Vector2 fleetPosition, Vector2 fleetRelativePosition, bool returningFromAttack)
        {
            this.returningFromAttack = returningFromAttack;
            FleetPosition = fleetPosition;
            FleetRelativePosition = fleetRelativePosition;
            State = EnemyShipState.EnterFleet;
            EndPosition.CenterInPosition(FleetPosition);
            if (SquadCtr != null)
                SquadCtr.SquadShipOut();
            SquadCtr = null;
        }

        internal virtual void SetAttack()
        {
            Point = 0;
            fleetExited = false;
            BezierCurve = BezierPreDefined.Exit();
            if (FleetRelativePosition.X < 0)
                BezierCurve.Mirror();
            BezierCurve.RelativeBezierToPoint(Position);
            SoundManager.EnemyShipAttacking.Play();
            State = EnemyShipState.Attacking;
        }

        internal void Expand(Vector2 fleetPosition, float value)
        {
            Position = fleetPosition + (FleetRelativePosition * value);
            Image.SetPosition(Position);
        }

        internal void StartToExplode()
        {
            if (SquadCtr != null)
            {
                SquadCtr.SquadShipOut();
                SquadCtr = null;
            }
            Image.Color = Color.YellowGreen;
            HitTime = TotalSeconds;
        }

        void UpdateState()
        {
            switch (State)
            {
                case EnemyShipState.EnterScene: UpdateIdle(); break;
                case EnemyShipState.EnterFleet: UpdateFleetEntering(); break;
                case EnemyShipState.Attacking: UpdateAttackig(); break;
                case EnemyShipState.InFleet: Animation(); break;
            }
        }

        void UpdateFleetEntering()
        {
            var direction = FleetPosition - Position;
            direction.Normalize();
            if (returningFromAttack)
                direction = (direction * DIVE_VELOCITY) * Deltatime;
            else
                direction = (direction * MAX_VELOCITY) * Deltatime;

            Position.X = MathHelper.Clamp(Position.X + direction.X, 0, ApplicationMemory.ScreenController.GridPosition.X);
            Position.Y = MathHelper.Clamp(Position.Y + direction.Y, 0, ApplicationMemory.ScreenController.GridPosition.Y);

            if (Vector2.Distance(FleetPosition, Position) < 1.5f)
            {
                Position = FleetPosition;
                State = EnemyShipState.InFleet;
            }
            Image.SetPosition(Position);
        }

        void UpdateIdle()
        {
            if (FollowBazier() == true)
                TryToEnterInFleet();
        }

        protected virtual void UpdateAttackig()
        {
            if (GalagaFleet == null)
            {
                TryToEnterInFleet();
                bottomReached = false;
                shotAlready = false;
            }

            if (Gameplay.Status == GameplayStatus.Playing && DetectCollisions(base.Galagas.ToList<Dynamic>()))
            {
                ExplodeWithGalaga();
                return;
            }

            if (fleetExited == false)
            {
                PerformExitFleet();
                return;
            }

            if (FollowBazier() == true)
            {
                BezierCurve = BezierPreDefined.Attack();
                if (GalagaFleet.NearPositionToHitGalaga(Position).X - Position.X < 0)
                    BezierCurve.Mirror();
                BezierCurve.RelativeBezierToPoint(Position);
                Point = 0;
            }

            if (Position.Y > ApplicationMemory.ScreenController.GridPosition.Y * 0.5f)
                Shoot();

            if (Position.Y > ApplicationMemory.ScreenController.GridPosition.Y)
            {
                bottomReached = true;
                Position.Y -= ApplicationMemory.ScreenController.GridPosition.Y;
            }

            if (bottomReached && Position.Y > MARGIN)
            {
                TryToEnterInFleet();
                bottomReached = false;
                shotAlready = false;
            }
            Image.SetPosition(Position);
        }

        protected void PerformExitFleet()
        {
            if (FollowBazier() == true)
            {
                BezierCurve = BezierPreDefined.Attack();
                if (FleetRelativePosition.X > 0)
                    BezierCurve.Mirror();
                BezierCurve.RelativeBezierToPoint(Position);
                Point = 0;
                fleetExited = true;
            }
        }

        protected void ExplodeWithGalaga()
        {
            GalagaFleet.DestroyGalaga(Collisions.First() as GalagaController);
            Fleet.DesotroyShip(this);
        }

        void Shoot()
        {
            if (shotAlready)
                return;

            Fleet.AddBullet(new Bullet(Position, true));
            shotAlready = true;
        }


        bool FollowBazier()
        {
            var points = BezierCurve.GetDrawingPoints0();
            if (Vector2.Distance(Position, points[Point]) < 1.5f)
                Point++;

            if (Point == points.Count)
                return true;

            var direction = points[Point] - Position;
            direction.Normalize();
            SpriteToDirection(direction);

            if (State == EnemyShipState.Attacking)
                Position += direction * DIVE_VELOCITY * Deltatime;
            else
                Position += direction * MAX_VELOCITY * Deltatime;

            Image.SetPosition(Position);
            return false;
        }

        protected void TryToEnterInFleet()
        {
            Image.Rotation = 0;
            if (Fleet == null)//PahDesign mode
            {
                Position = BezierCurve.GetDrawingPoints0()[0];
                Point = 1;
                return;
            }

            Fleet.AddShipInFleet(this, FleetIndex);
        }

        internal void Move(float positionX)
        {
            Position.X = positionX + FleetRelativePosition.X;
            //Image.CenterInPosition(Position);
            Image.SetPosition(Position);
        }

        protected virtual void Animation()
        {
            if (AnimationTime && ((int)TotalSeconds) % 2 == 1)
            {
                Image.Frame = new Rectangle(0, 0, 14, 14);
                AnimationTime = false;
            }
            else if (!AnimationTime && ((int)TotalSeconds) % 2 == 0)
            {
                Image.Frame = new Rectangle(14, 0, 14, 14);
                AnimationTime = true;
            }
        }

        protected virtual void SpriteToDirection(Vector2 direction)
        {
            var angle = (float)Math.Atan2(direction.X, direction.Y);
            angle = (angle + MathHelper.ToRadians(180 - 7)) * -1;
            /*
            //Rotate fixed to sprites
            var quad = MathHelper.WrapAngle(angle);
            if (quad < (-MathHelper.Pi / 2))
                Image.Rotation = MathHelper.ToRadians(180);
            else if (quad < 0)
                Image.Rotation = MathHelper.ToRadians(90);
            else if (quad < (MathHelper.Pi / 2))
                Image.Rotation = MathHelper.ToRadians(0);
            else
                Image.Rotation = MathHelper.ToRadians(-90);

            angle = Math.Abs(MathHelper.ToDegrees(angle)) % 90;

            if (angle < 7)
                Image.Frame = new Rectangle(0, 0, 14, 14);
            else if (angle < 22)
                Image.Frame = new Rectangle(28, 0, 14, 14);
            else if (angle < 37)
                Image.Frame = new Rectangle(42, 0, 14, 14);
            else if (angle < 52)
                Image.Frame = new Rectangle(56, 0, 14, 14);
            else if (angle < 67)
                Image.Frame = new Rectangle(70, 0, 14, 14);
            else if (angle < 82)
                Image.Frame = new Rectangle(84, 0, 14, 14);
            else
                Image.Frame = new Rectangle(0, 0, 14, 14);
            */
            Image.Rotation = angle;
            Animation();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            /*
            if (State == EnemyShipState.EnterFleet)
                EndPosition.Draw(spriteBatch);
            */

            //BezierCurve.Draw(spriteBatch);
        }
    }
}

