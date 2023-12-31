﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using wstoccob.Engine.Input;
using wstoccob.Engine.States;
using wstoccob.Objects;
using wstoccob.Particles;

namespace wstoccob.States.Dev;

public class DevState : BaseGameState
{
    private const string ExhaustTexture = "Cloud001";
    private const string MissileTexture = "Missile05";
    private const string PlayerFighter = "Fighter";
    private ExhaustEmitter _exhaustEmitter;
    private MissileSprite _missile;
    private PlayerSprite _player;

    public override void LoadContent()
    {
        var exhaustPosition = new Vector2(_viewportWidth / 2, _viewportHeight / 2);
        _exhaustEmitter = new ExhaustEmitter(LoadTexture(ExhaustTexture), exhaustPosition);
        AddGameObject(_exhaustEmitter);
        _player = new PlayerSprite(LoadTexture(PlayerFighter));
        _player.Position = new Vector2(500, 500);
        AddGameObject(_player);
    }

    public override void HandleInput(GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is DevInputCommand.DevQuit)
            {
                NotifyEvent(new BaseGameStateEvent.GameQuit());
            }

            if (cmd is DevInputCommand.DevShoot)
            {
                _missile = new MissileSprite(LoadTexture(MissileTexture), LoadTexture(ExhaustTexture));
                _missile.Position = new Vector2(_player.Position.X, _player.Position.Y - 25);
                AddGameObject(_missile);
            }
        });
    }

    public override void UpdateGameState(GameTime gameTime)
    {
        _exhaustEmitter.Position = new Vector2(_exhaustEmitter.Position.X, _exhaustEmitter.Position.Y - 3f);
        _exhaustEmitter.Update(gameTime);
        if (_missile != null)
        {
            _missile.Update(gameTime);
            if (_missile.Position.Y < -100)
            {
                RemoveGameObject(_missile);
            }
        }
        if (_exhaustEmitter.Position.Y < -200)
        {
            RemoveGameObject(_exhaustEmitter);
        }
    }
    protected override void SetInputManager()
    {
        InputManager = new InputManager(new DevInputMapper());
    }
}