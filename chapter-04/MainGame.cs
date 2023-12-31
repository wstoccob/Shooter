﻿using chapter_04.Enum;
using chapter_04.State;
using chapter_04.States.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_04;

public class MainGame : Game
{
    private BaseGameState _currentGameState;
    
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private RenderTarget2D _renderTarget;
    private Rectangle _renderScaleRectangle;
    private const int DESIGNED_RESOLUTION_WIDTH = 640;
    private const int DESIGNED_RESOLUTION_HEIGHT = 480;
    private const float DESIGNED_RESOLUTION_ASPECT_RATIO = DESIGNED_RESOLUTION_WIDTH / (float)DESIGNED_RESOLUTION_HEIGHT;

    public MainGame()
    {
        graphics = new GraphicsDeviceManager(this);
        
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        graphics.PreferredBackBufferWidth = 1024;
        graphics.PreferredBackBufferHeight = 768;
        IsMouseVisible = true;
        graphics.IsFullScreen = false;
        graphics.ApplyChanges();
        _renderTarget = new RenderTarget2D(
            graphics.GraphicsDevice, 
            DESIGNED_RESOLUTION_WIDTH, 
            DESIGNED_RESOLUTION_HEIGHT, 
            false,
            SurfaceFormat.Color, 
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents);
        _renderScaleRectangle = GetScaleRectangle();
        
        
        base.Initialize();
    }

    private Rectangle GetScaleRectangle()
    {
        var variance = 0.5;
        var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
        Rectangle scaleRectangle;
        if (actualAspectRatio <= DESIGNED_RESOLUTION_ASPECT_RATIO)
        {
            var presentHeight = (int)(Window.ClientBounds.Width / DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
            var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
            scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
        }
        else
        {
            var presentWidth = (int)(Window.ClientBounds.Height * DESIGNED_RESOLUTION_ASPECT_RATIO + variance);
            var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
            scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
        }

        return scaleRectangle;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        
        SwitchGameState(new SplashState());

        // TODO: use this.Content to load your game content here
    }
    private void CurrentGameState_OnStateSwitched(object sender, BaseGameState e)
    {
        SwitchGameState(e);
    }

    private void SwitchGameState(BaseGameState gameState)
    {
        if (_currentGameState != null)
        {
            _currentGameState.OnStateSwitched -= CurrentGameState_OnStateSwitched;
            _currentGameState.OnEventNotification -= _currentGameState_OnEventNotification;
            _currentGameState.UnloadContent(Content);
        }
        
        _currentGameState = gameState;
        _currentGameState.LoadContent(Content);
        _currentGameState.OnStateSwitched += CurrentGameState_OnStateSwitched;
        _currentGameState.OnEventNotification += _currentGameState_OnEventNotification;
    }

    private void _currentGameState_OnEventNotification(object sender, Enum.Events e)
    {
        switch (e)
        {
            case Events.GAME_QUIT:
                Exit();
                break;
        }
    }

    protected override void UnloadContent()
    {
        _currentGameState?.UnloadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _currentGameState.HandleInput();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // render to the render target
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        _currentGameState.Render(spriteBatch);
        spriteBatch.End();
        
        // now render the scaled content
        graphics.GraphicsDevice.SetRenderTarget(null);
        graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }   
}